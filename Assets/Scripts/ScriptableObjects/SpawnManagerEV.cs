using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnManagerEV : MonoBehaviour
{
    public GameConstants gameConstants;
    void Start()
    {
        Debug.Log("Spawnmanager start");
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.greenEnemy);

    }

    void startSpawn(Scene scene, LoadSceneMode mode)
    {
        for (int j = 0; j < 2; j++)
            spawnFromPooler(ObjectType.gombaEnemy);
    }


    void spawnFromPooler(ObjectType i)
    {
        GameObject item = ObjectPooler.SharedInstance.GetPooledObject(i);

        if (item != null)
        {
            //set position
            if (i == ObjectType.gombaEnemy) {
                item.transform.localScale = new Vector3(1, 1, 1);
            } else {
                item.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            }
            item.transform.position = new Vector3(Random.Range(-4.5f, 4.5f), gameConstants.groundDistance, 0);
            item.SetActive(true);
        }
        else
        {
            Debug.Log("not enough items in the pool!");
        }
    }

    public void spawnNewEnemy()
    {
        ObjectType i = Random.Range(0, 2) == 0 ? ObjectType.gombaEnemy : ObjectType.greenEnemy;
        spawnFromPooler(i);
    }

}