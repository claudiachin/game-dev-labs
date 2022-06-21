using System.Collections;
using UnityEngine;


public class PlayerControllerEV : MonoBehaviour
{
    private float force;
    public IntVariable marioUpSpeed;
    public IntVariable marioMaxSpeed;
    public GameConstants gameConstants;
    public CustomCastEvent castEvent;

    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private  Animator marioAnimator;
    private AudioSource marioAudio;
    private bool faceRightState = true;
    private bool onGroundState = true;
    private bool isDead = false;
    // private bool countScoreState = false;
	  
	// other components and interal state
    void Start() 
    {
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        marioUpSpeed.SetValue(gameConstants.playerMaxJumpSpeed);
        marioMaxSpeed.SetValue(gameConstants.playerMaxSpeed);
        force = gameConstants.playerDefaultForce;
    }

    
    void Update() {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState) {
            faceRightState = false;
            marioSprite.flipX = true;

            if (Mathf.Abs(marioBody.velocity.x) >  1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        if (Input.GetKeyDown("d") && !faceRightState) {
            faceRightState = true;
            marioSprite.flipX = false;

            if (Mathf.Abs(marioBody.velocity.x) >  1.0) {
                marioAnimator.SetTrigger("onSkid");
            }
        }

        // // when jumping, and Gomba is near Mario and we haven't registered our score
        // if (!onGroundState && countScoreState) {
        //     if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f) {
        //         countScoreState = false;
        //         score++;
        //         Debug.Log(score);
        //     }
        // }

        marioAnimator.SetBool("onGround", onGroundState);

        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));

        if (Input.GetKeyDown("z")){
            // CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
            castEvent.Invoke(KeyCode.Z);
        }

        if (Input.GetKeyDown("x")){
            // CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
            castEvent.Invoke(KeyCode.X);
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float moveHorizontal = Input.GetAxis("Horizontal");
            if (Mathf.Abs(moveHorizontal) > 0) {
                Vector2 movement = new Vector2(moveHorizontal, 0);
                if (marioBody.velocity.magnitude < marioMaxSpeed.Value) {
                    marioBody.AddForce(movement * force);
                }
            }
            if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
                // stop
                marioBody.velocity = Vector2.zero;
            }

            if (Input.GetKeyDown("space") && onGroundState) {
                marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
                onGroundState = false;
                // countScoreState = true; //check if Gomba is underneath
            }
            // //check if a or d is pressed currently
            // if (!isADKeyUp)
            // {
            //     float direction = faceRightState ? 1.0f : -1.0f;
            //     Vector2 movement = new Vector2(force * direction, 0);
            //     if (marioBody.velocity.magnitude < marioMaxSpeed.Value)
            //         marioBody.AddForce(movement);
            // }

            // if (!isSpacebarUp && onGroundState)
            // {
            //     marioBody.AddForce(Vector2.up * marioUpSpeed.Value, ForceMode2D.Impulse);
            //     onGroundState = false;
            //     // part 2
            //     marioAnimator.SetBool("onGround", onGroundState);
            //     countScoreState = true; //check if goomba is underneath
            // }
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles") || col.gameObject.CompareTag("Pipes")) {
            onGroundState = true; // back on ground
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
    }

    void PlayJumpSound() {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    public void PlayerDiesSequence()
    {
        isDead = true;
        marioAnimator.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        marioBody.AddForce(Vector3.up * 30, ForceMode2D.Impulse);
        marioBody.gravityScale = 30;
        StartCoroutine(dead());
    }
    
    IEnumerator dead()
    {
        yield return new WaitForSeconds(1.0f);
        marioBody.bodyType = RigidbodyType2D.Static;
    }
}