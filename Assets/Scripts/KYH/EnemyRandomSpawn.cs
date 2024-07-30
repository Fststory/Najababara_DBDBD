using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomSpawn : MonoBehaviour
{
    // �� �÷��̸��� ���ʹ̰� ���� ��ǥ���� ���� ������ �ٶ󺸸� ����
    
    int startLookDir; // ������ �� y�� ȸ����(����)
    Transform floorTransform;

    void Start()
    {
        SetStartPosition();
        SetStartLookDir();
    }
    void SetStartPosition() // ó�� ������ ��ġ�� ����
    {
        floorTransform = GameObject.FindGameObjectWithTag("Floor").transform;
        float startPosX, startPosY, startPosZ;
        startPosX = Random.Range(-5, 5);   // ���� ���� ũ�⿡ ���� ��ǥ�� �ּ�/�ִ븦 ���� ******************************************
        startPosY = floorTransform.position.y + 1;  // Floor ���̿��� Ű��ŭ �ö�� ��ġ => ���� ĳ������ Ű�� ���� ���� ********
        startPosZ = Random.Range(-5, 5);
        transform.position = new Vector3(startPosX, startPosY, startPosZ);

    }

    void SetStartLookDir()  // ó�� �ٶ󺸴� ������ ����
    {
        startLookDir = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, startLookDir, 0);
    }
}
