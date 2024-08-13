using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletCollider : MonoBehaviour
{
    public PalletSystem palletSystem; // PalletSystem ��ũ��Ʈ�� ����
    public Animator playerAnim;
    public CharacterController cc;

    public GameObject climbUI;

    private void Start()
    {
        // PalletSystem�� palletSystem�� �޴´�.
        palletSystem = palletSystem.GetComponent<PalletSystem>();

        if(climbUI != null)
        {
        climbUI.SetActive(false);

        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            print("���� ���� ����");
            playerAnim = other.gameObject.GetComponent<Animator>();
            cc = other.gameObject.GetComponent<CharacterController>();
            palletSystem.isPlayerInTrigger = true;
            climbUI.SetActive(true);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            palletSystem.isPlayerInTrigger = false;
            climbUI.SetActive(false);

        }
    }
}
