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
    public float gravity = -9.81f;  // 중력 값
    public float groundDistance = 0.4f;  // 땅과의 거리
    public LayerMask groundMask;  // 땅 레이어 마스크

    public bool isGrounded;
    public Transform groundCheck;  // 땅 체크용 트랜스폼

    public float smoothness = 10.0f;
    public bool toggleCameraRotation;


    public bool alraedyHooked;
    public bool HookedLv2;

    public Vector3 arriveCC;

    // 발자국 소리 관련 변수
    public List<AudioClip> footstepClips; // 여러 발자국 소리 클립
    private AudioSource audioSource; // 발자국 소리를 재생할 오디오 소스
    public float footstepInterval = 0.5f; // 기본 발자국 소리 간격
    private float footstepTimer; // 발자국 소리 타이머


    // 숨소리 관련 변수
    public List<AudioClip> breathingClips; // 여러 숨소리 클립
    private AudioSource breathingAudioSource; // 숨소리를 재생할 오디오 소스
    public float breathingInterval = 2.0f; // 기본 숨소리 간격
    private float breathingTimer; // 숨소리 타이머



    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
        PlayerFSM = GetComponent<PlayerFSM>();

        

        // 오디오 소스 초기화
        audioSource = GetComponent<AudioSource>();
        breathingAudioSource = gameObject.AddComponent<AudioSource>();
    }
    

    void Update()
    {
        if(PlayerFSM.pyState != PlayerFSM.PlayerState.InAction)
        {
            InputMovement();
            HandleFootsteps(); // 발자국 소리 처리
            HandleBreathing(); // 숨소리 처리
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // 둘러보기 활성화
        }
        else
        {
            toggleCameraRotation = false; // 둘러보기 비활성화
        }

        // 부상 상태 확인 및 애니메이터 파라미터 업데이트
        bool isInjured = PlayerFSM.pyState == PlayerFSM.PlayerState.Injured;
        _animator.SetBool("isInjured", isInjured);

        // LeftShift 누르면 달리기
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


        // Ctrl 누르면 앉기
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

            // 땅 체크
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            // 땅에 닿아있는 경우 속도 초기화
            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;  // 약간의 음수 값을 주어 땅에 착지하는 느낌을 줍니다.
            }
        
            // 중력 적용
            velocity.y += gravity * Time.deltaTime;
            // 속도에 따른 이동
            _controller.Move(velocity * Time.deltaTime);
        }
    }

    void LateUpdate()
    {
        // 카메라 회전 활성화 시 캐릭터의 회전 로직을 LateUpdate에서 수행
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

        // 앉을 때 속도 조절
        if (_animator.GetBool("isCrouch"))
        {
            finalSpeed = moveSpeed * 0.5f; //  앉을 때 속도를 50%로 줄임
        }
        else
        {
            finalSpeed = run ? runSpeed : moveSpeed;
        }


        // 만약 run이 true라면, finalSpeed는 runSpeed 값을 갖는다.
        // 만약 run이 false라면, finalSpeed는 speed 값을 갖는다.
        finalSpeed = run ? runSpeed : moveSpeed;

        // 카메라의 방향을 기준으로 이동 벡터 계산
        Vector3 forward = _camera.transform.forward;
        Vector3 right = _camera.transform.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        // 마우스 상하좌우 값을 h, v에 담는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 애니메이터의 플롯변수 값에 h,v 를 전달한다.
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
            print("안아파요");

        }

        else if (PlayerFSM.pyState == PlayerFSM.PlayerState.Injured)
        {

            _animator.SetBool("isInjured", true);
            _animator.SetFloat("FloatInjured", moveDirection.magnitude);
            print("아파요");


        }




    }

    void HandleFootsteps()
    {
        if (isGrounded && _controller.velocity.magnitude > 0.1f)
        {
            // 달릴 때 발자국 소리 간격 줄이기
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
        // 속도에 따른 숨소리 간격 조절
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
                // 서 있을 때 숨소리 (0~8번 클립 중에서 랜덤 선택)
                index = Random.Range(0, 9);
            }
            else if (!run)
            {
                // 걸을 때 숨소리 (0~8번 클립 중에서 랜덤 선택)
                index = Random.Range(0, 9);
            }
            else
            {
                // 뛸 때 숨소리 (10~19번 클립 중에서 랜덤 선택)
                index = Random.Range(10, 20);
            }

            breathingAudioSource.PlayOneShot(breathingClips[index]);
            
        }
    }
}