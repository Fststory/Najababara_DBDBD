using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crows : MonoBehaviour
{
    AudioSource audioSource;
    //bool flying = false;
    //float currentTime = 0;
    //Transform startTransform;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //startTransform.position = transform.position;
    }

    void Update()
    {
        //if (flying)
        //{
        //    currentTime += Time.deltaTime;
        //    if (currentTime > 10.0f)
        //    {
        //        flying = false;
        //        transform.position = Vector3.Lerp(endTransform.position, startTransform.position, currentTime /10);
        //    }
        //    else
        //    {
        //        transform.position = Vector3.Lerp(endTransform.position, startTransform.position, currentTime / 10);
        //    }
        //}
    }

    //void 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioSource.Play();
            //flying = true;
        }
    }
}
