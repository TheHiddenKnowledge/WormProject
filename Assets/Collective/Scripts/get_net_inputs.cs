using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class get_net_inputs : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject target;
    Vector3 off;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food" && target == null)
        {
            target = collision.gameObject;
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        Vector3 norm = transform.parent.GetComponent<create_worm_v2>().normals[1];
        transform.up = norm;
        if (screenPos.x < 0 || screenPos.x > Screen.width)
        {
            transform.parent.GetComponent<create_worm_v2>().normals[0] = new Vector3(-norm.x, norm.y, norm.z);
        }
        if (screenPos.y < 0 || screenPos.y > Screen.height)
        {
            transform.parent.GetComponent<create_worm_v2>().normals[0] = new Vector3(norm.x, -norm.y, norm.z);
        }
        if (target != null)
        {
            off = target.transform.position - transform.position;
            Camera.main.GetComponent<net_manager>().net.inputs = new double[2] { off.x, off.y };
        }
    }
}
