using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingManager : MonoBehaviour
{
    // ���ʹ��� ���� �������� ���� �߰ݸ�带 ����(�������� ����)�ϴ� �Ŵ��� ���� ��ũ��Ʈ �Դϴ�.

    /*
        - ���� �߰� ��带 ���� �־�� �Ѵ�.
        =>

    */

    public static EnemyMovingManager emm;

    public string nowTracingMode;   

    private void Awake()
    {
        if (emm == null)
        {
            emm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        nowTracingMode = "JustStarted"; // ó�� �߰ݸ��� "JustStarted"
    }

    void Update()
    {
        if(nowTracingMode== "JustStarted")
        {
            print("JustStarted");
        }
        else if(nowTracingMode == "NoEvidence")
        {
            print("NoEvidence");
        }else if(nowTracingMode == "PlayerFind")
        {
            print("PlayerFind");
        }
    }
}
