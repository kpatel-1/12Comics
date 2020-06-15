using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class Leaderboard : MonoBehaviour
{

    public Button Personal;
    public Button MySchool;
    public Button Global;
    public GameObject Search;
    public GameObject CurrentRank;
    //public GameObject rankings;
    public GameObject rank;
    public GameObject uname;
    public GameObject hname;
    public GameObject uscore;


    static public string theCurrentUser;
    static public string theCurrentHero;

    //Arrays for the leaderboard components.
    List<string> theNames = new List<string>();
    List<string> theHeroes = new List<string>();
    List<int> theScores = new List<int>();
    List<int> theRanks = new List<int>();

    string theText1;
    string theText2;
    string theText3;
    string theText4;
    string theText5;

    string theRank;
    string theHero;
    string theScr;

    int currentHighScore;
    int playerRank;



    // Start is called before the first frame update
    void Start()
    {
        theNames = new List<string>();
        theScores = new List<int>();
        if (MenuBtnScript.debugOn == true)
        {
            theCurrentUser = "Katheryne";
            currentHighScore = 0;
            theCurrentHero = "Kat warrior";
        }
        else
        {
            currentHighScore = LoginDisp.highScore;
            theCurrentUser = MenuBtnScript.currentUser;
            theCurrentHero = LoginDisp.currHero;
        }

        StartCoroutine(getNames());
        Search.SetActive(false);
        //CurrentRank.SetActive(false);

    }



    public void PersonalBoard()
    {

        Search.SetActive(false);
        CurrentRank.SetActive(false);
    }

    public void MySchoolBoard()
    {
        Search.SetActive(true);
        CurrentRank.SetActive(true);
    }

    public void GlobalBoard()
    {
        StartCoroutine(getNames());
        StartCoroutine(getScores());
        Search.SetActive(true);
        CurrentRank.SetActive(true);
    }

    /*
     * --COROUTINES!!!!--
     ******************
     */

    public IEnumerator getNames()
    {

        //WWW www = new WWW("https://web.njit.edu/~mrk38/LeaderboardNamesAll.php");
        WWW www = new WWW("https://web.njit.edu/~rp553/LeaderboardNamesAll.php");
        yield return www;

        string[] users = www.text.Split(',');


        for (int i = 0; i < users.Length; i++)
        {
            theNames.Add(users[i]);

        }

        //Wait here before calling getHeroes() because otherwise the name list may not be fully populated yet!
        yield return new WaitForFixedUpdate();
        StartCoroutine(getHeroes());

    }
    public IEnumerator getHeroes()
    {

        //WWW www2 = new WWW("https://web.njit.edu/~mrk38/LeaderboardHeroesAll.php");
        WWW www2 = new WWW("https://web.njit.edu/~rp553/LeaderboardHeroesAll.php");
        yield return www2;

        string[] heroes = www2.text.Split(',');


        for (int i = 0; i < heroes.Length; i++)
        {
            theHeroes.Add(heroes[i]);

        }

        //Wait here before calling getScores() because otherwise the hero list may not be fully populated yet!
        yield return new WaitForFixedUpdate();
        StartCoroutine(getScores());

    }

    public IEnumerator getScores()
    {

        //WWW www3 = new WWW("https://web.njit.edu/~mrk38/LeaderboardScoresAll.php");
        WWW www3 = new WWW("https://web.njit.edu/~rp553/LeaderboardScoresAll.php");
        yield return www3;

        string[] scores = www3.text.Split(',');

        for (int i = 0; i < scores.Length; i++)
        {
            theScores.Add((int.Parse(scores[i])));
            theRanks.Add((i + 1));
        }
        //Call the function to display the ranks to the screen.

        sortRanks();
    }

    //Sort the scores and display them.
    void sortRanks()
    {
        //Take the lists that were formed via the Coroutines, and use them to print the leaderboard data to the screen in descending order.

        int numList;
        int currentNum;
        int userIndex = 0;
        string currentName;
        string currentHero;

        numList = theScores.Count;

        for (int i = 0; i < numList - 1; i++)
        {
            for (int j = i + 1; j < numList; j++)
            {
                if (theScores[i] < theScores[j])
                {
                    //Actual sorting of arrays here to ensure everything matches up.
                    currentNum = theScores[i];
                    currentName = theNames[i];
                    currentHero = theHeroes[i];

                    theScores[i] = theScores[j];
                    theScores[j] = currentNum;

                    theNames[i] = theNames[j];
                    theNames[j] = currentName;

                    theHeroes[i] = theHeroes[j];
                    theHeroes[j] = currentHero;
                }
            }

            //theText1 = theText1 + "\t \t" + theRanks[i] + "\t \t \t \t" + theNames[i] + "\t" + theHeroes[i] + "\t \t \t \t" + theScores[i] + " \t \t \t \n";
            theText2 = theText2 + "" + theRanks[i] + "\n";
            theText3 = theText3 + "" + theNames[i] + "\n";
            theText4 = theText4 + "" + theHeroes[i] + "\n";
            theText5 = theText5 + "" + theScores[i] + "\n";

        }

        //Find out what current player's rank is.
        string lowerName = theCurrentUser.ToLower();
        List<string> lowerNames = new List<string>();

        for (int m = 0; m < numList; m++)
        {
            currentName = theNames[m].ToLower();
            lowerNames.Add(currentName);
        }
        userIndex = (lowerNames.IndexOf(lowerName) + 1);
        playerRank = (theRanks.IndexOf(userIndex) + 1);
        //rankings.GetComponent<Text>().text = theText1;
        rank.GetComponent<Text>().text = theText2;
        uname.GetComponent<Text>().text = theText3;
        hname.GetComponent<Text>().text = theText4;
        uscore.GetComponent<Text>().text = theText5;



        CurrentRank.GetComponent<Text>().text = "" + (playerRank);

    }


}