using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{
    public Transform noteAxis; // 스킬체크 노트
    public Transform normalAxis; // 스킬체크 노트
    public GameObject noteCanvas; // 노트 캔버스UI

    public bool isChecked = false;
    public bool finish = false;


    private void Start()
    {
        noteCanvas.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Check();
            print("스페이스바 눌림!");
        }


    }


    public void CheckStart()
    {
        noteCanvas.SetActive(true);

    }

    public void Check()
    {
           
            if (noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z)
            {
                print("성공!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
                isChecked = true;
                finish = true;
                noteCanvas.SetActive(false);
                print(finish);
                
           
             }
            else
            {
                print("실패!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
                isChecked = false;
                finish = true;
                noteCanvas.SetActive(false);
                print(finish);
                
            }

    }



}