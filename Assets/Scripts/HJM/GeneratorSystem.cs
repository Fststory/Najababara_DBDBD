using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{
    public SkillCheckSystem skillCheckSystem; // �� �������� SkillCheckSystem�� ����
    public Animator playerAnim; // �÷��̾� �ִϸ�����

    public GameObject repairGuide; // ���� �ȳ� ����
    public GameObject repairSlider; // ���� �����̴�
    public Slider repairSliderUI; // ���� �����̴� UI

    public float repairSpeed = 1.0f; // ���� �ӵ�
    public bool isPlayerInTrigger = false; // �÷��̾ Ʈ���ſ� ���Դ��� üũ

    public float repairPercent = 0.0f; // ���� ���� �ۼ�Ʈ

    public bool isSkillChecking = false; // ��ų üũ ������
    public bool Complete = false; // ���� �Ϸ��ߴ���

    public bool SkillCheck1 = false; // ��ų üũ�� �� �� �ߵ��ߴ��� üũ

    public float randomPercent = 0f; // ��ų üũ�� �ߵ��ϴ� ���� �ð�

    public GameObject explosion; // ���� ����Ʈ

    public GameObject geLight;


    private void Awake()
    {
        // UI ��� �ʱ�ȭ: �� �������� UI ��Ҹ� �����ɴϴ�.
        repairGuide = GameObject.Find("img_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();
        geLight = FindChildGameObjectByName(gameObject, "GeneratorLight");

        // SkillCheckSystem�� ��ȣ ���� ����
        skillCheckSystem = GetComponentInChildren<SkillCheckSystem>();
        skillCheckSystem.generatorSystem = this; // GeneratorSystem�� SkillCheckSystem�� ���� �����ϵ��� ����

        

    }

    void Start()
    {
        // UI �ʱ�ȭ �� ���� �����̴� ���� ���� ���� �ۼ�Ʈ �Ҵ�
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        repairSliderUI.value = repairPercent;
        geLight.SetActive(false);


        // �÷��̾� �ִϸ����� ������Ʈ �Ҵ�
        playerAnim = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetMouseButton(0) && !Complete)
        {
            repairSliderUI.value = repairPercent;

            // ���� �ۼ�Ʈ�� 50 �̸��̸鼭 ��ų üũ ���� �ƴ� ��
            if (repairPercent < 100 && !isSkillChecking)
            {
                RepairGenerator();
            }

            // ���� �ۼ�Ʈ�� 20 �̻��� ��
            if (repairPercent >= 20.0f)
            {
                if (randomPercent == 0)
                {
                    // ��ų üũ �ߵ� ���� �ð��� ��÷�մϴ�.
                    randomPercent = Random.Range(25.0f, 30.0f);
                }

                // ���� �ۼ�Ʈ�� ���� �ۼ�Ʈ �̻��̸鼭 ��ų üũ�� �ߵ����� �ʾ��� ��
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
    }

    private void StartSkillCheck()
    {
        isSkillChecking = true; // ��ų üũ ���� �� ���� ����
        skillCheckSystem.CheckStart(); // ����� SkillCheckSystem�� �˸�
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
            repairPercent *= 0.7f; // ���� �� ���� �ۼ�Ʈ ����
        }
        isSkillChecking = false; // ��ų üũ �Ϸ� �� ���� ����
    }

    public void RestartRepair()
    {
        if (!isSkillChecking)
        {
            randomPercent = 0f; // ���� �ۼ�Ʈ �ʱ�ȭ
            print("��ų üũ �Ϸ� �� �ٽ� ���� ����.");
            RepairGenerator();
        }
    }

    public void FailedCheck()
    {
        print("��!");
        explosion.SetActive(true);
        playerAnim.SetTrigger("Exposion"); // �ִϸ������� Exposion ���� ����
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
        geLight.SetActive(true);

        SkillCheck1 = false; // ���� �Ϸ� �� ��ų üũ �ʱ�ȭ
        randomPercent = 0f; // ���� �ۼ�Ʈ �ʱ�ȭ

        GeneratorManager.generatorManager.AddRepairCount(1);
    }


    // �ڽ� GameObject�� �̸����� ã�� �Լ�
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
