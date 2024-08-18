using UnityEngine;

public class HitSound : MonoBehaviour
{
    // ��� �Ҹ� Ŭ���� ������ �迭
    public AudioClip[] screamClips;

    // �´� �Ҹ� Ŭ���� ������ �迭
    public AudioClip[] hitClips;

    // ����� �ҽ� ������Ʈ
    private AudioSource audioSource;

    public GameObject bloodGo;   // ���� ��ũ�� ������Ʈ

    private void Awake()
    {
        // �� ���� ������Ʈ�� �پ��ִ� ����� �ҽ��� ������
        audioSource = GetComponent<AudioSource>();
        print("111111111111");
        // ����� �ҽ��� ������ �߰�
        if (audioSource == null)
        {
            print("22222222");

            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // �ִϸ��̼� �̺�Ʈ
    public void PlayHitSound()
    {
        // ��� �Ҹ� ���
        if (screamClips.Length > 0)
        {
            print("3333333333");

            int randomScreamIndex = Random.Range(0, screamClips.Length);
            print("4444444");

            audioSource.PlayOneShot(screamClips[randomScreamIndex]);
            print("5555555");

            Debug.Log("��� �Ҹ� �����");
        }
        else
        {
            Debug.LogWarning("��� �Ҹ� Ŭ���� �����ϴ�.");
        }

        // �´� �Ҹ� ���
        if (hitClips.Length > 0)
        {
            print("666666");

            int randomHitIndex = Random.Range(0, hitClips.Length);

            print("77777777");

            audioSource.PlayOneShot(hitClips[randomHitIndex]);

            print("888888888");

            Debug.Log("�´� �Ҹ� �����");
        }
        else
        {

            Debug.LogWarning("�´� �Ҹ� Ŭ���� �����ϴ�.");
        }
    }

}
