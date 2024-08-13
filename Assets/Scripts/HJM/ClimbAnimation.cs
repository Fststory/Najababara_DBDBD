using System.Security.Claims;
using UnityEngine;

public class ClimbAnimation : MonoBehaviour
{
    public Vector3 targetPosition; // �ִϸ��̼� ���� �� �̵��� ��ǥ ��ġ
    public CharacterController cc;
    public PlayerController playerController;
    
    private Vector3 startPosition;
    private Animator playerAnim;



     public bool isAnimating = false;
    PlayerFSM.PlayerState currentState;

    PlayerFSM playerFSM;

    Climb climb;


    void Start()
    {
        // Animator�� CharacterController ������Ʈ�� �����ɴϴ�.
        playerAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        cc = GetComponent<CharacterController>();


        // �ʱ� ��ġ�� �����մϴ�.
        startPosition = transform.position;

        playerFSM = GetComponent<PlayerFSM>();
    }

    void Update()
    {
        
    }




    // Climb�ִϸ��̼��� �����ϴ� �����ӿ��� �����ϴ� �̺�Ʈ
    public void OnAnimationStart()
    {
        // ���� ����(�Ѿ�� ����)�� ��Ƶд�.
        currentState = playerFSM.pyState;
        print("�ִϸ��̼� ����");
        isAnimating = true;
        // �÷��̾��� ���¸� �ξ׼� ���·� ��ȯ�Ѵ�.
        print("�ξ׼� ����");
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        //playerController.enabled = false;

        

    }



    // Climb�ִϸ��̼��� ������ �����ӿ��� �����ϴ� �̺�Ʈ
    public void OnAnimationEnd()
    {
        isAnimating = false;

        print("�ִϸ��̼� ����, ĳ���Ͱ� �̵��Ǿ����ϴ�.");
         // �ִϸ����Ϳ� false�� ��ȯ�ϰ� �Ѿ�� �� ���·� �ǵ�����.
        playerAnim.SetBool("isClimb", false);
        playerFSM.pyState = currentState;
        print("�� ��ġ: " + transform.position);
        print("������: " + playerController.arriveCC);


        // ĳ������Ʈ�ѷ��� ��������Ʈ�� �����̵� ��Ų��.
        transform.position = playerController.arriveCC;

        playerController.enabled = true;
        cc.enabled = true;
    }

}
