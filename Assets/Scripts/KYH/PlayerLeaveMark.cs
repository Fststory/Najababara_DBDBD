using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaveMark : MonoBehaviour
{
    // 플레이어가 이동하며 흔적을 남기는 기능

    /*  
        뛰어다닐 때 흔적을 남김
     */

    public Transform player;    // 플레이어 트랜스폼 넣기
    public GameObject traceMarkPrefab;  // TraceMark 프리팹 넣기
    float currentTime = 0;  // 흔적 타이머
    public float markCycle = 0.5f;  // 흔적을 남기는 주기
    PlayerController playerController;   // 플레이어에 달려있는 PlayerController 컴포넌트 (start함수에서 캐싱할 것임)

    private void Start()
    {
        playerController = player.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.run)
        {
            LeaveMark();
        }
    }

    public void LeaveMark() // 플레이어가 뛰는 상태일 때 사용
    {
        currentTime += Time.deltaTime;
        if (currentTime > markCycle)
        {
            // 짧은 간격 동안 계속해서 흔적을 플레이어의 위치에 생성
            GameObject traceMark = Instantiate(traceMarkPrefab);
            traceMark.transform.position = new Vector3(player.position.x, 0.1f, player.position.z);  // 지면.position.y 보다 0.1f만큼 올려서 Zfighting을 방지, 이후 변수들로 관리하면 좋을 듯 *********************************************
            traceMark.transform.eulerAngles = player.eulerAngles + new Vector3(90, 0, 0);
            currentTime = 0;
        }
    }
}
