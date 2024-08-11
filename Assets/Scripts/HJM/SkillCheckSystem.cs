using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{

    public GeneratorSystem generatorSystem;
    public GameObject generator;
    public Transform noteAxis; // ��ųüũ ��Ʈ
    public Transform normalAxis; // ��ųüũ ��Ʈ
    public GameObject noteCanvas; // ��Ʈ ĵ����UI


    public bool isChecked = false;
    public bool finish = false;


    private void Start()
    {
        noteCanvas.SetActive(false);

        print("���ʷ����� ã�� ����");
        generator = GameObject.FindWithTag("Generator");
        generatorSystem = generator.GetComponent<GeneratorSystem>();
        print("���ʷ����� ã�� �õ� �Ϸ�");

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
            print(finish);
            noteCanvas.SetActive(false);
            generatorSystem.RestartRepair();
            generatorSystem.isSkillChecking = false;

        }

        else
        {
            print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
            isChecked = false;
            finish = true;
            print(finish);
            noteCanvas.SetActive(false);
            generatorSystem.FailedCheck();
            generatorSystem.repairPercent = generatorSystem.repairPercent * 0.7f;
            generatorSystem.isSkillChecking = false;
        }
    }
}