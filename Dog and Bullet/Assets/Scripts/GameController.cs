using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Vector2 spawnValues = new Vector2 (0, -3.75f);
    public int EnemyWaveCount;
    public int EnemyEachWaves;
    public float SpawnDelay = 0.325f;
    public float StartDelay = 2;
    public float waveDelay = 3;
    public int LifeCount = 3;
    public Vector2 spawnPoint = new Vector2 (0, -3.75f);

    public Text scoreText;
    public Text RestartText;
    public Text GameOverText;

    public GameObject Background2;
    public GameObject Background3;

    private int score;
    private bool restart;
    private bool gameOver;
    private bool Destroyed;
    private bool Started = false;

    void Start()
    {
        gameOver = false;
        restart = false;
        Destroyed = false;
        RestartText.text = "Lives :" + LifeCount;
        GameOverText.text = "";
        UpdateScore();
        StartCoroutine(SpawnEnemies());
    }

    void Update()
    {
        if(!Started)
        {
            Started = true;
            SpawnPlayer();
        }
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(StartDelay);

        while (true)
        {
            if (Destroyed)
            {
                while(EnemyStillPresent())
                {
                    yield return new WaitForSeconds(waveDelay);
                }
                Destroyed = false;
                SpawnPlayer();
                yield return new WaitForSeconds(StartDelay);
            }

            for (int i = 0; i < EnemyWaveCount; i++)
            {
                float xspawn = Random.Range(-spawnValues.x, spawnValues.x);

                int enemies = EnemyEachWaves;

                while (enemies > 0)
                {
                    SpawnEnemy(xspawn);
                    enemies--;
                    yield return new WaitForSeconds(SpawnDelay);
                }
                if (Destroyed) break;
            }
            yield return new WaitForSeconds(waveDelay);

            if (gameOver)
            {
                RestartText.text = "Press 'R' for restart";
                restart = true;
                break;
            }
        }
    }

    public void SpawnPlayer()
    {
        GameObject Player = gameObject.GetComponent<ObjectPool>().GetPooledObject("Player");

        Player.transform.position = spawnPoint;
        Player.transform.rotation = Quaternion.identity;
        Player.SetActive(true);
    }

    private void SpawnEnemy(float x)
    {
        Vector2 spawnPosition = new Vector2(x, spawnValues.y);
        Quaternion spawnRotation = Quaternion.identity;

        GameObject Enemy = gameObject.GetComponent<ObjectPool>().GetPooledObject("Enemy");

        Enemy.transform.position = spawnPosition;
        Enemy.transform.rotation = spawnRotation;
        Enemy.SetActive(true);
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void ReduceLive()
    {
        LifeCount--;
        RestartText.text = "Lives: " + LifeCount;
        if (LifeCount == 0) GameOver();
        else Destroyed = true;
    }

    public void GameOver()
    {
        GameOverText.text = "Game Over\n Your Score:\n" + score;
        gameOver = true;
    }

    private bool EnemyStillPresent()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject enemy in Enemies)
        {
            if (enemy.activeInHierarchy)
            {
                return true;
            }
        }
        return false;
    }

    private bool ground2 = true;
    public void RequestBackground()
    {
        if(ground2)
        {
            ground2 = false;
            Instantiate(Background2, new Vector3 (0, 9.5f, 5), Quaternion.identity);
        }
        else
        {
            ground2 = true;
            Instantiate(Background3, new Vector3(0, 9.5f, 5), Quaternion.identity);
        }

    }
}
