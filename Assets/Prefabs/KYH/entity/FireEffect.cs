using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public Material[] materials; // 사용할 머티리얼 배열
    public float speed = 1.0f; // 노이즈 속도
    public string noiseParam = "_NoiseValue"; // 쉐이더의 노이즈 파라미터 이름

    void Update()
    {
        // 각 머티리얼에 대해 노이즈 값을 업데이트
        foreach (Material material in materials)
        {
            float noiseValue = Mathf.PerlinNoise(Time.time * speed, 0); // 펄린 노이즈 값 계산
            material.SetFloat(noiseParam, noiseValue); // 쉐이더의 노이즈 파라미터 업데이트
        }
    }
}
