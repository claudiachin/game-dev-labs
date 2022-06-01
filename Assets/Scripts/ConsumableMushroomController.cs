using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroomController : MonoBehaviour
{
    private Rigidbody2D mushroom;

    public float speed;
    private Vector2 velocity;
    private int moveRight = 1;

    // Start is called before the first frame update
    void Start()
    {
        mushroom = GetComponent<Rigidbody2D>();
        ComputeVelocity();

        mushroom.AddForce(Vector2.up * 2, ForceMode2D.Impulse);

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

    void  OnBecameInvisible() {
        Destroy(gameObject);	
    }

    void  OnCollisionEnter2D(Collision2D col) {
        // stop mushroom from moving on collision with Player
        if (col.gameObject.CompareTag("Player")) {
            Debug.Log("Player collided with Mushroom!");
            moveRight = 0;
        }

        // change mushroom direction on collision
        if (col.gameObject.CompareTag("Pipes")) {
            Debug.Log("Player collided with a Pipe!");
            moveRight *= -1;
        }
    }
}
