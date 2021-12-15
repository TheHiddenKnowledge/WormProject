using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generate_course : MonoBehaviour
{
    public GameObject[] item;
    public int[] number;
    public bool[] rand;
    Camera cam;
    float height;
    float width;
    // Start is called before the first frame update
    void Spawn_stuff(float xmin, float xmax, float ymin, float ymax, int count, GameObject spawned, bool random)
    {
        if (random)
        {
            for (int i = 0; i < count; i++)
            {
                var clone = (GameObject)Instantiate(spawned.gameObject, (new Vector3((Random.Range(xmin, xmax)), (Random.Range(ymin, ymax)), 0.0f)), Quaternion.Euler(0.0f, 0.0f, 0.0f));
            }
        }
        else
        {
            Sprite sp = spawned.GetComponent<SpriteRenderer>().sprite;
            Vector3 lower = sp.bounds.min;
            float width_sp = Mathf.Abs(lower.x) * 2f * spawned.transform.localScale.x;
            float height_sp = Mathf.Abs(lower.y) * 2f * spawned.transform.localScale.x;
            int countx = (int)(width / width_sp);
            int county = (int)(height / height_sp);
            for (int i = 0; i < countx; i++)
            {
                for (int j = 0; j < county; j++)
                {
                    var clone = (GameObject)Instantiate(spawned.gameObject, new Vector3(((float)i / countx) * (xmax - xmin) + xmin + width_sp / 2, ((float)j / county) * (ymax - ymin) + ymin + height_sp / 2, 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0f, 360f)));
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<net_manager>().worm)
        {
            GetComponent<net_manager>().change = true;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
            {
                Destroy(obj);
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
            {
                DestroyImmediate(obj);
            }
            cam = Camera.main;
            height = 2f * cam.orthographicSize;
            width = height * cam.aspect;
            //Spawning all the designated gameobjects 
            for (int i = 0; i < item.Length; i++)
            {
                Spawn_stuff(-width / 2, width / 2, -height / 2, height / 2, number[i], item[i], rand[i]);
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
            {
                GetComponent<net_manager>().worm = obj;
                GetComponent<net_manager>().LearnNet();
            }
        }
        else
        {
            GetComponent<net_manager>().change = false;
        }
    }
}
