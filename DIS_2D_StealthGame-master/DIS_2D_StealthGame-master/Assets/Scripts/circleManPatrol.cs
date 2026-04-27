using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class circleManPatrol : MonoBehaviour
{

    public float circleRadius;
    public float patrolSpeed;
    public bool clockwise;

    private float timeCounter;
    private Vector2 origin;
    private float originX;
    private float originY;
    

    //public Animator Animator;
    private bool playerMoving;

    private float lastX;
    private float lastY;
    private bool alertState;


    private Vector2 prevPos;

    private bool turning;


    Rigidbody2D rb;
    //BoxCollider2D collision;


    // Start is called before the first frame update
    void Start()
    {
        turning = false;
        alertState = false;

        prevPos = transform.position;
        origin = transform.position;
        originX = origin.x + circleRadius;
        originY = origin.y + circleRadius;

        timeCounter = 0;

        rb = GetComponent<Rigidbody2D>();
        //collision = GetComponent<BoxCollider2D>();




        if (clockwise)
        {
            patrolSpeed = -patrolSpeed;
        }
        //Animator.SetBool("PlayerMoving", true);
        lastX = transform.position.x;
        lastY = transform.position.y;

    }

    // Update is called once per frame
    void Update()
    {

        if (!PauseMenu.GameIsPaused)
        {
            if (Time.deltaTime != 0)
            {
                timeCounter += Time.deltaTime * patrolSpeed;
            }
            else
            {
                timeCounter += patrolSpeed;
            }
            float x = -Mathf.Cos(timeCounter) * circleRadius + originX;
            float y = -Mathf.Sin(timeCounter) * circleRadius + originY;





            transform.position = new Vector2(x, y);

            transform.right = (Vector2)transform.position - prevPos;



            float cos = -Mathf.Cos(timeCounter);
            float sin = -Mathf.Sin(timeCounter);

            prevPos = transform.position;

        }

    }

    public void setAlertState(bool alert)
    {
        alertState = alert;
    }
    public bool getAlertState()
    {
        return alertState;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.tag == "wall") || (collision.gameObject.tag == "Wall"))
        {
            StartCoroutine(turnAround());
        }
    }

    IEnumerator turnAround()
    {
        if (!turning)
        {
            print("turning");
            turning = true;
            patrolSpeed = -patrolSpeed;
            clockwise = !clockwise;
            yield return new WaitForSeconds(2f);
            turning = false;
        }
        
    }


}
