using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> bgmClips; // BGM Ŭ�� ���
    private AudioSource audioSource; // BGM�� ����� ����� �ҽ�
    private int currentBGMIndex = -1; // ���� ��� ���� BGM Ŭ�� �ε���

    public float fadeDuration = 5.0f; // ���̵� ��/�ƿ� �ð�

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayRandomBGMWithFade()); // ó�� ������ �� BGM ���̵� �ΰ� �Բ� ���
    }

    IEnumerator PlayRandomBGMWithFade()
    {
        while (true)
        {
            // ���ο� BGM ���
            if (bgmClips.Count > 0)
            {
                int newIndex = GetRandomBGMIndex();
                currentBGMIndex = newIndex;
                audioSource.clip = bgmClips[currentBGMIndex];

                // ���̵� ��
                yield return FadeIn(audioSource, fadeDuration);

                // Ŭ���� ����� ������ ���
                yield return new WaitForSeconds(audioSource.clip.length);

                // ���̵� �ƿ�
                yield return FadeOut(audioSource, fadeDuration);
            }
            else
            {
                // ���� BGM Ŭ���� ���ٸ� ���
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
            } while (newIndex == currentBGMIndex); // ���� �ε����� ������ �ʵ��� ó��
            return newIndex;
        }
        else
        {
            return 0; // Ŭ���� �ϳ��� ��� 0�� �ε��� ��ȯ
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
        audioSource.volume = startVolume; // ������ �ʱ�ȭ
    }
}
