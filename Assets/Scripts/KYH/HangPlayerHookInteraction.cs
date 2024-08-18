using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerController playerController;
    public CharacterController cc;
    public EnemyController enemyController;
    public AnimationClip animClip;

    public Animator playerAnim;

    public GameObject img_Dying;
    public GameObject img_Hang;

    private void Awake()
    {
        img_Dying = GameObject.Find("img_Dying");
        img_Hang = GameObject.Find("img_Hang");
    }

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "�÷��̾�" ������Ʈ ĳ��
        playerFSM = playerObject.GetComponent<PlayerFSM>(); // �÷��̾� ���� ĳ��
        playerController = playerObject.GetComponent<PlayerController>(); // �÷��̾� ��Ʈ�ѷ� ĳ��
        animClip = Resources.Load<AnimationClip>("Hook");
        cc = playerObject.GetComponent<CharacterController>();
        playerAnim = playerObject.GetComponent<Animator>();


    }
    
    public void HangPlayerOnMe()    // �÷��̾ ���� ���
    {
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        playerObject.transform.SetParent(hangPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        //playerController.enabled = false;   // �÷��̾� ��Ʈ�ѷ� ��Ȱ��ȭ!
        cc.enabled = false;   // ĳ���� ��Ʈ�ѷ� ���ϱ� �Դٰ��� �ϴ� �� ���� ��

        img_Dying.SetActive(false);
        img_Hang.SetActive(true);
        print("�� ������ �ҷ���");
    }

    public void HangPlayerOnHook()  // �÷��̾ �Ŵٴ� ���
    {
        playerFSM.pyState = PlayerFSM.PlayerState.Hooked;
        playerObject.transform.SetParent(hookPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        Invoke("AfterHook", animClip.length);

        playerAnim.SetTrigger("hang");

    }
    
    void AfterHook()
    {
        enemyController.ChangeState(EnemyController.EnemyState.NoEvidence);
        enemyController.hooking = false;
    }
}
