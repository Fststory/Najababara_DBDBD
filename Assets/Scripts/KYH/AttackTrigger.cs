using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrigger : MonoBehaviour
{    
    public PlayerFSM playerFSM;
    public Animator playerAnim;
    public BoxCollider boxCol;

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
        print(other.name + "¶§¸²");
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
}
