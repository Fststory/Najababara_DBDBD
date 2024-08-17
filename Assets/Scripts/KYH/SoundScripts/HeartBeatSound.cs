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

        if (distance < 32.0f && !audioSource.isPlaying) // �Ÿ��� ������ �÷��� && �÷��� �߿��� �÷��̸� �ϸ� �� ��
        {
            audioSource.Play();
        }
        else if (distance >= 32.0f) // �Ÿ��� �־����� ��ž
        {
            audioSource.Stop();
        }

        if (audioSource.isPlaying)  // ���� ������ ����� �÷��� �߿���
        {
            audioSource.volume = (32.0f - distance) / 32.0f;
        }
    }
}
