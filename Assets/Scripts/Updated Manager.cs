using UnityEngine;
using UnityEngine.UI;

public class UManager : MonoBehaviour
{
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float decaySpeed = 3f;
    public float minimumFill = 0.12f;
    public Image fillImage;
    public Image backgroundImage;
    public AudioSource heartbeatSource;
    public float minVolume = 0.3f;
    public float maxVolume = 1.0f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.8f;
    private float currentRate;
    private bool inZone = false;

    void Start()
    {
        currentRate = baseRate;
        if (heartbeatSource != null)
        {
            heartbeatSource.loop = true;
            heartbeatSource.Play();
        }
    }

    void Update()
    {
        if (!inZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime);
        UpdateBarVisual();
        UpdateHeartbeatAudio();
    }

    void UpdateBarVisual()
    {
        if (fillImage == null) return;

        float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate));
        float finalFill = Mathf.Max(normalized, minimumFill);
        fillImage.fillAmount = finalFill;
        fillImage.color = Color.Lerp(Color.green, Color.red, normalized);
        if (backgroundImage != null)
            backgroundImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
    }

    void UpdateHeartbeatAudio()
    {
        if (heartbeatSource == null) return;
        float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate));
        heartbeatSource.volume = Mathf.Lerp(minVolume, maxVolume, normalized);
        heartbeatSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalized);
    }

    public void IncreaseRate(float amount)
    {
        currentRate = Mathf.Min(currentRate + amount, maxRate);
        inZone = true;
    }

    public void ExitZone()
    {
        inZone = false;
    }
}