using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{
    // 발전기 컨트롤러

    // 발전기는 수리도를 갖는다.
    // 수리도를 UI와 연결한다.
    // 플레이어가 마우스 좌클 시 수리가 진행된다.(수리도가 올라간다.)
    // 일정 타이밍에 수리 타이밍게임이 나온다.

    public GameObject repairGuide;
    public GameObject repairSlider;
    public Slider repairSliderUI;

    public float repairSpeed = 0.01f;
    public bool isPlayerInTrigger = false;


    void Start()
    {
        // 수리안내, 슬라이더 UI를 false 상태로 시작한다.
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        // 슬라이더 바의 값을 0 으로 시작한다.
        repairSliderUI.value = 0;

    }

    void Update()
    {
        // 수리 영역 안에서 마우스 좌클 누르고 있는 때에는
        // 수리 안내 문구를 숨기고, 슬라이더 바만 보이게 한다.
        if(isPlayerInTrigger == true && Input.GetMouseButton(0))
        {
            print("수리 중입니다.");
            repairGuide.SetActive(false);
            repairSlider.SetActive(true);
            repairSliderUI.value += repairSpeed * Time.deltaTime;
        }


        // 수리 영역 안에서 마우스 좌클을 뗐을 때에는
        // 수리 슬라이더를 감추고, 수리 안내 문구를 띄운다.
        if(isPlayerInTrigger == true && Input.GetMouseButtonUp(0))
        {
            print("수리 중도 정지.");
            repairGuide.SetActive(true);
            repairSlider.SetActive(false);
        }

 
    }

    // 수리 영역안에 들어오면, 수리 안내 문구를 띄우고 isPlayerInTrigger true를 반환한다.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == ("Player"))
        {
            print("수리가능 영역에 들어옴.");
            repairGuide.SetActive(true);
            isPlayerInTrigger = true;
}
    }

    // 수리 영역에서 나가면, 수리 안내 문구를 감추고 isPlayerInTrigger false를 반환한다.
    // 수리 슬라이더도 감춘다.
    private void OnTriggerExit(Collider other)
    {
        print("수리가능 영역에서 나감.");
        repairGuide.SetActive(false);
        isPlayerInTrigger = false;
        repairSlider.SetActive(false);
    }






}
