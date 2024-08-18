using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public float radius; // ���� ������
    public float speed; // ȸ�� �ӵ�
    public float roughness; // ������ ����

    private float angle; // ���� ����


    void Update()
    {
        // ���� ������Ʈ
        angle += speed * Time.deltaTime;

        // ������ �������� ���� �ణ�� ���������� �߰�
        float randomOffset = Random.Range(-roughness, roughness);

        // ���� ��ġ ���
        float x = Mathf.Cos(angle) * radius + randomOffset;
        float z = Mathf.Sin(angle) * radius + randomOffset;

        // Ʈ�������� ������ ������Ʈ
        transform.localEulerAngles += new Vector3(x, 0, z);
        transform.localPosition += new Vector3(0, -0.0003f, 0);
    }
}
