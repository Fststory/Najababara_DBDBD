using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingManager : MonoBehaviour
{
    // 에너미의 증거 수집도에 따라 추격모드를 설정(움직임을 관리)하는 매니저 역할 스크립트 입니다.

    /*
        - 현재 추격 모드를 갖고 있어야 한다.
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
        nowTracingMode = "JustStarted"; // 처음 추격모드는 "JustStarted"
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
