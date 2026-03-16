using UnityEngine;
public class Paranoia : MonoBehaviour
{
    public Transform player;
    public Manager hrManager;
    public Transform[] spawnpoints;
    public float moveSpeed = 5f;
    public float groundY = 11f;
    public AudioSource audioSource;
    public float maxDistance = 25f;
    public float heartSpikePerSecondOnCollision = 40f;
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        audioSource.loop = true;
        audioSource.playOnAwake = true;
        audioSource.volume = 0f;
        if (spawnpoints.Length > 0)
        {
            int spawnIndex = Random.Range(0, spawnpoints.Length);
            Vector3 spawnPos = spawnpoints[spawnIndex].position;
            spawnPos.y = groundY;
            transform.position = spawnPos;
        }
    }
    void Update()
    {
        if (player == null) return;
        ChasePlayer();
        UpdateSoundVolume();
    }
    void ChasePlayer()
    {
        Vector3 targetPos = player.position;
        targetPos.y = groundY;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        transform.LookAt(new Vector3(targetPos.x, transform.position.y, targetPos.z));
    }
    void UpdateSoundVolume()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        float closeness = 1f - Mathf.Clamp01(distance / maxDistance);
        audioSource.volume = Mathf.Lerp(0.05f, 1.2f, closeness);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && hrManager != null)
            hrManager.IncreaseRate(heartSpikePerSecondOnCollision * Time.deltaTime);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && hrManager != null)
            hrManager.ExitDangerZone();
    }
}