using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPorte : MonoBehaviour
{
    public Vector3 otherMapPosition;
    public bool keyNeeded = false;
    public bool unlocked = false;

    //Téléporte le joueur vers la deuxieme porte qui a pour coordonnées celles indiquées dans l'inspector de la porte en question
    public void Teleport()
    {
        GameObject go = GameObject.Find("Player");

        go.transform.position = otherMapPosition;
        Camera.main.transform.position = new Vector3(go.transform.position.x, go.transform.position.y, -10);
    }
}
