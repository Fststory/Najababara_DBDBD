using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorManager : MonoBehaviour
{

    public static GeneratorManager generatorManager { get; private set; }
    public ExitSystem exitSystem;
    public GameObject exit;
    private GameObject[] generators;

    public int numberOfRepairs = 2; // �����ؾ��� Generator�� ��
    public int CompleteRepair;

    public GameObject geneImage;
    public GameObject exitImage;
    public GameObject geneTXT;
    public Text geneCount;
 

    
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
        exitImage.SetActive(false);



    }

    public void AddRepairCount(int CompleteGenerator)
    {
        // ���� �Ϸ�� �������� ���� CompleteRepair�� �����Ѵ�.
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        geneCount.text = (numberOfRepairs - CompleteRepair).ToString();

        //���� numberOfRepairs(�����ؾ��� ��) ���� CompleteRepair(�����Ϸ��� ��) �� ũ�ų� ���ٸ�
        if (CompleteRepair >= numberOfRepairs)
        {
            // exitSystem�� OpenExit �Լ��� �����Ѵ�.
            exitSystem.OpenExit();

            geneTXT.SetActive(false);
            geneImage.SetActive(false);
            exitImage.SetActive(true);

        }


        


    }

}