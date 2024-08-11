using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{

    public static GeneratorManager generatorManager { get; private set; }
    public ExitSystem exitSystem;
    public GameObject generatorPrefab; // Generator ������
    public int numberOfGenerators = 3; // ������ Generator�� ��
    public int numberOfRepairs; // �����ؾ��� Generator�� ��
    private GameObject[] generators;
    public float x_min, x_max, y_min, y_max;

 

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
        generators = new GameObject[numberOfGenerators];

        // numberOfGenerators�� �� ��ŭ �ݺ��Ѵ�.
        for (int i = 0; i < numberOfGenerators; i++)
        {
            // Generator ������ �ν��Ͻ� ����
            GameObject generator = Instantiate(generatorPrefab, new Vector3(Random.Range(x_min,x_max), 0.0f, Random.Range(y_min,y_max)), Quaternion.identity);
            generators[i] = generator;


            // exitSystem.generator�� ������ generator�������̴�.
            exitSystem.generator = generator;

            // exitSystem.generatorSystem�� ������ generator�������� ������Ʈ�� GeneratorSystem�̴�.
            exitSystem.generatorSystem = generator.GetComponent<GeneratorSystem>();
            GeneratorSystem generatorSystem = generator.GetComponent<GeneratorSystem>();
            
           
            // numberOfRepairs(�����ؾ��� ��)�� numberOfGenerators(������ ������ ��)���� 1�� �� ���̴�.
            numberOfRepairs = (numberOfGenerators - 1); 

        }
       
    }

    public void AddRepairCount(int CompleteGenerator)
    {
        // ���� �Ϸ�� �������� ���� CompleteRepair�� �����Ѵ�.
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        // ���� numberOfRepairs(�����ؾ��� ��) ���� CompleteRepair(�����Ϸ��� ��) �� ũ�ų� ���ٸ�
        if (CompleteRepair >= numberOfRepairs)
        {
            // exitSystem�� OpenExit �Լ��� �����Ѵ�.
            exitSystem.OpenExit();
        }
        
    }

}