using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggers : MonoBehaviour
{
    public GameObject snake;
    public GameObject controlDialog;
    public GameObject endTimeline;
    public GameObject audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "SnakeTrigger")
        {
            snake.SetActive(true);
        }
        if (collision.gameObject.name == "ControlTrigger")
        {
            controlDialog.SetActive(true);
        }
        if (collision.gameObject.name == "EndTrigger")
        {
            endTimeline.SetActive(true);
            GetComponent<PlayerController>().enabled = false;
            audioManager.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "ControlTrigger")
        {
            controlDialog.SetActive(false);
        }
    }
}
