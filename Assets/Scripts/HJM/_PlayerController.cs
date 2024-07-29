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

    public float mouseSensitivity = 10000.0f; // ���콺 ����
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



    private void Move() // �÷��̾� WASD �⺻�̵�
    { // ī�޶��� ���� ���⿡ ���� ���� ǥ���Ѵ�. cameraArm�� y���� ������������ ���� ����
        Debug.DrawRay(cameraArm.position, new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized, Color.red);
        Vector2 moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool isMove = moveInput.magnitude != 0;
        animator.SetBool("isMove", isMove);

        if (isMove)
        {
            Vector3 lookForward = new Vector3(cameraArm.forward.x, 0f, cameraArm.forward.z).normalized;
            Vector3 lookRight = new Vector3(cameraArm.right.x, 0f, cameraArm.right.z).normalized;
            Vector3 moveDir = lookForward * moveInput.y + lookRight * moveInput.x;
            // WASD �Է��� �޾� Vector3 playerDir�� �����ϰ� P = P0 + vt�� �����δ�
            //characterBody.forward = lookForward;
            characterBody.forward = moveDir;
            transform.position += moveDir * playerSpeed*  Time.deltaTime;
        }
        

      
    }
    
    private void Run()
    {
        // shift Ű�� ������ �ִٸ�, �÷��̾��� �ӵ��� ������.
        if (Input.GetButtonDown("Debug Multiplier"))
        {
            playerSpeed = 15.0f;

        }
        // shift Ű�� ���ٸ�, �÷��̾��� �ӵ��� �ٽ� �ǵ�����.
        if (Input.GetButtonUp("Debug Multiplier"))
        {
            playerSpeed = 7.0f;
        }
    } // �÷��̾� Shift �޸���

    private void LookAround()
    {   // Vector2 mouseDelta ������ ���콺 ������ ���� �����Ѵ�.
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * mouseSensitivity;

        // cameraArm�� �������� ���Ϸ� ������ ��ȯ�Ѵ�.
        Vector3 camAngle = cameraArm.rotation.eulerAngles;
        // camX ���� ������ ���� �ɰ��� cameraArm.rotation�� �־��ش�.
        float camX = camAngle.x - mouseDelta.y;

        if(camX < 180f)
        {
            camX = Mathf.Clamp(camX, -1f, 70f);
        }
        else
        {
            camX = Mathf.Clamp(camX, 335f, 361f);
        }

        // ���콺 �¿�->ī�޶��¿����� /  ���콺 ����-> ī�޶�������� ������ �� ���� �����ش�.
        // ���콺 ����� �ٶ󺸴� ������ ��ġ��Ű���� mouseDelta.y�� camAngle.x���� ���ָ�ȴ�.
        cameraArm.rotation = Quaternion.Euler(camX, camAngle.y + mouseDelta.x, camAngle.z);


        // camAngle�� mouseDelta�� ���ļ� ���ο� ȸ������ �����. 
        // cameraArm.rotation�� �־�����Ѵ�.


    } // ī�޶� ȸ��


}
