using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPerfectSystem : MonoBehaviour
{
    public GameObject note; // 스킬체크 노트
    private bool isNoteTrigger = false;




    private void OnTriggerEnter(Collider other)
    {
        // 만일, 충돌한 대상의 태그가 "SkillNote"라면
        if (other.gameObject.tag == "SkillNote")
        {
            isNoteTrigger = true;
            print("퍼펙트 노트");
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
            // 스페이스바가 눌렸을 때
            //print("퍼펙트 스페이스바 눌림!");
        }

        if (!isNoteTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            // 스페이스바가 눌렸을 때
           // print("퍼펙트 miss!");
        }
    }
}