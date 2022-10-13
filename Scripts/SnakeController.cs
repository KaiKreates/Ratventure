using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public bool playerInRange;
    private bool faceRight = false;
    bool wallCheck;
    public bool groundCheck;
    public bool canJump;
    
    [SerializeField]
    private int random;
    private int dashRandom;
    private float switchTime;
    private float jumpTime;
    public int snakeHp = 10;
    [SerializeField]
    int spitting;
    float speed = 10f;

    private Rigidbody2D rb;
    public GameObject player;
    public GameObject tail;
    public GameObject poisonPrefab;
    public GameObject bloodParticles;
    public Transform face;
    public Transform center;
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsPlatform;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        wallCheck = Physics2D.OverlapCircle(face.position, 0.2f, whatIsGround);
        groundCheck = Physics2D.OverlapCircle(tail.transform.position, 0.1f, whatIsGround);
        playerInRange = Physics2D.OverlapCircle(face.position, 4f, whatIsPlayer);
        
        bool platformInRange = Physics2D.OverlapBox(center.position, new Vector2(6, 0.4f), whatIsPlatform);
        if (platformInRange) snakeHp = 0;

        if (!faceRight && speed > 0f)
        {
            Flip();
        }
        else if (faceRight && speed < 0f)
        {
            Flip();
        }
        if (switchTime < Time.time)
        {
            switchTime = Time.time + Random.Range(3f, 4f);
            random = Random.Range(0, 5);
            dashRandom = Random.Range(0, 2);
            spitting = 0;
        }

        if(jumpTime < Time.time)
        {
            canJump = true;
        }
        /*if(canJump && playerInRange)
        {
            jumpTime = Time.time + 5f;
            canJump = false;
            Debug.Log("jomp");
        }*/
        if (random == 0 || random == 1)
        {
            Dash();
            GetComponent<Animator>().SetBool("Running", true);
            GetComponent<PolygonCollider2D>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = true;
        }
        else
        {
            GetComponent<Animator>().SetBool("Running", false);
            if (wallCheck)
            {
                Flip();
            }
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<CapsuleCollider2D>().enabled = false;
        }
        if (random == 3 || random == 2)
        {
            Spit();
        }

        if(face.position.x - tail.transform.position.x > 0)
        {
            faceRight = true;
        }
        else
        {
            faceRight = false;
        }
    }

    void Dash()
    {
        if (wallCheck)
        {
            speed *= -1;
            Flip();
        }
        rb.velocity = transform.right * speed;
        FindObjectOfType<AudioManager>().Play("Snake");
    }

    void Spit()
    {
        rb.velocity = new Vector2(0, 0);
        if (spitting <= 50)
        {
            if (player.transform.position.x - transform.position.x > 0f)
            {
                if (!faceRight)
                {
                    Flip();
                }
                GameObject poison = Instantiate(poisonPrefab, face.position, Quaternion.identity);
                poison.GetComponent<Rigidbody2D>().AddForce(new Vector2(2, 1) * 20f * Time.deltaTime);
                spitting += 1;
            }
            else if(player.transform.position.x - transform.position.x < 0f)
            {
                if (faceRight)
                {
                    Flip();
                }
                GameObject poison = Instantiate(poisonPrefab, face.position, Quaternion.identity);
                poison.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2, 1) * 20f * Time.deltaTime);
                spitting += 1;
            }
        }
    }

    public void Flip()
    {
        faceRight = !faceRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center.position, new Vector2(6, 0.4f));
    }

    public void Damage(int damage)
    {
        snakeHp -= damage;
        Instantiate(bloodParticles,new Vector2(transform.position.x, transform.position.y - 2), Quaternion.identity);

        Debug.Log(snakeHp);
    }
}
