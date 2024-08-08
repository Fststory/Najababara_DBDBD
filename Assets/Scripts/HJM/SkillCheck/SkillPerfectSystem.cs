using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPerfectSystem : MonoBehaviour
{
    public GameObject note; // ��ųüũ ��Ʈ
    private bool isNoteTrigger = false;




    private void OnTriggerEnter(Collider other)
    {
        // ����, �浹�� ����� �±װ� "SkillNote"���
        if (other.gameObject.tag == "SkillNote")
        {
            isNoteTrigger = true;
            print("����Ʈ ��Ʈ");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "SkillNote")
        {
            isNoteTrigger = false;
        }
    }

    void Update()
    {
        if (isNoteTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            // �����̽��ٰ� ������ ��
            //print("����Ʈ �����̽��� ����!");
        }

        if (!isNoteTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            // �����̽��ٰ� ������ ��
           // print("����Ʈ miss!");
        }
    }
}