using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_offenseBlock : MonoBehaviour
{
    float myTime = 0;
    public float fireRate;
    public GameObject bullet;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;
    private void Start()
    {
        //Debug mode check.
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
        }
    }

     void Update()
    {
        shoot_basic();
    }
    void shoot_basic()
    {
        myTime += Time.deltaTime;

        if (myTime >= fireRate)
        {
            //Debug.Log(Enemy.name + "has spawned");
            Instantiate(bullet, this.transform.position, Quaternion.identity);

            if (MenuBtnScript.debugOn == true)
            {
                
            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Enemy_Shoot");
            }

            myTime = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player_shot")
        {
            //Check for debug mode to play sound or not.
            if (MenuBtnScript.debugOn == true)
            {

            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss_shield_hit");
            }
            Destroy(collision.gameObject);
        }
    }
}
