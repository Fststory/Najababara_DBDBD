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

        for (int i = 0; i < numberOfGenerators; i++)
        {
            // Generator ������ �ν��Ͻ� ����
            GameObject generator = Instantiate(generatorPrefab, new Vector3(Random.Range(0, 10), 0.5f, Random.Range(0, 10)), Quaternion.identity);
            generators[i] = generator;

            exitSystem.generator = generator;

            exitSystem.generatorSystem = generator.GetComponent<GeneratorSystem>();

            exitSystem.repairCount = exitSystem.generatorSystem.repairCount;

            numberOfRepairs = (numberOfGenerators - 1); 



        }
    }

    public void AddRepairCount(int CompleteGenerator)
    {
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        if(CompleteRepair >= numberOfRepairs)
        { 
            exitSystem.OpenExit();
        }
        
    }

}