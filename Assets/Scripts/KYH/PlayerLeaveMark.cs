using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLeaveMark : MonoBehaviour
{
    // �÷��̾ �̵��ϸ� ������ ����� ���

    /*  
        �پ�ٴ� �� ������ ����
     */

    public Transform player;    // �÷��̾� Ʈ������ �ֱ�
    public GameObject traceMarkPrefab;  // TraceMark ������ �ֱ�
    float currentTime = 0;  // ���� Ÿ�̸�
    public float markCycle = 0.5f;  // ������ ����� �ֱ�
    PlayerController playerController;   // �÷��̾ �޷��ִ� PlayerController ������Ʈ (start�Լ����� ĳ���� ����)

    private void Start()
    {
        playerController = player.gameObject.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (playerController.run)
        {
            LeaveMark();
        }
    }

    public void LeaveMark() // �÷��̾ �ٴ� ������ �� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > markCycle)
        {
            // ª�� ���� ���� ����ؼ� ������ �÷��̾��� ��ġ�� ����
            GameObject traceMark = Instantiate(traceMarkPrefab);
            traceMark.transform.position = new Vector3(player.position.x, 0.1f, player.position.z);  // ����.position.y ���� 0.1f��ŭ �÷��� Zfighting�� ����, ���� ������� �����ϸ� ���� �� *********************************************
            traceMark.transform.eulerAngles = player.eulerAngles + new Vector3(90, 0, 0);
            currentTime = 0;
        }
    }
}
