using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class follow : MonoBehaviour
{
    public bool activef = false;
    public bool activeb = false;
    public bool moving = false;
    private float previous; 
    private Vector3 NewClampMagnitude(Vector3 vector, float minLength, float maxLength)
    {
        if (vector.sqrMagnitude > maxLength * maxLength)
            return vector.normalized * maxLength;
        if (vector.sqrMagnitude < minLength * minLength)
            return vector.normalized * minLength;
        return vector;
    }
    public GameObject front;
    public GameObject back;
    public float scale;
    float radius = .1f;
    float maxtorsion = 20f;
    public float dist;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0f,0f,transform.eulerAngles[2]);
        float difference = scale - previous;
        previous = scale;
        if (difference >= 0)
        {
            activef = false;
            if(this.tag != "Tail")
            {
                activeb = true;
            }
            else
            {
                activeb = false;
            }
        }
        else
        {
            if (this.tag != "Head")
            {
                activef = true;
            }
            else
            {
                activef = false;
            }
            activeb = false;
        }    
        if (activef && front != null)
        {
            transform.up = front.transform.position - transform.position;
            if (moving || front.GetComponent<follow>().moving)
            {
                dist = radius * scale;
            }
            else
            {
                dist = radius;
            }
            if (this.tag == "Tail")
            {
                transform.up = front.transform.position - transform.position;
            }
            transform.position = front.transform.position - NewClampMagnitude(transform.up, dist, dist + .005f);
        }
        if (activeb && back != null)
        {
            if (moving || back.GetComponent<follow>().moving)
            {
                dist = radius * scale;
                if (this.tag == "Body")
                {
                    transform.up = Vector3.Dot(front.transform.up.normalized, transform.up.normalized*dist)*front.transform.up.normalized + transform.up.normalized*dist;
                }
            }
            else
            {
                dist = radius;
            }
            transform.position = back.transform.position + NewClampMagnitude(transform.up, dist, dist + .005f);
        }
        float angle = Vector3.SignedAngle(back.transform.up,transform.up,transform.forward);
        if (angle < -maxtorsion)
        {
            transform.eulerAngles += new Vector3(0,0,-20-angle);
        }
        else if (angle > maxtorsion)
        {
            transform.eulerAngles += new Vector3(0, 0, 20 - angle);
        }
    }
}

