using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static EnemyController;
using static PalletFSM;
using static PlayerFSM;

public class PalletSystem : MonoBehaviour
{
    PalletFSM palFsm;
    public GameObject player;
    public Animator playerAnim;
    public CharacterController cc;

    EnemyController enemyController;

    public GameObject enemy;

    public Collider palletCollider;
    public Transform palletAxis;

    public bool isPlayerInTrigger = false;

    private bool palFallen = false;
    public float fallDuration; // �Ѿ��� �� �ɸ��� �ð�
    public float fallTime;
    private Vector3 targetRotation;
    private Vector3 startRotation;

    private void Start()
    {
        palFsm = GetComponent<PalletFSM>();

        targetRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 50);


        if(player != null)
        {
        playerAnim = player.GetComponent<Animator>();
        cc = player.GetComponent<CharacterController>();
        }


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

            palletAxis.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRotation, t);

            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            //print(t);
            //print(transform.eulerAngles);
            //palletCollider.isTrigger = false;


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


        // ����, ������ ���°� ������ �Ѿ��� �� ���
        if (palFsm.palState == PalletFSM.PalletState.FallDown)
        {
            if ((isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && palFallen == true))
            {
                print("�Ѿ��, �����̽��� �� ����");
                // �Ѿ�� �ִϸ��̼��� ����Ѵ�.
                

            }

        }

    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            isPlayerInTrigger = true;
            print("���� ���� ����");

        }
        //if (other.gameObject.tag == ("Enemy") && palFsm.palState == PalletFSM.PalletState.Falling)
        //{

        //    enemyController = enemy.GetComponent<EnemyController>();

        //    print("���ʹ� ������Ŵ");
        //    enemyController.Stuned(2);

        //}
    }


}

