using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroomController : MonoBehaviour
{
    private Rigidbody2D mushroom;
    public float speed;
    private Vector2 velocity;
    private int moveRight;

    // private bool collected;

    // Start is called before the first frame update
    void Start()
    {
        mushroom = GetComponent<Rigidbody2D>();
        ComputeVelocity();

        // make mushroom spring out of the box
        mushroom.AddForce(Vector2.up * 2, ForceMode2D.Impulse);

        // make mushroom move towards centre initially
        if (mushroom.position.x < 0) {
            moveRight = 1;
        } else {
            moveRight = -1;
        }

    }

    // Update is called once per frame
    void Update()
    {
        // move mushroom randomly left or right at constant speed
        ComputeVelocity();
        MoveMushroom();
        
    }

    void MoveMushroom() {
        mushroom.MovePosition(mushroom.position + velocity * Time.fixedDeltaTime);
    }
    void ComputeVelocity() {
        velocity = new Vector2((moveRight)*speed, 0);
    }

    // void  OnBecameInvisible() {
    //     Destroy(gameObject);	
    // }

    void  OnCollisionEnter2D(Collision2D col) {
        // stop mushroom from moving on collision with Player
        if (col.gameObject.CompareTag("Player")) {
            Debug.Log("Player collided with Mushroom!");
            moveRight = 0;
            // collected = true;
            // this.gameObject.SetActive(false);
        }

        // change mushroom direction on collision
        if (col.gameObject.CompareTag("Pipes")) {
            Debug.Log("Player collided with a Pipe!");
            moveRight *= -1;
        }
    }

}
