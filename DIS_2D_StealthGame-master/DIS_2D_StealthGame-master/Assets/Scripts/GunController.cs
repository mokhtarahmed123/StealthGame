using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Meant to control Enemy Shooting
public class GunController : MonoBehaviour
{
    // Start is called before the first frame update
    private RayEnemyDetect red;
    public GameObject gun;
    private Rigidbody2D rb;

    void Start()
    {
        red = gameObject.GetComponent<RayEnemyDetect>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.magnitude > 0)
        {
            //gun.transform.up = lastMove;
        }
    }
}
