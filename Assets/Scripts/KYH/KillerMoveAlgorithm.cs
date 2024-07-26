using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMoveAlgorithm : MonoBehaviour
{
    // 킬러의 움직임 알고리즘을 구현하는 스크립트

    #region 구현 과정
    /* 킬러가 지형지물을 인지하며 앞뒤좌우로 이동한다. 로봇 청소기처럼                  (+) 지형지물 인지는 시야 스크립트에서 구현

        1. 킬러가 처음으로 이동하는 방향을 정한다.
            1-1. 킬러가 처음 바라보는 방향은 랜덤으로 정한다. (Start 함수에서 실행)

        2. 킬러는 획득한 증거를 바탕으로 추격 상태를 정한다.
            2-1. 생존자 미발견/ 생존자 발견/ 흔적 발견/ 소리 감지 등
            2-2. 각 상태마다 다른 움직임을 보인다. (상태마다 함수 선언)
            2-3. 추격 상태는 매 프레임 업데이트 되고 전환될 수 있다.
            2-4. 추격 상태 간에는 우선 순위가 있다. (0. 시야 내 생존자/ 1. 시야 밖 흔적 o/ 2. 시야 밖 소리 o <- 이런 느낌)


    */
    #endregion

    #region SetStartLookDir() 변수
    int startLookDir;   // 킬러가 처음 바라볼 방향 (랜덤 값[0 ~ 359]을 transform.eulerAngles.y에 대입)
    #endregion

    #region SetNowTracingState() 변수
    #endregion

    void Start()
    {
        SetStartLookDir();  // 시작할 때 랜덤한 방향을 바라본다.
    }

    void Update()
    {
        SetNowTracingState();   // 현재 추격 상태를 설정한다.
    }

    void SetStartLookDir()  // 처음 바라보는 방향을 정함
    {
        startLookDir = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, startLookDir, 0);
    }

    void SetNowTracingState()  // 현재 추격 상태를 설정하는 함수 (1. 상태 간 우선순위를 매긴다.)
    {

    }



}
