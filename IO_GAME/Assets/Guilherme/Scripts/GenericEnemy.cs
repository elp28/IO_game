using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected BoxCollider2D fisCollider;
    protected CircleCollider2D areaCollider;
    protected Rigidbody2D rb;

    // Deixei public para os filhos poderem acessar facilmente, 
    // ou você pode continuar usando a Propriedade se preferir.
    protected bool feltPlayer;
    public float life;
    public float damage;
    protected PlayerSwimming player;
   protected bool isAttack;
    protected bool canAttack;
    public float cooldown;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerSwimming tempPlayer = collision.gameObject.GetComponent<PlayerSwimming>();
        if (tempPlayer != null)
        {
            player = tempPlayer;
            feltPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerSwimming tempPlayer = collision.gameObject.GetComponent<PlayerSwimming>();
        if (tempPlayer != null)
        {
            feltPlayer = false;
            player = null;
        }
    }
}