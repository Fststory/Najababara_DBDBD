using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckSystem : MonoBehaviour
{
    public Transform noteAxis; // ��ųüũ ��Ʈ
    public Transform normalAxis; // ��ųüũ ��Ʈ
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �����̽��ٰ� ������ ��
            print("�����̽��� ����!");
            if (noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z)
            {
                print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            }
            else
            {
                print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            }
        }

    }
}