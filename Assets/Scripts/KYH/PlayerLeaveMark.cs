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
    float currentTime = 0;
    public float markCycle = 0.5f;  // ������ ����� �ֱ�
    public PlayerController playerController;   // �÷��̾ �޷��ִ� PlayerController ������Ʈ �־���� �� => �÷��̾ �ٰ� �ִ����� �޾ƿ��� ���ؼ�


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
            traceMark.transform.position = new Vector3(player.position.x, 0.1f, player.position.z);  // Zfighting�� �����ϱ� ����, ���� ������� �����ϸ� ���� �� *********************************************
            traceMark.transform.eulerAngles = player.eulerAngles + new Vector3(90, 0, 0);
            currentTime = 0;
        }
    }
}
