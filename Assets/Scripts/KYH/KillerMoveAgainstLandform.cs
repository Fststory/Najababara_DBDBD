using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMoveAgainstLandform : MonoBehaviour
{
    // 이동하다가 벽을 감지하면 방향을 바꾼다

    #region 구현 과정
    /*
        
        방향 재설정하는 함수 구현
     */
    #endregion

    float moveSpeed = 7.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // 앞으로 이동한다.
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void dirRetarget()
    {

    }
}
