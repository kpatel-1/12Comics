using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_shooter : enemy
{
    //basic shooting declarations
    public GameObject bullet;
    Vector2 bullet_spawn;
    public float fireRate;
    private float myTime = 0f;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    // Start is called before the first frame update
    void Start()
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
    }

    // Update is called once per frame
    void Update()
    {
        bullet_spawn = new Vector2(this.transform.position.x - .5f, this.transform.position.y);
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        shoot_basic();
        MaxHealth();
    }

    void shoot_basic()
    {
        myTime += Time.deltaTime;

        if (myTime >= fireRate)
        {
            //Debug.Log(Enemy.name + "has spawned");
            Instantiate(bullet, bullet_spawn, Quaternion.identity);
            if (MenuBtnScript.debugOn == true)
            {
                //Randomness for debug purposes.
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
