using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2 : MonoBehaviour
{
    //Boss 2 Declaration
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

    //Stage 1
    public GameObject shot;

    //Stage 2
    float stage2_timer = 0;
    float action_time;
    public GameObject top_ship;
    public GameObject mid_ship;
    public GameObject bot_ship;
    Animator shipT_anim;
    Animator shipM_anim;
    Animator shipB_anim;
    bool stage2_coroutine = true;
    bool stage2_shooting = true;
    int noBeam;
    public GameObject top_light,
                      mid_light,
                      bot_light;
    public GameObject top_Beam, 
                      mid_Beam, 
                      bot_Beam;

    //Stage 3_setup
    public GameObject bubble;
    public GameObject PE_ShootUP;
    ParticleSystem ps;

    //Stage 3
    public GameObject bulletShower;
    bool Stage3_coroutine = true;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    //Death particle
    public GameObject particleDestruct;

    enum BossStage
    {
        ENTRANCE,
        STAGE_1,
        STAGE_2_SETUP,
        STAGE_2,
        STAGE_3_SETUP,
        STAGE_3
    }
    BossStage bossStage;

    // Start is called before the first frame update
    void Start()
    {
        ps = PE_ShootUP.GetComponent<ParticleSystem>();
        bubble.SetActive(false);
        PE_ShootUP.SetActive(false);
        bulletShower.SetActive(false);

        bullet_spawn = new Vector2(this.transform.position.x - .03f, this.transform.position.y + .98f);
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

        bossStage = BossStage.ENTRANCE;

        cityMax = city.city_health_Max;

        action_time = 10f;
        shipT_anim = top_ship.GetComponent<Animator>();
        shipM_anim = mid_ship.GetComponent<Animator>();
        shipB_anim = bot_ship.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bullet_spawn = new Vector2(this.transform.position.x - .03f, this.transform.position.y + .98f);

        switch (bossStage)
        {
            case BossStage.ENTRANCE:
                StartCoroutine("entrance_timer");
                break;

            case BossStage.STAGE_1:
                invince = false;
                if (health <= stage_health * 2)
                    bossStage = BossStage.STAGE_2_SETUP;

                shoot_basic();
                break;

            case BossStage.STAGE_2_SETUP:
                StartCoroutine("stage2_beams");
                break;

            case BossStage.STAGE_2:
                stage2_timer += Time.deltaTime;
                if (stage2_timer > action_time)
                {
                    StartCoroutine("stage2_beams");
                }

                if (stage2_shooting)
                    shoot_basic();

                if (health <= stage_health)
                    bossStage = BossStage.STAGE_3_SETUP;
                break;

            case BossStage.STAGE_3_SETUP:
                StartCoroutine("stage3_blast");
                break;

            case BossStage.STAGE_3:
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

            Destroy(this.transform.parent.gameObject);
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
    //          Coroutine
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
        renderer.material.color = new Color(255, 255, 255, 125);
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

    IEnumerator stage2_beams()
    {
        if (stage2_coroutine)
        {
            stage2_coroutine = false;
            stage2_shooting = false;
            noBeam = Random.Range(1, 3);

            if (noBeam == 0)
                Debug.Log("Top and Mid");
            if (noBeam == 1)
                Debug.Log("Mid and Bottom");
            if (noBeam == 2)
                Debug.Log("Top and Bottom");

            //setup
            Animator shipT_anim = top_ship.GetComponent<Animator>();
            Animator shipM_anim = mid_ship.GetComponent<Animator>();
            Animator shipB_anim = bot_ship.GetComponent<Animator>();

            //becomes invincible as to not mess up coroutine, and flies up
            invince = true;
            anim.SetTrigger("up");
            yield return new WaitForSeconds(1.5f);

            //the beam ships fade in
            shipT_anim.SetTrigger("Fade_In");
            shipM_anim.SetTrigger("Fade_In");
            shipB_anim.SetTrigger("Fade_In");

            yield return new WaitForSeconds(.5f);

            //set up warning lights to stay away from
            if (noBeam == 1)
            {
                mid_light.SetActive(true);
                bot_light.SetActive(true);
            }
            else if (noBeam == 2)
            {
                top_light.SetActive(true);
                bot_light.SetActive(true);
            }
            else
            {

                mid_light.SetActive(true);
                top_light.SetActive(true);
            }
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss2_Laser_Charge");

            yield return new WaitForSeconds(2f);
            
            //The Beams are in full effect and damaging
            if (noBeam == 1)
            {
                mid_Beam.SetActive(true);
                bot_Beam.SetActive(true);
            }
            else if (noBeam == 2)
            {
                top_Beam.SetActive(true);
                bot_Beam.SetActive(true);
            }
            else
            {

                mid_Beam.SetActive(true);
                top_Beam.SetActive(true);
            }
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss2_Laser_Fire");

            yield return new WaitForSeconds(2f);

            //turns off the damaging beams and leaves the warning ones on
            top_Beam.SetActive(false);
            mid_Beam.SetActive(false);
            bot_Beam.SetActive(false);

            yield return new WaitForSeconds(1f);

            //warning ones now fade
            mid_ship.transform.GetChild(0).gameObject.SetActive(false);
            top_ship.transform.GetChild(0).gameObject.SetActive(false);
            bot_ship.transform.GetChild(0).gameObject.SetActive(false);

            yield return new WaitForSeconds(.5f);
            
            //ships now fade out
            shipT_anim.SetTrigger("Fade_Out");
            shipM_anim.SetTrigger("Fade_Out");
            shipB_anim.SetTrigger("Fade_Out");

            yield return new WaitForSeconds(1f);

            //ship comes back into screen
            anim.SetTrigger("stationary");

            yield return new WaitForSeconds(1f);

            //no longer invincible, and goes back to moving in its normal pattern
            invince = false;
            anim.SetTrigger("pattern");
            stage2_coroutine = true;
            stage2_shooting = true;
            stage2_timer = 0;
            if (bossStage == BossStage.STAGE_2_SETUP)
                bossStage = BossStage.STAGE_2;
        }
    }

    IEnumerator stage3_blast()
    {
        if (Stage3_coroutine == true)
        {
            Stage3_coroutine = false;
            //activates bubble to be invincible
            bubble.SetActive(true);
            invince = true;

            //returns to origin quickly and sprays shots upward
            anim.SetTrigger("stationary");
            PE_ShootUP.SetActive(true);
            yield return new WaitForSeconds(3f);

            //stops shooting upwards and stays still
            var emission = ps.emission;
            emission.rateOverTime = 0f;
            yield return new WaitForSeconds(1f);

            //moves like normal but now fire rains from the sky
            anim.SetTrigger("pattern");
            bulletShower.SetActive(true);

            //no more bubble, no more invincibility
            invince = false;
            bubble.SetActive(false);

            //stage 3 begins with slower fire rate because hell is already raining
            Stage3_coroutine = true;
            fireRate += 1;
            bossStage = BossStage.STAGE_3;
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
