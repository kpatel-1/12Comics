using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCount : MonoBehaviour
{

    public static int scoreValue = 0;
    public Text scoreText;
    public Text multiplierText;
    public static int currentMulti;
    public Text highScoreText;
    int currentHigh;
    int theMax = 999999;
    bool newHigh = false;

    //Get audioManager components!
    public GameObject audioManagerMusic;
    public GameObject audioManagerSFX;

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

        highScoreText.text = "";
        if (MenuBtnScript.debugOn == true)
        {
            currentHigh = 99999;
        }
        else
        {
            currentHigh = LoginDisp.highScore;
        }
        currentMulti = PlayerStats.multiLevel;
        multiplierText.text = "Multiplier: x" + currentMulti;
        scoreText.text = "Score: " + scoreValue;

    }

    // Update is called once per frame
    void Update()
    {
        //If we let the multiplier change throughout gameplay, do that calculation here. Will need another reference to it so we don't mess with multiLevel though.
        if (scoreValue > theMax)
        {
            scoreValue = theMax;
        }
        //Check for if player got a new high score.
        if(newHigh == false)
        {
            if(scoreValue > currentHigh)
            {
                highScoreText.text = "New high score!";
                newHigh = true;
                if (MenuBtnScript.debugOn == true)
                {

                }
                else
                {
                    audioManagerSFX.GetComponent<AudioManagerSFX>().Play("New_High_Score");
                }
            }
        }

        scoreText.text = "Score: " + scoreValue;
    }

}
