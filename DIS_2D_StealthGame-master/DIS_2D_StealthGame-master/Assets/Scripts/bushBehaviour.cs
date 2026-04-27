using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bushBehaviour : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite[] sprites;
    private SpriteRenderer spriteR;

    void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            spriteR.sprite = sprites[0];
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            spriteR.sprite = sprites[1];
        }
    }
}
