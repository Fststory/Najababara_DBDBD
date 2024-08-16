using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{

    public static GeneratorManager generatorManager { get; private set; }
    public ExitSystem exitSystem;
    public GameObject exit;
    public int numberOfRepairs = 2; // �����ؾ��� Generator�� ��
    private GameObject[] generators;
    

 

    public int CompleteRepair;
    
    private void Awake()
    {
        // �ٸ� �ν��Ͻ��� �̹� �����ϸ� �� ��ü�� �ı��մϴ�.
        if (generatorManager == null)
        {
            generatorManager = this;
            DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü�� �ı����� �ʵ��� ����
        }
        else
        {
            Destroy(gameObject);
        }
    }




    void Start()
    {
        

        AddRepairCount(0);
        exitSystem = exit.GetComponent<ExitSystem>();

        

    }

    public void AddRepairCount(int CompleteGenerator)
    {
        // ���� �Ϸ�� �������� ���� CompleteRepair�� �����Ѵ�.
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        //���� numberOfRepairs(�����ؾ��� ��) ���� CompleteRepair(�����Ϸ��� ��) �� ũ�ų� ���ٸ�
        if (CompleteRepair >= numberOfRepairs)
        {
            // exitSystem�� OpenExit �Լ��� �����Ѵ�.
            exitSystem.OpenExit();
        }



    }

}