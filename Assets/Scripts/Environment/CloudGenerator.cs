using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] clouds;

    [SerializeField]
    float spawnInterval;

    [SerializeField]
    GameObject endPoint;

    Vector3 startPos;

  

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Prewarm();
        Invoke("AttemptSpawn", spawnInterval);
    }
  
    // Update is called once per frame
    void SpawnCloud(Vector3 startPos)
    {
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);
        GameObject cloud = Instantiate(clouds[randomIndex]);

      
        float startY = UnityEngine.Random.Range(startPos.y - 4.0f, startPos.y + 14.0f);

      
        cloud.transform.position = new Vector3(startPos.x, startY, startPos.z);

        float scale = UnityEngine.Random.Range(1.5f, 3.0f);
        cloud.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(1.5f, 3.5f);

        if (startY > startPos.y) 
        {

            speed += 1.0f;
        
        }
        

        cloud.GetComponent<CloudScript>().StartsFloating(speed, endPoint.transform.position.x); 
    }

    void AttemptSpawn() 
    {

        SpawnCloud(startPos);

        Invoke("AttemptSpawn", spawnInterval);

    }

    void Prewarm() 
    {

        for (int i = 0; i < 10; i++)
        {

            Vector3 spawnPos = startPos + Vector3.right * (i * 15);
            SpawnCloud(spawnPos);

        }
    
    }
}
