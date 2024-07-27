using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] // 플레이어를 감지할 시야 콜라이더 => 이후 가능하면 절두체 형태의 콜라이더로 변경하고 싶음
public class EnemyTracingMod : MonoBehaviour
{
    // 증거에 따라 결정된 추격모드를 "EnemyMovingManager"에 전달하는 스크립트
    
    /*  현재 AI가 갖고 있는 증거에 따라 추격상태에 영향을 끼침.
        하지만 증거가 추가되거나 사라진다고 해도 추격상태에 변함이 없을 수 있음
        현재 추격 상태가 중요한 증거를 바탕으로 설정돼있다면 덜 중요한 증거가 추가/삭제되더라도 변동이 없을 것임

    */

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // 만약 플레이어가 트리거에 닿지 않는다면
        if (!other.gameObject.CompareTag("Player"))
        {
            // 기본 탐색모드 On
            EnemyMovingManager.emm.nowTracingMode = "NoEvidence";
        }
        // 만약 플레이어가 트리거에 닿는다면
        else
        {
            // 직접 추격모드 On
            EnemyMovingManager.emm.nowTracingMode = "PlayerFind";
        }
    }
}
