using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public List<AudioClip> bgmClips; // BGM 클립 목록
    private AudioSource audioSource; // BGM을 재생할 오디오 소스
    private int currentBGMIndex = -1; // 현재 재생 중인 BGM 클립 인덱스

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomBGM(); // 처음 시작할 때 BGM 재생
    }

    void Update()
    {
        if (!audioSource.isPlaying) // 현재 재생 중인 클립이 없다면
        {
            PlayRandomBGM(); // 다음 클립 재생
        }
    }

    void PlayRandomBGM()
    {
        if (bgmClips.Count > 0)
        {
            int newIndex = Random.Range(0, bgmClips.Count);

            // 이전에 재생된 BGM과 같은 인덱스가 나오지 않도록 처리 (선택사항)
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
