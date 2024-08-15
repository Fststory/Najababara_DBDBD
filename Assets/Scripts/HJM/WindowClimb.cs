using Unity.VisualScripting;
using UnityEngine;


public class WindowClimb : MonoBehaviour
{
    public Transform pointA; // 판자 왼쪽 포인트
    public Transform pointB; // 판자 오른쪽 포인트
    public GameObject player;
    public GameObject pallet;
    public Vector3 camStart;
    public GameObject climbUI;

    
    ClimbAnimation climbAnimation;

    public bool isPlayerInTrigger = false; // 플레이어가 트리거 안에 있는지 여부
    public Animator playerAnim;
    public PlayerController playerController;
    public CharacterController cc;

    public float moveSpeed = 1.0f;
    public bool isClimbing;

    private bool isMovingPoint = false; // 캐릭터가 포인트로 가는지 여부


    private Vector3 targetPoint; // 넘어가기 전 이동할 목표 포인트
    private Vector3 arrivePoint; // 넘어간 후 도착할 목표 포인트

    //public GameObject cam;

    void Start()
    {
        if (player != null)
        {
            playerAnim = player.GetComponent<Animator>();
            playerController = player.GetComponent<PlayerController>();
            cc = player.GetComponent<CharacterController>();
            climbAnimation = player.GetComponent<ClimbAnimation>();
            //camStart = cam.transform.localPosition;
        }

        

        if (climbUI != null)
        {
            climbUI.SetActive(false);

        }
    }

    void Update()
    {
        if (isMovingPoint)
        {
            playerController.enabled = false;
            cc.enabled = false;
            MoveTowardsPoint();
            return;
        }

        if (isPlayerInTrigger && !isMovingPoint && Input.GetKeyDown(KeyCode.Space))
        {
            // 현재 위치와 가까운 포인트를 기준으로 이동 방향 결정
            float distanceToA = Vector3.Distance(player.transform.position, pointA.position);
            float distanceToB = Vector3.Distance(player.transform.position, pointB.position);

            if (distanceToA < distanceToB)
            {
                targetPoint = pointA.position; // A 방향으로 이동
                print("A로 이동, B 방향으로 넘어감");
                //Vector3 dirA = transform.forward;
                //player.transform.localEulerAngles = dirA;
                player.transform.LookAt(pointB); // B 포인트를 바라보도록 회전
                arrivePoint = pointB.position;
            }
            else
            {
                targetPoint = pointB.position; // B 방향으로 이동
                print("B로 이동, A 방향으로 넘어감");
                //Vector3 dirB = transform.forward;
                //player.transform.localEulerAngles = dirB;
                player.transform.LookAt(pointA); // A 포인트를 바라보도록 회전
                arrivePoint = pointA.position;

            }
            // 포인트로 이동 시작
            isMovingPoint = true;
            print("판자포인트 이동시작");
            //playerController.enabled = false; // 이동 중에는 캐릭터 컨트롤러 비활성화

            // 플레이어 컨트롤러에 도착포인트를 전달한다.
            playerController.arriveCC = arrivePoint;
        }


        //MoveTowardsCam();




    }

    void MoveTowardsPoint()
    {
        // 목표 위치 이동
        Vector3 destination = Vector3.Lerp(player.transform.position, targetPoint, Time.deltaTime * 3);

        player.transform.position = destination;

        // 목표 지점에 도달했는지 확인
        if (Vector3.Distance(player.transform.position, targetPoint) < 0.1f)
        {

            print("판자포인트 이동완료");
            isMovingPoint = false;
            //playerController.enabled = true;
            //cc.enabled = true;
            // 포인트에 도달하면 애니메이션 실행
            playerAnim.SetTrigger("isClimb");
            isClimbing = true;


        }
    }




    //void MoveTowardsCam()
    //{
    //    if (climbAnimation.isAnimating == true)
    //    {// 포인트에 도달하면 도착포인트로 카메라 이동
    //        print("카메라 이동 중");
    //        Vector3 camArrive = new Vector3(arrivePoint.x, cam.transform.position.y, arrivePoint.z);
    //        print(camArrive);
    //        print(camStart);
    //        Vector3 camPosition = Vector3.Lerp(cam.transform.position, camArrive, Time.deltaTime * 2);
    //        cam.transform.position = camPosition;
    //        if (climbAnimation.isAnimating == false)
    //        {
    //            cam.transform.localPosition = new Vector3(0.0280000009f, 1.34500003f, -0.138999999f);
    //            return;
    //        }
    //    }
    //}




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            climbUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInTrigger = false;
            climbUI.SetActive(false);

        }
    }


}
