using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showAttackTip : MonoBehaviour
{

    public GameObject gameCam;

    private GameObject thingToFade;

    private bool fading;
    private float fade;

    // Start is called before the first frame update
    void Start()
    {
        fade = 1;
        thingToFade = GameObject.Find("AttackTip");
    }

    // Update is called once per frame
    void Update()
    {
        if ((fading) && (thingToFade != null))
        {
            thingToFade.GetComponent<TextMeshPro>().faceColor = new Color(256,256,256,fade);

            fade -= (float).01;
        }
        if (fade <= 0)
        {
            Destroy(thingToFade);
            fading = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Entered");
        if (collision.tag == "Player")
        {
            GameObject wasd = GameObject.Find("WASD Tip");
            if (wasd != null)
            {
                print("deleting");
                Destroy(GameObject.Find("WASD Tip"));
            }

            thingToFade.GetComponent<MeshRenderer>().enabled = true;
            StartCoroutine(fadeAttackTip());


        }
    }

    IEnumerator fadeAttackTip()
    {
        yield return new WaitForSeconds(3f);
        fading = true;
    }

}
