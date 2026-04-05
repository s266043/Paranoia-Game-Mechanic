using UnityEngine; // Import the UnityEngine namespace to access Unity's core classes and functions
using UnityEngine.UI; // Import the UnityEngine.UI namespace to access UI components like Image and Text
using TMPro; // Import the TMPro namespace to access TextMeshPro components for better text rendering

public class UManager : MonoBehaviour // This class manages the heart rate bar, visual effects, and heartbeat audio based on the player's heart rate
{
    public float baseRate = 80f; // The normal heart rate when the player is not in a stressful situation
    public float maxRate = 180f; // The maximum heart rate the player can reach in a stressful situation
    public float maxSpikePerSecond = 15f; // The maximum increase in heart rate per second when the player is in a stressful situation
    public float decaySpeed = 3f; // The speed at which the heart rate returns to the base rate when the player is not in a stressful situation
    public float minimumFill = 0.12f; // The minimum fill amount for the heart rate bar to ensure it is always visible, even at low heart rates
    public Image fillImage; // Reference to the component that represents the fill of the heart rate bar
    public Image backgroundImage; // Reference to the component that represents the background of the heart rate bar
    public TextMeshProUGUI bpmText; // Reference to the component that displays the current heart rate in Beats Per Minute
    public Image[] redGlows; // Array of components that represent the red glow effect around the heart rate bar when the player is in a stressful situation
    public float glowIntensity = 0.35f; // The intensity of the red glow effect, which can be adjusted to make it more or less noticeable
    public AudioSource heartbeatSource; // Reference to the component that plays the heartbeat sound, which will increase in volume and pitch as the heart rate increases
    public float minVolume = 0.3f; // The minimum volume of the heartbeat sound when the player is at the base heart rate
    public float maxVolume = 1.0f; // The maximum volume of the heartbeat sound when the player reaches the maximum heart rate
    public float minPitch = 0.8f; // The minimum pitch of the heartbeat sound when the player is at the base heart rate
    public float maxPitch = 1.8f; // The maximum pitch of the heartbeat sound when the player reaches the maximum heart rate
    private float currentRate; // The current heart rate of the player, which will be updated based on the player's actions and the passage of time
    private bool inZone = false; // A flag to indicate whether the player is currently in a stressful situation that increases heart rate

    void Start() // Initialisation
    {
        currentRate = baseRate; // Set the current heart rate to the base rate at the start of the game

        if (heartbeatSource != null) // If the heartbeat audio source is assigned, set it to loop and start playing with an initial volume of 0
        {
            heartbeatSource.loop = true; // Set the heartbeat audio to loop so it continues playing as the heart rate changes
            heartbeatSource.volume = 0f; // Start with the heartbeat sound muted, it will increase in volume as the heart rate increases
            heartbeatSource.Play(); // Start playing the heartbeat sound immediately, it will be adjusted in volume and pitch based on the heart rate
        }
        foreach (Image glow in redGlows) // Initialise the red glow images to be fully transparent at the start of the game
        {
            if (glow != null) // Check if the glow image reference is assigned before trying to set its colour
                glow.color = new Color(1, 0, 0, 0); // Set the red glow to be fully transparent at the start, it will become more visible as the heart rate increases
        }
    }

