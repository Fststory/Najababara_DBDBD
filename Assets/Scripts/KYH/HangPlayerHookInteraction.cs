using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangPlayerHookInteraction : MonoBehaviour
{
    // �÷��̾ ���� �Ŵ� �� �ִ� ����� ������ ��ũ��Ʈ

    /* �÷��̾ ���� �ۿ�/ �÷��̾ �Ŵٴ� �ۿ�
        
    */

    public GameObject playerObject;
    public Transform hangPoint;
    public Transform pillarTransform;
    public Transform hookPoint;
    public PlayerFSM playerFSM;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "�÷��̾�" ������Ʈ ĳ��
        playerFSM = playerObject.GetComponent<PlayerFSM>(); // �÷��̾� ���� ĳ��
    }
    
    public void HangPlayerOnMe()    // �÷��̾ ���� ���
    {
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        playerObject.transform.SetParent(hangPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void HangPlayerOnHook()  // �÷��̾ �Ŵٴ� ���
    {
        playerFSM.pyState = PlayerFSM.PlayerState.Hooked;
        playerObject.transform.SetParent(hookPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
