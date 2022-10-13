using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public LayerMask whatIsPlayer;

    void Start()
    {
        
    }

    
    void Update()
    {
        Collider2D damage = Physics2D.OverlapCircle(transform.position, 1, whatIsPlayer);
        if(damage == true){
            damage.GetComponent<PlayerController>().Damage(1);
        }
    }

    public void Break(){
        Destroy(this.gameObject);
    }
}
