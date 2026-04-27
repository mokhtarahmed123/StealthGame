using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public float duration = 0.0f;
    public float magnitude = 0.7f;
    public float dampingSpeed = 1.0f;
    public GameObject player;

    private Vector3 initialPosition;
    private SmoothCamera2D smoother;

    // Start is called before the first frame update
    void Start()
    {
        smoother = GetComponent<SmoothCamera2D>();
    }

    void OnEnable()
    {
        initialPosition = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (duration > 0)
        {
            smoother.SetShaking(true);

            Vector3 shakeAmt = Random.insideUnitSphere * magnitude*2;

            Vector3 target = new Vector3(player.transform.position.x + shakeAmt.x, player.transform.position.y + shakeAmt.y, -10);

            transform.position = Vector3.MoveTowards(transform.position, target, 25f * Time.deltaTime);

            duration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            smoother.SetShaking(false);
        }

    }

    public void Shake(float shakeTime = 0.5f, float shakeMagnitude = 0.7f, float shakeDamping = 1.0f)
    {
        duration = shakeTime;
        magnitude = shakeMagnitude;
        shakeDamping = dampingSpeed;
    }
}
