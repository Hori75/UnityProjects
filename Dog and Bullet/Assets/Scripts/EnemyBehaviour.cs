using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float EnemySpeed = 7.5f;
    Rigidbody2D rb;

    public int BulletCount = 2;
    public float BulletCoolDown = 3;
    private float nextEnemyFire;
    public GameObject BulletSpawn;
    public GameObject EnemyBullet;
    public int scoreValue;
    public GameObject Explosion;

    GameObject Player;
    Vector3 relativepos;
    Quaternion aim;

    private GameController gameController;

    bool onBoundary;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else Debug.Log("GameController not found");
    }

    void Update()
    {
        if ((Time.time >= nextEnemyFire)&&(BulletCount > 0)&&(Player != null)&&(onBoundary)&&(Randomfire()))
        {
            EnemyFire();
            nextEnemyFire = Time.time + BulletCoolDown;
            BulletCount--;
        }
    }

    void FixedUpdate()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.velocity = transform.up * EnemySpeed * -1;
    }

    private Vector2 hit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Boundary")||(other.tag == "EnemyBullet"))
        {
            onBoundary = true;
            return;
        }
        else if (other.tag == "Bullet")
        {
            hit = gameObject.transform.position;
            gameController.AddScore(scoreValue);
            other.gameObject.SetActive(false);

            gameObject.SetActive(false);
            Instantiate(Explosion, hit, Quaternion.identity);
        }
    }

    private void EnemyFire()
    {
        relativepos = Player.transform.position - gameObject.transform.position;
        GameObject EnemyBulletClone = Instantiate(EnemyBullet, BulletSpawn.transform.position, Quaternion.identity) as GameObject;
        EnemyBulletClone.GetComponent<BulletMover>().setDirection(relativepos);
    }

    private bool Randomfire()
    {
        int x = Random.Range(0, 2);
        return (x == 1);
    }
}
