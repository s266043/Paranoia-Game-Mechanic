using UnityEngine;
public class Paranoia : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Manager hrManager;
    [Header("Spawn & Chase")]
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float groundY = 11f;
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip paranoiaSFX;
    public float sfxInterval = 25f;
    public float maxDistance = 25f;
    [Header("Effects")]
    public float heartSpikePerSecondOnCollision = 40f;
    private float sfxTimer;
    private int spawnIndex;
    void Start()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        if (waypoints.Length > 0)
        {
            spawnIndex = Random.Range(0, waypoints.Length);
            Vector3 spawnPos = waypoints[spawnIndex].position;
            spawnPos.y = groundY;
            transform.position = spawnPos;
            Debug.Log($"Paranoia spawned at random Waypoint {spawnIndex} (Y=11)");
        }
        sfxTimer = sfxInterval;
    }
    void Update()
    {
        if (player == null) return;

        ChasePlayer();
        UpdateSoundVolume();
        PlaySFXTimer();
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
        audioSource.volume = closeness * 1.2f;
    }
    void PlaySFXTimer()
    {
        sfxTimer -= Time.deltaTime;
        if (sfxTimer <= 0f)
        {
            if (paranoiaSFX != null)
            {
                audioSource.PlayOneShot(paranoiaSFX);
                Debug.Log("Paranoia SFX played!");
            }
            sfxTimer = sfxInterval;
        }
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