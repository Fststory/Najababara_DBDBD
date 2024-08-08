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
        if (player != null)  // player가 null이 아닌지 확인
        {
            isHiding = true;
            player.tag = "HidePlayer";  // 태그를 "HidePlayer"로 설정
            //player.SetActive(false);  // 예를 들어, 플레이어를 숨기는 코드

            // 플레이어를 비활성화하거나 숨기는 추가적인 코드가 필요할 수 있습니다.
        }
    }

    void UnhidePlayer()
    {
        isHiding = false;
        player.tag = "Player";
        //player.SetActive(true);
    }
}
