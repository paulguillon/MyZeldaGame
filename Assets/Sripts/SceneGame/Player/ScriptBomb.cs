using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBomb : MonoBehaviour
{
    public float TIMER = 0f;
    public float range = 1.5f;
    public float timeBeforeExplosion = 3f;

    void Start()
    {
        GetComponent<BoxCollider2D>().size = new Vector2(0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
            collision.gameObject.GetComponent<ScriptPlayer>().PerteOuGainVie(-1f);
        else if (collision.gameObject.tag == "Enemy")
            collision.gameObject.GetComponent<ScriptEnemy>().Hurt(-1f);
        else if (collision.gameObject.name == "Boss")
            collision.gameObject.GetComponent<ScriptBoss>().Hurt(-1f);
        Destroy(gameObject);
    }

    void Update()
    {
        TIMER += Time.deltaTime;

        //Pour detecter l'explosion de la bombe, on set la taille du box à 0 puis on l'agrandi au moment de l'explosion pour detecter les objets dans la zone d'effet via trigger
        if (TIMER > timeBeforeExplosion)
        {
            FindObjectOfType<ScriptAudioManager>().Play("Explosion");
            GetComponent<BoxCollider2D>().size = new Vector2(range * 2, range * 2);
        }
        if (TIMER > timeBeforeExplosion + Time.deltaTime)
        {
            Destroy(gameObject);
        }
    }
}