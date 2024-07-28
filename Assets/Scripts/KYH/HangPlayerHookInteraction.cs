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

    void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player"); // "플레이어" 오브젝트 캐싱
    }
    
    public void HangPlayerOnMe() // 플레이어를 업는 기능
    {
        playerObject.transform.SetParent(hangPoint); // 업는 지점에 플레이어의 위치를 둔다.
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }

    public void HangPlayerOnHook()
    {
        playerObject.transform.SetParent(hookPoint); // 갈고리에 플레이어의 위치를 둔다.
        playerObject.transform.localPosition = new Vector3(0, 0, 0);
    }
}
