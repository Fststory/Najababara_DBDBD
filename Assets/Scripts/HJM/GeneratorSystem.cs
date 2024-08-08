using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{
    // 발전기 컨트롤러

    // 발전기는 수리도를 갖는다.
    // 수리도를 UI와 연결한다.
    // 플레이어가 마우스 좌클 시 수리가 진행된다.(수리도가 올라간다.)
    // 일정 타이밍에 수리 타이밍게임이 나온다.

    //public PlayerController playerController;

    public SkillCheckManager skillCheckManager;

    public Animator playerAnim;

    public GameObject repairGuide;
    public GameObject repairSlider;
    public Slider repairSliderUI;

    public float repairSpeed = 1.0f;
    public bool isPlayerInTrigger = false;

    public int CompleteGenerator = 0;
    public float repairPercent = 0.0f;
    public int repairCount;
    public bool Complete = false;


    private void Awake()
    {
        // repairGuide, repairSlider, repairSliderUI을 게임에서 찾아서 할당한다.
        repairGuide = GameObject.Find("TXT_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();
    }
    void Start()

    {
        Debug.Log("Start method called in GeneratorSystem.");

        skillCheckManager = FindObjectOfType<SkillCheckManager>();
        if (skillCheckManager == null)
        {
            Debug.LogError("SkillCheckManager를 찾을 수 없습니다.");
            return;
        }
        Debug.Log("SkillCheckManager found.");

        
        // 수리안내, 슬라이더 UI를 false 상태로 시작한다.
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        // 슬라이더 바의 값을 0 으로 시작한다.
        repairSliderUI.value = repairPercent;
        repairPercent = 38.0f;
        //// playerAnim은 playerController의 애니메이터 컴포넌트다.
        //playerAnim = playerController.GetComponent<Animator>();

    }

    void Update()
    {
        // 수리 영역 안에서 마우스 좌클 누르고 있는 때에는
        // 수리 안내 문구를 숨기고, 슬라이더 바만 보이게 한다.


        // 일반 수리
        // 플레이어가 트리거 안이며, 마우스 좌클을 누르고 있는 동안이며, 수리퍼센트가 50이하 일 때 실행한다.
        if (isPlayerInTrigger == true && Input.GetMouseButton(0) && repairPercent < 50)
        {
            repairSliderUI.value = repairPercent;

            skillCheckManager.RandomPercent(repairPercent); // 현재 repairPercent 전달
            print(skillCheckManager.randomPer);
            print(skillCheckManager.repairPercent);
            skillCheckManager.RandomPercent(repairPercent);
            RepairGenerator();
        }

        // 스킬체크 수리
        // 플레이어가 트리거 안이며, 마우스 좌클을 누르고 있는 동안이며, 수리퍼센트가 50이상 일 때 실행한다.
        if (isPlayerInTrigger == true && Input.GetMouseButton(0) && repairPercent > 50)
        {
            repairSliderUI.value = repairPercent;

            skillCheckManager.RandomPercent(repairPercent); // 현재 repairPercent 전달
            print(skillCheckManager.randomPer);
            print(skillCheckManager.repairPercent);
            skillCheckManager.RandomPercent(repairPercent);
            RepairGenerator();
        }


        // 수리 영역 안에서 마우스 좌클을 뗐을 때에
        // 수리 완료 상태가 false일 때에
        // 수리 슬라이더를 감추고, 수리 안내 문구를 띄운다.
        if (Complete == false)
        {
            if (isPlayerInTrigger == true && Input.GetMouseButtonUp(0))
            {
                repairSliderUI.value = repairPercent;

                print("수리 중도 정지.");
                playerAnim.SetBool("isRepair", false);
                repairGuide.SetActive(true);
                repairSlider.SetActive(false);
            }
        }
    }

    // 수리 영역안에 들어오면, 수리 안내 문구를 띄우고 isPlayerInTrigger true를 반환한다.
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.name == ("Player"))
        {
            // 여기서 플레이어 애니메이터 받아와
            playerAnim = other.gameObject.GetComponent<Animator>();

            if (Complete == false)
            {
                print("수리가능 영역에 들어옴.");

                repairGuide.SetActive(true);
                repairSlider.SetActive(false);
                isPlayerInTrigger = true;
            }
        }
    }

    // 수리 영역에서 나가면, 수리 안내 문구를 감추고 isPlayerInTrigger false를 반환한다.
    // 수리 슬라이더도 감춘다.
    private void OnTriggerExit(Collider other)
    {
        if (Complete == false)
        {
            print("수리가능 영역에서 나감.");
            playerAnim.SetBool("isRepair", false);
            repairGuide.SetActive(false);
            isPlayerInTrigger = false;
            repairSlider.SetActive(false);
        }
    }

    void RepairsComplete()
    {
        print("수리 완료됨");
        GeneratorManager.generatorManager.AddRepairCount(1);
        //repairCount += CompleteGenerator;
        repairPercent = 100;
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        Complete = true;

    }

    void RepairGenerator()
    {
        print("수리 중입니다.");
        repairSliderUI.value = repairPercent;
        //print("수리 애니 재생");
        playerAnim.SetBool("isRepair", true);
        repairGuide.SetActive(false);
        repairSliderUI.value = repairPercent;
        repairSlider.SetActive(true);
        repairPercent += repairSpeed * Time.deltaTime;
        if (repairPercent >= 100)
        {
            print("완료 카운트 1");
            playerAnim.SetBool("isRepair", false);
            RepairsComplete();
            Complete = true;
            return;
        }
    }

    void SkillCheck()
    {




    }


}