using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    public static bool canPause = true;
    public GameObject PauseMenuUI;
    public GameObject GOver;
    public GameObject H2P;

    public GameObject PanelCustomize;
    public GameObject PanelMenu;

    //Game Settings vars
    public GameObject settingsMenu;
    public Button settingsExit;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;


    void Start()
    {
        canPause = true;

        if (MenuBtnScript.debugOn == true)
        {
           
        }
        else
        {         
         
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");          
        }

        PauseMenuUI.SetActive(false);
        settingsMenu.SetActive(false);
        
    }
    // Update is called once per frame
    void Update()
    {

        if (PauseMenuUI.activeSelf) {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                settingsMenu.SetActive(false);
            }
        }

       if (Input.GetKeyDown(KeyCode.Escape) && canPause == true)
             {
                PanelMenu.SetActive(false);
                PanelCustomize.SetActive(false);

                if (GameIsPaused)
                {

                    Resume();
                }
                else
                {

                    Pause();
                }
                if (MenuBtnScript.debugOn == true)
                {

                }
                else
                {

                    audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Pause");
                }
            }

            if (GOver.activeSelf)
            {
                Resume();
            }

            if (H2P.activeSelf)
            {
                Pause();
            }
    }

    void Resume()
    {
        
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

    void Pause()
    {
       
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void gameSettings()
    {
        if (settingsMenu != null)
        {
            bool isActive = settingsMenu.activeSelf;

            settingsMenu.SetActive(!isActive);
        }
    }

    public void closeSettings()
    {
        if (settingsMenu.activeSelf)
        {
            settingsMenu.SetActive(false);
        }
    }
}
