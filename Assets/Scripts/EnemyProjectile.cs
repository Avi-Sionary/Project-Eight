using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Projectile"))
            Destroy(gameObject);

        /* where player would take damage
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Health>().Hit(20);
        }
        */
    }
}