using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  class GameManager : Singleton<GameManager>
{
	public  Text score;
	private  int playerScore =  0;
    public  delegate  void gameEvent();
    public  static  event  gameEvent OnPlayerDeath;
    public  static  event  gameEvent SpawnNewEnemy;

    // Singleton Pattern
    private  static  GameManager _instance;
    // Getter
    public  static  GameManager Instance
    {
        get { return  _instance; }
    }
	
	public  void  increaseScore(){
		playerScore  +=  1;
		score.text  =  "SCORE: "  +  playerScore.ToString();
        SpawnNewEnemy();
	}

    public  void  damagePlayer(){
        OnPlayerDeath();
    }

    //Member variables can be referred to as fields.  
    private  int _healthPoints; 

    //healthPoints is a basic property  
    public  int healthPoints { 
        get { 
            //Some other code  
            // ...
            return _healthPoints; 
        } 
        set { 
            // Some other code, check etc
            // ...
            _healthPoints = value; // value is the amount passed by the setter
        } 
    }

    override  public  void  Awake(){
		base.Awake();
		Debug.Log("awake called");
		// other instructions...
	}

}