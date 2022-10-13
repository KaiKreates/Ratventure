using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    public GameObject player;
    private Rigidbody2D rb;
    public Texture grapple;
    public DistanceJoint2D dj;
    public Transform hookCheck;
    public Transform parasite;
    
    public bool grounded;
    private bool pullable;
    
    Vector2 pull;

    public LayerMask whatIsGrappleable;
/*    public VirtualButton lungeButton;*/

    private void Start() {
        dj.enabled = false;
        rb = player.GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        
        
    }
    private void Update() {

        Collider2D hook = Physics2D.OverlapCircle(hookCheck.position, 10, whatIsGrappleable);
        lr.material.SetTexture("_MainTex", grapple);
        grounded = player.GetComponent<PlayerController>().grounded;
        

        if (hook == true){
            pull = hook.transform.position - player.transform.position;
            if (pullable)
            {
                if (Input.GetKeyDown(KeyCode.X))
                {
                    lr.SetPosition(1, parasite.position);
                    lr.SetPosition(0, hook.transform.position);
                    dj.connectedAnchor = hook.transform.position;
                    dj.enabled = true;
                    lr.enabled = true;
                    rb.AddForce(new Vector2(pull.normalized.x * 200, pull.normalized.y * 100));
                    pullable = false;
                    player.GetComponent<PlayerController>().health -= 0.5f;
                }
            }
        }
        
        if (Input.GetKeyUp(KeyCode.X)){
            dj.enabled = false;
            lr.enabled = false;
        }

        if (!hook)
            lr.enabled = false;

        if(grounded == true)
            pullable = true;

        if(dj.enabled)
            lr.SetPosition(1, parasite.position);      

        
    }
    
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hookCheck.position, 10);
    }
}
