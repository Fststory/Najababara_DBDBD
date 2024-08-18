using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushSound : MonoBehaviour
{
    public RushCollision rushCollision;

    public AudioClip[] rushSounds;  // 질주 소리 클립 배열              
    public AudioClip[] lethalRushSounds;    // 치명적인 질주 소리 클립 배열
    public AudioClip[] crashedSounds;


    private EnemyController enemyController;

    // 각각의 사운드를 위한 AudioSource (인스펙터에서 할당)    
    public AudioSource rushAudioSource;
    public AudioSource lethalRushAudioSource;
    public AudioSource crashedAudioSource;
    
    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        // 인스펙터에서 할당된 AudioSource를 사용
        if (!rushAudioSource || !lethalRushAudioSource)
        {
            Debug.LogError("오디오 소스가 할당되지 않았습니다! 인스펙터에서 오디오 소스를 할당해주세요.");
        }
    }

    private void Update()
    {
        if (rushCollision.crashed && !crashedAudioSource.isPlaying)
        {
            rushAudioSource.Stop();
            lethalRushAudioSource.Stop();
            PlayRandomCrashedSound();
            print("질주 충돌음 발생");
        }
    }

    void PlayRandomCrashedSound()
    {
        if (crashedSounds.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(crashedSounds);
        if (selectedClip == null)
        {
            Debug.LogWarning("Selected clip is null, skipping playback.");
            return;
        }

        Debug.Log($"Playing clip: {selectedClip.name} on {crashedAudioSource.name}");
        crashedAudioSource.PlayOneShot(selectedClip);  // PlayOneShot을 사용하여 클립 재생
    }

    void PlayRandomRushSound()
    {
        if (rushSounds.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(rushSounds);
        if (selectedClip == null)
        {
            Debug.LogWarning("Selected clip is null, skipping playback.");
            return;
        }

        Debug.Log($"Playing clip: {selectedClip.name} on {rushAudioSource.name}");
        if (!rushAudioSource.isPlaying)
        {
            rushAudioSource.PlayOneShot(selectedClip);  // PlayOneShot을 사용하여 클립 재생
        }
    }

    void PlayRandomLethalRushSound()
    {
        if (lethalRushSounds.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(lethalRushSounds);
        if (selectedClip == null)
        {
            Debug.LogWarning("Selected clip is null, skipping playback.");
            return;
        }

        Debug.Log($"Playing clip: {selectedClip.name} on {lethalRushAudioSource.name}");
        if (!lethalRushAudioSource.isPlaying)
        {
            lethalRushAudioSource.PlayOneShot(selectedClip);  // PlayOneShot을 사용하여 클립 재생
        }
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return null;
        int index = Random.Range(0, audioClips.Length);
        Debug.Log($"Selected clip index: {index}");
        return audioClips[index];
    }
}
