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

    
    private bool isPlayerInTrigger = false;
    private bool palFallen = false;
    public float fallDuration = 3.5f; // �Ѿ��� �� �ɸ��� �ð�
    public float fallTime;
    private Quaternion targetRotation;

    private void Start()
    {
        palFsm = GetComponent<PalletFSM>();
        targetRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 50));
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
            // Slerp�� �̿��� Ÿ�ٷ����̼����� ������ ���ϰ� ���ش�.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);


            // ���ڰ� ������ �Ѿ����ٸ� ���¸� FallDown���� �����ϰ� fallTime�� �ʱ�ȭ�Ѵ�.
            if (t >= 1.0f)
            {
                palFsm.palState = PalletFSM.PalletState.FallDown;
                fallTime = 0;
            }
        }

        else if (isPlayerInTrigger && Input.GetKey(KeyCode.Space) && !palFallen)
        {
            print("�����̽��� ����");
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
            print("�����̽��� �� ����");

            }

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {

           print("���� ���� ����");
           isPlayerInTrigger = true;
            
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