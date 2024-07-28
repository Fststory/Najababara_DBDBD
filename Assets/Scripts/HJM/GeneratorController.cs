using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorController : MonoBehaviour
{

    public GameObject repairGuideUI;
    public GameObject repairPercentUI;
    public GameObject timingGameUI;
    public Scrollbar repairPercentBar;

    public float speed = 0.0000000000000000001f;

    public float repairPercent = 0.0f;

    private bool isPlayerInTrigger = false;


    // ������ ��Ʈ�ѷ�

    // ������� �������� ���´�.
    // �������� UI�� �����Ѵ�.
    // �÷��̾ ���콺 ��Ŭ �� ������ ����ȴ�.(�������� �ö󰣴�.)
    // ���� Ÿ�ֿ̹� ���� �̴ϰ����� ���´�.

    // ���� �̴ϰ����̶�
    // ĭ�� �߰� ȭ��ǥ�� ĭ ���� ��������. 
    // ǥ�õ� ���� �ȿ� ȭ��ǥ�� ������ �����̽��ٸ� �������Ѵ�.
    // Ÿ�̹��� ������ ���ϸ� �������� �������� �����Ѵ�.


    // �����⸦ ���� ���� �����ϸ� �ⱸ��ġ�� ����� �� �ִ�.







    void Start()
    {
        // ���� UI�� false ���·� �����Ѵ�.
        repairGuideUI.SetActive(false);
        repairPercentUI.SetActive(false);
        timingGameUI.SetActive(false);

        //// ��ũ�ѹ� ���� ����� �� ���� OnScrollbarValueChanged �Լ��� ȣ���Ѵ�.
        //repairPercentBar.onValueChanged.AddListener(OnScrollbarValueChanged);
        // ��ũ�ѹ� ���� 0���� �����Ѵ�.
        repairPercentBar.size = 0;
    }

    //void OnScrollbarValueChanged(float size)
    //{   // �ۼ�Ʈ�� ǥ���ϱ� ���� 100�� ���Ѵ�.
    //    repairPercent = repairPercentBar.size * 100;

    //}




    void Update()
    {
        // float�� repairPercent�� ��ũ�ѹ� ���� ��´�.
        repairPercent = repairPercentBar.size;

        // �÷��̾ Ʈ���� ���� ���� �� ���� ��Ʈ�� Ű ���� �Է��� Ȯ��
        if (isPlayerInTrigger && Input.GetKey(KeyCode.LeftControl))
        {
            print("�������Դϴ�.");
            repairGuideUI.SetActive(false);
            repairPercentUI.SetActive(true);
            repairPercentBar.size += speed * Time.deltaTime;
        }
        // �÷��̾ Ʈ���� ���� ���� �� ���� ��Ʈ�� Ű �� �Է��� Ȯ��
        if (isPlayerInTrigger && Input.GetKeyUp(KeyCode.LeftControl))
        {
            repairPercentUI.SetActive(false);
            timingGameUI.SetActive(false);
        }

        TimingGame();



    }


    // Ư�� �ۼ�Ʈ ������ ������Ÿ�ְ̹��� �Լ��� ȣ���Ѵ�.
    void TimingGame()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl) && repairPercentBar.size > 0.5)
        {
            timingGameUI.SetActive(true);
            print("50% ������");
           
        }
      

    }

    // �÷��̾ ������ Ʈ���ſ� ��� ���� �� Repair �ؽ�Ʈ�� �����Ѵ�.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("�÷��̾� ����");
            repairGuideUI.SetActive(true);
            isPlayerInTrigger = true;           
        }
    }

    // �÷��̾ Ʈ���ſ��� ������ �� UI���� ����� isPlayerInTrigger�� false ó���Ѵ�.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Player"))
        {
            print("�÷��̾� ����");
            repairGuideUI.SetActive(false);
            repairPercentUI.SetActive(false);
            isPlayerInTrigger = false;
            timingGameUI.SetActive(false);
        }
    }


}
