using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : MonoBehaviour
{

    public static GeneratorManager generatorManager { get; private set; }
    public ExitSystem exitSystem;
    public GameObject generatorPrefab; // Generator 프리팹
    public int numberOfGenerators = 7; // 생성할 Generator의 수
    public int numberOfRepairs = 2; // 수리해야할 Generator의 수
    private GameObject[] generators;
    public float x_min, x_max, y_min, y_max;

 

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

        // numberOfGenerators의 수 만큼 반복한다.
        for (int i = 0; i < numberOfGenerators; i++)
        {
            // Generator 프리팹 인스턴스 생성
           // GameObject generator = Instantiate(generatorPrefab, new Vector3(Random.Range(x_min,x_max), 0.0f, Random.Range(y_min,y_max)), Quaternion.identity);
            //generators[i] = generator;


            // exitSystem.generator는 생성된 generator프리팹이다.
            //exitSystem.generator = generator;

            // exitSystem.generatorSystem은 생성된 generator프리팹의 컴포넌트인 GeneratorSystem이다.
            //exitSystem.generatorSystem = generator.GetComponent<GeneratorSystem>();
           // GeneratorSystem generatorSystem = generator.GetComponent<GeneratorSystem>();
            
           
            // numberOfRepairs(수리해야할 수)는 numberOfGenerators(생성할 발전기 수)에서 1을 뺀 값이다.
            //numberOfRepairs = (numberOfGenerators - 1); 

        }
       
    }

    public void AddRepairCount(int CompleteGenerator)
    {
        // 수리 완료된 발전기의 수를 CompleteRepair에 누적한다.
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        // 만일 numberOfRepairs(수리해야할 수) 보다 CompleteRepair(수리완료한 수) 가 크거나 같다면
        if (CompleteRepair >= numberOfRepairs)
        {
            // exitSystem의 OpenExit 함수를 실행한다.
            exitSystem.OpenExit();
        }
        
    }

}