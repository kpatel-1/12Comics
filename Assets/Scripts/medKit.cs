using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medKit : MonoBehaviour
{
    public int health;
    public float speed;
    public float drop;
    int count;
    Rigidbody2D rb;

    //Get audioManager components!
    GameObject audioManagerMusic;
    GameObject audioManagerSFX;

    private void Start()
    {
        if (MenuBtnScript.debugOn == true)
        {

        }
        else
        {
            audioManagerMusic = GameObject.FindWithTag("MusicManager");
            audioManagerSFX = GameObject.FindWithTag("SFXManager");
        }

        count = 0;
    }

    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "player_shot")
        {
            audioManagerSFX.GetComponent<AudioManagerSFX>().Play("Medkit_hit");
            count++;
            speed += (drop + (.5f * count));
        }
    }
}
