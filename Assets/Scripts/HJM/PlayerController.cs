using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animator _animator;
    Camera _camera;
    CharacterController _controller;

    public float moveSpeed;
    public float runSpeed;
    public float finalSpeed;
    public bool run;

    public float smoothness = 10.0f;
    public bool toggleCameraRotation;


    public bool alraedyHooked;
    public bool HookedLv2;

    float[] idleAnims = new float[3] { 0.0f, 0.5f, 1.0f };



    void Start()
    {
        _animator = this.GetComponent<Animator>();
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();

        


    }

    

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true; // 둘러보기 활성화
        }
        else
        {
            toggleCameraRotation = false; // 둘러보기 비활성화
        }

        // LeftShift 누르면 달리기
        if (Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
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

    public void InputMovement(float moveSpeed, float runSpeed)
    {
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
        _animator.SetFloat("DirLength", moveDirection.magnitude);

        //float percent = (run ? 1 : 0.5f) * moveDirection.magnitude;
        //_animator.SetFloat("Blend", percent, 0.1f, Time.deltaTime);
    }
}