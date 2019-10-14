using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 BulletDirection;
    public float BulletSpeed = 5;

    public void setDirection(Vector2 Direction)
    {
        BulletDirection = Direction.normalized;
    }

    void Update()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = BulletDirection * BulletSpeed;
    }
}
