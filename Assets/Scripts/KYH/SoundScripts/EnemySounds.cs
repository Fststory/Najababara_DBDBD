using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    EnemyController enemyController;
    Enum enemyFSm;

    // Start is called before the first frame update
    void Start()
    {
        enemyController = GetComponent<EnemyController>();
        enemyFSm = enemyController.currentState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
