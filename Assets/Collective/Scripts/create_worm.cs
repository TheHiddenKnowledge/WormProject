using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class create_worm : MonoBehaviour
{
    //Prefabs for the worm segments 
    public GameObject head;
    public GameObject body;
    public GameObject tail;
    //The variable for the simulation speed 
    public float speed = 1;
    //The max hunger for the worm as well as the current hunger 
    float maxhunger = 100f;
    public float hunger;
    //The minimum angle required for a shear condition, as well as the boolean for the shear. Determines whether to use coarse
    //or fine movement 
    float mintorsense = 20f;
    bool shear= false;
    //These variables are used to determine the total amount of segments for the worm 
    const int hdtl = 2;
    public int bodyseg = 10;
    int totseg;
    //Used in wave motion function for keeping track of the current body segment 
    public int bodycount = 1;
    //
    public int[] genes = new int[2]; 
    //These variables are used in the oscillation function, and can be changed to produce different movement speeds 
    public float max;
    float amp;
    float frequency;
    float nextActionTime;
    float period;
    //The scaling value that is sent to each worm segment 
    float finalscale;
    //Array that contains each segments gameobject data 
    public GameObject[] segments;
    //Start contains the initialization and assignment of necessary variables, as well as the creation of the worm itself 
    void Start()
    {
        //Initializing segment array and assigning values for oscillation function 
        totseg = hdtl + bodyseg;
        segments = new GameObject[totseg];
        hunger = maxhunger;
        amp = (max - 1f) / 2f;
        frequency = Mathf.PI;
        nextActionTime = 0.0f;
        period = 2f * Mathf.PI / frequency;
        //Creating the worms body (head, body, and tail segments) from input prefabs 
        segments[0] = Instantiate(head,transform.position,transform.rotation);
        for (int i = 0; i < bodyseg; i++)
        {
            segments[i+1] = Instantiate(body, transform.position-transform.up*.25f*(i+1), transform.rotation);
        }
        segments[totseg-1] = Instantiate(tail, transform.position + transform.up * -(bodyseg+1)*.25f, transform.rotation);
        //Assigning the front and back segments for each individual worm segment
        for (int i = 0; i< totseg;i++)
        {
            follow fol = segments[i].GetComponent<follow>();
            if (fol != null)
            {
                if(i != 0)
                {
                    fol.front = segments[i - 1];
                }
                else {
                    fol.front = segments[i];
                }
                if (i != totseg-1)
                {
                    fol.back = segments[i + 1];
                }
                else
                {
                    fol.back = segments[i];
                }
            }
        }
        // Assigning the camera to follow the head of the worm
        segments[0].transform.position = transform.position;
        transform.parent = segments[0].transform;
    }
    //Update contains the functions that control movement and hunger for the worm 
    void Update()
    {
        //Manages the movement of the worm 
        MovementControl();
        //Manages the hunger of the worm 
        HungerManage();
    }
    void WaveMove1()
    {
        // When the body counter has reached the final body segment the counter is reset 
        if (bodycount > bodyseg)
        {
            bodycount = 1;
            segments[bodyseg].GetComponent<follow>().moving = false;
        }
        segments[bodycount].GetComponent<follow>().moving = true;
        // Sets the transform scale of the body segment at the index of the counter to the oscillation function 
        segments[bodycount].transform.localScale = new Vector3(1, finalscale, 0);
        // Whenever the game time has elapsed an interval equal to the period of the oscillation function, 
        // the next body segment is selected 
        if (Time.time > nextActionTime && finalscale < 1.01)
        {
            nextActionTime = Time.time + period*3/4;
            segments[bodycount].GetComponent<follow>().moving = false;
            segments[bodycount].transform.localScale = new Vector3(1f, 1f, 0);
            bodycount = 1;
        }
    }
    void Move(int i)
    {
        segments[i].GetComponent<follow>().moving = true;
        segments[i].transform.localScale = new Vector3(1, finalscale, 0);
    }
    void MovementControl()
    {
        finalscale =  amp * (-Mathf.Cos((speed * frequency+2*Mathf.PI) * Time.time) + 1) + 1;
        float angle = Vector3.SignedAngle(segments[totseg-1].transform.up,segments[0].transform.up,segments[0].transform.forward);
        if (angle > mintorsense)
        {
            shear = true;
        }
        else if (angle < -mintorsense)
        {
            shear = true;
        }
        else
        {
            shear = false;
        }
        // Sets the scale variable for all worm segments to the oscillation function  
        for (int i = 0; i < totseg; i++)
        {
            hunger -= speed*.0025f*totseg;
            segments[i].GetComponent<follow>().scale = finalscale;
            if (!shear)
            {
                segments[i].GetComponent<follow>().scale = finalscale;
                if (i != 0 && i != totseg - 1)
                {
                    Move(i);
                }
            }
            else
            {
                segments[i].GetComponent<follow>().moving = false;
                segments[i].transform.localScale = new Vector3(1, 1, 0);
                Move(1);
            }
        }
    }
    void HungerManage()
    {
        if(hunger > maxhunger)
        {
            hunger = maxhunger;
        }
        else if (hunger < 0)
        {
            for (int i = totseg-1; i >= 0; i--)
            {
                Destroy(segments[i]);
            }
        }
    }
}
