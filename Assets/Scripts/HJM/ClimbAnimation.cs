using System.Security.Claims;
using UnityEngine;

public class ClimbAnimation : MonoBehaviour
{
    public Vector3 targetPosition; // 애니메이션 종료 시 이동할 목표 위치
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
        // Animator와 CharacterController 컴포넌트를 가져옵니다.
        playerAnim = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        cc = GetComponent<CharacterController>();


        // 초기 위치를 저장합니다.
        startPosition = transform.position;

        playerFSM = GetComponent<PlayerFSM>();
    }

    void Update()
    {
        
    }




    // Climb애니메이션이 시작하는 프레임에서 시작하는 이벤트
    public void OnAnimationStart()
    {
        // 현재 상태(넘어가기 직전)을 담아둔다.
        currentState = playerFSM.pyState;
        print("애니메이션 시작");
        isAnimating = true;
        // 플레이어의 상태를 인액션 상태로 전환한다.
        print("인액션 시작");
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        //playerController.enabled = false;

        

    }



    // Climb애니메이션이 끝나는 프레임에서 시작하는 이벤트
    public void OnAnimationEnd()
    {
        isAnimating = false;

        print("애니메이션 종료, 캐릭터가 이동되었습니다.");
         // 애니메이터에 false를 반환하고 넘어가기 전 상태로 되돌린다.
        playerAnim.SetBool("isClimb", false);
        playerFSM.pyState = currentState;
        print("내 위치: " + transform.position);
        print("목적지: " + playerController.arriveCC);


        // 캐릭터컨트롤러를 도착포인트로 순간이동 시킨다.
        transform.position = playerController.arriveCC;

        playerController.enabled = true;
        cc.enabled = true;
    }

}
