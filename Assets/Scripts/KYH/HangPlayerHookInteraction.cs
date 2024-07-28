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

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "�÷��̾�" ������Ʈ ĳ��
    }
    
    public void HangPlayerOnMe() // �÷��̾ ���� ���
    {
        playerObject.transform.SetParent(hangPoint); // ���� ������ �÷��̾��� ��ġ�� �д�.
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void HangPlayerOnHook()
    {
        playerObject.transform.SetParent(hookPoint); // ������ �÷��̾��� ��ġ�� �д�.
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
