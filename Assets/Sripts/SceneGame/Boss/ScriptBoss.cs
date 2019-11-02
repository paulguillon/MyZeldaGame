using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptBoss : MonoBehaviour
{
    public float health = 20f;
    public Sprite phase2Sprite;
    public GameObject projectile;
    bool inRage = false;
    bool isMoving = false;
    bool hasFired = false;
    int speed = 3;
    public float movementSpeed = 2f;

    float TIMER = 0f;
    ScriptAudioManager audioManager;

    Vector3[] bossPossibleMoves = { new Vector3(-10, 0, 0), new Vector3(0, 10, 0), new Vector3(10, 0, 0), new Vector3(0, -10, 0) };
    Vector3[] projectilePossibleMoves = { new Vector2(-10, 0), new Vector2(0, 10), new Vector2(10, 0), new Vector2(0, -10), new Vector2(-10, 10), new Vector2(10, 10), new Vector2(10, -10), new Vector2(-10, -10) };

    void Start()
    {
        audioManager = FindObjectOfType<ScriptAudioManager>();
        audioManager.Play("BossRoaring");
    }

    // Update is called once per frame
    void Update()
    {
        TIMER += Time.deltaTime;

        //Move and shoot faster when in rage
        if (inRage)
            speed = 2;

        if (TIMER > speed)
        {
            if(!hasFired)
                Fire();
            if(!isMoving)
                Move();
        }

        if(TIMER > speed + 1)
        {
            TIMER = 0f;
            isMoving = false;
            hasFired = false;
        }
    }

    public void Hurt(float amount) //Perte de vie
    {
        audioManager.Play("Hurt");

        health += amount;

        //Si perdu la moitié de sa vie, entre en rage
        if (health <= 10)
        {
            GetComponent<SpriteRenderer>().sprite = phase2Sprite;
            inRage = true;
        }

        if (health <= 0)
        {
            Destroy(gameObject);
            audioManager.Play("BossDefeated");
            GameObject.Find("Player").GetComponent<ScriptPlayer>().bossDefeated = true;
        }
    }
    void Fire() //Lancement des projectiles
    {
        hasFired = true;
        audioManager.Play("BossFire");

        GameObject go = Instantiate(projectile, transform.position, Quaternion.identity);
        go.name = "bossProjectile";
        go.transform.position = transform.position;

        Vector2 pos = projectilePossibleMoves[Random.Range(0, 8)];

        go.GetComponent<Rigidbody2D>().velocity = pos;
    }
    void Move() //Mouvements du boss
    {
        isMoving = true;

        Vector3 pos = bossPossibleMoves[Random.Range(0, 4)];

        transform.position += pos * movementSpeed * Time.deltaTime;
    }
}
