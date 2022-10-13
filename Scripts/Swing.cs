using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    public Transform platform;
    public Vector2 ceiling;
    public float height;
    public DistanceJoint2D dj;
    public LineRenderer lr;
    public Texture rope;
    Vector2[] col = new Vector2[2];
    EdgeCollider2D edgeCollider;
    void Awake()
    {
        ceiling.y = platform.position.y + height;
        ceiling.x = platform.position.x;
        edgeCollider = GetComponent<EdgeCollider2D>();
    }


    void Update()
    {
        lr.material.SetTexture("_MainTex", rope);
        lr.SetPosition(1, ceiling);
        lr.SetPosition(0, platform.position);
        dj.connectedAnchor = ceiling;

        col[0] = transform.InverseTransformPoint(platform.position);
        col[1] = transform.InverseTransformPoint(ceiling);
        edgeCollider.points = col;
    }
}