using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableMushroomController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: move mushroom randomly left or right at constant speed
    // TODO: change mushroom direction on collision
    // TODO stop mushroom from moving on collision with Player
    // TODO: collide normally with Obstacles or Ground
    // TODO: do not collide with TopCollider
    // TODO: mushroom springs out of box when instantiated

    void  OnBecameInvisible() {
        Destroy(gameObject);	
    }
}
