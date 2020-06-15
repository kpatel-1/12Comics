using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_BulletShower : MonoBehaviour
{
    float x1 = -7.5f;
    float x2 = 4.5f;
    float x;

    float time = 0;
    public float dropRate;

    public GameObject laser;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;


    // Start is called before the first frame update
    void Start()
    {
        if (MenuBtnScript.debugOn == true)
        {
          
        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        x = Random.RandomRange(x1, x2);
        Vector2 laser_spawn = new Vector2(x ,this.transform.position.y);

        if (time >= dropRate)
        {
            //Debug.Log(Enemy.name + "has spawned");
            Instantiate(laser, laser_spawn, Quaternion.identity);
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss2_Laser_Drop");
            time = 0;
        }
    }
}
