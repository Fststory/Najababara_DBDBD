using UnityEngine;
using UnityEngine.UI;

public class GeneratorSystem : MonoBehaviour
{

    // ������ ��Ʈ�ѷ�

    // ������� �������� ���´�.
    // �������� UI�� �����Ѵ�.
    // �÷��̾ ���콺 ��Ŭ �� ������ ����ȴ�.(�������� �ö󰣴�.)
    // ���� Ÿ�ֿ̹� ���� Ÿ�ְ̹����� ���´�.

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
        // repairGuide, repairSlider, repairSliderUI�� ���ӿ��� ã�Ƽ� �Ҵ��Ѵ�.
        repairGuide = GameObject.Find("TXT_RepairGuide");
        repairSlider = GameObject.Find("SLD_RepairSlider");
        repairSliderUI = repairSlider.GetComponent<Slider>();
    }
    void Start()
    {
       
        // �����ȳ�, �����̴� UI�� false ���·� �����Ѵ�.
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        // �����̴� ���� ���� 0 ���� �����Ѵ�.
        repairPercent = 98;

    }

    void Update()
    {
        // ���� ���� �ȿ��� ���콺 ��Ŭ ������ �ִ� ������
        // ���� �ȳ� ������ �����, �����̴� �ٸ� ���̰� �Ѵ�.
        if(isPlayerInTrigger == true && Input.GetMouseButton(0) && repairPercent < 100)
        {
            print("���� ���Դϴ�.");
            repairGuide.SetActive(false);
            repairSlider.SetActive(true);
            repairSliderUI.value = repairPercent;
            repairPercent += repairSpeed * Time.deltaTime;
            if(repairPercent >= 100)
            {
                print("�Ϸ� ī��Ʈ 1");
                RepairsComplete();
                Complete = true;
                return;
            }
        }
           

        // ���� ���� �ȿ��� ���콺 ��Ŭ�� ���� ����
        // ���� �Ϸ� ���°� false�� ����
        // ���� �����̴��� ���߰�, ���� �ȳ� ������ ����.
        if (Complete == false)
        {
            if (isPlayerInTrigger == true && Input.GetMouseButtonUp(0))
            {
                print("���� �ߵ� ����.");
                repairGuide.SetActive(true);
                repairSlider.SetActive(false);
            }
        }
    }

    // ���� �����ȿ� ������, ���� �ȳ� ������ ���� isPlayerInTrigger true�� ��ȯ�Ѵ�.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == ("Player"))
        {
            if (Complete == false)
            {
                print("�������� ������ ����.");
                repairGuide.SetActive(true);
                repairSlider.SetActive(true);
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
            repairGuide.SetActive(false);
            isPlayerInTrigger = false;
            repairSlider.SetActive(false);
        }
    }

    void RepairsComplete()
         {
             //print("���� �Ϸ��");
            GeneratorManager.generatorManager.AddRepairCount(1);
             //repairCount += CompleteGenerator;
             repairPercent = 100;
             repairGuide.SetActive(false);
             repairSlider.SetActive(false);
             Complete = true;
            
         }

    

   




}
