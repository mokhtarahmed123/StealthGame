using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunItem : MonoBehaviour
{
    // Start is called before the first frame update
    public int bulletCount;
    private bool triggered;
    private GameObject player;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            player = col.gameObject;
            triggered = true;
            Debug.Log("triggered");
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            triggered = false;
        }
    }


    private void Update()
    {
        if (triggered && Input.GetKeyDown(KeyCode.E))
        {
            player.GetComponent<PlayerController>().gunActive = true;
            player.GetComponent<PlayerController>().knifeActive = false;
            Destroy(gameObject);
            GameObject gun = player.GetComponent<PlayerController>().gun;
            gun.GetComponent<Gun>().bulletCount = bulletCount;
            gun.GetComponent<Gun>().isPlayer = true;

        }
    }
}
