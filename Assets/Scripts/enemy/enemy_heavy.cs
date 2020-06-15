using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_heavy : enemy
{
    Vector3 pos;
    public float magnitude;
    public float frequency;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;
    private void Start()
    {
        //Checking if we're in debugMode or not.
        if (MenuBtnScript.debugOn == true)
        {
            //Randomness for debug purposes.
        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
        }

        findComponents();
        pos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        pos -= transform.right * Time.deltaTime * speed;
        transform.position = pos + transform.up * Mathf.Sin(Time.time * frequency) * magnitude;
        MaxHealth();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            killed_by_player();
        }

        if (collision.tag == "player_shot")
        {
            //Calculate score based on current multiplier. If multiplier will change throughout gameplay, we will need to use another reference than PlayerStats.multiLevel to store the multiplier.
            
        
            enemy_health--;
            if (enemy_health <= 0)
            {
                if (MenuBtnScript.debugOn == true)
                {
                    //Randomness for debug purposes.
                }
                else
                {
                    audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Enemy_Death");
                }
                ScoreCount.scoreValue += (10 * multiBonus);
                player.GetComponent<Player>().superMeterCharge(0.5f);
                killed_by_player();
            }

            else
            {
                if (MenuBtnScript.debugOn == true)
                {
                    //Randomness for debug purposes.
                }
                else
                {
                    audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Enemy_Hit");
                }
                StartCoroutine("flash");
            }
            Destroy(collision.gameObject);
        }

        if (collision.tag == "Despawner")
        {
            city.GetComponent<City>().city_health -= 1;
            onDeath();
        }
    }
}
