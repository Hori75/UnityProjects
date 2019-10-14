using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 10;
    public float FireRate = 1;
    public Boundary boundary;

    public Transform BulletSpawn1;
    public Transform BulletSpawn2;
    private float nextFire;

    public GameObject Explosion;

    private GameController gameController;
    private ObjectPool objectPool;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
            objectPool = gameControllerObject.GetComponent<ObjectPool>();
        }
        else Debug.Log("GameController not found");
    }

    void Update()
    {
        if ((Input.GetButton("Fire1"))&&(Time.time >= nextFire))
        {
            nextFire = Time.time + FireRate;
            PlayerFireBullet();
        }
    }

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        float Movex = Input.GetAxis("Horizontal") * PlayerSpeed;
        float Movey = Input.GetAxis("Vertical") * PlayerSpeed;
        Vector2 movement = new Vector2(Movex,Movey);

        rb.velocity = movement * PlayerSpeed;
        rb.position = new Vector2
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
        );
    }

    private Vector2 hit;

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool GotHit = false;
        if ((other.tag == "Boundary")||(other.tag == "Bullet"))
            return;
        else
        {
            bool HitTagCheck = ((other.tag == "EnemyBullet") || (other.tag == "Enemy"));
            if (HitTagCheck && (GotHit != true))
            {
                GotHit = true;
                hit = gameObject.transform.position;
                gameController.ReduceLive();
            }
        }

        gameObject.SetActive(false);
        other.gameObject.SetActive(false);
        Instantiate(Explosion, hit, Quaternion.identity);

    }

    private void PlayerFireBullet()
    {
        GameObject BulletClone1 = objectPool.GetPooledObject("Bullet");
        if (BulletClone1 != null)
        {
            BulletClone1.transform.position = BulletSpawn1.position;
            BulletClone1.transform.rotation = BulletSpawn1.rotation;
            BulletClone1.GetComponent<BulletMover>().setDirection(new Vector2(0, 1));
            BulletClone1.SetActive(true);
        }

        GameObject BulletClone2 = objectPool.GetPooledObject("Bullet");
        if (BulletClone2 != null)
        {
            BulletClone2.transform.position = BulletSpawn2.position;
            BulletClone2.transform.rotation = BulletSpawn2.rotation;
            BulletClone2.GetComponent<BulletMover>().setDirection(new Vector2(0, 1));
            BulletClone2.SetActive(true);
        }
    }
}
