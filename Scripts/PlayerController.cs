using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    public float health = 5;
    public float speed;
    public float jumpSpeed;
    float moveVelocity;
    public float dirx;
    public float diry;
    public float drag;
    public bool grounded;
    public bool wallJump;
    public bool clipping;
    bool faceRight = true;
    public bool vulnerable = true;
    private float vulCooldown;
    public float energy = 1f;
    private Vector2 lastGroundPos;
    
    public Texture grapple;
    public LineRenderer lr;
    public GameObject parasite;
    private GameObject startThrow;
    public GameObject stick;
    public GameObject throwStick;
    private GameObject thrown;
    public GameObject particles;
    private GameObject dust;
    public Transform head;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask whatIsStick;
    public LayerMask whatIsGround;
    public LayerMask whatIsClipping;
    Vector2 move;

    public GameObject gameOver;
    public GameObject pauseMenu;
    public GameObject hud;

/*    public VirtualDPad dpad;
    public VirtualButton jumpButton;
    public VirtualButton throwButton;
    public VirtualButton pullButton;
    public VirtualButton pauseButton;*/

    private void Start()
    {
        FindObjectOfType<AudioManager>().Play("Theme");
    }
    void Update()
    {
        
        clipping = Physics2D.OverlapCircle(head.position, 0.5f, whatIsClipping);
        lr.material.SetTexture("_MainTex", grapple);
        thrown = GameObject.FindGameObjectWithTag("Stick");

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (grounded)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;
                animator.Play("Rat_jumpingup");
            }
            if (wallJump && dirx != 0)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpSpeed;
            }
        }

        if (grounded)
        {
            animator.SetBool("Jump", false);
            lastGroundPos = transform.position;
            if (clipping)
            {
                if (faceRight)
                {
                    moveVelocity = -speed * 3;
                }
                else
                {
                    moveVelocity = speed * 3;
                }
            }

            if(GetComponent<Rigidbody2D>().velocity.y > 1 || GetComponent<Rigidbody2D>().velocity.y < -1)
            {
                dust = Instantiate(particles, transform.position, Quaternion.identity);
            }
        } else
        if(!grounded){
            animator.SetBool("Jump", true);
            if (GetComponent<Rigidbody2D>().velocity.y > 0)
            {
                animator.Play("Rat_jumpingup");
            }
            else if (GetComponent<Rigidbody2D>().velocity.y < 0)
            {
                animator.Play("Rat_falling");
            }
        }


        if(vulCooldown < Time.time)
        {
            vulnerable = true;
        }

        if(energy <= 0)
        {
            health -= 0.5f;
            energy = 1f;
        }

        if(health <= 0)
        {
            gameOver.SetActive(true);
            hud.SetActive(false);
            Time.timeScale = 0;
        }
        else
        {
            gameOver.SetActive(false);
            hud.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (stick.activeSelf && !stick.GetComponent<Attack>().clipping) {
                stick.SetActive(false);
                startThrow = Instantiate(throwStick, stick.transform.position, Quaternion.identity);
                startThrow.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Clamp(dirx * 30, -30f, 30f), Mathf.Clamp(diry * 30, -30f, 30f));
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    void FixedUpdate()
    {
        diry = Input.GetAxis("Vertical");
        dirx = Input.GetAxis("Horizontal");
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.6f, whatIsGround);
        wallJump = Physics2D.OverlapCircle(wallCheck.position, 0.1f, whatIsGround);
        
        if (!clipping)
        {
            moveVelocity = 0;
        }

        if (Input.GetKey (KeyCode.LeftArrow) && !wallJump && !Input.GetKey (KeyCode.RightArrow)) 
        {
            moveVelocity = -speed;
        }
        if (Input.GetKey (KeyCode.RightArrow) && !wallJump && !Input.GetKey (KeyCode.LeftArrow)) 
        {
            moveVelocity = speed;
        }
        /*if(dpad.handle.anchoredPosition.x > 0)
        {
            moveVelocity = speed;
            dirx = 1;
        }
        if(dpad.handle.anchoredPosition.x < 0)
        {
            moveVelocity = -speed;
            dirx = -1;
        }*/
        if(moveVelocity != 0 && grounded)
        {
            animator.SetBool("Running", true);
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
            head.gameObject.SetActive(false);
        }
        else
        {
            animator.SetBool("Running", false);
            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
            head.gameObject.SetActive(true);
        }

        GetComponent<Rigidbody2D>().velocity = new Vector2 (moveVelocity, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().AddForce(new Vector2(-dirx * drag, 0));
        
        if(!faceRight && dirx > 0)
        {
            Flip();
        }
        else if(faceRight && dirx < 0)
        {
            Flip();
        }
        ThrowStickControl();
    }

    public void Flip()
    {
        faceRight = !faceRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void PickUp()
    {
        stick.SetActive(true);
        Destroy(startThrow);
    }

    public void Damage(int damage)
    {
        if (vulnerable)
        {
            vulnerable = false;
            vulCooldown = Time.time + 3;
            health -= damage;
            GetComponent<Rigidbody2D>().velocity = new Vector2(-500, 20);
            FindObjectOfType<AudioManager>().Play("Hurt");
        }   
    }
    private void ThrowStickControl()
    {
        if (thrown)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Z))
            {
                lr.SetPosition(0, parasite.transform.position);
                lr.SetPosition(1, thrown.transform.position);
                lr.enabled = true;
                thrown.GetComponent<Rigidbody2D>().AddForce(new Vector2(parasite.transform.position.x - thrown.transform.position.x, parasite.transform.position.y - thrown.transform.position.y).normalized * 0.05f);
                health -= 0.3f * Time.deltaTime;
            }
            else
            {
                lr.enabled = false;
            }
            thrown.GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(thrown.GetComponent<Rigidbody2D>().velocity, 30f);
        }
        else
        {
            lr.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.6f);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Food" && health < 10)
        {
            health += 1;
            FindObjectOfType<AudioManager>().Play("Food");
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Base")
        {
            transform.position = lastGroundPos;
            health -= 1;
        }
        if(collision.gameObject.tag == "Snake")
        {
            Damage(1);
        }
        if(collision.gameObject.tag == "Poison")
        {
            Damage(1);
        }
    }
}