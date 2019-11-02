using UnityEngine;

public class ScriptFleche : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destruction de la flèche
        Destroy(gameObject);

        //Si l'objet touché et le boss ou un ennemie, alors on lui fait perdre de la vie en appelant leur fonction respective
        if (collision.gameObject.tag == "Enemy" && collision.gameObject.name != "Arrow")
        {
            if(collision.gameObject.name == "Boss")
                collision.gameObject.GetComponent<ScriptBoss>().Hurt(-1f);
            else
                collision.gameObject.GetComponent<ScriptEnemy>().Hurt(-1f);
        }
    }
}