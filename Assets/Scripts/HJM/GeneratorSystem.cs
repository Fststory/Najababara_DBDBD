using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{
    // ������ ��Ʈ�ѷ�

    // ������� �������� ���´�.
    // �������� UI�� �����Ѵ�.
    // �÷��̾ ���콺 ��Ŭ �� ������ ����ȴ�.(�������� �ö󰣴�.)
    // ���� Ÿ�ֿ̹� ���� Ÿ�ְ̹����� ���´�.

    //public PlayerController playerController; *****������� ������ ���۵� �� �����ϱ� �� �������� �ν����Ϳ��� �÷��̾� ���� ������Ʈ �־��ִ� ���� �Ұ������� ����!!!*****

    public SkillCheckSystem skillCheckSystem;

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
    public bool checking = false;
    public bool skillChecking = false;
    public bool check1;
    public bool check2;
    public bool checkStart = false;


    public float randomPercent;


    private void Awake()
    {
        // repairGuide, repairSlider, repairSliderUI�� ���ӿ��� ã�Ƽ� �Ҵ��Ѵ�.
        repairGuide = GameObject.Find("TXT_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();

        skillCheckSystem = FindObjectOfType<SkillCheckSystem>();
    }
    void Start()

    {

        // �����ȳ�, �����̴� UI�� false ���·� �����Ѵ�.
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        // �����̴� ���� ���� 0 ���� �����Ѵ�.
        repairSliderUI.value = repairPercent;
        repairPercent = 20.0f;
        //// playerAnim�� playerController�� �ִϸ����� ������Ʈ��.
        //playerAnim = playerController.GetComponent<Animator>();
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

    }

    void Update()
    {
        // ���� ���� �ȿ��� ���콺 ��Ŭ ������ �ִ� ������
        // ���� �ȳ� ������ �����, �����̴� �ٸ� ���̰� �Ѵ�.


        // �Ϲ� ����
        // �÷��̾ Ʈ���� ���̸�, ���콺 ��Ŭ�� ������ �ִ� �����̸�, �����ۼ�Ʈ�� 50���� �� �� �����Ѵ�.
        if (isPlayerInTrigger == true && Input.GetMouseButton(0) && repairPercent < 50 && !checkStart)
        {
            repairSliderUI.value = repairPercent;


            RepairGenerator();
        }

        //1�� ��ųüũ ����
        //�÷��̾ Ʈ���� ���̸�, ���콺 ��Ŭ�� ������ �ִ� �����̸�, �����ۼ�Ʈ�� 20�̻� �� �� �����Ѵ�.
        if (isPlayerInTrigger == true && Input.GetMouseButton(0) && repairPercent >= 20.0f)
        {
            repairSliderUI.value = repairPercent;
            print("�ۼ�Ʈ�� ��÷�մϴ�.");


            // ���� ���� ó�� �� ���� ��÷�ϵ��� ����
            if (randomPercent == 0)
            {
                // ���� ���� ����
                randomPercent = Random.Range(25.0f, 30.0f);
                print(randomPercent);
            }

            // ��÷�� ���� �������� �����ߴٸ� ��ų�ý����� üũ�����Լ��� ȣ���Ѵ�.
            if (repairPercent >= randomPercent && check1)
            {
                check1 = true;
                skillCheckSystem.CheckStart();
                print("��ųüũ ������");
                checking = true;


                // ��ųüũ�� ������ �ʾҴٸ�
                if (skillCheckSystem.finish && !checking)
                {
                    // üũ�����(��ųüũ�� ���� ������ �������)
                    CheckRepair();

                }
                // ��ųüũ�� �����ٸ�
                else if (skillCheckSystem.finish && checking)
                {
                    checkStart = true;
                    // �ٽ� ������ �����Ѵ�.
                    print("��ųüũ ������ �ٽ� �����մϴ�.");
                    RepairGenerator();

                }
            }






            // ���� ���� �ȿ��� ���콺 ��Ŭ�� ���� ����
            // ���� �Ϸ� ���°� false�� ����
            // ���� �����̴��� ���߰�, ���� �ȳ� ������ ����.
            if (Complete == false)
            {
                if (isPlayerInTrigger == true && Input.GetMouseButtonUp(0))
                {
                    repairSliderUI.value = repairPercent;

                    print("���� �ߵ� ����.");
                    playerAnim.SetBool("isRepair", false);
                    repairGuide.SetActive(true);
                    repairSlider.SetActive(false);
                }
            }
        }
    }
        // ���� �����ȿ� ������, ���� �ȳ� ������ ���� isPlayerInTrigger true�� ��ȯ�Ѵ�.
        private void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.name == ("Player"))
            {
                // ���⼭ �÷��̾� �ִϸ����� �޾ƿ�
                //playerAnim = other.gameObject.GetComponent<Animator>(); *********��ŸƮ�� �ű�ڽ��ϴ�*********

                if (Complete == false)
                {
                    print("�������� ������ ����.");

                    repairGuide.SetActive(true);
                    repairSlider.SetActive(false);
                    isPlayerInTrigger = true;
                }
            }
        }

        // ���� �������� ������, ���� �ȳ� ������ ���߰� isPlayerInTrigger false�� ��ȯ�Ѵ�.
        // ���� �����̴��� �����.
        private void OnTriggerExit(Collider other)
        {
            if (Complete == false)
            {
                print("�������� �������� ����.");
                playerAnim.SetBool("isRepair", false);
                repairGuide.SetActive(false);
                isPlayerInTrigger = false;
                repairSlider.SetActive(false);
            }
        }

        void RepairsComplete()
        {
            print("���� �Ϸ��");
            GeneratorManager.generatorManager.AddRepairCount(1);
            repairPercent = 100;
            repairGuide.SetActive(false);
            repairSlider.SetActive(false);
            Complete = true;

        }

        void RepairGenerator()
        {
            print("���� ���Դϴ�.");
            repairSliderUI.value = repairPercent;
            //print("���� �ִ� ���");
            playerAnim.SetBool("isRepair", true);
            repairGuide.SetActive(false);
            repairSlider.SetActive(true);
            repairPercent += repairSpeed * Time.deltaTime;
            if (repairPercent >= 100)
            {
                print("�Ϸ� ī��Ʈ 1");
                playerAnim.SetBool("isRepair", false);
                RepairsComplete();
                Complete = true;
                return;
            }
        }

        void CheckRepair()
        {
            print("��ųüũ�� ���� ���� ����");
            checkStart = true;
            repairSliderUI.value = repairPercent;
            playerAnim.SetBool("isRepair", true);
            repairGuide.SetActive(false);
            repairSlider.SetActive(true);

            if (skillCheckSystem.finish == true)
            {
                checking = false;
                checkStart = false;

            }

        }


    
}
