using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingPlaySound : MonoBehaviour
{
    public AudioSource audio2nd;
    float currenttime;

    void Update()
    {
        currenttime += Time.deltaTime;
        if (currenttime > 6.519f && !audio2nd.isPlaying)
        {
            audio2nd.Play();
        }
    }
}
