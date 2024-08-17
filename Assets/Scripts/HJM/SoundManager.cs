using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public List<AudioClip> bgmClips; // BGM Ŭ�� ���
    private AudioSource audioSource; // BGM�� ����� ����� �ҽ�
    private int currentBGMIndex = -1; // ���� ��� ���� BGM Ŭ�� �ε���

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomBGM(); // ó�� ������ �� BGM ���
    }

    void Update()
    {
        if (!audioSource.isPlaying) // ���� ��� ���� Ŭ���� ���ٸ�
        {
            PlayRandomBGM(); // ���� Ŭ�� ���
        }
    }

    void PlayRandomBGM()
    {
        if (bgmClips.Count > 0)
        {
            int newIndex = Random.Range(0, bgmClips.Count);

            // ������ ����� BGM�� ���� �ε����� ������ �ʵ��� ó�� (���û���)
            while (newIndex == currentBGMIndex && bgmClips.Count > 1)
            {
                newIndex = Random.Range(0, bgmClips.Count);
            }

            currentBGMIndex = newIndex;
            audioSource.clip = bgmClips[currentBGMIndex];
            audioSource.Play();
        }
    }
}
