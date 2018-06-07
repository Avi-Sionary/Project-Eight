using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public int playerHealth = 4;

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

    public Sprite playerStandSprite;
    public Sprite playerShootSprite;
    public Sprite playerJumpSprite;

    public LayerMask groundLayer;

    //Debug
    public Text horizontalAxis;
    public Text verticalAxis;
    public Text velocityX;
    public Text velocityY;

    float lastShot = 0;
    Rigidbody2D rb;
    Vector2 Velocity;
    Sprite currentSprite;
    SpriteRenderer sr;
    bool shooting;


    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 0.2f;

        Debug.DrawRay(position, direction, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            //Debug.Log("IsGrounded: " + hit.collider.name);
            
            return true;
        }

        return false;
    }

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        shooting = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (playerHealth <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }



    }
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {
            shooting = true;
            if (IsGrounded())
            {
                sr.sprite = playerShootSprite;
            }
            Fire();

            Debug.Log("Fire1 Button was pressed.");

        }
        if (Input.GetButtonUp("Fire1"))
        {
            shooting = false;
        }
        if (IsGrounded())
        {
            if (shooting)
            {
                sr.sprite = playerShootSprite;
            }
            else
            {
                sr.sprite = playerStandSprite;
            }
        }
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));

        horizontalAxis.text =   "XAxis: " +         input.x.ToString();
        verticalAxis.text =     "YAxis: " +         input.y.ToString();
        velocityX.text =        "Velocity X: " +    rb.velocity.x.ToString();
        velocityY.text =        "Velocity Y: " +    rb.velocity.y.ToString();


        if (input.x > joystickThreshold && (rb.velocity.x < moveSpeed))
        {
            rb.velocity += Vector2.right * moveSpeed;
            if (transform.eulerAngles.y < 179)
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
            sr.sprite = playerJumpSprite;
            rb.velocity = Vector2.up * jumpVelocity;
            //Debug.Log("Jump button was pressed.");
        }

        //Jumping (A on xBox)

        if (Input.GetButtonUp("Jump"))
        {
            //Debug.Log("Jump button was released.");
        }

        //Dashing (RB on xBox)

        if (dashCount < 3.0f)
        {
            dashCount = dashCount + dashIncrementPerFrame;
        }

        if (Input.GetButtonDown("Dash") && dashCount >= 1)
        {
            --dashCount;
            //Debug.Log("Dash button was pressed.");
            if (input.x > joystickThreshold)
            {
                if (input.y > joystickThreshold) //northeast; 9
                {
                    Debug.Log("Dash: 9");
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), Mathf.Sqrt(2 * dashDistance)));
                }
                else if (input.y < -joystickThreshold) //southeast; 3
                {
                    Debug.Log("Dash: 3");
                    transform.Translate(new Vector3(Mathf.Sqrt(2 * dashDistance), -Mathf.Sqrt(2 * dashDistance)));
                }
                else //east; 6
                {
                    Debug.Log("Dash: 6");
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), 0));
                }
            }
            else if (input.x < -joystickThreshold)
            {
                if (input.y > joystickThreshold) //northwest; 7
                {
                    Debug.Log("Dash: 7");
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), Mathf.Sqrt(2 * dashDistance)));
                }
                else if (input.y < -joystickThreshold) //southwest; 1
                {
                    Debug.Log("Dash: 1");
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), -Mathf.Sqrt(2 * dashDistance)));
                }
                else //west; 4
                {
                    Debug.Log("Dash: 4");
                    transform.Translate(new Vector3(-Mathf.Sqrt(2 * dashDistance), 0));
                }
            }
            else if (input.y == 1) //north; 8
            {
                Debug.Log("Dash: 8");
                transform.Translate(new Vector3(0, Mathf.Sqrt(2 * dashDistance)));
            }
            else if (input.y == -1) //south; 2
            {
                Debug.Log("Dash: 2");
                transform.Translate(new Vector3(0, -Mathf.Sqrt(2 * dashDistance)));
            }
            rb.velocity = new Vector2(0, 0);
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        //rb.velocity = Velocity;
        
    }

    void Fire()
    {
        if (Time.time > attackInterval + lastShot)
        {
            GameObject playerProjectile = (GameObject)Instantiate(playerProjectilePrefab, playerProjectileSpawn.position, playerProjectileSpawn.rotation);

            AudioSource fire = GetComponent<AudioSource>();

            playerProjectile.GetComponent<Rigidbody2D>().velocity = -playerProjectile.transform.right * playerProjectileSpeed;

            fire.Play();

            Destroy(playerProjectile, 3f);

            lastShot = Time.time;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Projectile" | collision.tag == "Enemy")
        {
            
            --playerHealth;
            print(playerHealth);
            if (playerHealth <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
