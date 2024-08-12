using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{
    public static SkillCheckSystem skillCheckSystem;
    public GeneratorSystem generatorSystem;
    public Transform noteAxis; // ��ųüũ ��Ʈ
    public Transform normalAxis; // ��ųüũ ��Ʈ
    public GameObject noteCanvas; // ��Ʈ ĵ����UI

    public bool isChecked = false;
    public bool finish = false;


    //private void Awake()
    //{
    //    // �ٸ� �ν��Ͻ��� �̹� �����ϸ� �� ��ü�� �ı��մϴ�.
    //    if (skillCheckSystem == null)
    //    {
    //        skillCheckSystem = this;
    //        DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü�� �ı����� �ʵ��� ����
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
            print("�����̽��� ����!");
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
            print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            isChecked = true;
            finish = true;
            noteCanvas.SetActive(false);
            print(finish);
            generatorSystem.RestartRepair();
            generatorSystem.isSkillChecking = false;




        }
        else
        {
            print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
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