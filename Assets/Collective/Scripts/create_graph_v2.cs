using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class create_graph_v2 : MonoBehaviour
{
    //Prefabs for graph components.
    public Text GraphText;
    public GameObject GraphBG;
    public GameObject marker;
    //Amount of graph ticks in x and y directions. 
    public float nx;
    public float ny;
    //
    public float y_min;
    public float y_max;
    //Previous and current generation counts. 
    int cgeneration;
    int pgeneration = -1;
    //Used for scaling the x axis. 
    int mult = 1;
    //List of the averages data (more lists added for more data interests).
    List<float> averages = new List<float>();
    //This function creates the graph based on the multiple of nx and the amount of generations.  
    void MakeGraph(int multiple)
    {
        //Getting the in-game size of the graph background for reference later. 
        Sprite sp = GraphBG.GetComponent<SpriteRenderer>().sprite;
        Vector3 lower = sp.bounds.min * 42;
        float width = Mathf.Abs(lower.x) * 2 - 30;
        float height = Mathf.Abs(lower.y) * 2 - 20;
        //Creating the background gameobject for the graph
        GameObject cloneBG = Instantiate(GraphBG, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        cloneBG.transform.SetParent(transform);
        cloneBG.transform.localPosition = new Vector3(0f, 0f, 0f);
        cloneBG.transform.localScale = new Vector3(42, 42, 0);
        //Creating the tick mark gameobjects in the y direction, starting from the bottom left corner.
        for (int i = 0; i <= ny; i++)
        {
            Text clone = Instantiate(GraphText, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            float total = (y_max-y_min) * i / ny+y_min;
            clone.text = total.ToString();
            clone.transform.position = transform.position;
            clone.transform.SetParent(transform);
            clone.transform.localScale = new Vector3(12f / 150f, 12f / 150f, 0);
            clone.transform.localPosition = new Vector3(lower.x - 15f, lower.y + height * i / ny + 10f, 0f);
        }
        //Creating the tick mark gameobjects in the x direction, starting from the bottom left corner.
        for (int i = 0; i <= nx; i++)
        {
            Text clone = Instantiate(GraphText, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            clone.text = (i*multiple).ToString();
            clone.transform.position = transform.position;
            clone.transform.SetParent(transform);
            clone.transform.localScale = new Vector3(12f / 150f, 12f / 150f, 0);
            clone.transform.localPosition = new Vector3(lower.x + width * (i) / nx + 15f, lower.y - 10f, 0f);
        }
        //Creating the data marker gameobjects, placed with respect to the current generation and the current data value. 
        for (int i = 0; i < averages.Count; i++)
        {
            GameObject cloneM = Instantiate(marker, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
            cloneM.transform.position = transform.position;
            cloneM.transform.SetParent(transform);
            cloneM.transform.localScale = new Vector3(50f, 50f, 0);
            cloneM.transform.localPosition = new Vector3(lower.x + width * (i) / (nx*mult) + 15f, lower.y + height * (averages[i]-y_min) / (y_max-y_min) + 10f, 0f);
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Sets the current generation and current average to the value held by the evolution simulator script.
        cgeneration = Camera.main.GetComponent<evolution_sim_v2>().generation;
        float average = Camera.main.GetComponent<evolution_sim_v2>().average;
        //Whenever the generation advances, the current average is added to the averages dataset and the previous generation is set. 
        //All graph components are deleted and then recreated to update the graph.
        if (cgeneration-pgeneration !=0)
        {
            averages.Add(average);
            foreach(GameObject obj in GameObject.FindGameObjectsWithTag("Graph"))
            {
                Destroy(obj);
            }
            if (Camera.main.GetComponent<evolution_sim_v2>().generation%nx==0)
            {
                mult = Camera.main.GetComponent<evolution_sim_v2>().generation / (int)nx + 1;
            }
            MakeGraph(mult);
            pgeneration = Camera.main.GetComponent<evolution_sim_v2>().generation;
        }
    }
}
