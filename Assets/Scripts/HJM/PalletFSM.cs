using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletFSM : MonoBehaviour
{
    public enum PalletState
    {
        Upright = 0, // ���ڰ� ������ �ִ� ����
        Falling = 1, // ���ڰ� �Ѿ����� ���� ����
        FallDown = 2, // ���ڰ� ������ �Ѿ��� ����
        Destroyed = 3 // ���ڰ� �ı��� ����
    }

    public PalletState palState;
    




    private void Update()
    {
        switch (palState)
        {
            case PalletState.Upright:
                break;
            case PalletState.Falling:
                break;
            case PalletState.FallDown:
                break;
            case PalletState.Destroyed:
                break;
        }
    }
}
