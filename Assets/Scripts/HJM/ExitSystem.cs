using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitSystem : MonoBehaviour
{
    public static ExitSystem exitSystem { get; private set; }



    public GameObject generator;
    public GeneratorSystem generatorSystem;
    public bool isPlayerInTrigger = false;



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

    public void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OpenExit();
            isPlayerInTrigger = true;
        }
    }

    public void OpenExit()
    {
        if(isPlayerInTrigger == true)
        {
        print("�ⱸ�� ���Ƚ��ϴ�!");
        transform.position += Vector3.up * Time.deltaTime;

        }

    }


}
