using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangPlayerHookInteraction : MonoBehaviour
{
    // 플레이어를 업고 매달 수 있는 기능을 구현한 스크립트

    /* 플레이어를 업는 작용/ 플레이어를 매다는 작용
        
    */

    public GameObject playerObject;
    public Transform hangPoint;
    public Transform pillarTransform;
    public Transform hookPoint;
    public PlayerFSM playerFSM;
    public PlayerController playerController;
    public CharacterController cc;
    public EnemyController enemyController;
    public AnimationClip animClip;

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "플레이어" 오브젝트 캐싱
        playerFSM = playerObject.GetComponent<PlayerFSM>(); // 플레이어 상태 캐싱
        playerController = playerObject.GetComponent<PlayerController>(); // 플레이어 컨트롤러 캐싱
        animClip = Resources.Load<AnimationClip>("Hook");
        cc = playerObject.GetComponent<CharacterController>();
    }
    
    public void HangPlayerOnMe()    // 플레이어를 업는 기능
    {
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        playerObject.transform.SetParent(hangPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        //playerController.enabled = false;   // 플레이어 컨트롤러 비활성화!
        cc.enabled = false;   // 캐릭터 컨트롤러 끄니까 왔다갔다 하는 거 없긴 함
    }

    public void HangPlayerOnHook()  // 플레이어를 매다는 기능
    {
        playerFSM.pyState = PlayerFSM.PlayerState.Hooked;
        playerObject.transform.SetParent(hookPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        Invoke("AfterHook", animClip.length);
    }
    
    void AfterHook()
    {
        enemyController.ChangeState(EnemyController.EnemyState.NoEvidence);
        enemyController.hooking = false;
    }
}
