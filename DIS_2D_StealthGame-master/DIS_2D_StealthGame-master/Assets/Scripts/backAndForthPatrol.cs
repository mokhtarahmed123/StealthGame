using UnityEngine;
public class backAndForthPatrol : MonoBehaviour
{
    public float direction;
    //0 is left
    //1 is up
    //2 is right
    //3 is down
    public Sprite[] sprites;
    public float wallDetectionDistance;
    public float patrolSpeed;
    private SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;
    Vector2 directionFacing;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();

        if (sprites == null || sprites.Length < 4)
        {
            Debug.LogError("Sprites array needs exactly 4 sprites on " + gameObject.name);
            return;
        }

        switch (direction)
        {
            case 0:
                directionFacing = Vector2.left;
                spriteRenderer.sprite = sprites[0];
                break;
            case 1:
                directionFacing = Vector2.up;
                spriteRenderer.sprite = sprites[1];
                break;
            case 2:
                directionFacing = Vector2.right;
                spriteRenderer.sprite = sprites[2];
                break;
            case 3:
                directionFacing = Vector2.down;
                spriteRenderer.sprite = sprites[3];
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (sprites == null || sprites.Length < 4)
        {
            Debug.LogError("Sprites array needs exactly 4 sprites on " + gameObject.name);
            return;
        }

        Vector2 origin = transform.position;
        if (direction == 0)
        {
            origin.x -= 1;
            spriteRenderer.sprite = sprites[0];
        }
        else if (direction == 1)
        {
            origin.y += 1;
            spriteRenderer.sprite = sprites[1];
        }
        else if (direction == 2)
        {
            origin.x += 1;
            spriteRenderer.sprite = sprites[2];
        }
        else if (direction == 3)
        {
            origin.y -= 1;
            spriteRenderer.sprite = sprites[3];
        }
        RaycastHit2D wall = Physics2D.Raycast(origin, directionFacing, wallDetectionDistance);
        if (wall.collider != null)
        {
            print(wall.collider.tag);
        }
        if ((wall.collider != null) && (wall.collider.tag == "wall"))
        {
            changeDirection();
        }
        else
        {
            Vector2 movement = new Vector2(0, 0);
            if (direction == 0)
            {
                movement = new Vector2(-patrolSpeed, 0);
            }
            else if (direction == 1)
            {
                movement = new Vector2(0, patrolSpeed);
            }
            else if (direction == 2)
            {
                movement = new Vector2(patrolSpeed, 0);
            }
            else if (direction == 3)
            {
                movement = new Vector2(0, -patrolSpeed);
            }
            rb2d.linearVelocity = movement;
        }
    }
    public void changeDirection()
    {
        if (direction == 0)
        {
            direction = 2;
        }
        else if (direction == 2)
        {
            direction = 0;
        }
        else if (direction == 1)
        {
            direction = 3;
        }
        else if (direction == 3)
        {
            direction = 1;
        }
        switch (direction)
        {
            case 0:
                directionFacing = Vector2.left;
                break;
            case 1:
                directionFacing = Vector2.up;
                break;
            case 2:
                directionFacing = Vector2.right;
                break;
            case 3:
                directionFacing = Vector2.down;
                break;
        }
    }
}