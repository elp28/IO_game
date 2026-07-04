using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PollutedArea : MonoBehaviour
{
    [Header("Configuração")]
    [SerializeField] private GameObject station;

    private List<GenericEnemy> _enemies = new List<GenericEnemy>();
    private Collider2D _areaBounds;
    private bool _isCleared = false;
    private bool _playerInside = false;

    public bool IsCleared => _isCleared;
    public int EnemyCount => _enemies.Count;
    public Bounds Bounds => _areaBounds.bounds;

    void Start()
    {
        _areaBounds = GetComponent<Collider2D>();

        Collider2D[] hits = Physics2D.OverlapBoxAll(
            _areaBounds.bounds.center,
            _areaBounds.bounds.size,
            0f
        );

        foreach (var hit in hits)
            TryRegisterEnemy(hit.GetComponent<GenericEnemy>());

        Debug.Log($"[{gameObject.name}] {_enemies.Count} inimigo(s) registrado(s).");

        if (station != null) station.SetActive(false);

        // Checa inimigos spawnados em runtime
        StartCoroutine(WatchForNewEnemies());
    }

    /// <summary>
    /// Registra um inimigo na área. Seguro chamar múltiplas vezes.
    /// </summary>
    public void TryRegisterEnemy(GenericEnemy enemy)
    {
        if (enemy == null || _enemies.Contains(enemy)) return;
        _enemies.Add(enemy);
        enemy.SetArea(this);

        if (_playerInside)
        {
            enemy.OnPlayerEnteredArea();
            AreaUIManager.instance?.UpdateEnemyCount(this);
        }
    }

    /// <summary>
    /// Fica checando se novos inimigos apareceram dentro da área (spawnados em runtime).
    /// </summary>
    private IEnumerator WatchForNewEnemies()
    {
        while (!_isCleared)
        {
            yield return new WaitForSeconds(0.5f);

            Collider2D[] hits = Physics2D.OverlapBoxAll(
                _areaBounds.bounds.center,
                _areaBounds.bounds.size,
                0f
            );

            foreach (var hit in hits)
                TryRegisterEnemy(hit.GetComponent<GenericEnemy>());
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.GetComponent<PlayerMove>() == null) return;

        _playerInside = true;

        foreach (var enemy in _enemies)
            if (enemy != null) enemy.OnPlayerEnteredArea();

        AreaUIManager.instance?.ShowArea(this);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.GetComponent<PlayerMove>() == null) return;

        _playerInside = false;

        foreach (var enemy in _enemies)
            if (enemy != null) enemy.OnPlayerExitedArea();

        AreaUIManager.instance?.HideArea();
    }

    public void OnEnemyDied(GenericEnemy enemy)
    {
        _enemies.Remove(enemy);
        AreaUIManager.instance?.UpdateEnemyCount(this);

        if (_enemies.Count == 0)
            ClearArea();
    }

    private void ClearArea()
    {
        if (_isCleared) return;
        _isCleared = true;

        Debug.Log($"[{gameObject.name}] Área limpa!");
        AreaUIManager.instance?.OnAreaCleared(this);

        if (station != null)
            station.SetActive(true);
    }

    public bool ContainsPoint(Vector3 point)
    {
        return _areaBounds.bounds.Contains(point);
    }

    public Vector3 ClampToBounds(Vector3 point)
    {
        Bounds b = _areaBounds.bounds;
        return new Vector3(
            Mathf.Clamp(point.x, b.min.x, b.max.x),
            Mathf.Clamp(point.y, b.min.y, b.max.y),
            point.z
        );
    }
}