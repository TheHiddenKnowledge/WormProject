using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirt_delete : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Head1")
        {
            col.transform.parent.GetChild(1).GetComponent<create_worm>().hunger += 7.5f;
            Destroy(gameObject);
        }
    }
}
