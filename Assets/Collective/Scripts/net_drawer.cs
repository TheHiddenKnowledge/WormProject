using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetRunner;
public class net_drawer : MonoBehaviour
{
    public GameObject GraphBG;
    public GameObject marker;
    public GameObject line;
    void MakeNet()
    {
        //Getting the in-game size of the graph background for reference later. 
        Net grab = Camera.main.GetComponent<net_manager>().net;
        List<List<Vector3>> points = new List<List<Vector3>>();
        List<Vector3> tempp = new List<Vector3>();
        Sprite sp = GraphBG.GetComponent<SpriteRenderer>().sprite;
        Vector3 lower = sp.bounds.min * 42;
        float width = Mathf.Abs(lower.x) * 2 - 30;
        float height = Mathf.Abs(lower.y) * 2 - 20;
        //Creating the background gameobject for the graph
        GameObject cloneBG = Instantiate(GraphBG, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
        cloneBG.transform.SetParent(transform);
        cloneBG.transform.localPosition = new Vector3(0f, 0f, 0f);
        cloneBG.transform.localScale = new Vector3(42, 42, 0); 
        for (int i = 0; i < grab.totalNeurons.Count; i++)
        {
            for(int j = 0; j < grab.totalNeurons[i]; j++)
            {
                Vector3 point = new Vector3(lower.x + width * (i) / (1.5f * (grab.totalNeurons.Count - 1)) + 85, -lower.y - height * j / (1.5f * (grab.totalNeurons[1] - 1)) - 75, 0f);
                GameObject cloneM = Instantiate(marker, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                cloneM.transform.position = transform.position;
                cloneM.transform.SetParent(transform);
                cloneM.transform.localScale = new Vector3(100f, 100f, 0);
                cloneM.transform.localPosition = point;
                tempp.Add(point);
            }
            points.Add(tempp);
            tempp = new List<Vector3>();
        }
        for(int i = 0; i < grab.synapses.Count; i++)
        {
            for (int j = 0; j < grab.synapses[i].Count; j++)
            {
                for (int k = 0; k < grab.synapses[i][j].Count; k++)
                {
                    if (grab.synapses[i][j][k] == 1)
                    {
                        Vector3 point1 = points[i][j];
                        Vector3 point2 = points[i + 1][k];
                        GameObject clone = Instantiate(line, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0));
                        clone.transform.position = transform.position;
                        clone.transform.SetParent(transform);
                        Vector3 point = (point1 + point2) / 2;
                        clone.transform.eulerAngles = new Vector3(0,0,180/Mathf.PI*Mathf.Atan2(point2.y-point1.y,point2.x-point1.x));
                        clone.transform.localScale = new Vector3(Vector3.Distance(point2,point1)*5, 50,0);
                        clone.transform.localPosition = point;
                        if (grab.weights[i][j][k] > 0)
                        {
                            clone.GetComponent<SpriteRenderer>().color = Color.red;
                        }
                        else
                        {
                            clone.GetComponent<SpriteRenderer>().color = Color.green;
                        }
                    }
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Graph"))
        {
            Destroy(obj);
        }
        MakeNet();
    }
}
