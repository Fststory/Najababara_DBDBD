using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTrigger : MonoBehaviour
{    
    public PlayerFSM playerFSM;
    public Animator enemyAnim;
    BoxCollider boxCol;

    void Start()
    {
        playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false;
    }

    private void Update()
    {
        //// Attack �ִϸ��̼��� �������� ���� boxCol�� Ȱ��ȭ ��Ų��. => duration?
        //if ()
        //{
        //    boxCol.enabled = true;
        //}
        //else
        //{
        //    boxCol.enabled = false;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (playerFSM.pyState == PlayerFSM.PlayerState.Normal)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Injured;
                print("Attack");
            }
            else if (playerFSM.pyState == PlayerFSM.PlayerState.Injured)
            {
                playerFSM.pyState = PlayerFSM.PlayerState.Dying;
                print("Attack");
            }
        }
    }
}
