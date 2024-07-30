using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaveMark : MonoBehaviour
{
    // �÷��̾ �̵��ϸ� ������ ����� ���

    /*  �پ�ٴ� �� ������ ����
        ������ ������ ���� => 

        �Ȱų� �ɾƼ� �� ���� �� ����
        
     */

    public Transform player;
    public GameObject traceMarkPrefab;
    float currentTime = 0;
    float markCycle = 0.5f;  // ������ ����� �ֱ�

    void Start()
    {
        
    }

    void Update()
    {
        LeaveMark();
    }

    public void LeaveMark() // �÷��̾ �ٴ� ������ �� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > markCycle)
        {
            // ª�� ���� ���� ����ؼ� ������ �÷��̾��� ��ġ�� ����
            GameObject traceMark = Instantiate(traceMarkPrefab);
            traceMark.transform.position = player.position + new Vector3(0, -0.9f, 0);  // -0.9f�� ����(-1.0f)���� (0.1f)��ŭ �ö������ Zfighting�� �����ϱ� ����, ���� ������� �����ϸ� ���� �� *********************************************
            traceMark.transform.eulerAngles = player.eulerAngles + new Vector3(90, 0, 0);
            currentTime = 0;
        }
    }
}
