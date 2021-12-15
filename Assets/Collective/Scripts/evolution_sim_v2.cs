using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class evolution_sim_v2 : MonoBehaviour
{
    //Arrays containing information on objects spawned, the amount spawns, and whether the objects spawn constantly
    public GameObject[] item;
    public int[] number;
    public bool[] constant;
    public bool[] rand;
    //Text gameobject for the generation count and the variable that keeps track of generation count 
    public Text gcount;
    public int generation = 0;
    //
    public float average = 0;
    //The variables denoted with "1" correspond to the timing of each generation, whereas the variables denoted
    //by "2" are used to time the spawning rate of spawned objects
    float nextActionTime1;
    float nextActionTime2;
    float period1 = 10f;
    float period2 = .25f;
    //The list that contains all spawned objects 
    public List<int[]> finalgenes = new List<int[]>();
    //Camera gameobjects and variables that will hold its in game dimensions, used for determining the spawning area
    Camera cam;
    float height;
    float width;
    //Variables that keep track of the simulation speed and changes in simulation speed 
    public Slider speed;
    float finalspeed;
    float initialspeed;
    //
    int[] geneslist = { 1, 2, 3, 4, 5 };
    //This function randomly creates a number of gameobjects within a defined area  
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
            float width_sp = Mathf.Abs(lower.x) * 1.25f * spawned.transform.localScale.x;
            float height_sp = Mathf.Abs(lower.y)* 1.25f * spawned.transform.localScale.x;
            int countx = (int)(width / width_sp);
            int county = (int)(height / height_sp);
            for (int i = 0; i < countx; i++) {
                for (int j = 0; j < county; j++)
                {
                    var clone = (GameObject)Instantiate(spawned.gameObject, new Vector3(((float)i / countx) * (xmax - xmin) + xmin+width_sp/2, ((float)j / county) * (ymax - ymin) + ymin + height_sp / 2, 0), Quaternion.Euler(0.0f, 0.0f, Random.Range(0f, 360f)));
                }
            }
        }
    } 
    void GeneRandom() {
        average = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
        {
            obj.GetComponent<create_worm_v2>().speed = speed.value;
            int gene1 = geneslist[Random.Range(0, geneslist.Length)];
            int gene2 = geneslist[Random.Range(0, geneslist.Length)];
            int dominantgene = 0;
            obj.GetComponent<create_worm_v2>().genes[0] = gene1;
            obj.GetComponent<create_worm_v2>().genes[1] = gene2;
            if (gene1 > gene2 || gene1 == gene2)
            {
                dominantgene = gene1;
            }
            else if (gene2 > gene1)
            {
                dominantgene = gene2;
            }
            float max = 0;
            if (dominantgene == 2)
            {
                max = 2.15f;
            }
            else if (dominantgene == 3)
            {
                max = 2f;
            }
            else if (dominantgene == 5)
            {
                max = 1.75f;
            }
            else if (dominantgene == 4)
            {
                max = 1.5f;
            }
            else if (dominantgene == 1)
            {
                max = 1.25f;
            }
            obj.GetComponent<create_worm_v2>().max = max;
            average += max;
            
        }
        average /= number[1];
    }
    void GeneCross(List<int[]> wormgenes)
    {
        average = 0;
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
        {
            int[] worm1 = wormgenes[Random.Range(0, wormgenes.Count)];
            int[] worm2 = wormgenes[Random.Range(0, wormgenes.Count)];
            int[] newgene = { worm1[Random.Range(0, 2)], worm2[Random.Range(0, 2)] };
            obj.GetComponent<create_worm_v2>().speed = speed.value;
            int dominantgene = 0;
            obj.GetComponent<create_worm_v2>().genes[0] = newgene[0];
            obj.GetComponent<create_worm_v2>().genes[1] = newgene[1];
            if (newgene[0] > newgene[1] || newgene[0] == newgene[1])
            {
                dominantgene = newgene[0];
            }
            else if (newgene[1] > newgene[0])
            {
                dominantgene = newgene[1];
            }
            float max = 0;
            if (dominantgene == 2)
            {
                max = 2.15f;
            }
            else if (dominantgene == 3)
            {
                max = 2f;
            }
            else if (dominantgene == 5)
            {
                max = 1.75f;
            }
            else if (dominantgene == 4)
            {
                max = 1.5f;
            }
            else if (dominantgene == 1)
            {
                max = 1.25f;
            }
            obj.GetComponent<create_worm_v2>().max = max;
            average += max;
        }
        average /= number[1];
    }
    //In the Start function variables are intiated, the initial average is taken, the initial speed is set, and the gameobjects to be 
    //created are spawned 
    void Start()
    {
        //Initiating variables 
        period1 = period1 / speed.value;
        period2 = period2 / speed.value;
        nextActionTime1 = period1;
        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
        //Spawning all the designated gameobjects 
        for (int i = 0; i < item.Length; i++)
        {
            Spawn_stuff(-width / 2, width / 2, -height / 2, height / 2, number[i], item[i], rand[i]);
        }
        //Setting the speed for each worm and getting the average value from all worms 
        GeneRandom();
        gcount.text = "Generation: " + generation.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //When the speed value changes, each worms speed is updated accordingly 
        if (speed.value-initialspeed!=0)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
            {
                obj.GetComponent<create_worm_v2>().speed = speed.value;
            }
            initialspeed = speed.value;
        }
        //Whenever the spawn timer is activated, certain objects that spawn constantly are spawned in 
        for (int i = 0; i < item.Length; i++)
         {
            if (constant[i] && Time.time > nextActionTime2)
            {
                nextActionTime2 = Time.deltaTime + period2;
                Spawn_stuff(-width / 2, width / 2, -height / 2, height / 2, number[i], item[i], rand[i]);
            }
         }
        //Whenver the generation timer is activated, all gameobjects that were spawned in are deleted and the generation counter is updated as well
        //as the average of the generation. Then the gameobjects are spawned in again to create a new generation
        if (Time.time > nextActionTime1)
        {
            finalgenes.Clear();
            nextActionTime1 = Time.time + period1;
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Food"))
            {
                Destroy(obj);
            }
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Brain"))
            {
                finalgenes.Add(obj.GetComponent<create_worm_v2>().genes);
                DestroyImmediate(obj);
            }
            generation += 1;
            gcount.text = "Generation: "+generation.ToString();
            for (int i = 0; i < item.Length; i++)
            {
                Spawn_stuff(-width / 2, width / 2, -height / 2, height / 2, number[i], item[i], rand[i]);
            }
            GeneCross(finalgenes);
            initialspeed = speed.value;
        }
    }
}
