using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.gameObject.CompareTag("Projectile"))
            Destroy(gameObject);

        /* where enemy would take damage
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Health>().Hit(20);
        }
        */
    }
}
