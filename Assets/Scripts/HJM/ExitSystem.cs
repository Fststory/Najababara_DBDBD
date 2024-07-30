using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSystem : MonoBehaviour
{
    public static ExitSystem exitSystem { get; private set; }



    public GameObject generator;
    public GeneratorSystem generatorSystem;
    public int repairCount;



private void Awake()
{
    // �ٸ� �ν��Ͻ��� �̹� �����ϸ� �� ��ü�� �ı��մϴ�.
    if (exitSystem == null)
    {
        exitSystem = this;
        DontDestroyOnLoad(gameObject);  // �� ��ȯ �� ��ü�� �ı����� �ʵ��� ����
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
     print("�ⱸ�� ���Ƚ��ϴ�!");
     transform.position += Vector3.up * Time.deltaTime;

    }


    
}
