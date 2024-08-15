using Unity.VisualScripting;
using UnityEngine;


public class WindowClimb : MonoBehaviour
{
    public Transform pointA; // ���� ���� ����Ʈ
    public Transform pointB; // ���� ������ ����Ʈ
    public GameObject player;
    public GameObject pallet;
    public Vector3 camStart;
    public GameObject climbUI;

    
    ClimbAnimation climbAnimation;

    public bool isPlayerInTrigger = false; // �÷��̾ Ʈ���� �ȿ� �ִ��� ����
    public Animator playerAnim;
    public PlayerController playerController;
    public CharacterController cc;

    public float moveSpeed = 1.0f;
    public bool isClimbing;

    private bool isMovingPoint = false; // ĳ���Ͱ� ����Ʈ�� ������ ����


    private Vector3 targetPoint; // �Ѿ�� �� �̵��� ��ǥ ����Ʈ
    private Vector3 arrivePoint; // �Ѿ �� ������ ��ǥ ����Ʈ

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
            // ���� ��ġ�� ����� ����Ʈ�� �������� �̵� ���� ����
            float distanceToA = Vector3.Distance(player.transform.position, pointA.position);
            float distanceToB = Vector3.Distance(player.transform.position, pointB.position);

            if (distanceToA < distanceToB)
            {
                targetPoint = pointA.position; // A �������� �̵�
                print("A�� �̵�, B �������� �Ѿ");
                //Vector3 dirA = transform.forward;
                //player.transform.localEulerAngles = dirA;
                player.transform.LookAt(pointB); // B ����Ʈ�� �ٶ󺸵��� ȸ��
                arrivePoint = pointB.position;
            }
            else
            {
                targetPoint = pointB.position; // B �������� �̵�
                print("B�� �̵�, A �������� �Ѿ");
                //Vector3 dirB = transform.forward;
                //player.transform.localEulerAngles = dirB;
                player.transform.LookAt(pointA); // A ����Ʈ�� �ٶ󺸵��� ȸ��
                arrivePoint = pointA.position;

            }
            // ����Ʈ�� �̵� ����
            isMovingPoint = true;
            print("��������Ʈ �̵�����");
            //playerController.enabled = false; // �̵� �߿��� ĳ���� ��Ʈ�ѷ� ��Ȱ��ȭ

            // �÷��̾� ��Ʈ�ѷ��� ��������Ʈ�� �����Ѵ�.
            playerController.arriveCC = arrivePoint;
        }


        //MoveTowardsCam();




    }

    void MoveTowardsPoint()
    {
        // ��ǥ ��ġ �̵�
        Vector3 destination = Vector3.Lerp(player.transform.position, targetPoint, Time.deltaTime * 3);

        player.transform.position = destination;

        // ��ǥ ������ �����ߴ��� Ȯ��
        if (Vector3.Distance(player.transform.position, targetPoint) < 0.1f)
        {

            print("��������Ʈ �̵��Ϸ�");
            isMovingPoint = false;
            //playerController.enabled = true;
            //cc.enabled = true;
            // ����Ʈ�� �����ϸ� �ִϸ��̼� ����
            playerAnim.SetTrigger("isClimb");
            isClimbing = true;


        }
    }




    //void MoveTowardsCam()
    //{
    //    if (climbAnimation.isAnimating == true)
    //    {// ����Ʈ�� �����ϸ� ��������Ʈ�� ī�޶� �̵�
    //        print("ī�޶� �̵� ��");
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
