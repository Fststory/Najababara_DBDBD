using UnityEngine;

public class SkillCheckSystem : MonoBehaviour
{
    [HideInInspector]
    public GeneratorSystem generatorSystem; // �� �������� GeneratorSystem�� ����
    public Transform noteAxis; // ��ų üũ ��Ʈ
    public Transform normalAxis; // ���� ��Ʈ (���� ��Ʈ ��ġ�� ��Ÿ��)
    public GameObject noteCanvas; // ��Ʈ ĵ���� UI

    private float timer = 0.0f; // ��ų üũ Ÿ�̸�
    private float rotationSpeed = 180.0f; // ��Ʈ�� ȸ�� �ӵ� (����/��)
    private float maxRotationTime; // ��Ʈ�� �� ���� ���� �� �ɸ��� �ִ� �ð�
    private bool isChecking = false; // ��ų üũ ���� ������ ����

    private void Start()
    {
        noteCanvas.SetActive(false);
        maxRotationTime = 500.0f / rotationSpeed; // �� ���� ���� �� �ɸ��� �ð� ���
    }

    void Update()
    {
        if (isChecking)
        {
            // Ÿ�̸� ������Ʈ
            timer += Time.deltaTime;

            // �����̽��� �Է� ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Check();
                print("�����̽��� ����!");
            }

            // �� ���� �ð��� �ʰ��Ǹ� ���� ó��
            if (timer >= maxRotationTime)
            {
                FailCheck();
            }
        }
    }

    public void CheckStart()
    {
        noteCanvas.SetActive(true);
        generatorSystem.isSkillChecking = true; // ��ų üũ ���� �� ���� ����
        timer = 0.0f; // Ÿ�̸� �ʱ�ȭ
        isChecking = true; // ��ų üũ ���� �� ����
    }

    public void Check()
    {
        // ��ų üũ ���� ���� �Ǵ�
        bool success = noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z;

        // ��� ó��
        generatorSystem.OnSkillCheckComplete(success);

        // ���� ó��
        noteCanvas.SetActive(false);
        isChecking = false; // ��ų üũ ���� �� ����
    }

    private void FailCheck()
    {
        print("��ų üũ �ð� �ʰ�: ����!!");
        generatorSystem.OnSkillCheckComplete(false);
        noteCanvas.SetActive(false);
        isChecking = false; // ��ų üũ ���� �� ����
    }
}