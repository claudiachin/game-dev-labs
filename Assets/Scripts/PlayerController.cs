using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public float maxSpeed = 10;
    public float upSpeed = 10;
    private bool onGroundState = true;

    // public Transform enemyLocation;
    // public Text scoreText;
    // private int score = 0;
    // private bool countScoreState = false;

    private GameObject restartBtn;
    private GameObject panel;

    private  Animator marioAnimator;
    private AudioSource marioAudio;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();

        marioAnimator = GetComponent<Animator>();
        marioAudio = GetComponent<AudioSource>();

        panel = GameObject.Find("Panel");
        restartBtn = GameObject.Find("RestartButton");
        restartBtn.SetActive(false);

        // subscribe to player event
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence;
    }

    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate() {
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0) {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")) {
            // stop
            marioBody.velocity = Vector2.zero;
        }

        if (Input.GetKeyDown("space") && onGroundState) {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // countScoreState = true; //check if Gomba is underneath
        }
    }

    // Update is called once per frame
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
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }

    }

    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.gameObject.CompareTag("Enemy")) {
    //         Debug.Log("Collided with Gomba!");
    //         Time.timeScale = 0;
    //         restartBtn.SetActive(true);
    //         panel.SetActive(true);
    //     }
    // }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("Ground") || col.gameObject.CompareTag("Obstacles")) {
            onGroundState = true; // back on ground
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
    }

    void PlayJumpSound() {
        marioAudio.PlayOneShot(marioAudio.clip);
    }

    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");
        restartBtn.SetActive(true);
        panel.SetActive(true);
        GameObject.Find("Mario").SetActive(false);
    }

}
