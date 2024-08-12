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
    public Animator playerAnim;
    public CharacterController cc;

    EnemyController enemyController;

    public GameObject enemy;

    public Collider palletCollider;
    public Transform palletAxis;

    public bool isPlayerInTrigger = false;

    private bool palFallen = false;
    public float fallDuration = 2.0f; // 넘어질 때 걸리는 시간
    public float fallTime;
    private Vector3 targetRotation;
    private Vector3 startRotation;

    private void Start()
    {
        palFsm = GetComponent<PalletFSM>();

        //startRotation = palletAxis.eulerAngles;
        targetRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 50);
        //targetRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 50));
        
            playerAnim = playerAnim.GetComponent<Animator>();
            cc = cc.GetComponent<CharacterController>();
        

    }


    private void Update()
    {
        // 만일, 판자의 상태가 넘어지는 중일 때(만 실행한다.)
        if (palFsm.palState == PalletFSM.PalletState.Falling)
        {
            // fallTime에 시간을 누적한다.
            fallTime += Time.deltaTime;
            // 넘어지는 시간을 t로 받고 클램프로 제한을 걸어준다.
            float t = Mathf.Clamp01(fallTime / fallDuration);
            // Lerp를 이용해 타겟로테이션으로 서서히 변하게 해준다. // 근데 서서히 변하지않아!!!!!!!!!

            palletAxis.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, targetRotation, t);

            //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
            //print(t);
            //print(transform.eulerAngles);
            //palletCollider.isTrigger = false;


            // 판자가 완전히 넘어졌다면 상태를 FallDown으로 변경하고 fallTime을 초기화한다.
            if (t >= 1.0f)
            {
                palFsm.palState = PalletFSM.PalletState.FallDown;

                fallTime = 0;
            }
        }

        else if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.Space) && !palFallen)
        {

            print("판자엎기 스페이스바 누름");
            // 판자의 상태를 넘어가는 중으로 전환한다.
            palFsm.palState = PalletFSM.PalletState.Falling;

            // fallTime을 0으로 초기화하고 palFallen 값을 true로 전환한다.
            fallTime = 0f;
            palFallen = true;


        }


        // 만일, 판자의 상태가 완전히 넘어진 후 라면
        if (palFsm.palState == PalletFSM.PalletState.FallDown)
        {
            if ((isPlayerInTrigger && Input.GetKey(KeyCode.Space) && palFallen == true))
            {
                print("넘어가기, 스페이스바 또 누름");
                // 넘어가는 애니메이션을 재생한다.
                playerAnim.SetBool("isClimb", true);
                // 넘어가기 전의 위치에 캐릭터컨트롤러를 고정시킨다.
               
            }

        }

    }

 
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            isPlayerInTrigger = true;
            print("판자 영역 들어옴");
            
        }
        if (other.gameObject.tag == ("Enemy") && palFsm.palState == PalletFSM.PalletState.Falling)
        {

            enemyController = enemy.GetComponent<EnemyController>();

            print("에너미 기절시킴");
            enemyController.Stuned(2);

        }
    }
  

}

    