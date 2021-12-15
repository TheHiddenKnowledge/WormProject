using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphController : MonoBehaviour
{
    //Determines whether the graph(s) are shown or not
    bool show = false;
    //The gameobject for the graph handler 
    public GameObject GraphHandler;
    private void Update()
    {
        //When the G key is pressed the show variable is switched from true to false and vice versa 
        if (Input.GetKeyDown(KeyCode.G))
        {
            show = !show;
        }    
        //When the graph is to be shown the scale is interpolated from 0 to 1 
        if (show)
        {
            if (GraphHandler.transform.localScale.x < 1)
            {
                StartCoroutine(changeScale(1));
            }
            else
            {
                GraphHandler.transform.localScale = new Vector3(1f, 1f, 0f);
            }
        }
        //When the graph is to be shown the scale is interpolated from 1 to 0 
        else
        {
            if (GraphHandler.transform.localScale.x > 0)
            {
                StartCoroutine(changeScale(-1));
            }
            else
            {
                GraphHandler.transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }
    }
    //This function is used to interpolate the scale of a gameobject
    IEnumerator changeScale(int i)
    {
        GraphHandler.transform.localScale += new Vector3(.1f, .1f, 0f)*i;
        yield return new WaitForSeconds(.1f);
    }
}
