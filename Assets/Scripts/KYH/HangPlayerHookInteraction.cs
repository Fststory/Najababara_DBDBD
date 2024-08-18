using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Animator playerAnim;

    public GameObject img_Dying;
    public GameObject img_Hang;

    private void Awake()
    {
        img_Dying = GameObject.Find("img_Dying");
        img_Hang = GameObject.Find("img_Hang");
    }

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "플레이어" 오브젝트 캐싱
        playerFSM = playerObject.GetComponent<PlayerFSM>(); // 플레이어 상태 캐싱
        playerController = playerObject.GetComponent<PlayerController>(); // 플레이어 컨트롤러 캐싱
        animClip = Resources.Load<AnimationClip>("Hook");
        cc = playerObject.GetComponent<CharacterController>();
        playerAnim = playerObject.GetComponent<Animator>();


    }
    
    public void HangPlayerOnMe()    // 플레이어를 업는 기능
    {
        playerFSM.pyState = PlayerFSM.PlayerState.InAction;
        playerObject.transform.SetParent(hangPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        //playerController.enabled = false;   // 플레이어 컨트롤러 비활성화!
        cc.enabled = false;   // 캐릭터 컨트롤러 끄니까 왔다갔다 하는 거 없긴 함

        img_Dying.SetActive(false);
        img_Hang.SetActive(true);
        print("행 유아이 불러옴");
    }

    public void HangPlayerOnHook()  // 플레이어를 매다는 기능
    {
        playerFSM.pyState = PlayerFSM.PlayerState.Hooked;
        playerObject.transform.SetParent(hookPoint);
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
        playerObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        Invoke("AfterHook", animClip.length);

        playerAnim.SetTrigger("hang");

    }
    
    void AfterHook()
    {
        enemyController.ChangeState(EnemyController.EnemyState.NoEvidence);
        enemyController.hooking = false;
    }
}
