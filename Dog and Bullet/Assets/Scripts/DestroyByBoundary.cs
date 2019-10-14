using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.tag == "Enemy") || (other.tag == "EnemyBullet") ||(other.tag == "Bullet"))
        {
            other.gameObject.SetActive(false);
        }
    }
}
