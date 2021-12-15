using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class head_movement : MonoBehaviour
{
    GameObject target;
    Vector3 start;
    Vector3 end;
    float t;
    float total=2*10;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Food" && target == null)
        {
            target = collision.gameObject;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);

        if (screenPos.x < 0 || screenPos.x > Screen.width )
        {
            transform.up = new Vector3(-transform.up.x, transform.up.y, transform.up.z);
        }
        if (screenPos.y < 0 || screenPos.y > Screen.height)
        {
            transform.up = new Vector3(transform.up.x, -transform.up.y, transform.up.z);
        }
        if (target != null)
        {
            if (t == 0)
            {
                start = transform.up;
                end = target.transform.position - transform.position;
            }
            if (t % 10 == 0)
            {
                transform.up = Vector3.Lerp(start, end, t / total);
            }

            t += 1;
            if (t == total)
            {
                t = 0;
            }
        }
    }
}
