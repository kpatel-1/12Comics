using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelManager : MonoBehaviour
{
    public GameObject Panel;
    public Button closePanel;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Panel.SetActive(false);
        }
    }
    
    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;

            Panel.SetActive(!isActive);
            
            
        }

    }

    public void exitPanel()
    {
        if (Panel.activeSelf)
        {
            Panel.SetActive(false);
        }
    }

    
}
    
