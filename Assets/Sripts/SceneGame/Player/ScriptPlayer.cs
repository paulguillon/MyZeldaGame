using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class ScriptPlayer : MonoBehaviour
{
    bool isInvincible = false;
    float invincibleTime = 2f;
    public float actualLife = 3f;
    float maxLife = 3f;
    int nbRubies = 0;
    int nbKeys = 0;
    int nbBombs = 0;
    GameObject slotB;
    GameObject slotA;

    Text textRuby;
    Text textKey;
    Text textBomb;

    //Prefabs
    public GameObject life;
    public GameObject halflife;
    public GameObject emptylife;
    public GameObject activebomb;
    public GameObject arrow;
    public GameObject boss;
    [HideInInspector]
    public bool bossDefeated = false;
    public bool isAttacking = false;

    Animator animator;
    ScriptAudioManager audioManager;

    string direction;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //inventaire
        slotB = GameObject.Find("slotB");
        slotA = GameObject.Find("slotA");

        textRuby = GameObject.Find("TextRuby").GetComponent<Text>();
        textKey = GameObject.Find("TextKey").GetComponent<Text>();
        textBomb = GameObject.Find("TextBomb").GetComponent<Text>();

        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<ScriptAudioManager>();

        HealthBar();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        switch (obj.name)
        {
            case "halflife":
                if(actualLife != maxLife)
                    PerteOuGainVie(0.5f);
                break;
            case "life":
                PerteOuGainVie(1f);
                break;
            case "emptylife":
                lifeSlot();
                audioManager.Play("LifeSlot");
                break;
            case "ruby":
                nbRubies += 1;
                textRuby.text = "x " + nbRubies;
                audioManager.Play("Ruby");
                break;
            case "key":
                nbKeys += 1;
                textKey.text = "x " + nbKeys;
                audioManager.Play("Key");
                break;
            case "bomb":
                nbBombs += 1;
                textBomb.text = "x " + nbBombs;
                break;
            case "woodenSword":
                GetEmptyInventorySlot().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                audioManager.Play("Received");
                break;
            case "woodenArrow":
                GetEmptyInventorySlot().sprite = obj.GetComponent<SpriteRenderer>().sprite;
                audioManager.Play("Received");
                break;
            case "bossProjectile":
                PerteOuGainVie(-0.5f);
                break;
            case "bossTrigger":
                //Déclanche le combat de boss
                GameObject go = Instantiate(boss);
                go.name = "Boss";
                Destroy(collision.gameObject);
                break;
            case "Boss":
                //Si on est en train d'attaquer et qu'on touche le boss, alors lui faire perdre de la vie
                if (isAttacking)
                    collision.gameObject.GetComponent<ScriptBoss>().Hurt(-1f);
                else
                    PerteOuGainVie(-0.5f);
                break;
            case "Zelda":
                //Si on touche zelda et qu'on a tué le boss avant la musique de fin se lance
                if (bossDefeated)
                {
                    audioManager.Stop("Overworld");
                    audioManager.Play("Zelda");
                }
                break;
            case "door":
                ScriptPorte laPorte = collision.gameObject.GetComponent<ScriptPorte>();
                //Seules les portes "d'entrée" nécessitent une clé
                if (laPorte.keyNeeded)
                {
                    //Si elle a déjà été ouverte
                    if (laPorte.unlocked)
                        laPorte.Teleport();
                    else if (nbKeys > 0)
                    {
                        nbKeys--;
                        textKey.text = "x " + nbKeys;
                        laPorte.Teleport();
                        laPorte.unlocked = true;
                    }
                }
                else
                    laPorte.Teleport();
                break;
        }
        //Detruit l'item mais detruit pas les portes
        if (obj.tag == "Item" && obj.name != "door")
            Destroy(obj);

        if (obj.tag == "Enemy")
        {
            if (!isInvincible && !isAttacking)//l'ennemi touche le joueur quand il attaque pas donc il perd des points de vie
            {
                PerteOuGainVie(-0.5f);
                audioManager.Play("Hurt");
            }
            else if (isAttacking)//Coup d'épée sur l'ennemi
                collision.gameObject.GetComponent<ScriptEnemy>().Hurt(-1f);

            isInvincible = true;
        }

        //Mort du joueur
        if (actualLife == 0)
        {
            GameObject.Find("UI").GetComponent<ScriptPauseMenu>().LoadMenu();
        }
    }

    void Update()
    {
        direction = GetComponent<ScriptPlayerMovement>().direction;
        timer += Time.deltaTime;

        //Invulnerability after hit
        if (isInvincible)
        {
            if (timer > invincibleTime)
            {
                timer = 0f;
                isInvincible = false;
            }
        }
        //B button
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            if (slotB.GetComponent<Image>().sprite.name == "verticalWoodenSword")
            {
                SwordAttack();
            }
            else if (slotB.GetComponent<Image>().sprite.name == "Arrow")
            {
                Arrow();
            }
        }
        //A button
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            if (slotA.GetComponent<Image>().sprite.name == "verticalWoodenSword")
            {
                SwordAttack();
            }
            else if (slotA.GetComponent<Image>().sprite.name == "Arrow")
            {
                Arrow();
            }
        }

        //Shift = bomb button
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            if (nbBombs > 0)
            {
                Debug.Log("Bouton shift");
                nbBombs--;
                textBomb.text = "x " + nbBombs;
                GameObject go = Instantiate(activebomb);
                go.transform.position = transform.position;
                go.name = "activeBomb";
            }
        }
    }

    public void PerteOuGainVie(float nb)
    {
        actualLife += nb;

        if (nb < 0)
            audioManager.Play("Hurt");

        if (actualLife > maxLife)
            actualLife = maxLife;
        if (actualLife <= 0)
            actualLife = 0;

        HealthBar();
    }

    void HealthBar()
    {
        Transform hb = GameObject.Find("HealthBar").transform;
        int nbLife = Mathf.FloorToInt(actualLife);
        List<GameObject> healthBar = new List<GameObject>();

        //Clear bar
        for (int i = 0; i < hb.childCount; i++)
        {
            Destroy(hb.GetChild(i).gameObject);
            Destroy(hb.GetChild(i).gameObject);
            Destroy(hb.GetChild(i).gameObject);
        }
        //life
        for (int i = 0; i < nbLife; i++)
            CreerCoeur(life, hb, false, healthBar);

        //halflife
        if (actualLife - nbLife == 0.5f)
            CreerCoeur(halflife, hb, false, healthBar);

        //emptylife
        for (int i = 0; i < maxLife - Mathf.CeilToInt(actualLife); i++)
            CreerCoeur(emptylife, hb, false, healthBar);

        //Place them all
        for (int i = 0; i < healthBar.Count; i++)
            healthBar[i].transform.position += new Vector3(i * 50, 0, 0);
    }

    void CreerCoeur(GameObject prefab, Transform parent, bool worldPosition, List<GameObject> healthBar)
    {
        GameObject obj = Instantiate(prefab, parent, false); //Create the GameObject
        obj.name = prefab.name;
        healthBar.Add(obj);
    }

    void lifeSlot()
    {
        maxLife += 1;
        actualLife = maxLife;
        HealthBar();
    }

    Image GetEmptyInventorySlot()
    {
        GameObject go = null;

        if (slotA.GetComponent<Image>().sprite == null)
            go = slotA;
        else
            go = slotB;

        return go.GetComponent<Image>();
    }

    void SwordAttack()
    {
        isAttacking = true;
        animator.SetBool("attack" + direction, true);
    }

    void Arrow()
    {
        GameObject go = Instantiate(arrow, transform.position, Quaternion.identity);
        go.name = "Arrow";

        if (direction == "Left")
        {
            go.transform.position = transform.position + new Vector3(-1.5f, 0, 0);
            go.transform.rotation = Quaternion.Euler(0, 0, 90);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(-15f, 0f);
        }
        else if (direction == "Up")
        {
            go.transform.position = transform.position + new Vector3(0, 1.5f, 0);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 15f);
        }
        else if (direction == "Right")
        {
            go.transform.position = transform.position + new Vector3(1.5f, 0, 0);
            go.transform.rotation = Quaternion.Euler(0, 0, -90);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(15f, 0f);
        }
        else if (direction == "Down")
        {
            go.transform.position = transform.position + new Vector3(0, -1.5f, 0);
            go.transform.rotation = Quaternion.Euler(0, 0, 180);
            go.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -15f);
        }
    }
}