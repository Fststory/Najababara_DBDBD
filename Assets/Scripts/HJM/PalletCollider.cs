using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletCollider : MonoBehaviour
{
    public PalletSystem palletSystem; // PalletSystem ��ũ��Ʈ�� ����
    public Animator playerAnim;
    public CharacterController cc;

    private void Start()
    {
        // PalletSystem�� palletSystem�� �޴´�.
        palletSystem = GetComponent<PalletSystem>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            print("���� ���� ����");
            playerAnim = other.gameObject.GetComponent<Animator>();
            cc = other.gameObject.GetComponent<CharacterController>();
            //palletSystem.isPlayerInTrigger = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            palletSystem.isPlayerInTrigger = false;
        }
    }
}
