using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRandomSpawn : MonoBehaviour
{
    // 매 플레이마다 에너미가 랜덤 좌표에서 랜덤 방향을 바라보며 시작
    
    int startLookDir; // 시작할 때 y축 회전값(랜덤)
    Transform floorTransform;

    void Start()
    {
        SetStartPosition();
        SetStartLookDir();
    }
    void SetStartPosition() // 처음 스폰될 위치를 정함
    {
        floorTransform = GameObject.FindGameObjectWithTag("Floor").transform;
        float startPosX, startPosY, startPosZ;
        startPosX = Random.Range(-5, 5);   // 이후 맵의 크기에 따라 좌표의 최소/최대를 대입 ******************************************
        startPosY = floorTransform.position.y + 1;  // Floor 높이에서 키만큼 올라온 위치 => 이후 캐릭터의 키에 따라 변경 ********
        startPosZ = Random.Range(-5, 5);
        transform.position = new Vector3(startPosX, startPosY, startPosZ);

    }

    void SetStartLookDir()  // 처음 바라보는 방향을 정함
    {
        startLookDir = Random.Range(0, 360);
        transform.eulerAngles = new Vector3(0, startLookDir, 0);
    }
}
