using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform characterBody;
    [SerializeField]
    private Transform cameraArm;

    Animator animator;


    public float playerSpeed = 7.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = characterBody.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Run();
        LookAround();
        
    }



    private void Move() // 플레이어 WASD 기본이동
    { // 카메라의 정면 방향에 빨간 선을 표시한다. cameraArm의 y값을 넣지않음으로 수평 유지
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            // WASD 입력을 받아 Vector3 playerDir에 저장하고 P = P0 + vt로 움직인다
            characterBody.forward = lookForward;
            transform.position += moveDir * playerSpeed*  Time.deltaTime;
        }
        

      
    }
    
    private void Run()
    {
        // shift 키를 누르고 있다면, 플레이어의 속도를 높힌다.
        if (Input.GetButtonDown("Debug Multiplier"))
        {
            playerSpeed = 15.0f;

        }
        // shift 키를 뗀다면, 플레이어의 속도를 다시 되돌린다.
        if (Input.GetButtonUp("Debug Multiplier"))
        {
            playerSpeed = 7.0f;
        }
    } // 플레이어 Shift 달리기

    private void LookAround()
    {   // Vector2 mouseDelta 변수에 마우스 움직임 값을 저장한다.
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // cameraArm의 각도값을 오일러 값으로 변환한다.
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        // camX 값에 제한을 먼저 걸고나서 cameraArm.rotation에 넣어준다.
        float camX = camAngle.x - mouseDelta.y;

        if(camX < 180f)
        {
            camX = Mathf.Clamp(camX, -1f, 70f);
        }
        else
        {
            camX = Mathf.Clamp(camX, 335f, 361f);
        }

        // 마우스 좌우->카메라좌우제어 /  마우스 수직-> 카메라상하제어 임으로 각 값을 더해준다.
        // 마우스 방향과 바라보는 방향을 일치시키려면 mouseDelta.y를 camAngle.x에서 빼주면된다.
        cameraArm.rotation = Quaternion.Euler(camX, camAngle.y + mouseDelta.x, camAngle.z);


        // camAngle과 mouseDelta를 합쳐서 새로운 회전값을 만든다. 
        // cameraArm.rotation에 넣어줘야한다.


    } // 카메라 회전


}
