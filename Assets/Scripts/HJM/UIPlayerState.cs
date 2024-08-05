using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerState : MonoBehaviour
{

    public Text txtPlayerState;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFSM currentStste = player.GetComponent<PlayerFSM>();
        string stateText = currentStste.pyState.ToString();

        txtPlayerState.text = stateText;


    }
}
