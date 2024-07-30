using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceMarkDisappearSelf : MonoBehaviour
{
    // ���� ������ ������ ���� �ð� ���� ������ ������� ���

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
