using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLightRays : MonoBehaviour
{
    public float viewAngle;
    public int rayCount;
    public float lookDistance;
    public GameObject rayRenderer;
    public int rayRenderDensity;
    public float alpha = 0.01f;
    public Color color = Color.white;
    public float lightWidth = 0.25f;
    public bool raveMode = false;
    public float raveChangeTime = 0.25f;


    private LineRenderer viewLineRenderer;
    private List<RaycastHit2D> rays;
    private List<GameObject> rayRenderersList;
    private float minViewAngle;
    private float maxViewAngle;
    private int layerMask;
    bool raving;
    // Start is called before the first frame update
    void Start()
    {
        rays = new List<RaycastHit2D>(rayCount);
        rayRenderersList = new List<GameObject>();

        viewLineRenderer = GetComponent<LineRenderer>();
        viewLineRenderer.positionCount = rayCount;

        maxViewAngle = viewAngle;
        minViewAngle = -viewAngle;
        rayCount /= 2;


        layerMask = 1 << 8;
        layerMask = ~layerMask;

        if (rayRenderDensity % 2 == 0)
            rayRenderDensity += 1;

        raving = false;

    }

    // Update is called once per frame
    void Update()
    {
        CheckRays(CreateRays());
    }

    private void CheckRays(List<Vector3> renderVectors)
    {

        int count = 0;

        foreach (GameObject obj in rayRenderersList)
            Destroy(obj);

        rayRenderersList.Clear();

        foreach (RaycastHit2D hit in rays)
        {

            if (hit.collider != null)
            {
                //Collision Logic goes here if you want any
                
            }

            if (count % rayRenderDensity*(lightWidth*10) == 0)
            {
                GameObject rayRendererObject = Instantiate(rayRenderer, transform);
                rayRendererObject.transform.SetParent(transform);
                rayRenderersList.Add(rayRendererObject);


                LineRenderer templine = rayRendererObject.GetComponent<LineRenderer>();
                templine.startWidth = lightWidth * rayRenderDensity;
                templine.endWidth = lightWidth * rayRenderDensity;
                templine.SetPosition(0, transform.position);

                if (hit.collider != null)
                {
                    if (count % rayRenderDensity == 0)
                        templine.SetPosition(1, hit.point);
                }
                else
                {
                    templine.SetPosition(1, renderVectors[count]);
                }

                if (raveMode && !raving)
                {
                    StartCoroutine(RaveMeUpBaby());
                }

                Gradient grad = new Gradient();
                grad.SetKeys(new GradientColorKey[] { new GradientColorKey(color, 0.0f) },
                             new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f) });

                templine.colorGradient = grad;

            }
            count++;



        }

        rays.Clear();
    }

    private List<Vector3> CreateRays()
    {

        float increment = viewAngle / rayCount;
        float angle1 = 0;
        float angle2 = 0;

        List<Vector3> renderVectors = new List<Vector3>();

        for (int i = 0; i < rayCount; i++)
        {

            angle1 += increment;
            angle2 -= increment;

            if (angle1 > maxViewAngle)
                angle1 = maxViewAngle;
            if (angle2 < minViewAngle)
                angle2 = minViewAngle;

            Vector3 dir1 = Quaternion.AngleAxis(angle1, transform.forward) * transform.right;
            Vector3 dir2 = Quaternion.AngleAxis(angle2, transform.forward) * transform.right;

            rays.Add(Physics2D.Raycast(transform.position, dir1, lookDistance)); //, layerMask));
            rays.Add(Physics2D.Raycast(transform.position, dir2, lookDistance)); //, layerMask));

            renderVectors.Add(transform.position + (dir1 * lookDistance));
            renderVectors.Add(transform.position + (dir2 * lookDistance));


            //Debug.DrawRay(transform.position, dir1, Color.green);
            //Debug.DrawRay(transform.position, dir2, Color.red);
        }

        return renderVectors;

    }


    IEnumerator RaveMeUpBaby()
    {
        raving = true;
        color = new Color(Random.value, Random.value, Random.value);
        yield return new WaitForSeconds(raveChangeTime);
        raving = false;
    }

}