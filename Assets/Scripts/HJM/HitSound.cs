using UnityEngine;

public class HitSound : MonoBehaviour
{
    // 비명 소리 클립을 저장할 배열
    public AudioClip[] screamClips;

    // 맞는 소리 클립을 저장할 배열
    public AudioClip[] hitClips;

    // 오디오 소스 컴포넌트
    private AudioSource audioSource;

    private void Awake()
    {
        // 이 게임 오브젝트에 붙어있는 오디오 소스를 가져옴
        audioSource = GetComponent<AudioSource>();

        // 오디오 소스가 없으면 추가
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 애니메이션 이벤트
    public void PlayHitSound()
    {
        // 비명 소리와 맞는 소리를 각각 무작위로 선택하여 재생

        // 비명 소리 재생
        if (screamClips.Length > 0)
        {
            int randomScreamIndex = Random.Range(0, screamClips.Length);
            audioSource.PlayOneShot(screamClips[randomScreamIndex]);
        }

        // 맞는 소리 재생
        if (hitClips.Length > 0)
        {
            int randomHitIndex = Random.Range(0, hitClips.Length);
            audioSource.PlayOneShot(hitClips[randomHitIndex]);
        }
    }
}
