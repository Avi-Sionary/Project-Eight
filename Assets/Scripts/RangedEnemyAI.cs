using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

//http://www.devination.com/2015/07/unity-2d-platformer-tutorial-part-4.html 13-16 32-49

public class RangedEnemyAI : MonoBehaviour {
    public LayerMask rangedEnemyMask;
    public float speed;
    public float enemyProjectileSpeed;
    public GameObject enemyProjectilePrefab;
    public Transform enemyProjectileSpawn;
    public float attackInterval;
    public float attackRange;
    public float groundDetectionRange;
    public Sprite enemyShootingSprite;
    public Sprite enemyStandingSprite;

    float lastShot = 0;
    Transform playerLocation; 
    bool shooting;
    Rigidbody2D myBody;
    float myWidth, myHeight;
    SpriteRenderer sr;
    // Use this for initialization
    void Start () {
        playerLocation = GameObject.FindObjectOfType<Player>().transform;
        shooting = false;
        myBody = GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        myWidth = sr.bounds.extents.x;
        myHeight = sr.bounds.extents.y;
    }
	
    void FixedUpdate()
    {
        if (!shooting)
        {
            sr.sprite = enemyStandingSprite;
            Vector2 lineCastPos = transform.position.toVector2() - transform.right.toVector2() * myWidth + Vector2.up * myHeight;

            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down*(groundDetectionRange/10));
            bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down*(groundDetectionRange/10), rangedEnemyMask);

            Debug.DrawLine(lineCastPos, lineCastPos - transform.right.toVector2() * .05f);
            bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - transform.right.toVector2() * .05f, rangedEnemyMask);
            if (!isGrounded || isBlocked)
            {
                Vector3 currRot = transform.eulerAngles;
                currRot.y += 180 % 360;

                transform.eulerAngles = currRot;
            }

            Vector2 myVel = myBody.velocity;
            myVel.x = -transform.right.x * speed;
            myBody.velocity = myVel;

            
            Vector2 heading = playerLocation.position.toVector2() - transform.position.toVector2();
            float xDistance = heading.magnitude;
            float yDistance = Mathf.Abs(playerLocation.position.toVector2().y - transform.position.toVector2().y);
            
            if (xDistance <= attackRange && yDistance < 0.1f)
            {
                shooting = true;
            }

        }
        else
        {
            sr.sprite = enemyShootingSprite;
            float myXposition = transform.position.x;
            float playerXposition = playerLocation.transform.position.x;
            //Debug.Log(myXposition + " " + playerXposition);
            if((myXposition - playerXposition) > 0)//right
            {
                Vector3 currRot = transform.eulerAngles;
                currRot.y = 0;

                transform.eulerAngles = currRot;
            }
            else if((myXposition - playerXposition) < 0)//left
            {
                Vector3 currRot = transform.eulerAngles;
                currRot.y = 180;

                transform.eulerAngles = currRot;
            }

            Fire();

            Vector2 heading = playerLocation.position.toVector2() - transform.position.toVector2();
            float distance = heading.magnitude;

            if (distance > attackRange)
            {
                shooting = false;
            }
        }
    }
    void Fire()
    {
        if (Time.time > attackInterval + lastShot)
        {
            GameObject enemyProjectile = (GameObject)Instantiate(enemyProjectilePrefab, enemyProjectileSpawn.position, enemyProjectileSpawn.rotation);
            enemyProjectile.GetComponent<Rigidbody2D>().velocity = -enemyProjectile.transform.right * enemyProjectileSpeed;


            Destroy(enemyProjectile, 3f);

            lastShot = Time.time;
        }
    }
}
