//https://www.salusgames.com/2016/12/28/smooth-2d-camera-follow-in-unity3d/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SmoothCamera2D : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public Transform Target;
    public float textFadeSpeed = 3.0f;


    private GameObject thingToFade;

    private bool beingShaken;

    private bool fading;
    private float fade;


    void Start()
    {
        fade = 1;
        thingToFade = GameObject.Find("WASD Tip");
        fading = false;
        StartCoroutine(fadeWASD());


        beingShaken = false;
    }

    void Update()
    {
        if (!beingShaken)
        {
            Vector3 newPosition = new Vector3(Target.position.x, Target.position.y, -10);
            transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);
        }


        if ((fading) && (thingToFade != null))
        {
            thingToFade.GetComponent<TextMeshPro>().faceColor = new Color(256,256,256,fade);

            fade -= (float).01;
        }
        if (fade <= 0)
        {
            thingToFade.GetComponent<MeshRenderer>().enabled = false;
            //Destroy(thingToFade);
            fading = false;
        }
    }

    public void SetShaking(bool shake)
    {
        beingShaken = shake;
    }


    IEnumerator fadeWASD()
    {
        yield return new WaitForSeconds(textFadeSpeed);
        fading = true;
    }


}