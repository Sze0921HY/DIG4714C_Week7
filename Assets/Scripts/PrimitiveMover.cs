using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimitiveMover : MonoBehaviour
{
    public GameObject[] pills;
    public Vector3[] startPoints;
    public Vector3[] endPoints;
    public float speed = 2f;
    public float t = 0;
    private bool translateMovingForward = true;
    private Rigidbody rb;
    private Vector3 rbTarget;

    void Start()
    {
        // Create a blue plane
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.position = new Vector3(0, 0, 0);
        plane.transform.localScale = new Vector3(1, 1, 1);
        plane.GetComponent<Renderer>().material.color = Color.blue;

        // Define start and end points
        startPoints = new Vector3[]
        {
            new Vector3(-5, 1, -3),
            new Vector3(-5, 1, -1),
            new Vector3(-5, 1, 1),
            new Vector3(-5, 1, 3)
        };

        endPoints = new Vector3[]
        {
            new Vector3(5, 1, -3),
            new Vector3(5, 1, -1),
            new Vector3(5, 1, 1),
            new Vector3(5, 1, 3)
        };

        pills = new GameObject[4];

        for (int i = 0; i < 4; i++)
        {
            pills[i] = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            pills[i].transform.position = startPoints[i];
        }

        // Set different colors for each object
        pills[0].GetComponent<Renderer>().material.color = Color.red;
        pills[1].GetComponent<Renderer>().material.color = Color.green;
        pills[2].GetComponent<Renderer>().material.color = Color.yellow;
        pills[3].GetComponent<Renderer>().material.color = Color.cyan;

        // Add Rigidbody to the fourth pill
        rb = pills[3].AddComponent<Rigidbody>();
        rb.useGravity = false;
        rbTarget = endPoints[3];
    }

    void Update()
    {
        // Lerp movement for first pill with eased movement and color interpolation
        t += Time.deltaTime * speed / Vector3.Distance(startPoints[0], endPoints[0]);
        float easedT = Mathf.SmoothStep(0, 1, t);
        pills[0].transform.position = Vector3.Lerp(startPoints[0], endPoints[0], easedT);
        pills[0].transform.localScale = Vector3.one * (1 + 0.3f * Mathf.Sin(easedT * Mathf.PI));
        pills[0].GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.white, easedT);
        if (t >= 1f)
        {
            t = 0;
            (startPoints[0], endPoints[0]) = (endPoints[0], startPoints[0]);
        }

        // MoveTowards for second pill
        pills[1].transform.position = Vector3.MoveTowards(pills[1].transform.position, endPoints[1], speed * Time.deltaTime);
        if (Vector3.Distance(pills[1].transform.position, endPoints[1]) < 0.1f)
        {
            (startPoints[1], endPoints[1]) = (endPoints[1], startPoints[1]);
        }

        // Translate movement for third pill with back and forth motion
        float step = speed * Time.deltaTime;
        if (translateMovingForward)
        {
            pills[2].transform.Translate(Vector3.right * step);
            if (Vector3.Distance(pills[2].transform.position, endPoints[2]) < 0.1f)
            {
                translateMovingForward = false;
            }
        }
        else
        {
            pills[2].transform.Translate(Vector3.left * step);
            if (Vector3.Distance(pills[2].transform.position, startPoints[2]) < 0.1f)
            {
                translateMovingForward = true;
            }
        }

        // Rigidbody-based movement for fourth pill
        Vector3 direction = (rbTarget - rb.position).normalized;
        // Draw the normalized direction vector in the Scene view
        Debug.DrawRay(rb.position, direction * 2, Color.magenta);

        rb.velocity = direction * speed;
        if (Vector3.Distance(rb.position, rbTarget) < 0.5f)
        {
            rbTarget = (rbTarget == startPoints[3]) ? endPoints[3] : startPoints[3];
        }
    }
}
