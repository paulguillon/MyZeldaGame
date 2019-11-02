using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEnnemyMoves : MonoBehaviour
{
    public float movementSpeed = 2f;

    public bool moving = false;

    float TIMER = 0f;

    Vector3[] possibleMoves = { new Vector3(-10, 0, 0), new Vector3(0, 10, 0), new Vector3(10, 0, 0), new Vector3(0, -10, 0) };
    
    void Update()
    {
        TIMER += Time.deltaTime;

        if (TIMER > 5 && !moving) //L'ennemi bouge toutes les 5 secondes
            AnimateEnemy();
        else if (TIMER > 6)
        {
            TIMER = 0;
            moving = false;
        }
    }

    void AnimateEnemy()
    {
        moving = true;

        Vector3 pos = possibleMoves[Random.Range(0, 4)];

        transform.position += pos * movementSpeed * Time.deltaTime;
    }
}