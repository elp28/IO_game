using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class ShootEnemy : GenericEnemy
{
    GenericEnemy genericEnemy;
    [SerializeField] GameObject shotPrefab;
   

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        genericEnemy = this;
        fisCollider = GetComponent<BoxCollider2D>();
        areaCollider = GetComponentInChildren<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (genericEnemy.FeltPlayer && !canAttack)
        {
            agent.SetDestination(player.transform.position);
        }

        if (canAttack && !isAttack)
        {
            StartCoroutine(CicleDamage());
        }
    }

    IEnumerator CicleDamage()
    {
        isAttack = true;
        Instantiate(shotPrefab, transform.localPosition, Quaternion.identity);
        yield return new WaitForSeconds(5);
        isAttack = false;
    }
}
