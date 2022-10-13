using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    private GameObject player;
    private float attackRange = 0.8f;
    public LayerMask whatIsEnemy;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsSnake;
    public Animator stickAnimator;
    private bool active = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        bool onEnemy = Physics2D.OverlapCircle(transform.position, attackRange, whatIsEnemy);
        bool onPlayer = Physics2D.OverlapCircle(transform.position, 0.6f, whatIsPlayer);
        Collider2D onSnake = Physics2D.OverlapCircle(transform.position, attackRange, whatIsSnake);

        if (active && onEnemy && GetComponent<Rigidbody2D>().velocity.magnitude > 20)
        {
            Collider2D enemiesToDamage = Physics2D.OverlapCircle(transform.position, attackRange, whatIsEnemy);
            enemiesToDamage.GetComponent<EnemyController>().Damage(1);
            active = false;
        }
        if (active && onSnake && GetComponent<Rigidbody2D>().velocity.magnitude > 20)
        {
            onSnake.GetComponent<SnakeController>().Damage(1);
        }
        if (active && onPlayer && GetComponent<Rigidbody2D>().velocity.magnitude > 15f)
        {
            player.GetComponent<PlayerController>().Damage(1);
        }
        bool pick = Physics2D.OverlapCircle(transform.position, 2f, whatIsPlayer);
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (pick)
            {
                player.GetComponent<PlayerController>().PickUp();
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
}
