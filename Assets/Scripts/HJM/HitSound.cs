using UnityEngine;

public class HitSound : MonoBehaviour
{
    // ��� �Ҹ� Ŭ���� ������ �迭
    public AudioClip[] screamClips;

    // �´� �Ҹ� Ŭ���� ������ �迭
    public AudioClip[] hitClips;

    // ����� �ҽ� ������Ʈ
    private AudioSource audioSource;

    private void Awake()
    {
        // �� ���� ������Ʈ�� �پ��ִ� ����� �ҽ��� ������
        audioSource = GetComponent<AudioSource>();

        // ����� �ҽ��� ������ �߰�
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �ִϸ��̼� �̺�Ʈ
    public void PlayHitSound()
    {
        // ��� �Ҹ��� �´� �Ҹ��� ���� �������� �����Ͽ� ���

        // ��� �Ҹ� ���
        if (screamClips.Length > 0)
        {
            int randomScreamIndex = Random.Range(0, screamClips.Length);
            audioSource.PlayOneShot(screamClips[randomScreamIndex]);
        }

        // �´� �Ҹ� ���
        if (hitClips.Length > 0)
        {
            int randomHitIndex = Random.Range(0, hitClips.Length);
            audioSource.PlayOneShot(hitClips[randomHitIndex]);
        }
    }
}
