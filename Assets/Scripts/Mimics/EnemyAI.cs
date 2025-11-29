using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Spawn")]
    [Tooltip("Approximate radius around the camera where the enemy will spawn (world units).")]
    [SerializeField] private float spawnRadius = 20f;
    [Tooltip("Extra margin to ensure spawn is outside camera view (world units).")]
    [SerializeField] private float spawnMargin = 2f;

    [Header("Movement")]
    [Tooltip("Movement speed in world units per second.")]
    [SerializeField] private float speed = 4f;
    [Tooltip("Use NavMeshAgent if this GameObject has one. If false, uses simple direct movement.")]
    [SerializeField] private bool useNavMeshAgent = false;

    [Header("References")]
    [Tooltip("Player transform. If null, will search for GameObject with tag 'Player'.")]
    [SerializeField] private Transform player;

    private UnityEngine.AI.NavMeshAgent navMeshAgent;

    private enum State { Idle, Chasing }
    private State state = State.Idle;

    private void Awake()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (navMeshAgent != null)
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updateUpAxis = false;
            navMeshAgent.speed = speed;
        }
    }

    private void OnEnable()
    {
        // Ensure we have a player reference
        if (player == null)
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) player = go.transform;
        }

        SpawnOutsideCamera();
        StartChasing();
    }

    private void SpawnOutsideCamera()
    {
        Camera cam = Camera.main;
        Vector3 spawnPos = transform.position;

        if (cam != null)
        {
            // For orthographic camera we compute the diagonal half-size and ensure the spawn is outside it.
            if (cam.orthographic)
            {
                float halfH = cam.orthographicSize;
                float halfW = halfH * cam.aspect;
                float diag = Mathf.Sqrt(halfH * halfH + halfW * halfW);
                float radius = Mathf.Max(spawnRadius, diag + spawnMargin);
                Vector2 d = Random.insideUnitCircle.normalized;
                // Spawn in camera XY-plane; keep enemy Z as original
                spawnPos = cam.transform.position + cam.transform.TransformDirection(new Vector3(d.x, d.y, 0f)) * radius;
                spawnPos.z = transform.position.z;
            }
            else
            {
                // Perspective: place on a circle in camera XY plane around camera position
                Vector2 d = Random.insideUnitCircle.normalized;
                spawnPos = cam.transform.position + new Vector3(d.x, d.y, 0f) * spawnRadius;
                spawnPos.z = transform.position.z;
            }
        }
        else if (player != null)
        {
            // No camera found: spawn around player
            Vector2 d = Random.insideUnitCircle.normalized;
            spawnPos = player.position + new Vector3(d.x, d.y, 0f) * spawnRadius;
            spawnPos.z = transform.position.z;
        }

        transform.position = spawnPos;
    }

    private void StartChasing()
    {
        state = State.Chasing;
        if (navMeshAgent != null && useNavMeshAgent)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.speed = speed;
            if (player != null) navMeshAgent.SetDestination(player.position);
        }
    }

    private void Update()
    {
        if (state != State.Chasing) return;
        if (player == null) return;

        if (navMeshAgent != null && useNavMeshAgent)
        {
            // Keep updating destination for moving player
            navMeshAgent.speed = speed;
            navMeshAgent.SetDestination(player.position);
            // Optional: smooth facing for 2D sprites
            Vector3 toTarget = player.position - transform.position;
            if (toTarget.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0f, 0f, angle - 90f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }
        else
        {
            // Direct, smooth movement towards the player
            Vector3 target = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

            Vector3 toTarget = target - transform.position;
            if (toTarget.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
                Quaternion targetRot = Quaternion.Euler(0f, 0f, angle - 90f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
            }
        }
    }
}
