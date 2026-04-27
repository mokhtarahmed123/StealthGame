using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageHitBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<poofOnDeath>().Poof();
            Destroy(col.gameObject);

            if(col.gameObject.GetComponent<GunBoiPatrol>() != null)
            {
                col.gameObject.GetComponent<GunBoiPatrol>().SpawnGun();
            }
        }
    }
}
