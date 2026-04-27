using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertNear : MonoBehaviour
{

    public float AlertRadius;
    public GameObject player;

    private bool alertExists;


    //private CircleCollider2D alertArea;

    circleManPatrol circleManScript;

    // Start is called before the first frame update
    void Start()
    {
        alertExists = false;
        circleManScript = GetComponent<circleManPatrol>();
    }

    // Update is called once per frame
    void Update()
    {
        bool alert = true;
        if (circleManScript != null)
        {
            alert = circleManScript.getAlertState();
        }
        if (alert)
        {
            StartCoroutine(createAlertRadius());   
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<WaypointPatrol>() != null)
            {
                collision.GetComponent<WaypointPatrol>().SetAlertState(true);
                collision.transform.right = player.transform.position - collision.transform.position;
            }
        }
    }

    IEnumerator createAlertRadius()
    {
        if (!alertExists)
        {
            alertExists = true;
            CircleCollider2D alertArea = gameObject.AddComponent<CircleCollider2D>();
            alertArea = gameObject.AddComponent<CircleCollider2D>();
            alertArea.radius = AlertRadius;
            alertArea.isTrigger = true;
            yield return new WaitForSeconds(1f);
            Destroy(alertArea);
            alertExists = false;
        }
    }

}
