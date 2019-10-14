using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float ScrollSpeed = 1;
    GameController gameController;
    Rigidbody2D rb;
    bool ReBack = true;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        else Debug.Log("GameController not found");
    }

    void FixedUpdate()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        rb.velocity = -ScrollSpeed * transform.up;

        if((gameObject.transform.position.y <= 0.5)&&(ReBack))
        {
            gameController.RequestBackground();
            ReBack = false;
        }

        if (gameObject.transform.position.y == -10)
        {
            Destroy(gameObject);
        }
    }
}
