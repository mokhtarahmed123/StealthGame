using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WaypointPatrol : MonoBehaviour
{
    [Tooltip("List of waypoint game objects for the enemy to move to, in order.")]
    public List<Transform> waypoints;
    [Tooltip("Speed of enemy")]
    public float chaseSpeed = 5;
    public float patrolSpeed = 1;
    public float lookAroundTime;
    public GameObject audioStealth;
    public GameObject audioFoundOne;
    public GameObject audioFoundTwo;
    public float fadeTime = 0.0f;
    AudioSource stealthMusic;
    AudioSource foundMusicOne;
    AudioSource foundMusicTwo;

    private int curpointidx;
    private int lastpointidx;
    private Rigidbody2D rb;
    private RayEnemyDetect detectScript;
    private bool alerted;
    private bool wasAlerted;
    private bool lookingAround;
    private static bool playingFoundMusic;
    private bool playingStealthMusic;
    private Vector2 playerPosition;
    private float speed;
    
    

    // Start is called before the first frame update
    void Start()
    {
        curpointidx = 0;
        lastpointidx = waypoints.Count - 1; //Assuming that count is the size of the list 
        rb = GetComponent<Rigidbody2D>();
        alerted = false;
        wasAlerted = false;
        lookingAround = false;
        playingFoundMusic = false;
        playingStealthMusic = true;
        playerPosition = Vector2.zero;
        speed = patrolSpeed;
        detectScript = GetComponent<RayEnemyDetect>();

        stealthMusic = audioStealth.gameObject.GetComponent<AudioSource>();
        foundMusicOne = audioFoundOne.gameObject.GetComponent<AudioSource>();
        foundMusicTwo = audioFoundTwo.gameObject.GetComponent<AudioSource>();

        // Safety check to make sure nothing is null, if something is then destroy gameobject
        foreach (var point in waypoints)
        {
            if (point == null)
            {
                Destroy(gameObject);
                print("WAYPOINT BOI HAS NULL WAYPOINT FOR SOME REASON: DESTROYING");
                throw new MissingReferenceException();
            }

            if (point.position.z != 0)
                point.position = new Vector3(point.position.x, point.position.y, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (wasAlerted && !alerted)
        {
            StopCoroutine(LookAround());
            StartCoroutine(LookAround());
        }
        else
        {
            if (!lookingAround)
            {
                if (!alerted)
                    MoveToWaypoint(waypoints[curpointidx]);
                else
                    ChasePlayer();
            }
        }

        if (rb.linearVelocity != Vector2.zero)
            rb.linearVelocity = Vector2.zero;
    }

    // Moves enemy towards the players position where it was hit by a raycast
    private void ChasePlayer()
    {
        //speed = detectScript.GetActivityState() ? chaseSpeed : 0;
        speed = chaseSpeed;
        chaseSpeed *= 1.01f;

        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, playerPosition, step);

        transform.right = (Vector3)playerPosition - transform.position;

        wasAlerted = true;

        if (playingStealthMusic == true)
        {
            stealthMusic.Stop();

            if(playingFoundMusic == false)
            {
                foundMusicOne.Play();
                foundMusicTwo.Play();
            }
            playingFoundMusic = true;
            playingStealthMusic = false;
        }
    }

    // Moves this transform towards destination by step units
    private void MoveToWaypoint(Transform destination)
    {
        //speed = detectScript.GetActivityState() ? patrolSpeed : 0;
        speed = patrolSpeed;

        var step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination.position, step);

        transform.right = destination.position - transform.position;

        if (transform.position == destination.position)
        {
            curpointidx++;
        }

        if (curpointidx > lastpointidx)
            curpointidx = 0;
    }

    //taken from https://forum.unity.com/threads/fade-out-audio-source.335031/
    public static IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void SetAlertState(bool alertState)
    {
        alerted = alertState;
        if(alerted)
        {
            lookingAround = false;
        }
    }

    public bool GetAlertState()
    {
        return alerted;
    }

    public void SetPlayerPosition(Vector2 point)
    {
        playerPosition = point;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(!lookingAround && collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator LookAround()
    {
        List<GameObject> wayPointBois = new List<GameObject>();
        foreach(GameObject obj in FindObjectsOfType(typeof (GameObject)))
        {
            if(obj.GetComponent<WaypointPatrol>() != null)
            {
                wayPointBois.Add(obj);
            }
        }

        int otherAlerted = 0;
        
        foreach(GameObject obj in wayPointBois)
        {
            if (obj.GetComponent<WaypointPatrol>().GetAlertState() == true)
            {
                otherAlerted++;
            }
        }

        if (playingFoundMusic == true && alerted == false && !(otherAlerted > 1))
        {
            if (playingStealthMusic == false)
            {
                stealthMusic.time = 5.0f;
                stealthMusic.Play();
            }
            StartCoroutine(FadeOut(foundMusicOne, fadeTime));
            StartCoroutine(FadeOut(foundMusicTwo, fadeTime));

            

            playingFoundMusic = false;
            playingStealthMusic = true;
        }

        wasAlerted = false;
        lookingAround = true;

        detectScript.SetActivityState(false);

        foreach (Transform waypoint in waypoints)
        {
            transform.right = waypoint.position - transform.position;
            yield return new WaitForSeconds(lookAroundTime / waypoints.Count);
        }

        transform.right = waypoints[curpointidx].position - transform.position;

        yield return new WaitForSeconds(lookAroundTime / waypoints.Count);

        detectScript.SetActivityState(true);

        lookingAround = false;

        otherAlerted = 0;     
    }

}
