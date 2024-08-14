using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{
    public SkillCheckSystem skillCheckSystem; // 각 발전기의 SkillCheckSystem을 참조
    public Animator playerAnim; // 플레이어 애니메이터

    public GameObject repairGuide; // 수리 안내 문구
    public GameObject repairSlider; // 수리 슬라이더
    public Slider repairSliderUI; // 수리 슬라이더 UI

    public float repairSpeed = 1.0f; // 수리 속도
    public bool isPlayerInTrigger = false; // 플레이어가 트리거에 들어왔는지 체크

    public float repairPercent = 0.0f; // 현재 수리 퍼센트

    public bool isSkillChecking = false; // 스킬 체크 중인지
    public bool Complete = false; // 수리 완료했는지

    public bool SkillCheck1 = false; // 스킬 체크가 한 번 발동했는지 체크

    public float randomPercent = 0f; // 스킬 체크가 발동하는 랜덤 시간

    public GameObject explosion; // 폭발 이펙트

    public GameObject geLight;


    private void Awake()
    {
        // UI 요소 초기화: 각 발전기의 UI 요소를 가져옵니다.
        repairGuide = GameObject.Find("img_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();
        geLight = FindChildGameObjectByName(gameObject, "GeneratorLight");

        // SkillCheckSystem과 상호 참조 설정
        skillCheckSystem = GetComponentInChildren<SkillCheckSystem>();
        skillCheckSystem.generatorSystem = this; // GeneratorSystem과 SkillCheckSystem을 서로 참조하도록 설정

        

    }

    void Start()
    {
        // UI 초기화 및 수리 슬라이더 값에 현재 수리 퍼센트 할당
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        repairSliderUI.value = repairPercent;
        geLight.SetActive(false);


        // 플레이어 애니메이터 컴포넌트 할당
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetMouseButton(0) && !Complete)
        {
            repairSliderUI.value = repairPercent;

            // 수리 퍼센트가 50 미만이면서 스킬 체크 중이 아닐 때
            if (repairPercent < 100 && !isSkillChecking)
            {
                RepairGenerator();
            }

            // 수리 퍼센트가 20 이상일 때
            if (repairPercent >= 20.0f)
            {
                if (randomPercent == 0)
                {
                    // 스킬 체크 발동 랜덤 시간을 추첨합니다.
                    randomPercent = Random.Range(25.0f, 30.0f);
                }

                // 수리 퍼센트가 랜덤 퍼센트 이상이면서 스킬 체크가 발동하지 않았을 때
                if (repairPercent >= randomPercent && !isSkillChecking && !SkillCheck1 && !Complete)
                {
                    StartSkillCheck();
                    SkillCheck1 = true;
                }
            }
        }

        if (isPlayerInTrigger && Input.GetMouseButtonUp(0) && !Complete)
        {
            StopRepair();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !Complete)
        {
            repairGuide.SetActive(true);
            isPlayerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !Complete)
        {
            StopRepair();
            repairGuide.SetActive(false);
            isPlayerInTrigger = false;
        }
    }

    // 수리 중일 때
    private void RepairGenerator()
    {
        playerAnim.SetBool("isRepair", true);
        repairGuide.SetActive(false);
        repairSlider.SetActive(true);

        repairPercent += repairSpeed * Time.deltaTime;
        if (repairPercent >= 100)
        {
            RepairsComplete();
        }
    }

    private void StartSkillCheck()
    {
        isSkillChecking = true; // 스킬 체크 시작 시 상태 설정
        skillCheckSystem.CheckStart(); // 연결된 SkillCheckSystem에 알림
    }

    public void OnSkillCheckComplete(bool success)
    {
        if (success)
        {
            RestartRepair();
        }
        else
        {
            FailedCheck();
            repairPercent *= 0.7f; // 실패 시 수리 퍼센트 감소
        }
        isSkillChecking = false; // 스킬 체크 완료 후 상태 변경
    }

    public void RestartRepair()
    {
        if (!isSkillChecking)
        {
            randomPercent = 0f; // 랜덤 퍼센트 초기화
            print("스킬 체크 완료 후 다시 수리 시작.");
            RepairGenerator();
        }
    }

    public void FailedCheck()
    {
        print("펑!");
        explosion.SetActive(true);
        playerAnim.SetTrigger("Exposion"); // 애니메이터의 Exposion 변수 설정
    }

    private void StopRepair()
    {
        print("수리를 종료합니다.");
        playerAnim.SetBool("isRepair", false);
        repairGuide.SetActive(true);
        repairSlider.SetActive(false);
    }

    private void RepairsComplete()
    {
        repairPercent = 100;
        playerAnim.SetBool("isRepair", false);
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        Complete = true;
        geLight.SetActive(true);

        SkillCheck1 = false; // 수리 완료 후 스킬 체크 초기화
        randomPercent = 0f; // 랜덤 퍼센트 초기화

        GeneratorManager.generatorManager.AddRepairCount(1);
    }


    // 자식 GameObject를 이름으로 찾는 함수
    GameObject FindChildGameObjectByName(GameObject parent, string name)
    {
        foreach (Transform child in parent.transform)
        {
            if (child.gameObject.name == name)
            {
                return child.gameObject;
            }
        }
        return null;
    }
}
