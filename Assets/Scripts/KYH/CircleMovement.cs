using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float radius; // 원의 반지름
    public float speed; // 회전 속도
    public float roughness; // 러프함 정도

    private float angle; // 현재 각도


    void Update()
    {
        // 각도 업데이트
        angle += speed * Time.deltaTime;

        // 러프한 움직임을 위해 약간의 무작위성을 추가
        float randomOffset = Random.Range(-roughness, roughness);

        // 원의 위치 계산
        float x = Mathf.Cos(angle) * radius + randomOffset;
        float z = Mathf.Sin(angle) * radius + randomOffset;

        // 트랜스폼의 포지션 업데이트
        transform.localEulerAngles += new Vector3(x, 0, z);
        transform.localPosition += new Vector3(0, -0.0001f, 0);
    }
}
