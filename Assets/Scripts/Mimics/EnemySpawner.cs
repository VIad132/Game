using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 8f;

    public Vector3 GetSpawnPosition(Transform player)
    {
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = player.position + new Vector3(dir.x, dir.y, 0f) * spawnRadius;
        spawnPos.z = 0f; // КРИТИЧНО ДЛЯ 2D
        return spawnPos;
    }
}
