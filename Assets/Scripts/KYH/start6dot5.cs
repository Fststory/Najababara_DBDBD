using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class start6dot5 : MonoBehaviour
{
    float currentTime = 0;
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > 7.0f)
        {
            Destroy(gameObject);
        }
    }
}
