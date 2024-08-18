using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{    
    public PlayerFSM playerFSM;
    public Animator playerAnim;
    public BoxCollider boxCol;

    public AudioClip[] attackSounds;  // �ֵθ��� �Ҹ� Ŭ�� �迭

    public AudioSource attackAudioSource;

    void Start()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");
        playerFSM = player.GetComponent<PlayerFSM>();
        playerAnim = player.GetComponent<Animator>();

        boxCol = GameObject.Find("AttackTrigger").GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    public void TriggerOn()
    {
        boxCol.enabled = true;
    }
    public void TriggerOff()
    {
        boxCol.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name + "����");
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerFSM.pyState == PlayerFSM.PlayerState.Normal)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Injured;
                print("Hit01");
                playerAnim.SetTrigger("Hit01");                
            }
            else if (playerFSM.pyState == PlayerFSM.PlayerState.Injured)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Dying;
                print("Hit02");
                playerAnim.SetTrigger("Hit02");
                playerAnim.SetBool("Dying", true);                
            }
        }
    }    

    public void PlayRandomAttackSound()    // ���� �ִϸ��̼� ���� ���� �̺�Ʈ
    {
        if (attackSounds.Length == 0) return;

        AudioClip selectedClip = GetRandomClip(attackSounds);
        if (selectedClip == null)
        {
            Debug.LogWarning("Selected clip is null, skipping playback.");
            return;
        }

        Debug.Log($"Playing clip: {selectedClip.name} on {attackAudioSource.name}");
        attackAudioSource.PlayOneShot(selectedClip);  // PlayOneShot�� ����Ͽ� Ŭ�� ���
    }

    AudioClip GetRandomClip(AudioClip[] audioClips)
    {
        if (audioClips.Length == 0) return null;
        int index = Random.Range(0, audioClips.Length);
        Debug.Log($"Selected clip index: {index}");
        return audioClips[index];
    }
}
