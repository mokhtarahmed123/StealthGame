using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class enemyDetection : MonoBehaviour
{
    //I saw some stuff online about overlapping circles and then running through an array to check for each
    //different thing that is being collided with, but it seems really complicated, might want to implement later though (time dependent)

    private bool seePlayer;

    PolygonCollider2D siteLine;

    // Start is called before the first frame update
    void Start()
    {
        seePlayer = false;
        siteLine = GetComponent<PolygonCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (seePlayer)
        {
            seePlayer = !seePlayer;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            print("GAME OVER. Run Some Function from here for game over");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            seePlayer = true;
        }
    }


    public void turnRight()
    {
        Quaternion rotation = gameObject.transform.rotation;
        rotation.z += 45;
        gameObject.transform.rotation = rotation;
    }

    public void turnLeft()
    {
        Quaternion rotation = gameObject.transform.rotation;
        rotation.z -= 45;
        gameObject.transform.rotation = rotation;
    }

}
