using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EnemyPool enemyPool;
    public EnemySpawner spawner;
    public Transform player;

    public float timeBetweenWaves = 5f;
    public int enemiesPerWave = 3;

    private int wave = 1;

    void Start()
    {
        StartCoroutine(WaveRoutine());
    }

    IEnumerator WaveRoutine()
    {
        while (true)
        {
            Debug.Log("Wave " + wave);

            for (int i = 0; i < enemiesPerWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.5f);
            }

            wave++;
            enemiesPerWave += 2;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy()
    {
    Vector3 pos = spawner.GetSpawnPosition(player);

    GameObject enemy = Instantiate(
        enemyPool.enemyPrefab,   // або напряму prefab
        pos,
        Quaternion.identity
    );

    Debug.Log("SPAWN AT " + pos);
    }


}
