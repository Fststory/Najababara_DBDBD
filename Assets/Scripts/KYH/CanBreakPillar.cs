using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanBreakPillar : MonoBehaviour
{
    // Pillar의 자식 오브젝트 CanBreakPillarZone의 트리거 안에서 실행할 수 있는 기능

    // 필요한 것 : PillarFSM, PlayerFSM

    PillarFSM pillarState;
    public PlayerFSM playerFSM;
    public float currentTime = 0;
    public float damagedTime;   // 고장낼 때 걸리는 시간

    void Start()
    {
        pillarState = gameObject.GetComponentInParent<PillarFSM>();
    }
        
    private void OnTriggerStay(Collider other) // 플레이어가 갈고리에 걸리지 않은 상태로 상호작용 범위에 있을 때 사용 가능한 기능
    {
        // 아무것도 달려있지 않은 갈고리의 트리거에 들어와있는 대상이 "플레이어"이고, 상태가 0(건강) or 1(부상)이라면...
        if (other.CompareTag("Player") && (int)playerFSM.pyState < 2 && pillarState.currentState == PillarFSM.pillarState.NoSacrifice)
        {
            print("고장낼 수 있어!");
            // 스페이스바를 누르고 있으면
            if (Input.GetKey(KeyCode.Space))
            {
                currentTime += Time.deltaTime;  // 시간을 잰다.
                if (currentTime > damagedTime)  // 고장낼 때 걸리는 시간을 채우면
                {
                    print("에잇!");
                    pillarState.TakeDamage();   // 고장낸다.
                    currentTime = 0;
                }
            }
        }
    }
}
