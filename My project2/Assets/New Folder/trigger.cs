using UnityEditor;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour

{
public GameObject prefab;
public Transform spawnPoint;
public float lifeTime = 10f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject spawned= Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            Destroy(spawned, lifeTime);
        
        }


    }

}