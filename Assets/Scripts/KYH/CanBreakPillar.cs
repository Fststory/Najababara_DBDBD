using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBreakPillar : MonoBehaviour
{
    // Pillar�� �ڽ� ������Ʈ CanBreakPillarZone�� Ʈ���� �ȿ��� ������ �� �ִ� ���

    // �ʿ��� �� : PillarFSM, PlayerFSM

    PillarFSM pillarState;
    public PlayerFSM playerFSM;
    public float currentTime = 0;
    public float damagedTime;   // ���峾 �� �ɸ��� �ð�

    void Start()
    {
        pillarState = gameObject.GetComponentInParent<PillarFSM>();
    }
        
    private void OnTriggerStay(Collider other) // �÷��̾ ������ �ɸ��� ���� ���·� ��ȣ�ۿ� ������ ���� �� ��� ������ ���
    {
        // Ʈ���ſ� �����ִ� ����� "�÷��̾�"�̰�, ���°� 0(�ǰ�) or 1(�λ�)�̶��...
        if (other.CompareTag("Player") && (int)playerFSM.pyState < 2)
        {
            print("���峾 �� �־�!");
            // �����̽��ٸ� ������ ������
            if (Input.GetKey(KeyCode.Space))
            {
                currentTime += Time.deltaTime;  // �ð��� ���.
                if (currentTime > damagedTime)  // ���峾 �� �ɸ��� �ð��� ä���
                {
                    print("����!");
                    pillarState.TakeDamage();   // ���峽��.
                    currentTime = 0;
                }
            }
        }
    }
}
