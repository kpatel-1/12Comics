using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Player : MonoBehaviour
{
    //movement declarations
    public float speed;
    float horizontal, vertical;
    Vector2 direction;

    //shooting declarations
    public GameObject shot;
    public Transform shot_spawn;
    public float fireRate;
    private float nextFire;
    private float myTime = 0f;


    int bultype = 1;
    Vector3 bullet_rotation1;
    Vector3 bullet_rotation2;
    public Text shot_Text;
    bool canShoot = true;

    //This list can be expanded in the inspector for whenever we add new characters to the game!!!!
    public List<GameObject>shots = new List<GameObject>();
    public List<string> names = new List<string>();
    int whichShot;

    //Invincibility after hit & Player UI image damage
    Renderer rend;
    Color color;
    public Image playerImg;
    bool gameOverAni = false;

    //For gameOver Buttons
    public GameObject gameOverPrefab;
    public GameObject returnCustomButton;
    public GameObject returnToMenuBtn;
    public GameObject GameOverUI;

    //Customization stats being accounted for here.
    static public int ammolvl;

    static public int multiplierlvl;

    //Supermeter vars
    static public int supermeterlvl;
    public float superCast;
    public float superMeterCurrent;
    public Text superMeterText;
    public GameObject enemyManager;
    public SupermeterBar superBar;
    bool usingMeter = false;
    public GameObject meterInfoText;
    bool meterFull = false;

    //Shake on City Hit vars
    public City city;
    public int current_cityHealth;
    public CameraShake cameraShake;

    //Shot type icon vars
    public Image single;
    public Image split;
    public Image burst;

    public Text OneShot;
    public Text TwoShot;
    public Text ThreeShot;

    //Figuring out who currentUser is and getting ready to change images based on that.
    public string current;
  

    //Get audioManager components!
    public GameObject audioManagerMusic;
    public GameObject audioManagerSFX;


    //Misc effects
    public GameObject medKitParticle;
    public GameObject superMeterParticle;
    public GameObject playerDeathParticle;

    void Awake()
    {
        ScoreCount.scoreValue = 0;
        
    }
    void Start()
    {
        canShoot = true;
        //Ensures that even if restarting the scene after being invincible, that the player can still take damage.
        Physics2D.IgnoreLayerCollision(0, 10, false);

        if (MenuBtnScript.debugOn == true)
        {
            current = "Katheryne";
        }
        else
        {
            current = MenuBtnScript.currentUser;
            //Maybe put this on Awake() to avoid potential overlap? Idk we'll test and see.
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");

            audioManagerMusic.GetComponent<AudioManager>().Stop("Boss_Fight");

            audioManagerMusic.GetComponent<AudioManager>().Play("GameplayMusic_DAY");
        }

        userInitialize();
        enemyManager = GameObject.Find("EnemyManager");
        ammolvl = PlayerStats.ammoLevel;
        supermeterlvl = PlayerStats.superMeterLevel;
        superCast = (float)supermeterlvl;
        multiplierlvl = PlayerStats.multiLevel;
        //superMeterCurrent = 100f;
        superMeterCurrent = 0;
        superMeterText.text = "SUPERMETER: " + superMeterCurrent + "%";
        rend = GetComponent<Renderer>();
        color = rend.material.color;

        meterInfoText.SetActive(false);
        gameOverPrefab.SetActive(false);
        returnCustomButton.SetActive(false);
        returnToMenuBtn.SetActive(false);
        GameOverUI.SetActive(false);

        bullet_rotation1 = new Vector3(0, 0, 12);
        bullet_rotation2 = new Vector3(0, 0, -12);


        current_cityHealth = city.city_health;

        superBar.SetMaxSuper(100);

    }


    void Update()
    {
        if(canShoot == true)
        {
            shoot();
        }
        superMeterUse();
        if (superMeterCurrent >= 100)
        {
            if(meterFull == false)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Supermeter_Charged");
                meterFull = true;
            }
            meterInfoText.SetActive(true);
        }
        else
        {
            meterInfoText.SetActive(false);
        }

        //City damage.
        if (city.city_health < current_cityHealth)
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("City_Hit");
            StartCoroutine(cameraShake.Shake(.15f, .4f));
            current_cityHealth = city.city_health;
        }

       
    }
    void FixedUpdate()
    {
       // superMeterCurrent = 100f;
        Movement();
        

        superBar.SetSuper(superMeterCurrent);

        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -7.7f, 4.9f),
            Mathf.Clamp(transform.position.y, -3.3f, 3.6f)
            );
    }

    //====================================
    //              functions
    //====================================

    /*
     * @brief this allows the character to move around the screen
     */

    //Who are we playing as. Change UI images, shot graphics, etc.
    void userInitialize()
    {
        if (Resources.Load<Sprite>("Hero_UI_Images/" + current) != null)
        {
            playerImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Hero_UI_Images/" + current);
        }
        else
        {
            playerImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Hero_UI_Images/12Comics_Logo");
        }

        if (Resources.Load<Sprite>("Hero_Gameplay_Sprites/" + current) != null)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/" + current);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/katheryne");
        }

        int numList;
        string currentName;
        //Find out what current player's rank is.
        string lowerName = current.ToLower();
        numList = names.Count();
        List<string> lowerNames = new List<string>();

        for (int m = 0; m < numList; m++)
        {
            currentName = names[m].ToLower();
            lowerNames.Add(currentName);
        }
        //Figure out which shot graphic to use.
        whichShot = (lowerNames.IndexOf(lowerName));
        Debug.Log("Shot we will use is: " + whichShot);

        Destroy(this.GetComponent<PolygonCollider2D>());
        this.gameObject.AddComponent<PolygonCollider2D>();
    }
    void Movement()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        direction = new Vector2(horizontal, vertical);

        this.transform.Translate(direction * speed * Time.deltaTime);
    }

    /*
     * @brief shooting and switching the ammo types
     */
    void shoot()
    {

        //Temp statement just to show that different ammo levels affect the type of shots you can use
        //This can probably be condensed. Let me know if you guys figure out a way to do so.
        switch (ammolvl)
        {

            //Level 1, only access to basic shot.
            case 1:
                bultype = 1;
                //shot_Text.text = "Shot Type: Basic";

                single.enabled = true;
                split.enabled = false;
                burst.enabled = false;

                OneShot.gameObject.SetActive(true);
                TwoShot.gameObject.SetActive(false);
                ThreeShot.gameObject.SetActive(false);


                myTime = myTime + Time.deltaTime;
                if (Input.GetKey(KeyCode.Space) && myTime > nextFire)
                {
                    nextFire = myTime + fireRate;
                    Instantiate(shots[whichShot], shot_spawn.position, shot_spawn.rotation);
                    
                    if (MenuBtnScript.debugOn == true)
                    {

                    }
                    else
                    {
                        audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
                    }
                    break;
                }
                break;

            //Level 2, access to basic shot and split shot.
            case 2:

                single.enabled = true;
                split.enabled = true;
                burst.enabled = false;
                OneShot.gameObject.SetActive(true);
                TwoShot.gameObject.SetActive(true);
                ThreeShot.gameObject.SetActive(false);

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if(bultype != 1)
                    {
                        bultype = 1;
                        //For debug mode.
                        if (MenuBtnScript.debugOn == true)
                        {

                        }
                        else
                        {
                            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Change_Shot");
                        }
                    }
                   
                   
                    //shot_Text.text = "Shot Type: Basic";
                    single.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    split.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (bultype != 2)
                    {
                        bultype = 2;
                        //For debug mode.
                        if (MenuBtnScript.debugOn == true)
                        {

                        }
                        else
                        {
                            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Change_Shot");
                        }
                    }
                    //shot_Text.text = "Shot Type: 3 Split";
                    single.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                    split.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
                myTime = myTime + Time.deltaTime;
                if (Input.GetKey(KeyCode.Space) && myTime > nextFire)
                {
                    switch (bultype)
                    {
                        case 1:
                            nextFire = myTime + fireRate;
                            Instantiate(shots[whichShot], shot_spawn.position, shot_spawn.rotation);
                            if (MenuBtnScript.debugOn == true)
                            {

                            }
                            else
                            {
                                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
                            }
                            break;

                        case 2:
                            nextFire = myTime + (fireRate + 0.25f);
                           StartCoroutine(tripleShot());
                            break;
                    }
                }
                break;

            //Level 3, access to third shot type as well.
            case 3:

                single.enabled = true;
                split.enabled = true;
                burst.enabled = true;
                OneShot.gameObject.SetActive(true);
                TwoShot.gameObject.SetActive(true);
                ThreeShot.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (bultype != 1)
                    {
                        bultype = 1;
                        //For debug mode.
                        if (MenuBtnScript.debugOn == true)
                        {

                        }
                        else
                        {
                            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Change_Shot");
                        }
                    }
                    //shot_Text.text = "Shot Type: Basic";
                    single.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    split.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                    burst.GetComponent<Image>().color = new Color32(55, 55, 55, 255);

                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (bultype != 2)
                    {
                        bultype = 2;
                        //For debug mode.
                        if (MenuBtnScript.debugOn == true)
                        {

                        }
                        else
                        {
                            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Change_Shot");
                        }
                    }
                    //shot_Text.text = "Shot Type: 3 Split";
                    single.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                    split.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                    burst.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    if (bultype != 3)
                    {
                        bultype = 3;
                        //For debug mode.
                        if (MenuBtnScript.debugOn == true)
                        {

                        }
                        else
                        {
                            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Change_Shot");
                        }
                    }
                    //shot_Text.text = "Shot Type: Burst";
                    single.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                    split.GetComponent<Image>().color = new Color32(55, 55, 55, 255);
                    burst.GetComponent<Image>().color = new Color32(255, 0, 0, 255);
                }
                myTime = myTime + Time.deltaTime;
                if (Input.GetKey(KeyCode.Space) && myTime > nextFire)
                {
                    switch (bultype)
                    {
                        case 1:
                            nextFire = myTime + fireRate;
                            Instantiate(shots[whichShot], shot_spawn.position, shot_spawn.rotation);
                            if (MenuBtnScript.debugOn == true)
                            {

                            }
                            else
                            {
                                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
                            }
                            break;

                        case 2:
                            nextFire = myTime + (fireRate + 0.25f);
                            StartCoroutine(tripleShot());
                            break;
                        case 3:
                            nextFire = myTime + (fireRate + 0.3f);
                            StartCoroutine(BurstShot());
                            break;
                    }
                }
                break;

        }

        nextFire = nextFire - myTime;
        myTime = 0f;
    }

    //Triple-shot coroutine.

    public IEnumerator tripleShot()
    {
        Instantiate(shots[whichShot], shot_spawn.position, Quaternion.identity);
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
        }
        GameObject ex1 = (GameObject)(Instantiate(shots[whichShot], shot_spawn.position, Quaternion.EulerAngles(bullet_rotation1)));
        GameObject ex2 = (GameObject)(Instantiate(shots[whichShot], shot_spawn.position, Quaternion.EulerAngles(bullet_rotation2)));
        yield return new WaitForSeconds(0.05f);
    }
    //Burst shot couroutine.
    public IEnumerator BurstShot()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(shots[whichShot], shot_spawn.position, shot_spawn.rotation);
            if (MenuBtnScript.debugOn == true)
            {

            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Shot");
            }
            yield return new WaitForSeconds(0.05f);
        }

    }


    //======================================
    //              collisions
    //======================================

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //player collides with boss
        if (collision.gameObject.tag == "Boss")
        {
            PlayerHealth.health -= 1;
            StartCoroutine("PicUIDamage");
            collision.gameObject.GetComponent<Boss>().health -= 1;
            this.transform.position = new Vector2(this.transform.position.x - 1, this.transform.position.y);
            if (PlayerHealth.health > 0)
            {
                StartCoroutine("PlayerInvince");
            }
            if (PlayerHealth.health <= 0)
            {
                gameOver();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //when player collides with a BasicEnemy
        if (collision.gameObject.tag == "BasicEnemy")
        {
            collision.gameObject.GetComponent<enemy>().onDeath();
            PlayerHealth.health -= 1;         
            StartCoroutine("PicUIDamage");
            if (PlayerHealth.health > 0)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Hit");
                StartCoroutine("PlayerInvince");
            }
            if (PlayerHealth.health <= 0)
            {

                gameOver();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Gold_enemy")
        {
            collision.gameObject.GetComponent<enemy>().onDeath();
            //Gold enemy does no damage to player.
        }

        //player collides with enemy bullet
        if (collision.gameObject.tag == "enemy_shot")
        {
            PlayerHealth.health -= 1;
            StartCoroutine("PicUIDamage");
            if (PlayerHealth.health > 0)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Hit");
                StartCoroutine("PlayerInvince");
            }
            if (PlayerHealth.health <= 0)
            {
                gameOver();
            }
            Destroy(collision.gameObject);
        }

        //when player collides with the med kit
        if (collision.tag == "medKit")
        {
            Instantiate(medKitParticle, transform.position, transform.rotation);
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Health_Pickup");
            //Let medkit give score so there's still incentive even if your health is full to go for it. Base it on their current health and multiplyer level.
            ScoreCount.scoreValue += ((PlayerHealth.health * 10) * multiplierlvl);
            PlayerHealth.health += collision.gameObject.GetComponent<medKit>().health;
            Destroy(collision.gameObject); 
        }

        if (collision.gameObject.tag == "Laser_Beam")
        {
            PlayerHealth.health -= 1;
            StartCoroutine("PicUIDamage");
            if (PlayerHealth.health > 0)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Hit");
                StartCoroutine("PlayerInvince");
            }
            if (PlayerHealth.health <= 0)
            {
                gameOver();
            }
        }
    }

    //Player U.I. turns red during damage.
    public IEnumerator PicUIDamage()
    {
        playerImg.GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(1.0f);
        playerImg.GetComponent<Image>().color = new Color(255, 255, 255);
    }

    //Invincibility state when getting hit.
    public IEnumerator PlayerInvince()
    {
        canShoot = false;
        //Change to hurt sprite for a moment.
        if (Resources.Load<Sprite>("Hero_Gameplay_Sprites/Hurt/" + current + "Hurt") != null)
        {

            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/Hurt/" + current + "Hurt");
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/Hurt/katheryneHurt");
        }

        Physics2D.IgnoreLayerCollision(0, 10, true);
        color.a = 0.5f;
        rend.material.color = color;
        yield return new WaitForSeconds(0.8f);

        canShoot = true;
        //Change back to regular sprite.
        if (Resources.Load<Sprite>("Hero_Gameplay_Sprites/" + current) != null)
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/" + current);
        }
        else
        {
            this.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Hero_Gameplay_Sprites/katheryne");
        }
        /*
        Physics2D.IgnoreLayerCollision(0, 10, true);
        color.a = 0.5f;
        rend.material.color = color;
        */
        yield return new WaitForSeconds(1.6f);
        Physics2D.IgnoreLayerCollision(0, 10, false);
        color.a = 1f;
        rend.material.color = color;
    }

    //======================================
    //              PlayerDeath
    //======================================

    public void gameOver()
    {
        PauseMenu.canPause = false;
        canShoot = false;
        PlayerHealth.health = 0;
        RetryButtonScript.score = ScoreCount.scoreValue;
       
        //Destroy all player shots so there are no collisions after player dies.
        GameObject[] theShots = GameObject.FindGameObjectsWithTag("player_shot");
        for (int i = 0; i < theShots.Length; i++)
        {
            GameObject.Destroy(theShots[i]);
        }

        //Create particle effect on death.
        if(gameOverAni == false)
        {
            Instantiate(playerDeathParticle, transform.position, transform.rotation);
            Destroy(gameObject.GetComponent<PolygonCollider2D>());
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            usingMeter = true;
            stopGameplayMusic();

            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Player_Death");
            gameOverAni = true;
            StartCoroutine(deathAni());
        }
        
        

    }

    IEnumerator deathAni()
    {
       
        yield return new WaitForSeconds(2.0f);
        audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Gameover_Voice");
        gameOverPrefab.SetActive(true);
        returnCustomButton.SetActive(true);
        returnToMenuBtn.SetActive(true);
        GameOverUI.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        audioManagerMusic.GetComponent<AudioManager>().Play("GameOver");
        Destroy(gameObject);
    }


    //======================================
    //              SuperMeter
    //======================================

    //Charge the supermeter based on the superMeter level.
    //Since we're doing shot collision seperately i'm making this public. We'll change it to private later.
    public void superMeterCharge(float s)
    {
        if (superMeterCurrent < 100f)
        {
            superMeterCurrent += s * supermeterlvl;
            if (superMeterCurrent >= 100f)
            {
                superMeterCurrent = 100f;
            }
            superMeterText.text = "SUPERMETER: " + superMeterCurrent + "%";
        }
        else
        {
            superMeterCurrent = 100f;
            superMeterText.text = "SUPERMETER: " + superMeterCurrent + "%";
        }
    }

    //Use the supermeter and reset it's value.
    private void superMeterUse()
    {

        if (Input.GetKeyDown(KeyCode.Alpha4) && usingMeter == false)
        {
            if (superMeterCurrent >= 100f)
            {
                //Play the animation of supermeter.
                StartCoroutine(superAni());
            }
            else
            {
                Debug.Log("Supermeter isn't full yet!");
            }
        }
    }

    //Animation for using the supermeter. Might not even need this if we add events to the animation.
    public IEnumerator superAni()
    {
        //Enable this as true so player can't spam '4' key and continue using meter.
        usingMeter = true;
        Instantiate(superMeterParticle, new Vector3(0,0,0), transform.rotation);
        audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Supermeter_Use");
        Physics2D.IgnoreLayerCollision(0, 10, true);
        Physics2D.IgnoreLayerCollision(0, 8, true);

        yield return new WaitForSeconds(1.8f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("BasicEnemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<enemy>().onDeath();
            ScoreCount.scoreValue += (5 * multiplierlvl);
            GameObject.Destroy(enemies[i]);
            enemyManager.GetComponent<enemy_manager>().enemiesKilled_total += 1;
            enemyManager.GetComponent<enemy_manager>().enemiesKilled_current += 1;
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Enemy_Death");
        }
        superMeterCurrent = 0f;
        superMeterText.text = "SUPERMETER: " + superMeterCurrent + "%";
        usingMeter = false;
        meterFull = false;
       
        //Give player a moment of invincibility after animation ends so they can get their bearings.
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(0, 10, false);
        Physics2D.IgnoreLayerCollision(0, 8, false);
       
    }


    //======================================
    //        Gameplay Music control
    //======================================

    public void stopGameplayMusic()
    {
        audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_DAY");
        audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_NIGHT");
        audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_SUNSET");
        audioManagerMusic.GetComponent<AudioManager>().Stop("Boss_Fight");
        audioManagerMusic.GetComponent<AudioManager>().Stop("GameOver");
        //Boss music
      //  audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_NIGHT");
       // audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_SUNSET");
    }



}
