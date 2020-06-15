using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuBtnScript : MonoBehaviour
{

    public Button loginButton;
    public Button PlayButton;
    public Button CustomizeButton;
    public Button LeaderboardsButton;
    public Button logoutButton;
    public Button backButton;

    public GameObject gameSettings;

    //Check for login.
    public string username;
    public string password;
    public GameObject userInput;
    public GameObject passInput;
    public GameObject failText;

    bool validLogin = false;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    //Store userinfo
    static public string currentUser;

    //For scene fade.
    public GameObject fader;

    //QuickFix for tab to work.
    static public bool onLogin = true;

    //Debug mode for testing check.
    static public bool debugOn = true;
    int counter = 0;
    void Start()
    {

        if (onLogin == true)
        {
            userInput.GetComponent<InputField>().Select();
        }
        else
        {
            if (debugOn == true)
            {

            }
            else
            {
                audioManagerMusic = GameObject.FindWithTag("MusicManager");
                audioManagerSFX = GameObject.FindWithTag("SFXManager");
            }

        }


    }
    public void LoadMenu()
    {
        username = userInput.GetComponent<InputField>().text;
        password = passInput.GetComponent<InputField>().text;
        //Call this function to check the DB for valid credentials.

        StartCoroutine(Login(username, password));


    }


    IEnumerator Login(string user, string pass)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", user);
        form.AddField("password", pass);
        //WWW www = new WWW("https://web.njit.edu/~mrk38/MiniLogin.php", form);
        WWW www = new WWW("https://web.njit.edu/~rp553/MiniLogin.php", form);
        yield return www;

        if (www.text == "1")
        {
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            validLogin = true;
            failText.SetActive(false);
            currentUser = username;
            onLogin = false;
            debugOn = false;
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Login_Button");
            fader.GetComponent<Scene_Fade>().FadeToLevel("PlayerMenu");
            yield return new WaitForSeconds(0.5f);
            audioManagerMusic.GetComponent<AudioManager>().Play("MenuMusic");
            // SceneManager.LoadScene("PlayerMenu"); //Loads PlayerMenu Scene
            Debug.Log("Login successful! PHP: " + www.text);
            counter += 1;
        }
        else
        {
            Debug.Log("oh no , login failed. php message: " + www.text);
            Debug.Log("User or pass is incorrect!");
            failText.SetActive(true);

            userInput.GetComponent<InputField>().Select();
        }

    }

    public IEnumerator grabStats()
    {

        WWWForm form2 = new WWWForm();
        if (MenuBtnScript.debugOn == true)
        {
            currentUser = "Katheryne";
        }
        form2.AddField("username", currentUser);
        //WWW www = new WWW("https://web.njit.edu/~mrk38/PlayerStats.php", form2);
        WWW www2 = new WWW("https://web.njit.edu/~rp553/PlayerStats.php", form2);
        yield return www2;

        //Grab the array from PHP , using commas to split each value. Convert them to Integers so we can use 'em in Unity for stat manip.
        string[] Sstats = www2.text.Split(',');
        int[] stats = new int[Sstats.Length];
        for (int i = 0; i < Sstats.Length; i++)
        {
            stats[i] = int.Parse(Sstats[i]);
            //  Debug.Log("The value of a string: " + Sstats[i] + " is now an int that is: " + stats[i]);
        }

        // Debug.Log(Sstats.Length);


        PlayerStats.healthLevel = stats[0];
        PlayerStats.ammoLevel = stats[1];
        PlayerStats.superMeterLevel = stats[2];
        PlayerStats.multiLevel = stats[3];
        PlayerStats.pointsRemaining = stats[4];
        PlayerStats.maxPoints = stats[5];
        fader.GetComponent<Scene_Fade>().FadeToLevel("TestMap");
        // SceneManager.LoadScene("TestMap");
    }

    void Update()
    {
        if (onLogin == true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (userInput.GetComponent<InputField>().isFocused)
                {
                    passInput.GetComponent<InputField>().Select();
                }
                else if (passInput.GetComponent<InputField>().isFocused)
                {
                    userInput.GetComponent<InputField>().Select();
                }
            }


            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadMenu();
            }
        }


    }

    public void PlayBtn()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
            audioManagerMusic.GetComponent<AudioManager>().Stop("MenuMusic");

        }

        StartCoroutine(grabStats());

    }

    public void CustomizeBtn()
    {
        audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
        fader.GetComponent<Scene_Fade>().FadeToLevel("PlayerCustomization");
        //SceneManager.LoadScene("PlayerCustomization");
    }

    public void LeaderboardsBtn()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
        }
        fader.GetComponent<Scene_Fade>().FadeToLevel("Leaderboards");
        //SceneManager.LoadScene("Leaderboards");
    }

    public void LogoutBtn()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
            audioManagerMusic.GetComponent<AudioManager>().Play("In_between");
        }
        onLogin = true;
       
        fader.GetComponent<Scene_Fade>().FadeToLevel("PlayerLogin");
        // SceneManager.LoadScene("PlayerLogin");
    }

    public void BackBtn()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Back");
        }
        fader.GetComponent<Scene_Fade>().FadeToLevel("PlayerMenu");
        // SceneManager.LoadScene("PlayerMenu");
    }

    public void CreditsBtn()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Button_Confirm");
        }
        fader.GetComponent<Scene_Fade>().FadeToLevel("Credits");
    }

}