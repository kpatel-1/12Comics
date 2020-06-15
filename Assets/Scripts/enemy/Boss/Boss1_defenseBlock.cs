using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1_defenseBlock : MonoBehaviour
{
    public float speed;
    public bool up_down; //true = up, false = down

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    enum Direction
    { 
        UP,
        DOWN
    } Direction direction;

    void Start()
    {
        //Debug mode check.
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
        }


        if (up_down)
            direction = Direction.UP;
        else
            direction = Direction.DOWN;
    }

    // Update is called once per frame
    void Update()
    {
        switch (direction)
        {
            case Direction.UP:
                move_UP();
                if (this.transform.localPosition.y >= .6)
                    direction = Direction.DOWN;
                break;

            case Direction.DOWN:
                move_DOWN();
                if (this.transform.localPosition.y <= -.6)
                    direction = Direction.UP;
                break;
        }

        //move up
        if (this.transform.localPosition.y <= -.6)
            move_UP();

        //move down
        else if (this.transform.localPosition.y >= .6)
            move_DOWN();
    }

    void move_UP()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void move_DOWN()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "player_shot")
        {
            //Check for debug mode to play sound or not.
            if (MenuBtnScript.debugOn == true)
            {

            }
            else
            {
                audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Boss_shield_hit");
            }
            Destroy(collision.gameObject);
        }
    }
}
