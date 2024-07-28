using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorController : MonoBehaviour
{

    public GameObject repairGuideUI;
    public GameObject repairPercentUI;
    public GameObject timingGameUI;
    public Scrollbar repairPercentBar;

    public float speed = 0.0000000000000000001f;

    public float repairPercent = 0.0f;

    private bool isPlayerInTrigger = false;


    // 발전기 컨트롤러

    // 발전기는 수리도를 갖는다.
    // 수리도를 UI와 연결한다.
    // 플레이어가 마우스 좌클 시 수리가 진행된다.(수리도가 올라간다.)
    // 일정 타이밍에 수리 미니게임이 나온다.

    // 수리 미니게임이란
    // 칸이 뜨고 화살표가 칸 위를 지나간다. 
    // 표시된 영역 안에 화살표가 들어오면 스페이스바를 눌러야한다.
    // 타이밍을 맞추지 못하면 발전기의 수리도가 감소한다.


    // 발전기를 일정 개수 수리하면 출구장치를 사용할 수 있다.







    void Start()
    {
        // 관련 UI를 false 상태로 시작한다.
        repairGuideUI.SetActive(false);
        repairPercentUI.SetActive(false);
        timingGameUI.SetActive(false);

        //// 스크롤바 값이 변경될 때 마다 OnScrollbarValueChanged 함수를 호출한다.
        //repairPercentBar.onValueChanged.AddListener(OnScrollbarValueChanged);
        // 스크롤바 값을 0으로 시작한다.
        repairPercentBar.size = 0;
    }

    //void OnScrollbarValueChanged(float size)
    //{   // 퍼센트로 표현하기 위해 100을 곱한다.
    //    repairPercent = repairPercentBar.size * 100;

    //}




    void Update()
    {
        // float형 repairPercent에 스크롤바 값을 담는다.
        repairPercent = repairPercentBar.size;

        // 플레이어가 트리거 내에 있을 때 좌측 컨트롤 키 누름 입력을 확인
        if (isPlayerInTrigger && Input.GetKey(KeyCode.LeftControl))
        {
            print("수리중입니다.");
            repairGuideUI.SetActive(false);
            repairPercentUI.SetActive(true);
            repairPercentBar.size += speed * Time.deltaTime;
        }
        // 플레이어가 트리거 내에 있을 때 좌측 컨트롤 키 뗌 입력을 확인
        if (isPlayerInTrigger && Input.GetKeyUp(KeyCode.LeftControl))
        {
            repairPercentUI.SetActive(false);
            timingGameUI.SetActive(false);
        }

        TimingGame();



    }


    // 특정 퍼센트 값마다 발전기타이밍게임 함수를 호출한다.
    void TimingGame()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && repairPercentBar.size > 0.5)
        {
            timingGameUI.SetActive(true);
            print("50% 수리중");
           
        }
      

    }

    // 플레이어가 발전기 트리거에 들어 왔을 때 Repair 텍스트를 송출한다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("플레이어 들어옴");
            repairGuideUI.SetActive(true);
            isPlayerInTrigger = true;           
        }
    }

    // 플레이어가 트리거에서 나갔을 때 UI들을 숨기고 isPlayerInTrigger를 false 처리한다.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("플레이어 나감");
            repairGuideUI.SetActive(false);
            repairPercentUI.SetActive(false);
            isPlayerInTrigger = false;
            timingGameUI.SetActive(false);
        }
    }


}
