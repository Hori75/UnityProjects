using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBullet : MonoBehaviour
{
    private float speed = 5;

    public float FireCoolDown;
    public float BetweenShots;
    public int Shots;
    private float nextFire;

    public GameObject EnemyBullet;
    public GameObject FireSpawn1;
    public GameObject FireSpawn2;

    private GameObject player;

    private Vector2 RelativePosition;
    private Quaternion rotation;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(FireSequence());
    }

    void Update()
    {
        RelativePosition = player.transform.position - gameObject.transform.position;
        float angle = Mathf.Atan2(RelativePosition.y, RelativePosition.x)* Mathf.Rad2Deg + 90;
        rotation = Quaternion.AngleAxis(angle,Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
    }

    IEnumerator FireSequence()
    {
        while (true)
        {
            for (int i = 0; i < Shots; i++)
            {
                FireEnemyBullet();
                yield return new WaitForSeconds(BetweenShots);
            }
            
            yield return new WaitForSeconds(FireCoolDown);
        }
    }

    void FireEnemyBullet()
    {
        GameObject EnemyBullet1 = Instantiate(EnemyBullet, FireSpawn1.transform.position, Quaternion.identity) as GameObject;
        GameObject EnemyBullet2 = Instantiate(EnemyBullet, FireSpawn2.transform.position, Quaternion.identity) as GameObject;
        EnemyBullet1.GetComponent<BulletMover>().setDirection(RelativePosition);
        EnemyBullet2.GetComponent<BulletMover>().setDirection(RelativePosition);
    }
}
