using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hud : MonoBehaviour
{
    public TextMeshProUGUI health;
    public GameObject player;
    void Update()
    {
        health.text = player.GetComponent<PlayerController>().health.ToString();
    }
}
