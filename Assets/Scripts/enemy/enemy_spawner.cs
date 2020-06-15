using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enemy_spawner : MonoBehaviour
{

    public GameObject enemyManager;
    public enemy[] Enemies;
    public GameObject Boss;
    public GameObject Boss2;
    int bossCount = 0;

    public float x_position;
    public float y_zone;
    public float timeInBetween;
    float timer;

    bool spawning;
    public float waveKills;
    public float maxEnemiesOnscreen;
    bool on = true;
    public bool boss_alive = false;

    //wave count declarations
    public int wave_count;
    public Text wave_text;
    bool wave_bool;

    //specific waves
    int heavy_count = 0;
    int shooter_count = 0;

    //background change, boss change
    public GameObject[] backgrounds;
    int current_background = 0;
    int changeCount = 0;
    int musicCount = 0;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    enum WaveState
    {
        WAVE,
        ENEMY,
        BOSS,
        RESET
    }
    WaveState waveState;

    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager");
        audioManagerMusic = GameObject.FindWithTag("MusicManager");
        audioManagerSFX = GameObject.FindWithTag("SFXManager");

        spawning = true;
        timer = 0;

        wave_count = 0;
        wave_bool = true;
        wave_text.gameObject.SetActive(false);

        waveState = WaveState.WAVE;
    }

    // Update is called once per frame
    void Update()
    {
        switch (waveState)
        {
            case WaveState.WAVE:
                StartCoroutine("wave_show");
                break;

            case WaveState.ENEMY:
                spawn_wave();
                spawn_boss_check();
                break;

            case WaveState.BOSS:
                if (boss_alive == false)
                    waveState = WaveState.RESET;
                break;

            case WaveState.RESET:
                waveKills_increase();
                enemyManager.GetComponent<enemy_manager>().enemiesKilled_current = 0;
                on = true;
                wave_bool = true;
                spawning = true;
                if (wave_count % 4 == 0)
                    timeInBetween -= .05f;

                waveState = WaveState.WAVE;
                break;
        }
    }

    //============================
    //          Functions
    //============================

    void spawn_wave()
    {
        enemy enemy_toSpawn;
        int chosen = Random.RandomRange(0, 100);
        int goldSpawn = Random.RandomRange(0, 15000);
        int pattern = Random.RandomRange(0, 100);

        if (goldSpawn >= 14999)
        {
            enemy_toSpawn = Enemies[3];
            Instantiate(enemy_toSpawn, new Vector2(x_position, this.transform.position.y + Random.RandomRange((0 - y_zone), y_zone)), Quaternion.identity);
        }

        //wave 1: basic only
        if (wave_count == 1)
            enemy_toSpawn = Enemies[0];

        //wave 2: heavy only
        else if (wave_count == 2)
        {
            if (heavy_count < 2)
            {
                heavy_count++;
                enemy_toSpawn = Enemies[2];
            }
            else 
            {
                enemy_toSpawn = Enemies[0];
                heavy_count = 0;
            }
        }

        //wave 3: shooter only
        else if (wave_count == 3)
        {
            if (shooter_count < 2)
            {
                shooter_count++;
                enemy_toSpawn = Enemies[1];
            }
            else
            {
                enemy_toSpawn = Enemies[0];
                shooter_count = 0;
            }
        }

        //wave 4 and over: everything
        else
        {
            if (chosen <= 80)
                enemy_toSpawn = Enemies[0];
            else if (chosen > 80 && chosen <= 90)
                enemy_toSpawn = Enemies[1];
            else
                enemy_toSpawn = Enemies[2];
        }
        
        //once too many enemies spawn, it stops
        if (spawning && enemyManager.GetComponent<enemy_manager>().active_enemies.Count <= maxEnemiesOnscreen)
            timer += Time.deltaTime;

        //actual act of spawning
        if (timer >= timeInBetween)
        {
            if (pattern <= 95)
                Instantiate(enemy_toSpawn, new Vector2(x_position, this.transform.position.y + Random.RandomRange((0 - y_zone), y_zone)), Quaternion.identity);
            else 
            {
                spawning = false;

                int pattern_select = Random.RandomRange(1, 4);
                if(pattern_select == 1)
                    StartCoroutine("X_pattern", enemy_toSpawn);
                if (pattern_select == 2)
                    StartCoroutine("T_pattern", enemy_toSpawn);
                if (pattern_select == 3)
                    StartCoroutine("V_pattern_left", enemy_toSpawn);
                if (pattern_select == 4)
                    StartCoroutine("V_pattern_right", enemy_toSpawn);

                StartCoroutine("RestartSpawning");
            }
            timer = 0;
        }
    }

    void spawn_boss_check()
    {        
        if (enemyManager.GetComponent<enemy_manager>().enemiesKilled_current >= waveKills)
        {
            spawning = false;
            if (enemyManager.GetComponent<enemy_manager>().active_enemies.Count > 0)
            {

                //Debug.Log("finish the enemies");
            }
            else
            {
                if (on)
                {
                    if (wave_count ==  3 + (4 * changeCount))
                    {
                        bossCount++;
                        GameObject Boss_to_Spawn;
                        if (bossCount % 2 == 0)
                            Boss_to_Spawn = Boss2;
                        else
                            Boss_to_Spawn = Boss;

                        changeCount++;
                        on = false;
                        StartCoroutine("spawn_boss", Boss_to_Spawn);
                    }
                    else
                    {
                        waveState = WaveState.RESET;
                    }
                }
            }
        }
    }

    void waveKills_increase()
    {
        //going onto wave 2
        if (wave_count == 1)
            waveKills = 15;

        //going onto wave 3
        if (wave_count == 2)
        {
            waveKills = 20;
            timeInBetween = 1.25f;
        }

        //going onto wave 4
        if (wave_count == 3)
        {
            waveKills = 35;
            timeInBetween = .75f;
        }

        //going onto wave 5 and onward
        if (wave_count >= 4)
            waveKills *= 1.15f;

    }

    
    void changeBackground()
    {
        if (wave_count % 4 == 0)
        {
            musicCount++;
            current_background++;
            if (current_background > backgrounds.Length - 1)
                current_background = 0;

            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].gameObject.SetActive(false);
            }
            //Telling the game when to fade the music into the next time of day.

            //Sunset Music
            if (musicCount == 1)
            {
              
                StartCoroutine(fadeMusic());
            }
            //Nighttime Music
            else if (musicCount == 2)
            {
             
                StartCoroutine(fadeMusic());
            }
            //Daytime Music
            else
            {
              
                StartCoroutine(fadeMusic());
            }
            if (musicCount == 3)
            {
                musicCount = 0;
            }
            backgrounds[current_background].gameObject.SetActive(true);
        }
    }

    //============================
    //          Coroutines
    //============================

    IEnumerator spawn_boss(GameObject Boss)
    {
        yield return new WaitForSeconds(1);
        boss_alive = true;

        //For debug mode check.
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            StartCoroutine(fadeMusic());
        }
        
        Instantiate(Boss, new Vector2(0, 0), Quaternion.identity);
        waveState = WaveState.BOSS;
    }

    //Makeshift audo fader...Yeah probably should do something better than this in the future lol.
    IEnumerator fadeMusic()
    {
        if(musicCount == 1 && boss_alive == false)
        {
            audioManagerMusic.GetComponent<AudioManager>().Play("GameplayMusic_SUNSET");
            yield return new WaitForSeconds(1.0f);
            audioManagerMusic.GetComponent<AudioManager>().Stop("Boss_Fight");
        }
        else if(musicCount == 2 && boss_alive == false)
        {
            audioManagerMusic.GetComponent<AudioManager>().Play("GameplayMusic_NIGHT");
            yield return new WaitForSeconds(1.0f);
            audioManagerMusic.GetComponent<AudioManager>().Stop("Boss_Fight");
        }
        else if(musicCount == 3 && boss_alive == false)
        {
            audioManagerMusic.GetComponent<AudioManager>().Play("GameplayMusic_DAY");
            yield return new WaitForSeconds(1.0f);
            audioManagerMusic.GetComponent<AudioManager>().Stop("Boss_Fight");
        }
        else
        {
            audioManagerMusic.GetComponent<AudioManager>().Play("In_between");
            yield return new WaitForSeconds(1.0f);
            audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_DAY");
            audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_SUNSET");
            audioManagerMusic.GetComponent<AudioManager>().Stop("GameplayMusic_NIGHT");
            audioManagerMusic.GetComponent<AudioManager>().Play("Boss_Fight");
        }
       
    }

    IEnumerator wave_show()
    {
        if (wave_bool)
        {
            wave_bool = false;
            wave_count++;
            wave_text.text = "Wave: " + wave_count;
            wave_text.gameObject.SetActive(true);
            if (wave_count == 1)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Ready_Voice");
            }
            
            yield return new WaitForSeconds(2);

            if (wave_count == 1)
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Go_Voice");
            }
            changeBackground();
            wave_text.gameObject.SetActive(false);
            waveState = WaveState.ENEMY;
        }
    }

    IEnumerator X_pattern(enemy enemy_toSpawn)
    {
        float y_spot = Random.RandomRange((-y_zone) + 1, y_zone-1);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + 1), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - 1), Quaternion.identity);

        yield return new WaitForSeconds(.5f);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        yield return new WaitForSeconds(.5f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + 1), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - 1), Quaternion.identity);
    }

    IEnumerator T_pattern(enemy enemy_toSpawn)
    {
        float y_spot = Random.RandomRange((-y_zone) + 1, y_zone - 1);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);

        yield return new WaitForSeconds(.25f);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + .5f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + 1f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - .5f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - 1f), Quaternion.identity);
        yield return new WaitForSeconds(.25f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
    }

    IEnumerator V_pattern_left(enemy enemy_toSpawn)
    {
        float y_spot = Random.RandomRange((-y_zone) + 1, y_zone - 1);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        yield return new WaitForSeconds(.25f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + .5f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - .5f), Quaternion.identity);
        yield return new WaitForSeconds(.25f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + 1f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - 1f), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
    }

    IEnumerator V_pattern_right(enemy enemy_toSpawn)
    {
        float y_spot = Random.RandomRange((-y_zone) + 1, y_zone - 1);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + 1f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - 1f), Quaternion.identity);
        yield return new WaitForSeconds(.25f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot + .5f), Quaternion.identity);
        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot - .5f), Quaternion.identity);
        yield return new WaitForSeconds(.25f);

        Instantiate(enemy_toSpawn, new Vector2(x_position, y_spot), Quaternion.identity);
        yield return new WaitForSeconds(.25f);
    }

    IEnumerator RestartSpawning()
    {
        yield return new WaitForSeconds(1f);
        spawning = true;
    }
}
