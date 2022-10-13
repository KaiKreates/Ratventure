using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class Hook : MonoBehaviour
{
    public Light2D shine;
    private float cooldown = 0f;
    private bool shining;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldown += 0.01f;
        if(cooldown > 6.28)
        {
            cooldown = 0;
        }
        shine.intensity = 3f * Mathf.Abs(Mathf.Sin(cooldown));
    }
}
