using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public float repairSpeed = 0.01f;
    public bool isPlayerInTrigger = false;


    void Start()
    {
        // �����ȳ�, �����̴� UI�� false ���·� �����Ѵ�.
        repairGuide.SetActive(false);
        repairSlider.SetActive(false);
        // �����̴� ���� ���� 0 ���� �����Ѵ�.
        repairSliderUI.value = 0;

    }

    void Update()
    {
        // ���� ���� �ȿ��� ���콺 ��Ŭ ������ �ִ� ������
        // ���� �ȳ� ������ �����, �����̴� �ٸ� ���̰� �Ѵ�.
        if(isPlayerInTrigger == true && Input.GetMouseButton(0))
        {
            print("���� ���Դϴ�.");
            repairGuide.SetActive(false);
            repairSlider.SetActive(true);
            repairSliderUI.value += repairSpeed * Time.deltaTime;
        }


        // ���� ���� �ȿ��� ���콺 ��Ŭ�� ���� ������
        // ���� �����̴��� ���߰�, ���� �ȳ� ������ ����.
        if(isPlayerInTrigger == true && Input.GetMouseButtonUp(0))
        {
            print("���� �ߵ� ����.");
            repairGuide.SetActive(true);
            repairSlider.SetActive(false);
        }

 
    }

    // ���� �����ȿ� ������, ���� �ȳ� ������ ���� isPlayerInTrigger true�� ��ȯ�Ѵ�.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == ("Player"))
        {
            print("�������� ������ ����.");
            repairGuide.SetActive(true);
            isPlayerInTrigger = true;
}
    }

    // ���� �������� ������, ���� �ȳ� ������ ���߰� isPlayerInTrigger false�� ��ȯ�Ѵ�.
    // ���� �����̴��� �����.
    private void OnTriggerExit(Collider other)
    {
        print("�������� �������� ����.");
        repairGuide.SetActive(false);
        isPlayerInTrigger = false;
        repairSlider.SetActive(false);
    }






}
