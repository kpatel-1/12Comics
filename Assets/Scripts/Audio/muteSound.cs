using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class muteSound : MonoBehaviour
{
    static public float theVolume = 1f;
    static public bool muted = false;

    public Sprite OffSprite;
    public Sprite OnSprite;
    public Button mute;

    public Slider volSlide;
    

    
    void Update()
    {

        volSlide.value = theVolume;
        AudioListener.volume = theVolume;


        //Get a reference to the slider. If we mute the volume, make the slider value = to 0.
    }

    public void Mute()
    {
        if (theVolume == 1f)
        {
            muted = true;
            theVolume = 0f;
            mute.image.sprite = OffSprite;
        }
        else
        {
            muted = false;
            theVolume = 1f;
            mute.image.sprite = OnSprite;
        }
        //AudioListener.volume = yourVolume;

        //AudioListener.pause = !AudioListener.pause;
    }

    public void adjustVolume(float slideVolume)
    {
        if (slideVolume == 0)
        {
            muted = true;
            //This will be for us forcing the mute picture.
            mute.image.sprite = OffSprite;
        }
        else
        {
            muted = false;
            mute.image.sprite = OnSprite;
        }
        theVolume = slideVolume;

        Debug.Log("Volume is now: " + theVolume);

    }


}
