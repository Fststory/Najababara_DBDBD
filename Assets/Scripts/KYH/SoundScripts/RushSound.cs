using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushSound : MonoBehaviour
{
    public RushCollision rushCollision;

    public AudioClip[] rushSounds;  // ���� �Ҹ� Ŭ�� �迭              
    public AudioClip[] lethalRushSounds;    // ġ������ ���� �Ҹ� Ŭ�� �迭
    public AudioClip[] crashedSounds;


    private EnemyController enemyController;

    // ������ ���带 ���� AudioSource (�ν����Ϳ��� �Ҵ�)    
    public AudioSource rushAudioSource;
    public AudioSource lethalRushAudioSource;
    public AudioSource crashedAudioSource;
    
    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        // �ν����Ϳ��� �Ҵ�� AudioSource�� ���
        if (!rushAudioSource || !lethalRushAudioSource)
        {
            Debug.LogError("����� �ҽ��� �Ҵ���� �ʾҽ��ϴ�! �ν����Ϳ��� ����� �ҽ��� �Ҵ����ּ���.");
        }
    }

    private void Update()
    {
        if (rushCollision.crashed && !crashedAudioSource.isPlaying)
        {
            rushAudioSource.Stop();
            lethalRushAudioSource.Stop();
            PlayRandomCrashedSound();
            print("���� �浹�� �߻�");
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
        crashedAudioSource.PlayOneShot(selectedClip);  // PlayOneShot�� ����Ͽ� Ŭ�� ���
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
            rushAudioSource.PlayOneShot(selectedClip);  // PlayOneShot�� ����Ͽ� Ŭ�� ���
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
            lethalRushAudioSource.PlayOneShot(selectedClip);  // PlayOneShot�� ����Ͽ� Ŭ�� ���
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
