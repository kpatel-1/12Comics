using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public int speed;
    public GameObject anchor_obj;
    Vector3 anchor_v3;
    Vector3 point;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anchor_v3 = anchor_obj.transform.position;
        point = this.transform.position;

        this.transform.RotateAround(anchor_v3, new Vector3(0,0,1), speed * Time.deltaTime);
    }
}
