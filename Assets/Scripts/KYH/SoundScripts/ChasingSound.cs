using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingSound : MonoBehaviour
{
    public AudioSource dist32, dist16, dist8, dist4;
    public Transform player, enemy;

    public float fadeDuration = 1.0f; // 페이드 인/아웃 시간

    void Update()
    {
        float distance = Vector3.Distance(player.position, enemy.position);

        if (distance <= 32.0f)
        {
            if (16.0f < distance && distance <= 32.0f && !dist32.isPlaying)
            {
                StartCoroutine(FadeInAndOut(dist32, dist16, dist8, dist4));
                dist32.Play();
            }
            else if (8.0f < distance && distance <= 16.0f && !dist16.isPlaying)
            {
                StartCoroutine(FadeInAndOut(dist16, dist32, dist8, dist4));
                dist16.Play();
            }
            else if (4.0f < distance && distance <= 8.0f && !dist8.isPlaying)
            {
                StartCoroutine(FadeInAndOut(dist8, dist32, dist16, dist4));
                dist8.Play();
            }
            else if (distance <= 4.0f && !dist4.isPlaying)
            {
                StartCoroutine(FadeInAndOut(dist4, dist32, dist16, dist8));
                dist4.Play();
            }
        }
    }

    IEnumerator FadeInAndOut(AudioSource newSource, params AudioSource[] sourcesToStop)
    {
        // 페이드 아웃
        foreach (AudioSource source in sourcesToStop)
        {
            if (source.isPlaying)
            {
                StartCoroutine(FadeOut(source, fadeDuration));
            }
        }

        // 페이드 인
        newSource.volume = 0f;
        newSource.Play();
        StartCoroutine(FadeIn(newSource, fadeDuration));

        yield return null;
    }

    IEnumerator FadeIn(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        audioSource.volume = 0f;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += Time.deltaTime / duration * startVolume;
            yield return null;
        }

        audioSource.volume = startVolume;
    }

    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0f)
        {
            audioSource.volume -= Time.deltaTime / duration * startVolume;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }
}
