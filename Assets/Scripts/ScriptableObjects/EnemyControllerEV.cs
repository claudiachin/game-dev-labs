using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerEV : MonoBehaviour
{
    // events to subscribe
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;
    public  GameConstants gameConstants;
	private  int moveRight;
	private  float originalX;
	private  Vector2 velocity;
	private  Rigidbody2D enemyBody;
	private SpriteRenderer enemySprite;
	
	void  Start()
	{
		enemyBody  =  GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();

		
		// get the starting position
		originalX  =  transform.position.x;
	
		// randomise initial direction
		moveRight  =  Random.Range(0, 2) ==  0  ?  -1  :  1;
		
		// compute initial velocity
		ComputeVelocity();

        // subscribe to player event
        // GameManager.OnPlayerDeath  +=  EnemyRejoice;
	}
	
	void  ComputeVelocity()
	{
			velocity  =  new  Vector2((moveRight) *  gameConstants.maxOffset  /  gameConstants.enemyPatroltime, 0);
	}
  
	void  MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position  +  velocity  *  Time.fixedDeltaTime);
	}

	void  Update()
	{
		if (Mathf.Abs(enemyBody.position.x  -  originalX) <  gameConstants.maxOffset)
		{// move goomba
			MoveEnemy();
		}
		else
		{
			// change direction
			moveRight  *=  -1;
			ComputeVelocity();
			MoveEnemy();
		}
	}

    void  KillSelf(){
		// enemy dies
		// CentralManager.centralManagerInstance.increaseScore();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator  flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, gameConstants.groundSurface  +  GetComponent<SpriteRenderer>().bounds.extents.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
	}

    // animation when player is dead
    public void PlayerDeathResponse(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
        StartCoroutine(rejoice());
    }

	IEnumerator  rejoice(){
		Debug.Log("Enemy is celebrating");
		int steps =  10;
		bool flip =  true;

		for (int i =  0; i  <  steps; i  ++){
			enemySprite.flipX = flip;
			flip = !flip;
			yield return new WaitForSeconds(0.2f);
		}

	}
    void OnTriggerEnter2D(Collider2D other)
    {
        // check if it collides with Mario
        if (other.gameObject.tag == "Player")
        {
            // check if collides on top
            float yoffset = (other.transform.position.y - this.transform.position.y);
            if (yoffset > 0.75f)
            {
                // enemyAudioSource.PlayOneShot(enemyAudioSource.clip);
                KillSelf();
                onEnemyDeath.Invoke();
            }
            else
            {
                // hurt player
                onPlayerDeath.Invoke();
            }
        }
    }
}
