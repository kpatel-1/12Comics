using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public float speed = 2;
    public int enemy_health;
    public int enemy_MaxHealth;
    static public int multiBonus;
    protected GameObject enemyManager;
    protected GameObject player;
    protected GameObject city;
    protected Renderer renderer;

    //health kit drop declarations
    public GameObject medKit;
    public float odds;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    //For enemy death
    public GameObject particleDestruct;

    //for difficulty progression
    GameObject WaveManager;
    int wave_count;
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
        difficulty_progression();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        MaxHealth();
    }

    //===============================
    //           Functions
    //===============================

    protected void findComponents()
    {
        enemyManager = GameObject.Find("EnemyManager");
        player = GameObject.Find("TestPlayer");
        multiBonus = PlayerStats.multiLevel;
        city = GameObject.Find("Despawn_Enemy");
        renderer = GetComponent<Renderer>();
        WaveManager = GameObject.Find("WaveManager");
        wave_count = WaveManager.GetComponent<enemy_spawner>().wave_count;

        if (this.tag != "Gold_enemy")
            enemyManager.GetComponent<enemy_manager>().active_enemies.Add(this);
    }
    public void onDeath()
    {
        enemyManager.GetComponent<enemy_manager>().active_enemies.Remove(this);

        if (this.tag == "Gold_enemy")
        {
            //gold enemy shouldn't drop med kits
        }
        else 
        {
            dropMed();
        }

        //Create particle effect on destruction.
        Instantiate(particleDestruct, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    void dropMed()
    {
        float med_num = Random.RandomRange(1, 100);
        float xrange = Random.RandomRange(-3.5f, 4);
        //percent = 1%
        if (med_num <= odds)
        {
            Debug.Log("here you go");
            Instantiate(medKit, new Vector2(xrange, 5), Quaternion.identity);
        }
    }

    protected void killed_by_player()
    {
        enemyManager.GetComponent<enemy_manager>().enemiesKilled_total += 1;
        enemyManager.GetComponent<enemy_manager>().enemiesKilled_current += 1;
        onDeath();
    }

    public void progressive_difficulty()
    {
        enemy_health += 1;
    }

    public void difficulty_progression()
    {
        //adding progressive difficuly by adding health... kind of bullshit tbh
        int progression_rate = wave_count / 4;
        if (progression_rate == 0)
        {
            //do nothing
        }
        else
        {
            enemy_health += progression_rate;
            speed += .1f * progression_rate;
        }      
    }

    protected void MaxHealth()
    {
        if (enemy_health > enemy_MaxHealth)
        {
            enemy_health = enemy_MaxHealth;
        }
    }

    //================================
    //          Collisions
    //================================

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

                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Enemy_Hit");
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

    //=================================
    //          Coroutines
    //=================================
    protected IEnumerator flash()
    {
        renderer.material.color = Color.white;
        yield return new WaitForSeconds(.1f);
        renderer.material.color = new Color(255, 255, 255, 125);
        yield return new WaitForSeconds(.1f);
        renderer.material.color = Color.white;
    }


}
