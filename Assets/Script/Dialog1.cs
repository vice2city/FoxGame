using System;
using UnityEngine;

public class Dialog1 : MonoBehaviour
{
    public GameObject dialog1;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            dialog1.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        dialog1.SetActive(false);
    }
}
