using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{

    public static GeneratorManager generatorManager { get; private set; }

    public ExitSystem exitSystem;
    public GameObject generatorPrefab; // Generator 프리팹
    public int numberOfGenerators = 3; // 생성할 Generator의 수
    public int numberOfRepairs; // 수리해야할 Generator의 수
    private GameObject[] generators;

    public int CompleteRepair;
    
    private void Awake()
    {
        // 다른 인스턴스가 이미 존재하면 이 객체를 파괴합니다.
        if (generatorManager == null)
        {
            generatorManager = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정
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
            // Generator 프리팹 인스턴스 생성
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