using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonController : MonoBehaviour
{
    float spawntime;
    public float cooldown;
    void Start()
    {
        spawntime = Time.time;
    }

    void Update()
    {
        if(spawntime + cooldown < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
