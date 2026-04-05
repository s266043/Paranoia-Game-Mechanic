using UnityEngine; // UnityEngine namespace for MonoBehaviour and related classes

public class UMeter : MonoBehaviour // UMeter class inherits from MonoBehaviour, allowing it to be attached to GameObjects in Unity
{
    public float maxRadius = 8f; // Maximum radius within which the meter will affect the player
    public float maxSpikePerSecond = 15f; // Maximum rate at which the meter can increase the player's rate per second
    public float falloffCurve = 2f; // EDetermining how the effect diminishes with distance
    public LayerMask wallLayer; // Layer mask to determine which layers are considered walls for line of sight checks
    private UManager playerManager; // Reference to the player's UManager component, which will be used to increase the player's rate

    void OnTriggerStay(Collider other) // Called every frame while another collider is within the trigger collider attached to the GameObject
    {
        if (!other.CompareTag("Player")) return; // If the other collider does not have the "Player" tag, exit the method
        if (playerManager == null) // If the playerManager reference is null, attempt to get the UManager component from the other collider
            playerManager = other.GetComponent<UManager>(); // Get the UManager component from the player collider
        float distance = Vector3.Distance(transform.position, other.transform.position); // Calculate the distance between the meter's position and the player's position
        if (distance >= maxRadius) return; // If the distance is greater than or equal to the maximum radius, exit the method
        if (Physics.Linecast(transform.position, other.transform.position, wallLayer)) // Perform a linecast to check if there is a wall between the meter and the player
            return; // If there is a wall in the way, exit the method
        float closeness = Mathf.Pow(1f - (distance / maxRadius), falloffCurve); // Calculate how close the player is to the meter, with a falloff based on distance
        float spike = closeness * maxSpikePerSecond * Time.deltaTime; // Calculate the spike to increase the player's rate based on closeness, maximum spike per second, and time
        playerManager.IncreaseRate(spike); // Call the IncreaseRate method on the player's UManager to increase the player's rate by the calculated spike
    }

    void OnTriggerExit(Collider other) // Called when another collider exits the trigger collider attached to the GameObject
    {
        if (other.CompareTag("Player") && playerManager != null) // If the other collider has the "Player" tag and the playerManager reference is not null
            playerManager.ExitZone(); // Call the ExitZone method on the player's UManager to handle the player exiting the meter's area of effect
    }
}