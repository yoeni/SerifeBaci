using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject[] BGinGame;
    public place Mekan=place.Tarla;
    public int km = 1;
    public enum place
    {
        Koy,
        Tarla,
        Orman
    }

    public  void createNewBG(Vector3 location)
    {
        if (km>=19)
        {
            Mekan = place.Koy;
        }
        Player.GetComponent<SerifeScript>().Km = km;
        if (km>=8&&km<=11)
        {
            Instantiate(BGinGame[1], location, Quaternion.identity);
        }
        else
        {
            if (Mekan == place.Koy)
            {

                Instantiate(BGinGame[2], location, Quaternion.identity);
            }
            else if (Mekan == place.Orman)
            {
                Instantiate(BGinGame[1], location, Quaternion.identity);
            }
            else
            {
                Instantiate(BGinGame[0], location, Quaternion.identity);
            }
        }
        km += 1;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position,new Vector3(Player.transform.position.x,0,-10),0.5f*Time.deltaTime);
    }
}
