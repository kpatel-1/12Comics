using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playGuide : MonoBehaviour

{

    public Button How2Play;
    public GameObject Guide;

    // Start is called before the first frame update
    void Start()
    {
        Guide.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Guide.activeSelf)
        {
            if (Input.GetKey("escape"))
            {
                Guide.SetActive(false);
            }
        }
    }

    public void how2Play()
    {
        Guide.SetActive(true);
    }
}
