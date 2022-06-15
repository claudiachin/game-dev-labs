using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this has methods callable by players
public  class CentralManager : MonoBehaviour
{
	public  GameObject gameManagerObject;
	private  GameManager gameManager;
    public  GameObject powerUpManagerObject;
    private  PowerUpManager powerUpManager;
	public  static  CentralManager centralManagerInstance;
	
	void  Awake(){
		centralManagerInstance  =  this;
	}
	// Start is called before the first frame update
	void  Start()
	{
		gameManager  =  gameManagerObject.GetComponent<GameManager>();
        powerUpManager  =  powerUpManagerObject.GetComponent<PowerUpManager>();
	}

	public  void  increaseScore(){
		gameManager.increaseScore();
	}

    public  void  damagePlayer(){
        gameManager.damagePlayer();
    }

    public  void  consumePowerup(KeyCode k, GameObject g){
        powerUpManager.consumePowerup(k,g);
    }

    public  void  addPowerup(Texture t, int i, ConsumableInterface c){
        powerUpManager.addPowerup(t, i, c);
    }
}