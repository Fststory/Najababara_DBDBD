using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEyeSight : MonoBehaviour
{
    // 킬러의 시야를 구현하는 스크립트

    #region 구현 과정
    /* 킬러의 시야가 인지해야 될 것 (레이어, 태그 이용)                        
        1. 지형지물 - 이동 알고리즘(최적 경로 탐색)
        2. 플레이어 - 직접적으로 시야에 보일 시 기존 이동방향을 플레이어로 수정
         ㄴ 오라 - 
         ㄴ 흔적 - 
        + @

    하나의 시야 오브젝트에 구현하면 안 될 것 같기도(플레이어[직접적 시야], 오라[벽 너머 시야] 등을 감지하는 트리거 범위는 달라야 할 테니까!)
    일단 이 스크립트에는 지형지물 감지를 구현해보자

    */
    #endregion


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void RecognizeLandform()
    {

    }
}
