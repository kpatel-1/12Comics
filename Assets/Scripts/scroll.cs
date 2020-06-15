using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scroll : MonoBehaviour
{
    public float speed = .5f;
    Material material;
    Vector2 offset;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        offset = new Vector2(speed, 0);
        material.mainTextureOffset += offset * Time.deltaTime;
    }
}
