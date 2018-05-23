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

    float lastShot = 0;
    Transform playerLocation; 
    bool shooting;
    Rigidbody2D myBody;
    Transform myTrans;
    float myWidth, myHeight;
    SpriteRenderer mySprite;
    // Use this for initialization
    void Start () {
        playerLocation = GameObject.FindObjectOfType<Player>().transform;
        shooting = false;
        myBody = GetComponent<Rigidbody2D>();
        myTrans = transform;
        mySprite = this.GetComponent<SpriteRenderer>();
        myWidth = mySprite.bounds.extents.x;
        myHeight = mySprite.bounds.extents.y;
    }
	
    void FixedUpdate()
    {
        if (!shooting)
        {
            Vector2 lineCastPos = myTrans.position.toVector2() - myTrans.right.toVector2() * myWidth + Vector2.up * myHeight;

            Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
            bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, rangedEnemyMask);

            Debug.DrawLine(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f);
            bool isBlocked = Physics2D.Linecast(lineCastPos, lineCastPos - myTrans.right.toVector2() * .05f, rangedEnemyMask);

            if (!isGrounded || isBlocked)
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y += 180 % 360;

                myTrans.eulerAngles = currRot;
            }

            Vector2 myVel = myBody.velocity;
            myVel.x = -myTrans.right.x * speed;
            myBody.velocity = myVel;

            Vector2 heading = playerLocation.position.toVector2() - myTrans.position.toVector2();
            float distance = heading.magnitude;
            
            if (distance <= attackRange)
            {
                shooting = true;
            }

        }
        else
        {
            float myXposition = myTrans.position.x;
            float playerXposition = playerLocation.transform.position.x;
            //Debug.Log(myXposition + " " + playerXposition);
            if((myXposition - playerXposition) > 0)//right
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y = 0;

                myTrans.eulerAngles = currRot;
            }
            else if((myXposition - playerXposition) < 0)//left
            {
                Vector3 currRot = myTrans.eulerAngles;
                currRot.y = 180;

                myTrans.eulerAngles = currRot;
            }

            Fire();

            Vector2 heading = playerLocation.position.toVector2() - myTrans.position.toVector2();
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
