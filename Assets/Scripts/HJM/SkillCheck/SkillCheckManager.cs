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
    bool isGeneratorFound = false; // Generator ������Ʈ�� ã�Ҵ��� ����

    // ������ �����̴����� ������ �ۼ�Ʈ �������� �����ϸ�, ��ųüũ�� Ȱ��ȭ �ȴ�.


    // ��ųüũUI�� ���۽� ���������� ȸ���� z�� ������÷�Ͽ� ������ �ִ�.


    // ��ųüũ�� ��Ÿ���� �ٽ� ��Ȱ��ȭ�� �� ���ο� �������� ��÷�Ѵ�.
    void Start()
    {

        
        randomPer = 0;


    }

    void Update()
    {




    }

    // �ֱ������� GeneratorSystem�� ã�� �ڷ�ƾ
    
    public void SkillCheckStart()
    {
        skillCanvus.SetActive(true);
        isCheckStart = true;
    }



    // ������ Ư�� �ۼ�Ʈ���� ��ųüũ�� Ȱ��ȭ��
    public void RandomPercent(float currentRepairPercent)
    {
        if (isCheckStart)
        {
            repairPercent = currentRepairPercent;
            randomPer = Random.Range(50.0f, 100.0f);
            if (repairPercent >= randomPer)
            {
                print("��ųüũ �߻�");
                print("��ųüũ �߻�");
            }

        }
    }
}
