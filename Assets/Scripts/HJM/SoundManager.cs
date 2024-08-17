using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> bgmClips; // BGM 클립 목록
    private AudioSource audioSource; // BGM을 재생할 오디오 소스
    private int currentBGMIndex = -1; // 현재 재생 중인 BGM 클립 인덱스

    public float fadeDuration = 5.0f; // 페이드 인/아웃 시간

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomBGMWithFade()); // 처음 시작할 때 BGM 페이드 인과 함께 재생
    }

    IEnumerator PlayRandomBGMWithFade()
    {
        while (true)
        {
            // 새로운 BGM 재생
            if (bgmClips.Count > 0)
            {
                int newIndex = GetRandomBGMIndex();
                currentBGMIndex = newIndex;
                audioSource.clip = bgmClips[currentBGMIndex];

                // 페이드 인
                yield return FadeIn(audioSource, fadeDuration);

                // 클립이 재생될 때까지 대기
                yield return new WaitForSeconds(audioSource.clip.length);

                // 페이드 아웃
                yield return FadeOut(audioSource, fadeDuration);
            }
            else
            {
                // 만약 BGM 클립이 없다면 대기
                yield return null;
            }
        }
    }

    int GetRandomBGMIndex()
    {
        if (bgmClips.Count > 1)
        {
            int newIndex;
            do
            {
                newIndex = Random.Range(0, bgmClips.Count);
            } while (newIndex == currentBGMIndex); // 같은 인덱스가 나오지 않도록 처리
            return newIndex;
        }
        else
        {
            return 0; // 클립이 하나일 경우 0번 인덱스 반환
        }
    }

    public IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = 0f;
        audioSource.volume = startVolume;
        audioSource.Play();

        while (audioSource.volume < 0.8f)
        {
            audioSource.volume += Time.deltaTime / duration;
            yield return null;
        }

        audioSource.volume = 0.8f;
    }

    public IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 볼륨을 초기화
    }
}
