using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScript : MonoBehaviour
{
    private float mf_speed = 3.0f;
    private float _endPosX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartsFloating(float speed, float endPosX) 
    {

        mf_speed = speed;
       _endPosX = endPosX;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3.right * (Time.deltaTime * mf_speed));

        if (transform.position.x > _endPosX)
        {
            Destroy(gameObject);
        }
    }
}
