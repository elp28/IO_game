using UnityEngine;

public class Projectil : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 4f;

    private Vector2 _direction;
    private float   _damage;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Chamado pelo ProjectileEnemy logo após instanciar.
    /// </summary>
    public void Fire(Vector2 direction, float damage)
    {
        _direction = direction;
        _damage    = damage;

        // Rotaciona o sprite na direção do disparo
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_rb != null)
            _rb.linearVelocity = _direction * speed;

        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Ignora outros projéteis e triggers
        if (col.isTrigger) return;

        PlayerLife playerLife = col.GetComponent<PlayerLife>();
        if (playerLife != null)
        {
            playerLife.TakeDamage(_damage);
            Destroy(gameObject);
            return;
        }

        // Destrói ao bater em qualquer coisa sólida que não seja inimigo
        if (col.GetComponent<GenericEnemy>() == null)
            Destroy(gameObject);
    }
}