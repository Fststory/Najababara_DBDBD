using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkill : MonoBehaviour
{
    // 에너미(블라이트) 질주, 충돌, 치명적인 질주, 탈진 구현 스크립트

    /*
    플레이어를 추격할 때 [currentState == EnemyState.FindPlayer] 스킬각 판단

    1. 질주 부터 하자
    ㄴ 빠른 속도로 직진(자신이 바라보던 방향, 질주 중엔 방향 변경 불가)
    ㄴ 목표 지점을 ray가 맞은 곳으로 하자(9.2 m/s로 3초 동안 이동한다면 총 27.6 m 이동 가능할 것이다.=> ray 최대 거리)

    5개의 토큰이 모두 차있을 때 질주 사용 가능, 토큰은 2초마다 1개씩 충전

    기본 이속: 4.6 (m/s)
    질주: 9.2 (m/s) 3초간 + 공격 불가
    치명적인 질주: 9.2 (m/s) 3초간 + 공격 가능
    연속 질주 대기: 1.25 (s)
    질주 후 탈진(충돌 실패 or 치질 발동 실패): 2.5 (s)
    충돌 각: 45도
     */    
}
