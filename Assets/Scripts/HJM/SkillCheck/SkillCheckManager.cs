using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckManager : MonoBehaviour
{
    public float randomPer = 0;

    public GeneratorSystem generatorSystem;
    public float repairPercent = 0;
    public GameObject generator;
    public GameObject skillCanvus;

    bool isCheckStart = false;
    bool isGeneratorFound = false; // Generator 오브젝트를 찾았는지 여부

    // 수리도 슬라이더에서 랜덤한 퍼센트 게이지에 도달하면, 스킬체크가 활성화 된다.


    // 스킬체크UI는 시작시 판정범위의 회전값 z를 랜덤추첨하여 가지고 있다.


    // 스킬체크가 나타나고 다시 비활성화된 후 새로운 랜덤값을 추첨한다.
    void Start()
    {

        
        randomPer = 0;


    }

    void Update()
    {




    }

    // 주기적으로 GeneratorSystem을 찾는 코루틴
    
    public void SkillCheckStart()
    {
        skillCanvus.SetActive(true);
        isCheckStart = true;
    }



    // 랜덤한 특정 퍼센트에서 스킬체크가 활성화됨
    public void RandomPercent(float currentRepairPercent)
    {
        if (isCheckStart)
        {
            repairPercent = currentRepairPercent;
            randomPer = Random.Range(50.0f, 100.0f);
            if (repairPercent >= randomPer)
            {
                print("스킬체크 발생");
                print("스킬체크 발생");
            }

        }
    }
}
