using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public float moveSpeed;
    public float jumpVelocity;
    public float fallMultiplier;
    public float lowJumpMultiplier;
    public GameObject playerProjectilePrefab;
    public float playerProjectileSpeed;
    public Transform playerProjectileSpawn;
    public float attackInterval;

    public float joystickThreshold = 0.2f;

    public float dashCount = 3f;
    public float dashDistance;
    public float dashIncrementPerFrame;

    public LayerMask groundLayer;

    //Debug
    public Text horizontalAxis;
    public Text verticalAxis;
    public Text velocityX;
    public Text velocityY;

    float lastShot = 0;
    Rigidbody2D rb;
    Vector2 Velocity;

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.2f;

        Debug.DrawRay(position, direction, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            Debug.Log("IsGrounded: " + hit.collider.name);
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update () {

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));

        horizontalAxis.text = "XAxis: " + input.x.ToString();
        verticalAxis.text = "YAxis: " + input.y.ToString();
        velocityX.text = "Velocity X: " + rb.velocity.x.ToString();
        velocityY.text = "Velocity Y: " + rb.velocity.y.ToString();

        if (input.x > joystickThreshold && (rb.velocity.x < moveSpeed))
        {
            rb.velocity += Vector2.right * moveSpeed;
            if(transform.eulerAngles.y < 179)
            {
                Vector3 currRot = transform.eulerAngles;
                currRot.y += 180 % 360;

                transform.eulerAngles = currRot;
            }
        }

        if (input.x < -joystickThreshold && (rb.velocity.x > -moveSpeed))
        {
            rb.velocity += Vector2.left * moveSpeed;
            if (transform.eulerAngles.y > 179 && transform.eulerAngles.y < 181)
            {
                Vector3 currRot = transform.eulerAngles;
                currRot.y += 180 % 360;

                transform.eulerAngles = currRot;
            }
        }

        if (input.x < joystickThreshold && input.x > -joystickThreshold)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //Velocity.y = jumpVelocity;
            rb.velocity = Vector2.up * jumpVelocity;
            Debug.Log("Jump button was pressed.");
        }

        //Jumping (A on xBox)

        if (Input.GetButtonUp("Jump"))
        {
            Debug.Log("Jump button was released.");
        }

        //Dashing (RB on xBox)

        if (dashCount < 3.0f)
        {
            dashCount = dashCount + dashIncrementPerFrame;
        }

        if (Input.GetButtonDown("Dash") && dashCount >= 1)
        {
            --dashCount;
            Debug.Log("Dash button was pressed.");
            if (input.x > joystickThreshold)
            {
                if (input.y > joystickThreshold) //northeast
                {
                    transform.Translate(new Vector3(Mathf.Sqrt(2 * dashDistance), Mathf.Sqrt(2 * dashDistance)));
                }
                else if (input.y < -joystickThreshold) //southeast
                {
                    transform.Translate(new Vector3(Mathf.Sqrt(2 * dashDistance), -Mathf.Sqrt(2 * dashDistance)));
                }
                else //east
                {
                    transform.Translate(new Vector3(Mathf.Sqrt(2 * dashDistance), 0));
                }
            }
            else if (input.x < -joystickThreshold)
            {
                if (input.y > joystickThreshold) //northwest
                {
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), Mathf.Sqrt(2 * dashDistance)));
                }
                else if (input.y < -joystickThreshold) //southwest
                {
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), -Mathf.Sqrt(2 * dashDistance)));
                }
                else //west
                {
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), 0));
                }
            }
            else if (input.y == 1) //north
            {
                transform.Translate(new Vector3(0, Mathf.Sqrt(2 * dashDistance)));
            }
            else if (input.y == -1) //south
            {
                transform.Translate(new Vector3(0, -Mathf.Sqrt(2 * dashDistance)));
            }
            rb.velocity = new Vector2 (0,0);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //rb.velocity = Velocity;

        
    }
    void FixedUpdate()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1 Button was pressed.");
            Fire();

        }
    }

    void Fire()
    {
        if (Time.time > attackInterval + lastShot)
        {
            GameObject playerProjectile = (GameObject)Instantiate(playerProjectilePrefab, playerProjectileSpawn.position, playerProjectileSpawn.rotation);

            if (transform.eulerAngles.y > 179 && transform.eulerAngles.y < 181)
            {
                playerProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(playerProjectileSpeed, 0));
            }
            else
            {
                playerProjectile.GetComponent<Rigidbody2D>().AddForce(new Vector2(-playerProjectileSpeed, 0));
            }

            Destroy(playerProjectile, 3f);

            lastShot = Time.time;
        }
    }
}
