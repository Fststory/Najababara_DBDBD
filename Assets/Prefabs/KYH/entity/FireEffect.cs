using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEffect : MonoBehaviour
{
    public Material[] materials; // ����� ��Ƽ���� �迭
    public float speed = 1.0f; // ������ �ӵ�
    public string noiseParam = "_NoiseValue"; // ���̴��� ������ �Ķ���� �̸�

    void Update()
    {
        // �� ��Ƽ���� ���� ������ ���� ������Ʈ
        foreach (Material material in materials)
        {
            float noiseValue = Mathf.PerlinNoise(Time.time * speed, 0); // �޸� ������ �� ���
            material.SetFloat(noiseParam, noiseValue); // ���̴��� ������ �Ķ���� ������Ʈ
        }
    }
}
