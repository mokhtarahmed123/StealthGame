using UnityEngine;


public class Gun : MonoBehaviour
{
    public int bulletCount;
    public GameObject casing;
    public int casingVeclocity;
    public GameObject bullet;
    public int bulletVeclocity = 20;
    public bool isPlayer = true;

    void Update()
    {
        if (GetComponentInParent<PlayerController>() != null)
        {
            isPlayer = true;
        }
    }

    public void shoot()
    {
        if (isPlayer && bulletCount == 0)
        {
            return;
        }
        GameObject casingDrop = (GameObject)Instantiate(casing, transform.position, Quaternion.identity);
        casingDrop.transform.rotation = Random.rotation;
        Vector3 euler = transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        casingDrop.transform.eulerAngles = euler;
        casingDrop.GetComponent<Rigidbody2D>().linearVelocity = transform.right * casingVeclocity;


        if (isPlayer)
        {
            GameObject bulletObj = (GameObject)Instantiate(bullet, transform.position, Quaternion.identity);
            bulletObj.transform.up = transform.up;
            bulletObj.GetComponent<Rigidbody2D>().linearVelocity = transform.up * bulletVeclocity;
            bulletCount -= 1;
        }
        else
        {
            Vector3 pos = transform.parent.position + transform.parent.right;
            GameObject bulletObj = (GameObject)Instantiate(bullet, pos, Quaternion.identity);
            bulletObj.transform.up = transform.up;
            bulletObj.GetComponent<Rigidbody2D>().linearVelocity = transform.up * bulletVeclocity;
        }

    }

}
