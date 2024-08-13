using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushCollision : MonoBehaviour
{
    // ���� �� Ȱ��ȭ�Ǵ� Ʈ���� �ݶ��̴�: �浹�� ����

    EnemyController enemyController;
    public bool crashed = false;
    BoxCollider boxCol;

    private void Start()
    {
        enemyController = transform.GetComponentInParent<EnemyController>();
        boxCol = GetComponent<BoxCollider>();
        boxCol.enabled = false; // ó���� NoEvidence �����̹Ƿ� Ʈ���� Off
    }

    void Update()
    {
        //���ʹ� ���°� ���ֶ��...
        if (enemyController.currentState == EnemyController.EnemyState.Rush)
        {
            // Ʈ���Ÿ� Ȱ��ȭ��Ų��.
            boxCol.enabled = true;
        }
        else
        {
            // ���ʹ� ���°� ���� ���°� �ƴ϶�� ��Ȱ��ȭ
            boxCol.enabled = false;
        }
    }

    // ������Ʈ�� Ʈ���ſ� (�÷��̾ ������ <= �̱���) ���� ��´ٸ�
    private void OnTriggerEnter(Collider other)
    {
        if (other != null)
        {
            // �浹�ߴٴ� ��ȣ�� ��� (���Ĵ� EnemyController���� ������ �� �ֵ��� �浹�ߴٴ� �͸� bool�� ��ȯ)
            crashed = true;
            print(other.gameObject.name + "�� �浹��!!");
            enemyController.NMA.ResetPath();
            // �浹�� ������ �ٽ� EnemyController���� false�� ��ȯ
        }
    }
}
