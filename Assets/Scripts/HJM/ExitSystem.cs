using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSystem : MonoBehaviour
{
    public static ExitSystem exitSystem { get; private set; }



    public GameObject generator;
    public GeneratorSystem generatorSystem;
    



private void Awake()
{
    // 다른 인스턴스가 이미 존재하면 이 객체를 파괴합니다.
    if (exitSystem == null)
    {
        exitSystem = this;
        DontDestroyOnLoad(gameObject);  // 씬 전환 시 객체가 파괴되지 않도록 설정
    }
    else
    {
        Destroy(gameObject);
    }
}

private void Start()
    {
       
    }

    public void OpenExit()
    {
     print("출구가 열렸습니다!");
     transform.position += Vector3.up * Time.deltaTime;

    }


    
}
