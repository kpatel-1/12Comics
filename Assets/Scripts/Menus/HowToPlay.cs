using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HowToPlay : MonoBehaviour

{

    public GameObject How2Play;
    //public Button h2p_button;

    public GameObject P1;
    public GameObject P2;
    public GameObject P3;
    public GameObject P4;
    public GameObject P5;

    public Button LeftBtn;
    public Button RightBtn;
    public Button XBtn;



    // Start is called before the first frame update
    void Start()
    {
        How2Play.SetActive(false);
        P1.SetActive(true);
        P2.SetActive(false);
        P3.SetActive(false);
        P4.SetActive(false);
        P5.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (How2Play.activeSelf)
        {
            if (P1.activeSelf) {
                if (Input.GetKeyDown("right"))
                {
                    PG2();
                }
                RightBtn.onClick.AddListener(PG2);
            }

            else if (P2.activeSelf)
            {
                if (Input.GetKeyDown("right"))
                {
                    PG3();
                }

                if (Input.GetKeyDown("left"))
                {
                    PG1();
                }
                RightBtn.onClick.AddListener(PG3);
                LeftBtn.onClick.AddListener(PG1);
            }

            else if (P3.activeSelf)
            {
                if (Input.GetKeyDown("right"))
                {
                    PG4();
                }

                if (Input.GetKeyDown("left"))
                {
                    PG2();
                }
                RightBtn.onClick.AddListener(PG4);
                LeftBtn.onClick.AddListener(PG2);
            }

            else if (P4.activeSelf)
            {
                if (Input.GetKeyDown("right"))
                {
                    PG5();
                }

                if (Input.GetKeyDown("left"))
                {
                    PG3();
                }
                RightBtn.onClick.AddListener(PG5);
                LeftBtn.onClick.AddListener(PG3);
            }

            else if (P5.activeSelf)
            {
                if (Input.GetKeyDown("left"))
                {
                    PG4();
                }
                LeftBtn.onClick.AddListener(PG4);
            }
            XBtn.onClick.AddListener(iEsc);

            if (Input.GetKeyDown("escape"))
            {
                iEsc();
            }
        }
    }

    public void H2P()
    {
        How2Play.SetActive(true);
        Debug.Log("Pressed");
    }


    public void PG1()
    {
        P1.SetActive(true);
        P2.SetActive(false);
        P3.SetActive(false);
        P4.SetActive(false);
        P5.SetActive(false);
    }
    public void PG2()
    {
        P1.SetActive(false);
        P2.SetActive(true);
        P3.SetActive(false);
        P4.SetActive(false);
        P5.SetActive(false);
    }
    public void PG3()
    {
        P1.SetActive(false);
        P2.SetActive(false);
        P3.SetActive(true);
        P4.SetActive(false);
        P5.SetActive(false);
    }
    public void PG4()
    {
        P1.SetActive(false);
        P2.SetActive(false);
        P3.SetActive(false);
        P4.SetActive(true);
        P5.SetActive(false);
    }
    public void PG5()
    {
        P1.SetActive(false);
        P2.SetActive(false);
        P3.SetActive(false);
        P4.SetActive(false);
        P5.SetActive(true);
    }
    public void iEsc()
    {
        How2Play.SetActive(false);
        P1.SetActive(true);
        P2.SetActive(false);
        P3.SetActive(false);
        P4.SetActive(false);
        P5.SetActive(false);
    }
}
