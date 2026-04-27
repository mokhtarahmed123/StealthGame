using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Casing : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeAway());
    }

    IEnumerator fadeAway()
    {
        yield return new WaitForSeconds(60f);
        Destroy(gameObject);
    }
}
