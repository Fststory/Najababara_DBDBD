using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{    
    PlayerFSM playerFSM;
    public BoxCollider boxCol;

    void Start()
    {
        playerFSM = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSM>();
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
        print(other.name + "¶§¸²");
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
