using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class create_worm_v2 : MonoBehaviour
{
    int groups = 6;
    int groupcount = 5;
    int segments;
    float[] length;
    float[] width;
    float[] length_active;
    float[] width_active;
    float base_length = .1f;
    float base_width = .3f;
    public float[] adjangles;
    float[] angles;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    Mesh mesh;
    Vector3[] vertices_list;
    Vector3[] nodes;
    Vector3[] prevnodes;
    public Vector3[] normals;
    int[] tris;
    public float max;
    float amp;
    public float frequency;
    float nextActionTime;
    float period;
    float nextActionTime1;
    float period1 = 2f;
    //The scaling value that is sent to each worm segment 
    float finalscale;
    float prevscale;
    public int bodycount = 1;
    float maxtorsion = 45f/180f*Mathf.PI;
    float maxhunger = 100f;
    public float hunger;
    public float speed = 1;
    public int[] genes = new int[2];
    public GameObject bodycol;
    GameObject[] clones;
    void make_Mesh()
    {
        mesh = new Mesh();
        for (int j = 0; j <= segments; j++)
        {
            vertices_list[j * 2] = nodes[j]+new Vector3(-width_active[j] / 2*Mathf.Cos(angles[j]), -width_active[j] / 2 * Mathf.Sin(angles[j]), 0);
            vertices_list[j * 2 + 1] = nodes[j]+new Vector3(width_active[j] / 2 * Mathf.Cos(angles[j]), width_active[j] / 2 * Mathf.Sin(angles[j]), 0);
            if (j != segments)
            {
                if (angles[j]-angles[j+1] < -maxtorsion)
                {
                    normals[j] = Quaternion.Euler(0,0, - maxtorsion + angles[j + 1]) *normals[j];
                    angles[j] = -maxtorsion + angles[j + 1];
                }
                else if (angles[j] - angles[j + 1] > maxtorsion)
                {
                    normals[j] = Quaternion.Euler(0, 0, maxtorsion + angles[j + 1]) * normals[j];
                    angles[j] = maxtorsion + angles[j + 1];
                }
                tris[3 * j * 2] = j * 2;
                tris[3 * j * 2 + 1] = j * 2 +1;
                tris[3 * j * 2 + 2] = j * 2 + 2;
                tris[3 * j * 2 + 3] = j * 2 + 2;
                tris[3 * j * 2 + 4] = j * 2 + 1;
                tris[3 * j * 2 + 5] = j * 2 + 3;
            }
            mesh.vertices = vertices_list;
            mesh.triangles = tris;
            meshFilter.mesh = mesh;
        }
    }
    void followback()
    {
        for (int i = nodes.Length - 1; i >= 0; i--)
        {
            if (i == nodes.Length - 1)
            {
                nodes[i] = prevnodes[i];
                normals[i] = prevnodes[i - 1] - nodes[i];
                normals[i] = normals[i].normalized;
            }
            else
            {
                nodes[i] = length_active[i]*normals[i]+prevnodes[i+1];
                if (i != 0)
                {
                    normals[i] = Vector3.Dot(normals[i - 1].normalized* length_active[i-1], normals[i].normalized) * normals[i].normalized + normals[i-1].normalized * length_active[i-1];
                    normals[i] = normals[i].normalized;
                }
                prevnodes[i] = nodes[i];
            }
        }
        transform.position = nodes[0];
        for (int i = 0; i<nodes.Length;i++)
        {
            nodes[i] = nodes[i] - transform.position;
                
        }
    }
    void followfront()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (i == 0)
            {
                nodes[i] = prevnodes[i];
            }
            else
            {
                if(Mathf.Abs(adjangles[i-1]-adjangles[i])<10)
                {
                    nodes[i] = -length_active[i] * normals[i] + prevnodes[i - 1];
                }
                else
                {
                    nodes[i] = prevnodes[i];
                }
                normals[i] = nodes[i-1]-nodes[i];
                normals[i] = normals[i].normalized;
                prevnodes[i] = nodes[i];
            }
        }
        transform.position = nodes[0];
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = nodes[i] - transform.position;
        }
    }
    void WaveMove()
    {
        // When the body counter has reached the final body segment the counter is reset 
        if (bodycount > groups-1-groups/2)
        {
            bodycount = 0;
        }
        // Sets the transform scale of the body segment at the index of the counter to the oscillation function
        for (int i = 0; i<groupcount;i++)
        {
            length_active[bodycount * groupcount+i] = finalscale * length[bodycount * groupcount+i];
            width_active[bodycount * groupcount + i] = width[bodycount * groupcount + i] / finalscale;
        }
        // Whenever the game time has elapsed an interval equal to the period of the oscillation function, 
        // the next body segment is selected 
        if (Time.time > nextActionTime && finalscale < 1.01)
        {
            nextActionTime = Time.time + period * 3 / 4;
            for (int i = 0; i < groupcount; i++)
            {
                length_active[bodycount * groupcount + i] = length[bodycount * groupcount + i];
                width_active[bodycount * groupcount + i] = width[bodycount * groupcount + i];
            }
            bodycount += 1;
        }
    }
    void LongMove()
    {
        for (int i = 2; i < segments-1;i++)
        {
            length_active[i] = finalscale * length[i];
            width_active[i] = width[i]/ finalscale;
        }
    }
    void HungerManage()
    {
        if (hunger > maxhunger)
        {
            hunger = maxhunger;
        }
        else if (hunger < 0)
        {
            Destroy(this.gameObject);
        }
    }
    void DelayedStart()
    {
        for (int i = 0; i < clones.Length; i++)
        {
            if (i != 0)
            {
                clones[i].tag = "Untagged";
                clones[i].GetComponent<Collider2D>().isTrigger = false;
            }
        }
    }
    public void Start()
    {
        hunger = maxhunger;
        segments = groupcount * groups;
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshFilter = gameObject.GetComponent<MeshFilter>();
        vertices_list = new Vector3[(segments + 1) * 2];
        tris = new int[2 * segments * 3];
        nodes = new Vector3[segments+1];
        prevnodes = new Vector3[segments + 1];
        amp = (max - 1f) / 2f;
        frequency = 2.5f*Mathf.PI;
        nextActionTime = 0.0f;
        period = 2f * Mathf.PI / frequency;
        length = new float[segments+1];
        width = new float[segments + 1];
        length_active = new float[segments + 1];
        width_active = new float[segments + 1];
        angles = new float[segments + 1];
        adjangles = new float[segments + 1];
        normals = new Vector3[segments + 1];
        clones = new GameObject[groups+1];
        Camera cam = Camera.main;
        float height_cam = 2f * cam.orthographicSize;
        float width_cam = height_cam * cam.aspect;
        //Spawning all the designated gameobjects 
        Vector3 offset = new Vector3((Random.Range(-width_cam/2, width_cam/2)), (Random.Range(-height_cam/2, height_cam/2)), 0.0f);
        float angle = Random.Range(0,360);
        for (int i =0; i<length.Length;i++)
        {
            prevnodes[i] = new Vector3(i * base_length * Mathf.Sin(angle), -i*base_length*Mathf.Cos(angle),0)+offset;
            if ((i) % groupcount == 0)
            {
                clones[i / groupcount] = (GameObject)Instantiate(bodycol, new Vector3(0,0,0), Quaternion.Euler(0.0f, 0.0f, 0.0f));
                clones[i / groupcount].transform.parent =this.gameObject.transform;
                clones[i / groupcount].transform.position = prevnodes[i];
            }
            normals[i] = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
            length[i] = base_length;
            length_active[i] = base_length;
            if (i == 0 || i == width.Length - 1)
            {
                width[i] = base_width / 2;
                width_active[i] = base_width / 2;
            }
            else if (i>=groupcount && i < groupcount*2)
            {
                width[i] = base_width * 1.25f;
                width_active[i] = base_width * 1.25f;
            }
            else
            {
                width[i] = base_width;
                width_active[i] = base_width;
            }
        }
        Invoke("DelayedStart", 1f);
        make_Mesh();
        }
    public void Update()
    {
        hunger -= speed * .05f * segments;
        finalscale =  amp * (-Mathf.Cos(speed * frequency * Time.time) + 1) + 1;
        if (finalscale-prevscale>=0)
        {
            followback();
        }
        else
        {
            followfront();
        }
        prevscale = finalscale;
        for (int i = 0; i<segments+1; i++)
        {
            angles[i] = -Mathf.Atan2(normals[i].x,normals[i].y);
            adjangles[i] = angles[i] * 180 / Mathf.PI;
            if ((i) % groupcount == 0)
            {
                clones[i / groupcount].transform.position = prevnodes[i];
            }
        }
        /*
        if (Time.time > nextActionTime1)
        {
            nextActionTime1 = Time.time + period1;
            normals[0] = Quaternion.Euler(0, 0, angles[0] + Random.Range(-30f, 30f)) * normals[0];
        }*/
        WaveMove();
        make_Mesh();
        HungerManage();
    }
}
