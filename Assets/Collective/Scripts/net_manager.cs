using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetRunner;
public class net_manager : MonoBehaviour
{
    public Net net = new Net(2,2,1,4);
    public GameObject worm;
    public bool change = false;
    double fitness; 
    float maxtorsion = 45f / 180f * Mathf.PI;
    // Start is called before the first frame update
    void DebugNet()
    {
        for (int i = 0; i < net.synapses.Count; i++)
        {
            for (int j = 0; j < net.synapses[i].Count; j++)
            {
                string text = "";
                for (int k = 0; k < net.synapses[i][j].Count; k++)
                {
                    text += net.synapses[i][j][k];
                    text += " ";
                }
                Debug.Log(text);
            }
            Debug.Log("");
        }
    }
    public void LearnNet()
    {
        net.fitness = fitness;
        //net.CheckFitness();
        net.Mutate();
        fitness = 0;
    }
    void Start()
    {
        net.RandomGenes();
        net.inputs = new double[2]{ 0, 0};
        DebugNet();
    }

    // Update is called once per frame
    void Update()
    {
        if (worm)
        {
            fitness += 1;
            net.In2Out();
            if (net.outputs[0] > 5f * Mathf.PI)
            {
                worm.GetComponent<create_worm_v2>().frequency = 5f * Mathf.PI;
            }
            else if (net.outputs[0] < 2.5f * Mathf.PI)
            {
                worm.GetComponent<create_worm_v2>().frequency = 2.5f * Mathf.PI;
            }
            else
            {
                worm.GetComponent<create_worm_v2>().frequency = (float)net.outputs[0];
            }
            if(worm.GetComponent<create_worm_v2>().normals.Length != 0)
            {
                if (net.outputs[1] > maxtorsion)
                {
                    worm.GetComponent<create_worm_v2>().normals[0] = Quaternion.Euler(0, 0, maxtorsion) * worm.GetComponent<create_worm_v2>().normals[0];
                }
                else if (net.outputs[1] < -maxtorsion)
                {
                    worm.GetComponent<create_worm_v2>().normals[0] = Quaternion.Euler(0, 0, -maxtorsion) * worm.GetComponent<create_worm_v2>().normals[0];
                }
                else
                {
                    worm.GetComponent<create_worm_v2>().normals[0] = Quaternion.Euler(0, 0, (float)net.outputs[1]) * worm.GetComponent<create_worm_v2>().normals[0];
                }
            }
        }
    }
}
