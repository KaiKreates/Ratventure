using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float startBtw = 0.8f;
    public float btw = 0; 
    public float attackRange;

    private GameObject player;
    public GameObject leaves;
    public Transform attackPos;
    public LayerMask whatIsGround;
    public LayerMask whatIsEnemy;
    public LayerMask environment;
    public LayerMask whatIsSwing;
    public LayerMask whatIsSnake;
    public Animator stickAnimator;
    public bool clipping;

    //public VirtualButton attackButton;
   
    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update() 
    {
        clipping = Physics2D.OverlapBox(attackPos.position, new Vector2(1.3f, 0.5f), 0f, whatIsGround);
        if(btw <= 0)
        {
            if(Input.GetKey(KeyCode.C)){
                stickAnimator.Play("Attack_melee");
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                Collider2D vine = Physics2D.OverlapCircle(attackPos.position, attackRange, environment);
                Collider2D rope = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsSwing);
                Collider2D snake = Physics2D.OverlapCircle(attackPos.position, attackRange, whatIsSnake);
                
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<EnemyController>().Damage(1);
                }

                if (snake)
                {
                    snake.GetComponent<SnakeController>().Damage(1);
                }

                if(vine)
                {
                    vine.GetComponent<Vine>().Break();
                    Instantiate(leaves, transform.position, Quaternion.identity); 
                }
                
                if(rope)
                {
                    rope.GetComponentInParent<Swing>().dj.enabled = false;
                    Destroy(rope.gameObject);
                    Instantiate(leaves, transform.position, Quaternion.identity);
                }
                btw = startBtw;

                FindObjectOfType<AudioManager>().Play("Attack");
            }
        }
        else
        {
            btw -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attackPos.position, new Vector2(1.3f, 0.5f));
    }
}