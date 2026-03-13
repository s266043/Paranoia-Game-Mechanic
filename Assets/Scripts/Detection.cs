using UnityEngine;
public class Detection : MonoBehaviour
{
    private Manager manager;
    void Start()
    {
        manager = GetComponent<Manager>();
        Debug.Log("Detection ready on Player root");
    }
    void OnTriggerStay(Collider other)
    {
        Paranoia zone = other.GetComponent<Paranoia>();
        if (zone != null && manager != null)
        {
            manager.IncreaseRate(zone.heartRateSpikePerSecond * Time.deltaTime);
            Debug.Log($"Heart rate spiking +{zone.heartRateSpikePerSecond * Time.deltaTime:F2}");
        }
    }
    void OnTriggerExit(Collider other)
    {
        Paranoia zone = other.GetComponent<Paranoia>();
        if (zone != null && manager != null)
        {
            manager.ExitDangerZone();
            Debug.Log("Left paranoia zone - decaying");
        }
    }
}