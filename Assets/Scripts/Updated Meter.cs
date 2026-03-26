using UnityEngine;

public class StressZone : MonoBehaviour
{
    public float maxRadius = 8f;
    public float maxSpikePerSecond = 45f;
    public float falloffCurve = 2f;
    public LayerMask wallLayer;
    private UManager playerManager;

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (playerManager == null)
            playerManager = other.GetComponent<UManager>();
        float distance = Vector3.Distance(transform.position, other.transform.position);
        if (distance >= maxRadius) return;
        if (Physics.Linecast(transform.position, other.transform.position, wallLayer))
            return;
        float closeness = Mathf.Pow(1f - (distance / maxRadius), falloffCurve);
        float spike = closeness * maxSpikePerSecond * Time.deltaTime;
        playerManager.IncreaseRate(spike);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && playerManager != null)
            playerManager.ExitZone();
    }
}