using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCheckSystem : MonoBehaviour
{
    public Transform noteAxis; // 스킬체크 노트
    public Transform normalAxis; // 스킬체크 노트
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 스페이스바가 눌렸을 때
            print("스페이스바 눌림!");
            if (noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z)
            {
                print("성공!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            }
            else
            {
                print("실패!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            }
        }

    }
}