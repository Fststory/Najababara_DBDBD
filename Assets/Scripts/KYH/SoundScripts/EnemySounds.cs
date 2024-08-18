using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public AudioClip[] walkSounds;   // �ȱ� �Ҹ� Ŭ�� �迭
    public AudioClip[] runSounds;    // �޸��� �Ҹ� Ŭ�� �迭
    public AudioClip[] breathSounds; // ���Ҹ� Ŭ�� �迭
    public AudioClip[] roarSounds;   // ���� �����Ҹ� Ŭ�� �迭

    public float walkSoundInterval = 1.0f; // �ȱ� �Ҹ� ����
    public float runSoundInterval = 0.5f;  // �޸��� �Ҹ� ����
    public float breathSoundInterval = 5.0f; // ���Ҹ� ����
    public float roarSoundInterval = 10.0f; // �����Ҹ� ����

    private EnemyController enemyController;

    // ������ ���带 ���� AudioSource (�ν����Ϳ��� �Ҵ�)
    public AudioSource walkAudioSource;
    public AudioSource runAudioSource;
    public AudioSource breathAudioSource;
    public AudioSource roarAudioSource;

    private float nextWalkSoundTime;
    private float nextRunSoundTime;
    private float nextBreathSoundTime;
    private float nextRoarSoundTime;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();

        // �ν����Ϳ��� �Ҵ�� AudioSource�� ���
        if (!walkAudioSource || !runAudioSource || !breathAudioSource || !roarAudioSource)
        {
            Debug.LogError("����� �ҽ��� �Ҵ���� �ʾҽ��ϴ�! �ν����Ϳ��� ����� �ҽ��� �Ҵ����ּ���.");
        }

        nextWalkSoundTime = Time.time;
        nextRunSoundTime = Time.time;
        nextBreathSoundTime = Time.time;
        nextRoarSoundTime = Time.time;
    }

    void Update()
    {
        if (enemyController.currentState == EnemyController.EnemyState.NoEvidence ||
            enemyController.currentState == EnemyController.EnemyState.FindTrace ||
            enemyController.currentState == EnemyController.EnemyState.FindPlayer)
        {
            HandleWalkingSounds();
            HandleBreathSounds();
        }
        else if (enemyController.currentState == EnemyController.EnemyState.Rush)
        {
            HandleRunningSounds();
            HandleBreathSounds();
        }
        else if (enemyController.currentState == EnemyController.EnemyState.GetPlayer)
        {
            HandleBreathSounds();
            HandleWalkingSounds();
        }
    }

    void HandleWalkingSounds()
    {
        if (Time.time > nextWalkSoundTime && enemyController.NMA.velocity.magnitude > 0.1f)
        {
            if (!walkAudioSource.isPlaying)
            {
                PlayRandomSound(walkSounds, walkAudioSource);
            }
            nextWalkSoundTime = Time.time + walkSoundInterval;
        }
    }

    void HandleRunningSounds()
    {
        if (enemyController.NMA.velocity.magnitude > 0.1f)
        {
            if (Time.time > nextRunSoundTime)
            {
                PlayRandomSound(runSounds, runAudioSource);
                nextRunSoundTime = Time.time + runSoundInterval;
            }
        }
    }

    void HandleBreathSounds()
    {
        if (Time.time > nextBreathSoundTime && !breathAudioSource.isPlaying)
        {
            PlayRandomSound(breathSounds, breathAudioSource);
            nextBreathSoundTime = Time.time + breathSoundInterval;
        }
    }

    void HandleRoarSounds()
    {
        if (enemyController.targetTransform != null)
        {
            float distance = Vector3.Distance(transform.position, enemyController.targetTransform.position);

            if (distance <= enemyController.attackRange)
            {
                if (Time.time > nextRoarSoundTime && !roarAudioSource.isPlaying)
                {
                    PlayRandomSound(roarSounds, roarAudioSource);
                    nextRoarSoundTime = Time.time + roarSoundInterval;
                }
            }
        }
    }

    void PlayRandomSound(AudioClip[] audioClips, AudioSource audioSource)
    {
        if (audioClips.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(audioClips);
        if (selectedClip == null)
        {
            Debug.LogWarning("Selected clip is null, skipping playback.");
            return;
        }

        Debug.Log($"Playing clip: {selectedClip.name} on {audioSource.name}");
        audioSource.PlayOneShot(selectedClip);  // PlayOneShot�� ����Ͽ� Ŭ�� ���
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return null;
        int index = Random.Range(0, audioClips.Length);
        Debug.Log($"Selected clip index: {index}");
        return audioClips[index];
    }
}
