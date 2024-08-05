using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletCollider : MonoBehaviour
{
    public PalletSystem palletSystem; // PalletSystem 스크립트를 참조
    public Animator playerAnim;
    public CharacterController cc;

    private void Start()
    {
        // PalletSystem를 palletSystem에 받는다.
        palletSystem = palletSystem.GetComponent<PalletSystem>();
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            print("판자 영역 들어옴");
            playerAnim = other.gameObject.GetComponent<Animator>();
            cc = other.gameObject.GetComponent<CharacterController>();
            palletSystem.isPlayerInTrigger = true;

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
