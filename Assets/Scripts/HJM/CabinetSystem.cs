using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetSystem : MonoBehaviour
{
    public GameObject player;
    private EnemyController enemyController;
    private bool isHiding = false;
    private bool isPlayerInTrigger = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyController = GameObject.FindObjectOfType<EnemyController>();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            if (!isHiding)
            {
                HidePlayer();
            }
            else
            {
                UnhidePlayer();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerInTrigger = false;
        }
    }

    void HidePlayer()
    {
        if (player != null)  // player�� null�� �ƴ��� Ȯ��
        {
            isHiding = true;
            player.tag = "HidePlayer";  // �±׸� "HidePlayer"�� ����
            //player.SetActive(false);  // ���� ���, �÷��̾ ����� �ڵ�

            // �÷��̾ ��Ȱ��ȭ�ϰų� ����� �߰����� �ڵ尡 �ʿ��� �� �ֽ��ϴ�.
        }
    }

    void UnhidePlayer()
    {
        isHiding = false;
        player.tag = "Player";
        //player.SetActive(true);
    }
}
