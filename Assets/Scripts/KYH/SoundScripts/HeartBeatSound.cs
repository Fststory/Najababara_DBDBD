using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeatSound : MonoBehaviour
{
    public AudioSource audioSource;
    public Transform enemy, player;

    void Update()
    {
        float distance = Vector3.Distance(enemy.position, player.position);

        if (distance < 32.0f && !audioSource.isPlaying) // 거리가 가까우면 플레이 && 플레이 중에는 플레이를 하면 안 됨
        {
            audioSource.Play();
        }
        else if (distance >= 32.0f) // 거리가 멀어지면 스탑
        {
            audioSource.Stop();
        }

        if (audioSource.isPlaying)  // 볼륨 조절은 오디오 플레이 중에만
        {
            audioSource.volume = (32.0f - distance) / 32.0f;
        }
    }
}
