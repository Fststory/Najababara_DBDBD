using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushCollision : MonoBehaviour
{
    // 질주 시 활성화되는 트리거 콜라이더: 충돌을 구현

    EnemyController enemyController;
    public bool crashed = false;
    BoxCollider boxCol;

    private void Start()
    {
        enemyController = transform.GetComponentInParent<EnemyController>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false; // 처음엔 NoEvidence 상태이므로 트리거 Off
    }

    void Update()
    {
        //에너미 상태가 질주라면...
        if (enemyController.currentState == EnemyController.EnemyState.Rush)
        {
            // 트리거를 활성화시킨다.
            boxCol.enabled = true;
        }
        else
        {
            // 에너미 상태가 질주 상태가 아니라면 비활성화
            boxCol.enabled = false;
        }
    }

    // 오브젝트의 트리거에 (플레이어를 제외한 <= 미구현) 무언가 닿는다면
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // 충돌했다는 신호를 기록 (이후는 EnemyController에서 구현할 수 있도록 충돌했다는 것만 bool로 반환)
            crashed = true;
            print(other.gameObject.name + "와 충돌함!!");
            enemyController.NMA.ResetPath();
            // 충돌이 끝나면 다시 EnemyController에서 false로 반환
        }
    }
}
