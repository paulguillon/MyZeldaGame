using UnityEngine;
using System.Threading;

public class ScriptBossProjectile : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Pour pas que les projectiles soient détruits dès qu'ils apparaissent
        if (collision.gameObject.name != "Boss")
            Destroy(gameObject);
    }
}