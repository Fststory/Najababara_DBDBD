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

    // ���ڱ� �Ҹ� ���� ����
    public List<AudioClip> footstepClips; // ���� ���ڱ� �Ҹ� Ŭ��
    private AudioSource audioSource; // ���ڱ� �Ҹ��� ����� ����� �ҽ�
    public float footstepInterval = 0.5f; // �⺻ ���ڱ� �Ҹ� ����
    private float footstepTimer; // ���ڱ� �Ҹ� Ÿ�̸�


    // ���Ҹ� ���� ����
    public List<AudioClip> breathingClips; // ���� ���Ҹ� Ŭ��
    private AudioSource breathingAudioSource; // ���Ҹ��� ����� ����� �ҽ�
    public float breathingInterval = 2.0f; // �⺻ ���Ҹ� ����
    private float breathingTimer; // ���Ҹ� Ÿ�̸�



    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
        PlayerFSM = GetComponent<PlayerFSM>();

        _animator.SetTrigger("Hit02");

        // ����� �ҽ� �ʱ�ȭ
        audioSource = GetComponent<AudioSource>();
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
    }
    

    void Update()
    {
        if(PlayerFSM.pyState != PlayerFSM.PlayerState.InAction)
        {
            InputMovement();
            HandleFootsteps(); // ���ڱ� �Ҹ� ó��
            HandleBreathing(); // ���Ҹ� ó��
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // �ѷ����� Ȱ��ȭ
        }
        else
        {
            toggleCameraRotation = false; // �ѷ����� ��Ȱ��ȭ
        }

        // �λ� ���� Ȯ�� �� �ִϸ����� �Ķ���� ������Ʈ
        bool isInjured = PlayerFSM.pyState == PlayerFSM.PlayerState.Injured;
        _animator.SetBool("isInjured", isInjured);

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

        if (PlayerFSM.pyState == PlayerFSM.PlayerState.Normal || PlayerFSM.pyState == PlayerFSM.PlayerState.Injured)
        {
            _controller.enabled = true;

            // �� üũ
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // ���� ����ִ� ��� �ӵ� �ʱ�ȭ
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;  // �ణ�� ���� ���� �־� ���� �����ϴ� ������ �ݴϴ�.
            }
        
            // �߷� ����
            velocity.y += gravity * Time.deltaTime;
            // �ӵ��� ���� �̵�
            _controller.Move(velocity * Time.deltaTime);
        }
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

        if(PlayerFSM.pyState == PlayerFSM.PlayerState.Normal)
        {
            _animator.SetFloat("DirLength", moveDirection.magnitude);
            _animator.SetBool("isInjured", false);
            print("�Ⱦ��Ŀ�");

        }

        else if (PlayerFSM.pyState == PlayerFSM.PlayerState.Injured)
        {

            _animator.SetBool("isInjured", true);
            _animator.SetFloat("FloatInjured", moveDirection.magnitude);
            print("���Ŀ�");


        }




    }

    void HandleFootsteps()
    {
        if (isGrounded && _controller.velocity.magnitude > 0.1f)
        {
            // �޸� �� ���ڱ� �Ҹ� ���� ���̱�
            float speedFactor = run ? 0.4f : 0.33f;
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                PlayFootstepSound();
                footstepTimer = footstepInterval / (_controller.velocity.magnitude * speedFactor);
            }
        }
    }

    void PlayFootstepSound()
    {
        if (footstepClips.Count > 0)
        {
            int index = Random.Range(0, footstepClips.Count);
            audioSource.PlayOneShot(footstepClips[index]);
        }
    }

    void HandleBreathing()
    {
        // �ӵ��� ���� ���Ҹ� ���� ����
        float speedFactor = run ? 0.55f : (_controller.velocity.magnitude > 0.1f ? 0.15f : 0.1f);
        breathingTimer -= Time.deltaTime;

        if (breathingTimer <= 0f)
        {
            PlayBreathingSound();
            breathingTimer = breathingInterval / speedFactor;
        }
    }

    void PlayBreathingSound()
    {
        if (breathingClips.Count > 0)
        {
            int index;

            if (!run && _controller.velocity.magnitude <= 0.1f)
            {
                // �� ���� �� ���Ҹ� (0~8�� Ŭ�� �߿��� ���� ����)
                index = Random.Range(0, 9);
            }
            else if (!run)
            {
                // ���� �� ���Ҹ� (0~8�� Ŭ�� �߿��� ���� ����)
                index = Random.Range(0, 9);
            }
            else
            {
                // �� �� ���Ҹ� (10~19�� Ŭ�� �߿��� ���� ����)
                index = Random.Range(10, 20);
            }

            breathingAudioSource.PlayOneShot(breathingClips[index]);
            
        }
    }
}