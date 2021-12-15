using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_delete_v2 : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        //When the worm head collides with the food object 
        if(col.tag == "Body")
        {
            col.enabled = false;
            col.enabled = true;
            col.transform.parent.GetComponent<create_worm_v2>().hunger += 10;
            Destroy(gameObject);
        }  
    }
}
