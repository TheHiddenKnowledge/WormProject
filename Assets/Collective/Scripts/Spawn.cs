using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public GameObject thing_spawned;
    GameObject spawnee;
    void OnBecameVisible()  
    {

        Spawnn();
    }
    
    void Spawnn()
    {
        
        spawnee = (GameObject)Instantiate(thing_spawned, transform.position, transform.rotation);
        spawnee.transform.parent = transform;

    }
    
    // Use this for initialization
    void Start () {
        
    }
    void OnBecameInvisible()
    {
        Destroy(spawnee);
        
    }
    // Update is called once per frame
    void Update () {
       
    }
}
