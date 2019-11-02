using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEnemy : MonoBehaviour
{
    public float health = 2f;

    public void Hurt(float amount) //Perte de vie
    {
        FindObjectOfType<ScriptAudioManager>().Play("Hurt");
        health += amount;

        if (health <= 0)
            Destroy(gameObject);
    }
}