using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerState : MonoBehaviour
{

    public Text txtPlayerState;
    public Text txtEnemyState;

    public GameObject player;
    public GameObject enemy;




    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        PlayerFSM pycurrentStste = player.GetComponent<PlayerFSM>();
        string pystateText = pycurrentStste.pyState.ToString();

        txtPlayerState.text = pystateText;

        EnemyController encurrentStste = enemy.GetComponent<EnemyController>();
        string enstateText = encurrentStste.currentState.ToString();

        txtEnemyState.text = enstateText;



    }
}
