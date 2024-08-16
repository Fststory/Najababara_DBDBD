using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    CharacterController _controller;
    PlayerFSM PlayerFSM;

    public float moveSpeed;
    public float runSpeed;
    public float finalSpeed;
    public bool run;

    
    private Vector3 velocity;
    public float gravity = -9.81f;  // �߷� ��
    public float groundDistance = 0.4f;  // ������ �Ÿ�
    public LayerMask groundMask;  // �� ���̾� ����ũ

    public bool isGrounded;
    public Transform groundCheck;  // �� üũ�� Ʈ������

    public float smoothness = 10.0f;
    public bool toggleCameraRotation;


    public bool alraedyHooked;
    public bool HookedLv2;

    public Vector3 arriveCC;


    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
        PlayerFSM = GetComponent<PlayerFSM>();

        _animator.SetTrigger("Hit02");
    }

    

    void Update()
    {
        if(PlayerFSM.pyState != PlayerFSM.PlayerState.InAction)
        {
            InputMovement();
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // �ѷ����� Ȱ��ȭ
        }
        else
        {
            toggleCameraRotation = false; // �ѷ����� ��Ȱ��ȭ
        }

        // LeftShift ������ �޸���
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
            _animator.SetBool("Run", true);
        }
        else
        {
            run = false;
            _animator.SetBool("Run", false);
        }


        // Ctrl ������ �ɱ�
        if (Input.GetKey(KeyCode.LeftControl))
        {
            _animator.SetBool("isCrouch", true);            
        }
        else
        {
            _animator.SetBool("isCrouch", false);
        }


        //if (PlayerFSM.pyState == PlayerFSM.PlayerState.Normal || PlayerFSM.pyState == PlayerFSM.PlayerState.Injured)
        //{
        //    // �� üũ
        //    isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //    // ���� ����ִ� ��� �ӵ� �ʱ�ȭ
        //    if (isGrounded && velocity.y < 0)
        //    {
        //        velocity.y = -2f;  // �ణ�� ���� ���� �־� ���� �����ϴ� ������ �ݴϴ�.
        //    }

        //    // �߷� ����
        //    velocity.y += gravity * Time.deltaTime;

        //    // �ӵ��� ���� �̵�
        //    _controller.Move(velocity * Time.deltaTime);
        //}
    }

    void LateUpdate()
    {
        // ī�޶� ȸ�� Ȱ��ȭ �� ĳ������ ȸ�� ������ LateUpdate���� ����
        if (toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smoothness);
        }
    }

    public void InputMovement()
    {
        moveSpeed = PlayerFSM.moveSpeed;
        runSpeed = PlayerFSM.runSpeed;

        // ���� �� �ӵ� ����
        if (_animator.GetBool("isCrouch"))
        {
            finalSpeed = moveSpeed * 0.5f; //  ���� �� �ӵ��� 50%�� ����
        }
        else
        {
            finalSpeed = run ? runSpeed : moveSpeed;
        }


        // ���� run�� true���, finalSpeed�� runSpeed ���� ���´�.
        // ���� run�� false���, finalSpeed�� speed ���� ���´�.
        finalSpeed = run ? runSpeed : moveSpeed;

        // ī�޶��� ������ �������� �̵� ���� ���
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // ���콺 �����¿� ���� h, v�� ��´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // �ִϸ������� �÷Ժ��� ���� h,v �� �����Ѵ�.
        _animator.SetFloat("MoveHorizontal", h);
        _animator.SetFloat("MoveVertical", v);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * smoothness);
        }

        //print(moveDirection.magnitude);
        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
        _animator.SetFloat("DirLength", moveDirection.magnitude);

        //float percent = (run ? 1 : 0.5f) * moveDirection.magnitude;
        //_animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}