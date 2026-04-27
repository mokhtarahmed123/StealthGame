using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class superHotBoi : MonoBehaviour
{
    public float patrolSpeed;
    public int rayCount;
    public float lookDistance;
    Vector2 playerPosition;
    private bool playerInSight;
    private int layerMask;
    private List<RaycastHit2D> rays;
    private int maxViewAngle;
    private int minViewAngle;

    void Start()
    {
        rays = new List<RaycastHit2D>();
        int viewAngle = 360;
        maxViewAngle = viewAngle;
        minViewAngle = -viewAngle;

        // ignore layer 9 (مثلاً الـ Enemy نفسه)
        layerMask = 1 << 9;
        layerMask = ~layerMask;
    }

    void Update()
    {
        playerInSight = false; // reset كل frame
        CreateRays();
        CheckRays();

        if (playerInSight)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) ||
                Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) ||
                Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                MoveTowards();
            }
        }
    }

    private void CheckRays()
    {
        foreach (RaycastHit2D hit in rays)
        {
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Player")
                {
                    playerInSight = true;
                    playerPosition = hit.point;
                }
            }
        }
        rays.Clear();
    }

    private void CreateRays()
    {
        if (rayCount <= 0) return;

        rays.Clear(); // امسح الـ rays القديمة الأول

        float increment = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            float angle1 = increment * i;
            float angle2 = -increment * i;

            Vector3 dir1 = Quaternion.AngleAxis(angle1, transform.forward) * transform.right;
            Vector3 dir2 = Quaternion.AngleAxis(angle2, transform.forward) * transform.right;

            rays.Add(Physics2D.Raycast(transform.position, dir1, lookDistance, layerMask));
            rays.Add(Physics2D.Raycast(transform.position, dir2, lookDistance, layerMask));
        }
    }

    private void MoveTowards()
    {
        var step = patrolSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);
        transform.right = (Vector3)playerPosition - transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            print("kill Player");
            StartCoroutine(LoadSceneAfter(0.1f, SceneManager.GetActiveScene().buildIndex));
        }
    }

    IEnumerator LoadSceneAfter(float delay, int buildIndex)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(buildIndex);
    }
}