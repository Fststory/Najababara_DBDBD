using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingSound : MonoBehaviour
{
    public AudioSource dist32, dist16, dist8, dist4;
    public Transform player, enemy;


    void Update()
    {
        float distance = Vector3.Distance(player.position, enemy.position);

        if (distance <= 32.0f)
        {
            if(16.0f < distance && distance <= 32.0f && !dist32.isPlaying)
            {
                print("32m 家府");
                dist32.Play();
                dist16.Stop();
                dist8.Stop();
                dist4.Stop();
            }
            else if (8.0f < distance && distance <= 16.0f && !dist16.isPlaying)
            {
                print("16m 家府");
                dist32.Stop();
                dist16.Play();
                dist8.Stop();
                dist4.Stop();
            }
            else if (4.0f < distance && distance <= 8.0f && !dist8.isPlaying)
            {
                print("8m 家府");
                dist32.Stop();
                dist16.Stop();
                dist8.Play();
                dist4.Stop();
            }
            else if(distance<=4.0f && !dist4.isPlaying)
            {
                print("4m 家府");
                dist32.Stop();
                dist16.Stop();
                dist8.Stop();
                dist4.Play();
            }
        }
    }
}
