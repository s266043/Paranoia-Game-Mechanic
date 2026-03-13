using UnityEngine;
public class Paranoia : MonoBehaviour
{
    public float heartRateSpikePerSecond = 25f;

    void OnTriggerStay(Collider other)
    {
        if (other.GetComponentInParent<Manager>() != null)
        {
            Debug.Log("Zone sees Player inside - spiking heart rate");
        }
    }
}