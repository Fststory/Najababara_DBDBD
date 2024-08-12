using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GeneratorSystem : MonoBehaviour
{
    public SkillCheckSystem skillCheckSystem; // 스킬 체크 시스템
    public Animator playerAnim; // 플레이어 애니메이터

    public GameObject repairGuide; // 수리 안내 문구
    public GameObject repairSlider; // 수리 슬라이더
    public Slider repairSliderUI; // 수리 슬라이더 UI

    public float repairSpeed = 1.0f; // 수리 속도
    public bool isPlayerInTrigger = false; // 플레이어가 트리거에 들어 왔는지 체크

    public float repairPercent = 0.0f; // 현재 수리 퍼센트

    public bool isSkillChecking = false; // 스킬체크 중인 지
    public bool Complete = false; // 수리 완료 했는 지

    public bool SkillCheck1 = false;

    public float randomPercent = 0f; // 스킬체크가 발동하는 랜덤 시간

    public GameObject explosion;

    private void Awake()
    {
        // UI 요소와 스킬체크 시스템을 할당
        repairGuide = GameObject.Find("TXT_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();

        skillCheckSystem = FindObjectOfType<SkillCheckSystem>();
    }

    void Start()
    {
        // UI 초기화 및 수리슬라이더 값에 현재수리퍼센트 할당
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        repairSliderUI.value = repairPercent;

        // 플레이어 애니메이터 컴포넌트 할당
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {

        if (isPlayerInTrigger && Input.GetMouseButton(0) && !Complete)
        {
            repairSliderUI.value = repairPercent;

            // 수리퍼센트가 50 미만이면서 스킬체크중일 떄
            if (repairPercent < 50 && !isSkillChecking)
            {
                RepairGenerator();
            }

            // 수리퍼센트가 20 이상일 때
            if (repairPercent >= 20.0f)
            {
                if (randomPercent == 0)
                {   // 스킬체크 발동 랜덤시간을 추첨한다.
                    randomPercent = Random.Range(25.0f, 30.0f);
                }

                // 수리퍼센트가 랜덤퍼센트 이상이면서 스킬체크가 발동하지 않았을 때
                if (repairPercent >= randomPercent && !isSkillChecking && !SkillCheck1)
                {
                    StartSkillCheck();
                    SkillCheck1 = true;
                }

                if (repairPercent >= randomPercent && !isSkillChecking && SkillCheck1)
                {
                    RestartRepair();

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
        playerAnim.SetBool("Exposion", false);
    }

    private void StartSkillCheck()
    {
        isSkillChecking = true;
        skillCheckSystem.CheckStart();


    }

    public void RestartRepair()
    {
        if (isSkillChecking == false)
        {
            print("스킬체크 완료 후 다시 수리시작.");
            RepairGenerator();


        }
    }
    public void FailedCheck()
    {
        print("펑!");
        Instantiate(explosion, transform.position, Quaternion.identity);
        playerAnim.SetBool("Exposion", true);
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

        GeneratorManager.generatorManager.AddRepairCount(1);
    }
}