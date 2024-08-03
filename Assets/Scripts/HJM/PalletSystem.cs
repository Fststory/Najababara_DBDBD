using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static PalletFSM;
using static PlayerFSM;

public class PalletSystem : MonoBehaviour
{
    PalletFSM palFsm;
    public Animator playerAnim;
    public CharacterController cc;

    public GameObject palletCollider;
    public Transform palletAxis;


    public bool isPlayerInTrigger = false;
    private bool palFallen = false;
    public float fallDuration = 3.0f; // �Ѿ��� �� �ɸ��� �ð�
    public float fallTime;
    private Vector3 targetRotation;
    private Vector3 startRotation;

    private void Start()
    {
        palFsm = GetComponent<PalletFSM>();

        //startRotation = palletAxis.eulerAngles;
        targetRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 50);
        //targetRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 50));
    }


    private void Update()
    {
        // ����, ������ ���°� �Ѿ����� ���� ��(�� �����Ѵ�.)
        if (palFsm.palState == PalletFSM.PalletState.Falling)
        {
            // fallTime�� �ð��� �����Ѵ�.
            fallTime += Time.deltaTime;
            // �Ѿ����� �ð��� t�� �ް� Ŭ������ ������ �ɾ��ش�.
            float t = Mathf.Clamp01(fallTime / fallDuration);
            // Lerp�� �̿��� Ÿ�ٷ����̼����� ������ ���ϰ� ���ش�. // �ٵ� ������ �������ʾ�!!!!!!!!!

            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetRotation, t);

            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            //print(t);
            //print(transform.eulerAngles);
            GetComponent<Collider>().isTrigger = false;


            // ���ڰ� ������ �Ѿ����ٸ� ���¸� FallDown���� �����ϰ� fallTime�� �ʱ�ȭ�Ѵ�.
            if (t >= 1.0f)
            {
                palFsm.palState = PalletFSM.PalletState.FallDown;

                fallTime = 0;
            }
        }

        else if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && !palFallen)
        {

            print("���ھ��� �����̽��� ����");
            // ������ ���¸� �Ѿ�� ������ ��ȯ�Ѵ�.
            palFsm.palState = PalletFSM.PalletState.Falling;

            // fallTime�� 0���� �ʱ�ȭ�ϰ� palFallen ���� true�� ��ȯ�Ѵ�.
            fallTime = 0f;
            palFallen = true;

            
        }


        // ����, ������ ���°� �Ѿ����� ���� ��(�� �����Ѵ�.)
        if (palFsm.palState == PalletFSM.PalletState.FallDown)
        {
            if((isPlayerInTrigger && Input.GetKey(KeyCode.Space) && palFallen))
            {
            print("�Ѿ��, �����̽��� �� ����");
                // �Ѿ�� �ִϸ��̼��� ����Ѵ�.
                playerAnim.SetTrigger("isClimb");
                // �Ѿ�� ���� ��ġ�� ĳ������Ʈ�ѷ��� ������Ų��.
                // ����, �Ѿ�� �ð��� �����ٸ�
                // cc�� ���濡�� �����Ÿ� ������ ������ ������.
                // ���� ���̶� �ݶ��̴��� ��Ƽ�� ���� �ľ��ҵ�, ���� ȸ���ϰ� �ݶ��̴��� ����
                isPlayerInTrigger = false;
                
            }

        }


    }

 



    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            isPlayerInTrigger = true;
            print("���� ���� ����");
            playerAnim = other.gameObject.GetComponent<Animator>();
            cc = other.gameObject.GetComponent<CharacterController>();
            
        }
    }

    


    public void DropPallet()
    {
        // ���ڰ� ������ �ִ� ������ �ൿ
    }

    private void FallingState()
    {
        // ��: ���ڰ� �Ѿ�� ���� ���� �ൿ
        
    }

    private void FallDownState()
    {
        // ��: ���ڰ� ������ �Ѿ����� ���� �ൿ
    }

    private void DestroyedState()
    {
        // ��: ���ڰ� �μ����� ���� �ൿ
       
    }


}