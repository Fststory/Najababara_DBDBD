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
    private AudioSource audioSource;  // ���� AudioSource
    private float nextWalkSoundTime;
    private float nextRunSoundTime;
    private float nextBreathSoundTime;
    private float nextRoarSoundTime;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        audioSource = GetComponent<AudioSource>(); // ���� AudioSource ������Ʈ ��������
        nextWalkSoundTime = Time.time;
        nextRunSoundTime = Time.time;
        nextBreathSoundTime = Time.time;
        nextRoarSoundTime = Time.time;
    }

    void Update()
    {
        if (enemyController.currentState == EnemyController.EnemyState.NoEvidence ||
            enemyController.currentState == EnemyController.EnemyState.FindTrace ||
            enemyController.currentState == EnemyController.EnemyState.FindAura)
        {
            HandleWalkingSounds();
            HandleBreathSounds();
        }
        else if (enemyController.currentState == EnemyController.EnemyState.Rush)
        {
            HandleRunningSounds();
        }

        // ���� �����Ÿ��� ���� ��� ���� �����Ҹ� ���
        HandleRoarSounds();
    }

    void HandleWalkingSounds()
    {
        if (Time.time > nextWalkSoundTime && enemyController.NMA.velocity.magnitude > 0.1f)
        {
            if (!audioSource.isPlaying || audioSource.clip == GetRandomClip(walkSounds))
            {
                PlayRandomSound(walkSounds);
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
                PlayRandomSound(runSounds);
                nextRunSoundTime = Time.time + runSoundInterval;
            }
        }
    }

    void HandleBreathSounds()
    {
        if (Time.time > nextBreathSoundTime && !audioSource.isPlaying)
        {
            PlayRandomSound(breathSounds);
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
                if (Time.time > nextRoarSoundTime && !audioSource.isPlaying)
                {
                    PlayRandomSound(roarSounds);
                    nextRoarSoundTime = Time.time + roarSoundInterval;
                }
            }
        }
    }

    void PlayRandomSound(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(audioClips);
        audioSource.clip = selectedClip;
        audioSource.Play();
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return null;
        int index = Random.Range(0, audioClips.Length);
        return audioClips[index];
    }
}