    void Update() // Called once a frame
    {
        if (!inZone) // If the player is not in a stressful situation, gradually decay the heart rate back towards the base rate
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime); // Smoothly transition the current heart rate back to the base rate over time
        UpdateBarVisual(); // Update the visual representation of the heart rate bar based on the current heart rate
        UpdateGlow(); // Update the red glow effect around the heart rate bar based on whether the player is in a stressful situation and how high the heart rate is
        UpdateHeartbeatAudio(); // Update the heartbeat audio's volume and pitch based on the current heart rate, making it more intense as the heart rate increases
    }

    void UpdateBarVisual() // Method to update the heart rate bar's fill amount and colour based on the current heart rate
    {
        if (fillImage == null) return; // If the fill image reference is not assigned, exit the method to avoid errors
        float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate)); // Normalise the current heart rate to a value between 0 and 1 based on the base and maximum rates
        float finalFill = Mathf.Max(normalized, minimumFill); // Ensure the fill amount is never below the minimum fill to keep the bar visible, even at low heart rates
        fillImage.fillAmount = finalFill; // Set the fill amount of the heart rate bar based on the normalised heart rate, which will visually represent how high the heart rate is
        fillImage.color = Color.Lerp(Color.green, Color.red, normalized); // Change the colour of the fill from green to red as the heart rate increases, providing a visual cue of the player's stress level
        if (bpmText != null)
            bpmText.text = Mathf.RoundToInt(currentRate).ToString() + " BPM"; // Update the BPM text to display the current heart rate rounded to the nearest integer, giving the player a clear numerical representation of their heart rate
        if (backgroundImage != null)
            backgroundImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f); // Set the background of the heart rate bar to a dark, semi transparent colour to ensure the fill and text are easily visible against it, regardless of the current heart rate
    }

    void UpdateGlow() // Method to update the red glow effect around the heart rate bar based on whether the player is in a stressful situation and how high the heart rate is
    {
        if (redGlows == null || redGlows.Length == 0) return; // If there are no red glow images assigned, exit the method to avoid errors
        if (inZone) // If the player is in a stressful situation, create a pulsing red glow effect that becomes more intense as the heart rate increases
        {
            float pulse = Mathf.Sin(Time.time * 8f) * 0.5f + 0.5f; // Create a pulsing effect
            float alpha = pulse * glowIntensity; // Scale the pulse by the glow intensity to control how strong the glow effect is, making it more noticeable as the heart rate increases

            foreach (Image glow in redGlows) // Loop through each red glow image and set its colour to a red with the calculated alpha value, creating a pulsing glow effect around the heart rate bar when the player is in a stressful situation
            {
                if (glow != null) // Check if the glow image reference is assigned before trying to set its colour
                    glow.color = new Color(1f, 0.15f, 0.15f, alpha); // Set the red glow colour with the calculated alpha to create a pulsing effect, making it more intense as the heart rate increases and providing a visual cue of the player's stress level
            }
        }
        else // If the player is not in a stressful situation, gradually fade out the red glow effect to make it less noticeable and eventually disappear, indicating that the player has calmed down
        {
            foreach (Image glow in redGlows) // Loop through each red glow image and gradually fade it out by lerping its colour towards fully transparent, creating a smooth transition as the player exits the stressful situation
            {
                if (glow != null) // Check if the glow image reference is assigned before trying to set its colour
                    glow.color = Color.Lerp(glow.color, new Color(1, 0, 0, 0), Time.deltaTime * 5f); // Gradually fade the red glow to fully transparent over time, making it less noticeable and eventually disappear as the player calms down, providing a visual cue that the player is no longer in a stressful situation
            }
        }
    }

    void UpdateHeartbeatAudio() // Method to update the heartbeat audio's volume and pitch based on the current heart rate, making it more intense as the heart rate increases
    {
        if (heartbeatSource == null) return; // If the heartbeat audio source reference is not assigned, exit the method to avoid errors
        if (inZone) // If the player is in a stressful situation, increase the volume and pitch of the heartbeat sound based on how high the heart rate is, creating a more intense audio experience that reflects the player's stress level
        {
            float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate)); // Normalise the current heart rate to a value between 0 and 1 based on the base and maximum rates, which will be used to determine how much to increase the volume and pitch of the heartbeat sound
            heartbeatSource.volume = Mathf.Lerp(minVolume, maxVolume, normalized); // Increase the volume of the heartbeat sound from the minimum to the maximum based on the normalised heart rate, making it louder as the heart rate increases and providing an audio cue of the player's stress level
            heartbeatSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalized); // Increase the pitch of the heartbeat sound from the minimum to the maximum based on the normalised heart rate, making it higher as the heart rate increases and providing an audio cue of the player's stress level
        }
        else // If the player is not in a stressful situation, gradually decrease the volume of the heartbeat sound to make it less intense and eventually silent, indicating that the player has calmed down
        {
            heartbeatSource.volume = Mathf.Lerp(heartbeatSource.volume, 0f, Time.deltaTime * 4f); // Gradually decrease the volume of the heartbeat sound to 0 over time, making it less intense and eventually silent as the player calms down, providing an audio cue that the player is no longer in a stressful situation
        }
    }

    public void IncreaseRate(float amount) // Method to increase the current heart rate by a specified amount, clamping it to the maximum heart rate, and setting the inZone flag to true to indicate that the player is in a stressful situation
    {
        currentRate = Mathf.Min(currentRate + amount, maxRate); // Increase the current heart rate by the specified amount, but ensure it does not exceed the maximum heart rate to maintain a realistic range for the player's heart rate
        inZone = true; // Set the inZone flag to true to indicate that the player is currently in a stressful situation that is increasing their heart rate, which will trigger the visual and audio effects associated with being in a stressful situation
    }

    public void ExitZone() // Method to set the inZone flag to false, indicating that the player has exited the stressful situation and allowing the heart rate to start decaying back towards the base rate, while also triggering the visual and audio effects associated with calming down
    {
        inZone = false; // Set the inZone flag to false to indicate that the player has exited the stressful situation, which will allow the heart rate to start decaying back towards the base rate and trigger the visual and audio effects associated with calming down, providing feedback to the player that they are no longer in a stressful situation
    }
}