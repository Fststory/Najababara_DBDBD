using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletFSM : MonoBehaviour
{
    public enum PalletState
    {
        Upright = 0, // 판자가 세워져 있는 상태
        Falling = 1, // 판자가 넘어지는 중인 상태
        FallDown = 2, // 판자가 완전히 넘어진 상태
        Destroyed = 3 // 판자가 파괴된 상태
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
