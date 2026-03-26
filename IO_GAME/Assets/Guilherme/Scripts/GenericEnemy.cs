using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public BoxCollider2D fisCollider;
    public CircleCollider2D areaCollider;
    public Rigidbody2D rb;

    // Deixei public para os filhos poderem acessar facilmente, 
    // ou você pode continuar usando a Propriedade se preferir.
    public bool feltPlayer;
    public float life;
    public float damage;
    public PlayerSwimming player;
    public bool isAttack;
    public bool canAttack;
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