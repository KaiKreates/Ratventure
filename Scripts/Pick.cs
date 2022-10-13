using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    public LayerMask playerLayer;
    public GameObject stick;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool range = Physics2D.OverlapCircle(transform.position, 2f, playerLayer);
        if(range == true){
            stick.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
