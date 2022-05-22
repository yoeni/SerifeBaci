using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScript : MonoBehaviour
{
    GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Serife");
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.transform.position.x - transform.position.x > 30)
        {
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<GameScript>().createNewBG(new Vector3(transform.position.x+76.8f,0,0));
        }
    }
}
