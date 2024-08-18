using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public AudioClip[] walkSounds;   // 걷기 소리 클립 배열
    public AudioClip[] runSounds;    // 달리기 소리 클립 배열
    public AudioClip[] breathSounds; // 숨소리 클립 배열
    public AudioClip[] roarSounds;   // 괴물 울음소리 클립 배열

    public float walkSoundInterval = 1.0f; // 걷기 소리 간격
    public float runSoundInterval = 0.5f;  // 달리기 소리 간격
    public float breathSoundInterval = 5.0f; // 숨소리 간격
    public float roarSoundInterval = 10.0f; // 울음소리 간격

    private EnemyController enemyController;

    // 각각의 사운드를 위한 AudioSource (인스펙터에서 할당)
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

        // 인스펙터에서 할당된 AudioSource를 사용
        if (!walkAudioSource || !runAudioSource || !breathAudioSource || !roarAudioSource)
        {
            Debug.LogError("오디오 소스가 할당되지 않았습니다! 인스펙터에서 오디오 소스를 할당해주세요.");
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

        //Debug.Log($"Playing clip: {selectedClip.name} on {audioSource.name}");
        audioSource.PlayOneShot(selectedClip);  // PlayOneShot을 사용하여 클립 재생
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return null;
        int index = Random.Range(0, audioClips.Length);
        //Debug.Log($"Selected clip index: {index}");
        return audioClips[index];
    }
}
