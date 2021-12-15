using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dirt_delete_v2 : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Body")
        {
            col.transform.parent.GetComponent<create_worm_v2>().hunger += 7.5f;
            Destroy(gameObject);
        }
    }
}
