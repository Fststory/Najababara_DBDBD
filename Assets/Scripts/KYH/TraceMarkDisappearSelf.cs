using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceMarkDisappearSelf : MonoBehaviour
{
    // 씬에 생성된 흔적이 일정 시간 이후 스스로 사라지는 기능

    public float currentTime = 0.0f;
    public float disappearTime = 8.0f;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > disappearTime)
        {
            Destroy(gameObject);
        }
    }
}
