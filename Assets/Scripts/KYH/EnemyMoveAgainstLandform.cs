using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAgainstLandform : MonoBehaviour
{
    // 이동하다가 벽을 감지하면 방향을 바꾼다

    #region 구현 과정
    /*
        
        방향 재설정하는 함수 구현
     */
    #endregion

    Rigidbody rb;
    float moveSpeed = 7.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        // 앞으로 이동한다.
        rb.velocity = transform.forward * moveSpeed;

    }

    void Update()
    {

    }

    void dirRetarget()
    {

    }
}
