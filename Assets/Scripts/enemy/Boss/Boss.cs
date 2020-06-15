using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Renderer renderer;
    public int health;
    int maxHealth;
    int stage_health;
    bool invince;
    bool isAlive;
    Animator anim;
    GameObject eSpawn;

    //City vars 
    public City city;
    public int cityMax;

    //Score vars
    static public int multiBonus;

    //basic shooting declarations
    public GameObject bullet;
    Vector2 bullet_spawn;
    public float fireRate;
    private float myTime = 0f;

    //entrance
    bool inEntrance = true;

    //Stage 2
    public GameObject defenseBlock_1;
    public GameObject defenseBlock_2;

    //Stage 3
    public GameObject offenseBlock_1;
    public GameObject offenseBlock_2;

    //For Boss death
    public GameObject particleDestruct;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    enum BossStage
    {
        ENTRANCE,
        STAGE_1,
        STAGE_2,
        STAGE_3
    } BossStage bossStage;

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

        multiBonus = PlayerStats.multiLevel;

        anim = GetComponent<Animator>();
        renderer = GetComponent<Renderer>();
        eSpawn = GameObject.Find("WaveManager");
        city = GameObject.Find("Despawn_Enemy").GetComponent<City>();
        //isAlive = true;
        maxHealth = health;
        stage_health = maxHealth / 3;

        defenseBlock_1.SetActive(false);
        defenseBlock_2.SetActive(false);
        offenseBlock_1.SetActive(false);
        offenseBlock_2.SetActive(false);

        bossStage = BossStage.ENTRANCE;

        cityMax = city.city_health_Max;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bullet_spawn = new Vector2(this.transform.position.x - .5f, this.transform.position.y);

        switch (bossStage)
        {
            case BossStage.ENTRANCE:
                StartCoroutine("entrance_timer");
                Debug.Log("ENTRANCE");
                break;

            case BossStage.STAGE_1:
                invince = false;
                if (health <= stage_health * 2)
                    bossStage = BossStage.STAGE_2;

                shoot_basic();
                break;

            case BossStage.STAGE_2:
                defenseBlock_1.SetActive(true);
                defenseBlock_2.SetActive(true);
                if (health <= stage_health)
                    bossStage = BossStage.STAGE_3;

                shoot_basic();
                break;

            case BossStage.STAGE_3:
                defenseBlock_1.SetActive(false);
                defenseBlock_2.SetActive(false);
                offenseBlock_1.SetActive(true);
                offenseBlock_2.SetActive(true);

                shoot_basic();
                break;
        }

        if (health <= 0)
        {

            if (city.city_health < cityMax)
            {
                city.city_health += 1;
            }

            ScoreCount.scoreValue += (1000 * multiBonus);
            //isAlive = false;
            //reset_boss();
            eSpawn.GetComponent<enemy_spawner>().boss_alive = false;
            //this.gameObject.SetActive(false);

            //Create particle effect on destruction.
            Instantiate(particleDestruct, transform.position, transform.rotation);
            if (MenuBtnScript.debugOn == true)
            {
                //Randomness for debug purposes.
            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss_Death");
            }
           
            //Add destruction on particle effect after a certain time.

            Destroy(this.gameObject);
        }
    }

    //=================================
    //          Functions
    //=================================

    //is called in enemy_spawner
    public bool isAlive_check()
    {
        return isAlive;
    }

    //is called in enemy_spawner
    public void reset_boss()
    {
        health = maxHealth;
        bossStage = BossStage.STAGE_1;
        //isAlive = true;
        defenseBlock_1.SetActive(false);
        defenseBlock_2.SetActive(false);
        offenseBlock_1.SetActive(false);
        offenseBlock_2.SetActive(false);
        inEntrance = true;
        invince = true;
    }

    // allows the Boss to shoot at the player with basic shots
    void shoot_basic()
    {
        myTime += Time.deltaTime;

        if (myTime >= fireRate)
        {
            //Debug.Log(Enemy.name + "has spawned");
            Instantiate(bullet, bullet_spawn, Quaternion.identity);
            if (MenuBtnScript.debugOn == true)
            {

            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss_Shot");
            }
            myTime = 0;
        }
    }

    //=================================
    //          Coroutines
    //=================================

    IEnumerator flash()
    {
        if (MenuBtnScript.debugOn == true)
        {
            //Randomness for debug purposes.
        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss_Hit");
        }
        renderer.material.color = Color.white;
        yield return new WaitForSeconds(.1f);
        renderer.material.color = new Color(255,255,255, 125);
        yield return new WaitForSeconds(.1f);
        renderer.material.color = Color.white;        
    }

    //real entrance is an animation of the boss flying in from the right
    IEnumerator entrance_timer()
    {
        if (inEntrance)
        {
            invince = true;
            inEntrance = false;
            yield return new WaitForSeconds(2f);
            invince = false;
            bossStage = BossStage.STAGE_1;
        }
    }
    
    //=================================
    //          Collision
    //=================================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player_shot")
        {
            if (invince)
            {
                Destroy(collision.gameObject);
            }
            else 
            {
                health--;
                StartCoroutine("flash");
                Destroy(collision.gameObject);
            }
        }
    }
}
