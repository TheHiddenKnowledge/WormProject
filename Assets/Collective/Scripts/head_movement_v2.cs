using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class head_movement_v2 : MonoBehaviour
{
    public GameObject target;
    Vector3 start;
    Vector3 end;
    public float speed = 10f;
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
        Vector3 norm = transform.parent.GetComponent<create_worm_v2>().normals[1];
        transform.up = norm;
        if (screenPos.x < 0 || screenPos.x > Screen.width )
        {
            transform.parent.GetComponent<create_worm_v2>().normals[0] = new Vector3(-norm.x, norm.y, norm.z);
        }
        if (screenPos.y < 0 || screenPos.y > Screen.height)
        {
            transform.parent.GetComponent<create_worm_v2>().normals[0] = new Vector3(norm.x, -norm.y, norm.z);
        }
        if (target != null)
        {
            start = norm;
            end = target.transform.position - transform.position;
            end = end.normalized;
            transform.parent.GetComponent<create_worm_v2>().normals[0] = Vector3.RotateTowards(transform.parent.GetComponent<create_worm_v2>().normals[1], end,speed*Time.deltaTime,0f);
        }
    }
}
