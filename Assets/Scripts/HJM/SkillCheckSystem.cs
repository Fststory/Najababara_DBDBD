using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{
    public static SkillCheckSystem skillCheckSystem;
    public GeneratorSystem generatorSystem;
    public Transform noteAxis; // 스킬체크 노트
    public Transform normalAxis; // 스킬체크 노트
    public GameObject noteCanvas; // 노트 캔버스UI

    public bool isChecked = false;
    public bool finish = false;


    //private void Awake()
    //{
    //    // 다른 인스턴스가 이미 존재하면 이 객체를 파괴합니다.
    //    if (skillCheckSystem == null)
    //    {
    //        skillCheckSystem = this;
    //        DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}
    private void Start()
    {
        noteCanvas.SetActive(false);
        generatorSystem = FindObjectOfType<GeneratorSystem>();
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
        finish = false;

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
            generatorSystem.RestartRepair();
            generatorSystem.isSkillChecking = false;




        }
        else
        {
            print("실패!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            isChecked = false;
            finish = true;
            noteCanvas.SetActive(false);
            print(finish);
            generatorSystem.FailedCheck();
            generatorSystem.repairPercent = generatorSystem.repairPercent * 0.7f;
            generatorSystem.isSkillChecking = false;
        }

    }



}