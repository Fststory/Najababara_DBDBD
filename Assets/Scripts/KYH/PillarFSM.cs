using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarFSM : MonoBehaviour
{
    enum pillarState
    {
        NoSacrifice,    // ���� ����
        SacrificeLV1,   // �ش� ���� ���൵ 1
        SacrificeLV2,   // �ش� ���� ���൵ 2
        AbsorbSacrifice  // ���� ����
    }

    public GameObject player;
    pillarState currentState;

    float currentTime = 0;
    public float eatTime;

    void Start()
    {
        
    }

    void Update()
    {
        switch (currentState)
        {
            case pillarState.NoSacrifice:
                WaitSacrifice();
                break;
            case pillarState.HaveSacrifice:
                TryToAbsorb();
                break;
            case pillarState.AbsorbSacrifice:
                Disappear();
                break;
        }
    }

    private void WaitSacrifice()    // ������ �ɸ��� ���� �غ��ϴ� ���·� ��ȯ ���
    {
        if (player.GetComponent<EnemyController>().currentState == EnemyController.EnemyState.GetPlayer)
        {
            ChangeState(pillarState.HaveSacrifice);
        }
    }

    private void TryToAbsorb()      // ���������� ���� �õ��� �ϴ� ���
    {
        // ���� �ð����� ���� �й�
        // ��ų üũ ���� �� ü�� ����
        ChangeState(pillarState.AbsorbSacrifice);
    }

    private void Disappear()        // �� �԰� ������� ���
    {
        currentTime += Time.deltaTime;
        if (currentTime > eatTime)
        {
            Destroy(gameObject);
        }
    }

    void ChangeState(pillarState newState)      // ���� ��ȯ ���
    {
        currentState = newState;
    }
}
