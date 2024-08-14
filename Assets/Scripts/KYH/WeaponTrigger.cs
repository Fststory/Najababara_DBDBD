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
        //// Attack 애니메이션이 진행중일 때만 boxCol을 활성화 시킨다. => duration?
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
