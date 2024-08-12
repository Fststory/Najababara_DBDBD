using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class GeneratorSystem : MonoBehaviour
{
    public SkillCheckSystem skillCheckSystem; // ��ų üũ �ý���
    public Animator playerAnim; // �÷��̾� �ִϸ�����

    public GameObject repairGuide; // ���� �ȳ� ����
    public GameObject repairSlider; // ���� �����̴�
    public Slider repairSliderUI; // ���� �����̴� UI

    public float repairSpeed = 1.0f; // ���� �ӵ�
    public bool isPlayerInTrigger = false; // �÷��̾ Ʈ���ſ� ��� �Դ��� üũ

    public float repairPercent = 0.0f; // ���� ���� �ۼ�Ʈ

    public bool isSkillChecking = false; // ��ųüũ ���� ��
    public bool Complete = false; // ���� �Ϸ� �ߴ� ��

    public bool SkillCheck1 = false;

    public float randomPercent = 0f; // ��ųüũ�� �ߵ��ϴ� ���� �ð�

    public GameObject explosion;

    private void Awake()
    {
        // UI ��ҿ� ��ųüũ �ý����� �Ҵ�
        repairGuide = GameObject.Find("TXT_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();

        skillCheckSystem = FindObjectOfType<SkillCheckSystem>();
    }

    void Start()
    {
        // UI �ʱ�ȭ �� ���������̴� ���� ��������ۼ�Ʈ �Ҵ�
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        repairSliderUI.value = repairPercent;

        // �÷��̾� �ִϸ����� ������Ʈ �Ҵ�
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {

        if (isPlayerInTrigger && Input.GetMouseButton(0) && !Complete)
        {
            repairSliderUI.value = repairPercent;

            // �����ۼ�Ʈ�� 50 �̸��̸鼭 ��ųüũ���� ��
            if (repairPercent < 50 && !isSkillChecking)
            {
                RepairGenerator();
            }

            // �����ۼ�Ʈ�� 20 �̻��� ��
            if (repairPercent >= 20.0f)
            {
                if (randomPercent == 0)
                {   // ��ųüũ �ߵ� �����ð��� ��÷�Ѵ�.
                    randomPercent = Random.Range(25.0f, 30.0f);
                }

                // �����ۼ�Ʈ�� �����ۼ�Ʈ �̻��̸鼭 ��ųüũ�� �ߵ����� �ʾ��� ��
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

    // ���� ���� ��
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
            print("��ųüũ �Ϸ� �� �ٽ� ��������.");
            RepairGenerator();


        }
    }
    public void FailedCheck()
    {
        print("��!");
        explosion.SetActive(true);
        playerAnim.SetBool("Exposion", true);
    }


    private void StopRepair()
    {
        print("������ �����մϴ�.");
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