using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))] // �÷��̾ ������ �þ� �ݶ��̴� => ���� �����ϸ� ����ü ������ �ݶ��̴��� �����ϰ� ����
public class EnemyTracingMod : MonoBehaviour
{
    // ���ſ� ���� ������ �߰ݸ�带 "EnemyMovingManager"�� �����ϴ� ��ũ��Ʈ
    
    /*  ���� AI�� ���� �ִ� ���ſ� ���� �߰ݻ��¿� ������ ��ħ.
        ������ ���Ű� �߰��ǰų� ������ٰ� �ص� �߰ݻ��¿� ������ ���� �� ����
        ���� �߰� ���°� �߿��� ���Ÿ� �������� �������ִٸ� �� �߿��� ���Ű� �߰�/�����Ǵ��� ������ ���� ����

    */

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // ���� �÷��̾ Ʈ���ſ� ���� �ʴ´ٸ�
        if (!other.gameObject.CompareTag("Player"))
        {
            // �⺻ Ž����� On
            EnemyMovingManager.emm.nowTracingMode = "NoEvidence";
        }
        // ���� �÷��̾ Ʈ���ſ� ��´ٸ�
        else
        {
            // ���� �߰ݸ�� On
            EnemyMovingManager.emm.nowTracingMode = "PlayerFind";
        }
    }
}
