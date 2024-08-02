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
    public float fallDuration = 3.5f; // 넘어질 때 걸리는 시간
    public float fallTime;
    private Quaternion targetRotation;

    private void Start()
    {
        palFsm = GetComponent<PalletFSM>();
        targetRotation = Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 50));
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
            // Slerp를 이용해 타겟로테이션으로 서서히 변하게 해준다.
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);


            // 판자가 완전히 넘어졌다면 상태를 FallDown으로 변경하고 fallTime을 초기화한다.
            if (t >= 1.0f)
            {
                palFsm.palState = PalletFSM.PalletState.FallDown;
                fallTime = 0;
            }
        }

        else if (isPlayerInTrigger && Input.GetKey(KeyCode.Space) && !palFallen)
        {
            print("스페이스바 누름");
            // 판자의 상태를 넘어가는 중으로 전환한다.
            palFsm.palState = PalletFSM.PalletState.Falling;

            // fallTime을 0으로 초기화하고 palFallen 값을 true로 전환한다.
            fallTime = 0f;
            palFallen = true;

            
        }


        // 만일, 판자의 상태가 넘어지는 중일 때(만 실행한다.)
        if (palFsm.palState == PalletFSM.PalletState.FallDown)
        {
            if((isPlayerInTrigger && Input.GetKey(KeyCode.Space) && palFallen))
            {
            print("스페이스바 또 누름");

            }

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == ("Player"))
        {

           print("판자 영역 들어옴");
           isPlayerInTrigger = true;
            
        }
    }
    public void DropPallet()
    {
        // 판자가 세워져 있는 상태의 행동
    }

    private void FallingState()
    {
        // 예: 판자가 넘어가는 중일 때의 행동
        
    }

    private void FallDownState()
    {
        // 예: 판자가 완전히 넘어졌을 때의 행동
    }

    private void DestroyedState()
    {
        // 예: 판자가 부서졌을 때의 행동
       
    }


}