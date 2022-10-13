using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    private float health;
    public float attackRange;
    private float dir;
    public float hurtbox;
    public float detectCollision;
    public float dropLength;
    
    private LineRenderer lr;
    private GameObject player;
    public GameObject food;
    public GameObject bloodParticles;
    public LayerMask ground;
    public LayerMask enemy;
    public LayerMask playerLayer;
    public Transform groundCheck;
    public Transform enemyCheck;
    
    private bool faceRight = true;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");

        if(gameObject.tag == "Spider"){
            lr = GetComponent<LineRenderer>();
        }
        health = 2;
    }

    private void Update() {

        bool grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        bool meet = Physics2D.OverlapCircle(enemyCheck.position, detectCollision, enemy);
        bool attack = Physics2D.OverlapCircle(transform.position, attackRange, playerLayer);
        bool damagePlayer = Physics2D.OverlapCircle(transform.position, hurtbox, playerLayer);

        if(gameObject.tag == "Worm")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(speed , GetComponent<Rigidbody2D>().velocity.y);

            if(grounded == false || meet == true){
                speed = -speed;
            }
            if(faceRight == false && speed > 0){
                Flip();
            }
            else if(faceRight == true && speed < 0){
                Flip();
            }

        }

        if(gameObject.tag == "Wasp"){

            dir = player.transform.position.x - transform.position.x;

            if(attack == true){
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed *Time.deltaTime);
            }
            if(faceRight == false && dir> 0){
                Flip();
            }
            else if(faceRight == true && dir < 0){
                Flip();
            }

        }

        if(gameObject.tag == "Spider"){
            
            bool drop = Physics2D.OverlapBox(transform.position, new Vector2(1.5f, dropLength), 0f, playerLayer);
            if(drop == true){
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, -speed);
            }
            /*if(meet == true){
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 10);

            }*/
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.up);
            if(hit){
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, hit.point);
            }
        }

        if(health <= 0){
            if (gameObject.tag == "Worm")
            {
                Instantiate(food, transform.position, Quaternion.identity);
            }
            Destroy(this.gameObject);
        }

        if(damagePlayer == true){
            player.GetComponent<PlayerController>().Damage(1);
        }
    }

    public void Flip()
    {
        faceRight = !faceRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    public void Damage(int damage){
        health -= damage;
        if(gameObject.tag == "Worm"){
            GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x, 10);
        }
        if(gameObject.tag == "Spider")
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(-GetComponent<Rigidbody2D>().velocity.x, 10);
            
        }
        Instantiate(bloodParticles, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, hurtbox);
        Gizmos.DrawWireCube(transform.position, new Vector2(1.5f, dropLength));
    }
}