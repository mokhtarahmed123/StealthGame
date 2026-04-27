using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed;
    private Rigidbody2D protagonist;
    public GameObject player;
    private bool facingRight = true;
    public Animator animator;
    public GameObject knife;
    public GameObject gun;
    private Animator knifeAnimator;
    private Animator gunAnimator;
    public GameObject damageHitBox;
    private bool wait;
    float prevMagnitude;
    private Vector2 lastMove;
    public float knifeRadius;
    public GameObject gunItem;
    public bool knifeActive = true;
    public bool gunActive = false;
    public AudioSource gunShot;
    public AudioSource gunEmpty;
    public AudioSource knifeSwing;

    // Start is called before the first frame update
    void Start()
    {
        protagonist = gameObject.GetComponent<Rigidbody2D>();
        knifeAnimator = knife.GetComponent<Animator>();
        gunAnimator = gun.GetComponent<Animator>();

        if (!knifeActive)
        {
            knife.SetActive(false);
        }
        if (!gunActive)
        {
            gun.SetActive(false);
        }
    }

    IEnumerator Knife()
    {
        wait = true;
        damageHitBox.SetActive(true);
        knifeAnimator.SetTrigger("isAttacking");
        yield return new WaitForSeconds(0.2f);
        Debug.Log("Working");
        damageHitBox.SetActive(false);
        knifeAnimator.ResetTrigger("isAttacking");
    }

    IEnumerator Pewpew()
    {
        gunShot.time = 0.5f;
        gunShot.Play();
        wait = true;
        gunAnimator.SetTrigger("isAttacking");
        gun.GetComponent<Gun>().shoot();
        yield return new WaitForSeconds(0.5f);
        knifeAnimator.ResetTrigger("isAttacking");
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.2f);
        wait = false;
    }

    // Update is called once per frame
    float knifeHori;
    float knifeVert;
    void FixedUpdate()
    {

        if (!knifeActive)
        {
            knife.SetActive(false);
        }
        if (!gunActive)
        {
            gun.SetActive(true);
        }

        if (knifeActive)
        {
            knife.SetActive(true);
        }
        if (gunActive)
        {
            gun.SetActive(false);
        }

        if (protagonist.linearVelocity.magnitude > 0 && knifeActive)
        {
            knife.transform.up = lastMove;
        }
        if (protagonist.linearVelocity.magnitude > 0 && gunActive)
        {
            gun.transform.up = lastMove;
        }

        if (Input.GetKeyDown("space") && knifeActive)
        {
            Debug.Log("SPACE");
            knifeSwing.time = 0.2f;
            knifeSwing.Play();
            StartCoroutine(Knife());
            StartCoroutine(Wait());
        }


        if (Input.GetKeyDown("space") && gunActive && gun.GetComponent<Gun>().bulletCount > 0 && !wait)
        {
            StartCoroutine(Pewpew());
            StartCoroutine(Wait());
        }

        if (Input.GetKeyDown("space") && gunActive && gun.GetComponent<Gun>().bulletCount == 0 && !wait)
        {
            gunEmpty.time = 0.1f;
            gunEmpty.Play();
        }

        if (Input.GetKeyDown(KeyCode.Q) && gunActive)
        {
            gunActive = false;
            knifeActive = true;

            // تأكد إن gunItem متربط الأول
            if (gunItem != null)
            {
                GameObject gunDrop = (GameObject)Instantiate(gunItem, gun.transform.position, Quaternion.identity);
                Vector3 euler = transform.eulerAngles;
                euler.z = Random.Range(0f, 360f);
                gunDrop.transform.eulerAngles = euler;
                gunDrop.GetComponent<Rigidbody2D>().linearVelocity = gun.transform.right * 2;
                gunDrop.GetComponent<gunItem>().bulletCount = gun.GetComponent<Gun>().bulletCount;
            }

            gun.SetActive(false); // بدل ما يتمسح
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            knifeHori = Input.GetAxis("Horizontal");
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            knifeHori = Input.GetAxis("Horizontal");
        }

        if (Input.GetAxis("Vertical") > 0)
        {
            knifeVert = Input.GetAxis("Vertical");
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            knifeVert = Input.GetAxis("Vertical");
        }

        Vector3 i = lastMove;

        /*V
        Debug.Log(transform.localPosition.x + " " + transform.localPosition.y);
        Vector2 cirPoint = new Vector2(knifeHori,knifeVert);*/

        float theta = -Vector3.SignedAngle(i, transform.right, Vector3.forward);

        Vector2 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 cirPoint = new Vector2(knifeRadius * Mathf.Cos(Mathf.Deg2Rad * theta), knifeRadius * Mathf.Sin(Mathf.Deg2Rad * theta));

        //if the player is moving       
        if (movement.magnitude > 0)
        {
            //set the animator to moving
            animator.SetBool("PlayerMoving", true);

            //if the magnitude is increasing you want to update the "running" state in the animator but also the last move - this way when
            //player releases the key, the last move will be stored already in the animator  
            if (movement.magnitude > prevMagnitude || prevMagnitude >= 0.99f)
            {
                animator.SetFloat("Horizontal", movement.x);
                animator.SetFloat("Vertical", movement.y);


                lastMove = movement;
                animator.SetFloat("LastMoveX", lastMove.x);
                animator.SetFloat("LastMoveY", lastMove.y);
            }

        }

        //set the player movement to false in the animator
        if (movement.magnitude == 0)
        {
            animator.SetBool("PlayerMoving", false);
        }

        //update the previous magnitude
        prevMagnitude = movement.magnitude;

        /*  if(knifeHori < 0)
          {
              cirPoint.x = -cirPoint.x;
          }
          if (knifeVert < 0)
          {
              cirPoint.y = -cirPoint.y;
          }*/

        if (knifeActive)
        {
            knife.transform.localPosition = new Vector3(cirPoint.x, cirPoint.y, 0.0f);
        }
        if (gunActive)
        {
            gun.transform.localPosition = new Vector3(cirPoint.x, cirPoint.y, 0.0f);
        }


        //redundant -> not used in the animator
        animator.SetFloat("Magnitude", movement.magnitude);

        //transform.position = transform.position + movement * Time.deltaTime * playerSpeed;
        protagonist.linearVelocity = movement * playerSpeed;


    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
