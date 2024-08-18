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

    public int numberOfRepairs = 2; // 수리해야할 Generator의 수
    public int CompleteRepair;

    public GameObject geneImage;
    public GameObject exitImage;
    public GameObject geneTXT;
    public Text geneCount;
 

    
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
        exitSystem = exit.GetComponent<ExitSystem>();
        exitImage.SetActive(false);



    }

    public void AddRepairCount(int CompleteGenerator)
    {
        // 수리 완료된 발전기의 수를 CompleteRepair에 누적한다.
        CompleteRepair += CompleteGenerator;
    }

    private void Update()
    {
        geneCount.text = (numberOfRepairs - CompleteRepair).ToString();

        //만일 numberOfRepairs(수리해야할 수) 보다 CompleteRepair(수리완료한 수) 가 크거나 같다면
        if (CompleteRepair >= numberOfRepairs)
        {
            // exitSystem의 OpenExit 함수를 실행한다.
            exitSystem.OpenExit();

            geneTXT.SetActive(false);
            geneImage.SetActive(false);
            exitImage.SetActive(true);

        }


        


    }

}