using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAgainstLandform : MonoBehaviour
{
    // �̵��ϴٰ� ���� �����ϸ� ������ �ٲ۴�

    #region ���� ����
    /*
        
        ���� �缳���ϴ� �Լ� ����
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
        // ������ �̵��Ѵ�.
        rb.velocity = transform.forward * moveSpeed;

    }

    void Update()
    {

    }

    void dirRetarget()
    {

        //transform.eulerAngles = 
    }
}
