using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    static public int healthLevel = 1;
    static public int ammoLevel = 1;
    static public int superMeterLevel = 1;
    static public int multiLevel = 1;
    static public int maxPoints = 20;
    static public int pointsRemaining = 10;
    //User who's currently logged in.
    public string currentUse;

    public Text healthText;
    public Text ammoText;
    public Text superMeterText;
    public Text multiplierText;
    public Text pointsRemainingText;

    //For scene fade.
    public GameObject fader;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    // Start is called before the first frame update
    void Start()
    {
        if (MenuBtnScript.debugOn == true)
        {
            currentUse = "Katheryne";
            setStats();
            initialDisplay();

        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
            currentUse = MenuBtnScript.currentUser;
            //Check the DB here for the grades and correspond that to the points given to student.
            setStats();
            initialDisplay();
        }
    }

  


    // Update is called once per frame
    void Update()
    {

    }


    /*
     ///////////////////////////////// 
     ******GETTING PLAYER STATS HERE***
     /////////////////////////////////
    */


    /*
   ------COROUTINES FOR GRABBING/SAVING STATS FROM THE DB-------
   */

    public IEnumerator grabStats()
    {

        WWWForm form = new WWWForm();
        form.AddField("username", currentUse);
        // WWW www = new WWW("https://web.njit.edu/~mrk38/PlayerStats.php", form);
        WWW www = new WWW("https://web.njit.edu/~rp553/PlayerStats.php", form);
        yield return www;

        //Grab the array from PHP , using commas to split each value. Convert them to Integers so we can use 'em in Unity for stat manip.
        string[] Sstats = www.text.Split(',');
        int[] stats = new int[Sstats.Length];
        for (int i = 0; i < Sstats.Length; i++)
        {
            stats[i] = int.Parse(Sstats[i]);
            //  Debug.Log("The value of a string: " + Sstats[i] + " is now an int that is: " + stats[i]);
        }

        // Debug.Log(Sstats.Length);


        healthLevel = stats[0];
        ammoLevel = stats[1];
        superMeterLevel = stats[2];
        multiLevel = stats[3];
        pointsRemaining = stats[4];
        maxPoints = stats[5];

        initialDisplay();
    }

    IEnumerator saveToDB()
    {
        WWWForm form2 = new WWWForm();
        form2.AddField("username", currentUse);
        form2.AddField("health", healthLevel);
        form2.AddField("ammo", ammoLevel);
        form2.AddField("superMeter", superMeterLevel);
        form2.AddField("multi", multiLevel);
        form2.AddField("pointsR", pointsRemaining);
        // WWW www2 = new WWW("https://web.njit.edu/~mrk38/SaveStats.php", form2);
        WWW www2 = new WWW("https://web.njit.edu/~rp553/SaveStats.php", form2);
        yield return www2;

        Debug.Log("PHP Message: " + www2.text);
    }

    //Based on who is logged in, the stats will be allocated accordingly. Obviously we'll save these stats into the DB so they're loaded in correctly each time but for now this is just to test.
    public void setStats()
    {
        StartCoroutine(grabStats());
    }

    /*
     /////////////////////////////////
     ****BUTTON FUNCTIONS BELOW*****
     /////////////////////////////////
     */


    /* Amount of starting HEALTH during gameplay
    *******************************************
    *
    */
    public void healthIncrease(int health)
    {
        //Make it so Healthlevel cannot exceed a maximum of 10.
        if (pointsRemaining <= 0 || healthLevel >= 10)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_UP");
            healthLevel += 1;
            pointsRemaining -= 1;
        }

        if (healthLevel == 10)
        {
            healthText.text = "Health level: " + healthLevel + " (MAX)";
        }

        else
        {
            healthText.text = "Health level: " + healthLevel;
        }
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;

        //\n (You begin with 10 health **MIGHT USE THIS UNDER EACH SECTION TO GIVE BETTER INDICATION OF WHAT HAPPENS BASED ON LEVEL**
    }

    public void healthDecrease(int health)
    {
        //Health can't go lower than 1.
        if (healthLevel <= 1)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_DOWN");
            healthLevel -= 1;
            pointsRemaining += 1;
        }
        healthText.text = "Health level: " + healthLevel;
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }

    /* Determines AMMO types available during gameplay 
    ************************************************
    *
    */
    public void ammoIncrease(int ammo)
    {
        //For now only 3 ammo types so yeah.
        if (pointsRemaining <= 0 || ammoLevel >= 3)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_UP");
            ammoLevel += 1;
            pointsRemaining -= 1;
        }

        if (ammoLevel == 3)
        {
            ammoText.text = "Ammo level: " + ammoLevel + " (MAX)";
        }

        else
        {
            ammoText.text = "Ammo level: " + ammoLevel;
        }
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }


    public void ammoDecrease(int ammo)
    {
        //Can't go lower than 1!
        if (ammoLevel <= 1)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_DOWN");
            ammoLevel -= 1;
            pointsRemaining += 1;
        }

        ammoText.text = "Ammo level: " + ammoLevel;
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }
    /* Rate of SUPERMETER growth during gameplay 
     *******************************************
     *
     */
    public void superMeterIncrease(int supMeter)
    {
        //Temp max for superMeter
        if (pointsRemaining <= 0 || superMeterLevel >= 5)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_UP");
            superMeterLevel += 1;
            pointsRemaining -= 1;
        }

        if (superMeterLevel == 5)
        {
            superMeterText.text = "SuperMeter level: " + superMeterLevel + " (MAX)";
        }

        else
        {
            superMeterText.text = "SuperMeter level: " + superMeterLevel;
        }
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }

    public void superMeterDecrease(int supMeter)
    {
        //Can't go below 1!
        if (superMeterLevel <= 1)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_DOWN");
            superMeterLevel -= 1;
            pointsRemaining += 1;
        }
        superMeterText.text = "SuperMeter level: " + superMeterLevel;
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }

    /* Your score Multiplier. Won't be exact to the level. But we'll see how we want to balance this since score is important.
     *************************************************
     *
     */
    public void multiIncrease(int multi)
    {
        //Maybe x5? Not entirely sure yet on this.
        if (pointsRemaining <= 0 || multiLevel >= 5)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_UP");
            multiLevel += 1;
            pointsRemaining -= 1;
        }

        if (multiLevel == 5)
        {
            multiplierText.text = "Multiplier level: " + multiLevel + " (MAX)";
        }

        else
        {
            multiplierText.text = "Multiplier level: " + multiLevel;
        }
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }

    public void multiDecrease(int multi)
    {
        //Can't go below 1!
        if (multiLevel <= 1)
        {
            return;
        }

        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Customize_DOWN");
            multiLevel -= 1;
            pointsRemaining += 1;
        }
        multiplierText.text = "Multiplier level: " + multiLevel;
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }

    //Button click to actually play the game!
    public void Play()
    {
        StartCoroutine(saveToDB());
        fader.GetComponent<Scene_Fade>().FadeToLevel("TestMap");

        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
            audioManagerMusic.GetComponent<AudioManager>().Stop("MenuMusic");

        }
      
        // SceneManager.LoadScene("TestMap");
    }

    public void Back()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Back");
        }
        StartCoroutine(saveToDB());
        fader.GetComponent<Scene_Fade>().FadeToLevel("PlayerMenu");
        // SceneManager.LoadScene("PlayerMenu");
    }


    void initialDisplay()
    {
        if (healthLevel == 10)
        {
            healthText.text = "Health level: " + healthLevel + " (MAX)";
        }
        else
        {
            healthText.text = "Health level: " + healthLevel;
        }

        if (ammoLevel == 3)
        {
            ammoText.text = "Ammo level: " + ammoLevel + " (MAX)";
        }
        else
        {
            ammoText.text = "Ammo level: " + ammoLevel;
        }
        if (superMeterLevel == 5)
        {
            superMeterText.text = "SuperMeter level: " + superMeterLevel + " (MAX)";
        }
        else
        {
            superMeterText.text = "SuperMeter level: " + superMeterLevel;
        }
        if (multiLevel == 5)
        {
            multiplierText.text = "Multiplier level: " + multiLevel + " (MAX)";
        }
        else
        {
            multiplierText.text = "Multiplier level: " + multiLevel;
        }
        pointsRemainingText.text = "Points remaining: " + pointsRemaining;
    }
}
