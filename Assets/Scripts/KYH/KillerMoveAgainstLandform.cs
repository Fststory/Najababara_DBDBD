using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillerMoveAgainstLandform : MonoBehaviour
{
    // �̵��ϴٰ� ���� �����ϸ� ������ �ٲ۴�

    #region ���� ����
    /*
        
        ���� �缳���ϴ� �Լ� ����
     */
    #endregion

    float moveSpeed = 7.0f;

    void Start()
    {
        
    }

    void Update()
    {
        // ������ �̵��Ѵ�.
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    void dirRetarget()
    {

    }
}
