using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCheckSystem : MonoBehaviour
{

    public GeneratorSystem generatorSystem;
    public Transform noteAxis; // ��ųüũ ��Ʈ
    public Transform normalAxis; // ��ųüũ ��Ʈ
    public GameObject noteCanvas; // ��Ʈ ĵ����UI


    

    public AudioSource successAudio;
    public AudioSource failedAudio;
    

    public bool isChecked = false;
    public bool finish = false;


    private void Start()
    {
        noteCanvas.SetActive(false);
        generatorSystem = FindObjectOfType<GeneratorSystem>();
        successAudio.playOnAwake = false;
        failedAudio.playOnAwake = false;

      
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Check();
            print("�����̽��� ����!");
        }


    }


    public void CheckStart()
    {
        noteCanvas.SetActive(true);
        finish = false;

    }

    public void Check()
    {
           
            if (noteAxis.eulerAngles.z > 310 + normalAxis.eulerAngles.z || noteAxis.eulerAngles.z < 0 + normalAxis.eulerAngles.z)
            {
                print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
                isChecked = true;
                finish = true;
                successAudio.gameObject.SetActive(true);
                
                noteCanvas.SetActive(false);
                print(finish);
                generatorSystem.RestartRepair();
                generatorSystem.isSkillChecking = false;
                successAudio.Play();




        }
            else
            {
                print("����!!" + ", " + noteAxis.transform.eulerAngles.z.ToString());
                isChecked = false;
                finish = true;
                failedAudio.gameObject.SetActive(true);
                noteCanvas.SetActive(false);
                print(finish);
                generatorSystem.FailedCheck();
                generatorSystem.repairPercent = generatorSystem.repairPercent * 0.7f;
                generatorSystem.isSkillChecking = false;
                failedAudio.Play();
             }

    }

  
}