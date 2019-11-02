using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayerMovement : MonoBehaviour
{
    public bool moving = false;
    public string direction = "Down";
    public float vitesse = 1f;
    Animator animator;

    Vector3 dir;
    Sprite playerSprite;
    public GameObject map;
    public GameObject miniMapLink;

    void Start()
    {
        animator = GetComponent<Animator>();

        playerSprite = GetPlayerSprite("lookDown");
    }

    void Update()
    {
        //Basic movements
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0 || inputY != 0)
            moving = true;
        else
            moving = false;

        if (moving)
        {
            //On reset les animation quand on commence a bouger
            animator.SetBool("pickup", false);
            animator.SetBool("attackUp", false);
            animator.SetBool("attackDown", false);
            animator.SetBool("attackLeft", false);
            animator.SetBool("attackRight", false);
            GetComponent<ScriptPlayer>().isAttacking = false;

            //LEFT
            if (inputX < 0)
            {
                animator.SetBool("walkLeft", true);
                direction = "Left";
            }
            //RIGHT
            if (inputX > 0)
            {
                animator.SetBool("walkRight", true);
                direction = "Right";
            }
            //UP
            if (inputY > 0)
            {
                animator.SetBool("walkUp", true);
                direction = "Up";
            }
            //DOWN
            if (inputY < 0)
            {
                animator.SetBool("walkDown", true);
                direction = "Down";
            }
        }

        if(!moving)
        {
            animator.SetBool("walkUp", false);
            animator.SetBool("walkDown", false);
            animator.SetBool("walkLeft", false);
            animator.SetBool("walkRight", false);

            switch (direction)
            {
                case "Right":
                    playerSprite = GetPlayerSprite("lookRight");
                    break;
                case "Left":
                    playerSprite = GetPlayerSprite("lookLeft");
                    break;
                case "Up":
                    playerSprite = GetPlayerSprite("lookUp");
                    break;
                case "Down":
                    playerSprite = GetPlayerSprite("lookDown");
                    break;
            }
        }
        //Set player's sprite
        GetComponent<SpriteRenderer>().sprite = playerSprite;

        //Player's position
        dir = new Vector3(inputX, inputY, 0);
        Vector3 pos = transform.position + dir * vitesse * Time.deltaTime;

        //Change player's position
        transform.position = pos;
        //Following camera
        Camera.main.transform.position = new Vector3(pos.x, pos.y + 1, -10);
        //Point vert qui bouge sur la minimap
        miniMapLink.transform.position += dir * vitesse * Time.deltaTime;
    }

    Sprite GetPlayerSprite(string spriteName)
    {
        return Resources.Load<Sprite>("Sprites/Link/" + spriteName);
    }
}