using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss1_stage3_shot : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;

    public GameObject target;
    Vector2 direction;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player");
        if (target != null)
            direction = (target.transform.position - this.transform.position).normalized * speed;
        else
            direction = Vector2.left;
        rb.velocity = new Vector2(direction.x, direction.y);
        Destroy(this.gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Despawner")
        {
            Destroy(this.gameObject);
        }
    }
}
