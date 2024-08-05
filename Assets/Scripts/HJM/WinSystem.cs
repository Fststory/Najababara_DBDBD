using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinSystem : MonoBehaviour
{
    public GameObject winTxtUI;
    

    // Start is called before the first frame update
    void Start()
    {
        winTxtUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        winTxtUI.SetActive(true);
    }
}
