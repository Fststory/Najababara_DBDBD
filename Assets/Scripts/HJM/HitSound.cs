using UnityEngine;

public class HitSound : MonoBehaviour
{
    // 비명 소리 클립을 저장할 배열
    public AudioClip[] screamClips;

    // 맞는 소리 클립을 저장할 배열
    public AudioClip[] hitClips;

    // 오디오 소스 컴포넌트
    private AudioSource audioSource;

    public GameObject bloodGo;   // 블러드 스크린 오브젝트

    private void Awake()
    {
        // 이 게임 오브젝트에 붙어있는 오디오 소스를 가져옴
        audioSource = GetComponent<AudioSource>();
        print("111111111111");
        // 오디오 소스가 없으면 추가
        if (audioSource == null)
        {
            print("22222222");

            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 애니메이션 이벤트
    public void PlayHitSound()
    {
        // 비명 소리 재생
        if (screamClips.Length > 0)
        {
            print("3333333333");

            int randomScreamIndex = Random.Range(0, screamClips.Length);
            print("4444444");

            audioSource.PlayOneShot(screamClips[randomScreamIndex]);
            print("5555555");

            Debug.Log("비명 소리 재생됨");
        }
        else
        {
            Debug.LogWarning("비명 소리 클립이 없습니다.");
        }

        // 맞는 소리 재생
        if (hitClips.Length > 0)
        {
            print("666666");

            int randomHitIndex = Random.Range(0, hitClips.Length);

            print("77777777");

            audioSource.PlayOneShot(hitClips[randomHitIndex]);

            print("888888888");

            Debug.Log("맞는 소리 재생됨");
        }
        else
        {

            Debug.LogWarning("맞는 소리 클립이 없습니다.");
        }
    }

}
