using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Patroltype2 : MonoBehaviour
{
    [Tooltip("Float defines detection distance in a little formula guy, Must be >0.5")]
    public float walldetectdistance = 1;
    [Tooltip("Speed of enemy")]
    public float speed = 3;
    [Tooltip("Sprites for enemy directions, order is right, up, left, down")]
    public Sprite[] orientationsprites;
    [Tooltip("Boolean for reverse of default order, default order is right, up, left, down")]
    public bool reverse = false;

    // Right, Up, Left, Down is the order for normal movement
    private enum Direction { Right = 0, Left = 1, Up = 2, Down = 3 }
    private int currentdir;
    private int nextdir;
    Rigidbody2D rb;
    SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentdir = (int)Direction.Up;
        nextdir = (int)Direction.Left;

        Vector2 velocity = rb.linearVelocity;
        velocity.x += speed;
        rb.linearVelocity = velocity;
    }

    // Update is called once per frame
    void Update()
    {
        Thread.Sleep(250);
        bool hitwall = WallCheck();
        if (hitwall)
        {
            print("Wall Hit Complete");
        }
    }

    // Checks if velocity needs updating based on current direction and checks if a wall has been hit through a raycast
    private bool WallCheck()
    {

        rb.linearVelocity = CheckVelocity(currentdir);

        Vector3 helpervector = (Mathf.Abs(rb.linearVelocity.x) > Mathf.Abs(rb.linearVelocity.y)) ? new Vector3(transform.position.x, transform.position.y + (rb.linearVelocity.x / rb.linearVelocity.x) * walldetectdistance, 0) :
                                                                                       new Vector3(transform.position.x + (rb.linearVelocity.y / rb.linearVelocity.y) * walldetectdistance, transform.position.y, 0);
        if (rb.linearVelocity.x < 0)
        {
            helpervector.y += -2 * walldetectdistance;
        }
        if (rb.linearVelocity.y < 0)
        {
            helpervector.x += -2 * walldetectdistance;
        }

        Vector3 raystart = helpervector;

        Vector2 direction = transform.position - helpervector;
        direction.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(raystart, direction, walldetectdistance);
        Debug.DrawRay(raystart, direction, Color.green);

       if (hit.collider == null || !hit.collider.CompareTag("wall"))
        {
            currentdir = nextdir;
            UpdateSprite();
            return true;
        }


        return false;
    }

    // Checks if velocity needs updating, if it does then updates velocity, current direction and next direction
    private Vector2 CheckVelocity(int curdir)
    {
        Vector2 velocity = rb.linearVelocity;
        switch (curdir)
        {
            case ((int)Direction.Right):
                velocity = new Vector2(speed, 0);
                currentdir = (int)Direction.Right;
                nextdir = !this.reverse ? (int)Direction.Up : (int)Direction.Down;
                break;
            case ((int)Direction.Left):
                velocity = new Vector2(-speed, 0);
                currentdir = (int)Direction.Left;
                nextdir = !this.reverse ? (int)Direction.Down : (int)Direction.Up;
                break;
            case ((int)Direction.Up):
                velocity = new Vector2(0, speed);
                currentdir = (int)Direction.Up;
                nextdir = !this.reverse ? (int)Direction.Left : (int)Direction.Right;
                break;
            case ((int)Direction.Down):
                velocity = new Vector2(0, -speed);
                currentdir = (int)Direction.Down;
                nextdir = !this.reverse ? (int)Direction.Right : (int)Direction.Left;
                break;
            default:
                velocity = new Vector2(speed, 0);
                currentdir = (int)Direction.Right;
                nextdir = !this.reverse ? (int)Direction.Up : (int)Direction.Down;
                break;
        }

        return velocity;
    }

    //Updates the sprite being rendered based on current velocity direction 
    private void UpdateSprite()
    {
        //switch (currentdir)
        //{
        //    case ((int)Direction.Right):
        //        sr.sprite = orientationsprites[0];
        //        break;
        //    case ((int)Direction.Left):
        //        sr.sprite = orientationsprites[1];
        //        break;
        //    case ((int)Direction.Up):
        //        sr.sprite = orientationsprites[2];
        //        break;
        //    case ((int)Direction.Down):
        //        sr.sprite = orientationsprites[3];
        //        break;
        //    default:
        //        sr.sprite = orientationsprites[0];
        //        break;
        //}
    }

    //Changes rotation based on current velocity, not really being used atm
    private void ChangeDirection()
    {
        Vector2 moveDirection = rb.linearVelocity;
        if (moveDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
