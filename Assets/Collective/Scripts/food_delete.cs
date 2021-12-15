using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_delete : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        //When the worm head collides with the food object 
        if(col.tag == "Head1")
        {
            col.enabled = false;
            col.enabled = true;
            col.transform.parent.GetChild(1).GetComponent<create_worm>().hunger += 10;
            Destroy(gameObject);
        }  
    }
}
