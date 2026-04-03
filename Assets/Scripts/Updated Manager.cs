using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UManager : MonoBehaviour
{
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float maxSpikePerSecond = 15f;
    public float decaySpeed = 3f;
    public float minimumFill = 0.12f;
    public Image fillImage;
    public Image backgroundImage;
    public TextMeshProUGUI bpmText;
    public Image[] redGlows;
    public float glowIntensity = 0.35f;
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
            heartbeatSource.volume = 0f;
            heartbeatSource.Play();
        }

        foreach (Image glow in redGlows)
        {
            if (glow != null)
                glow.color = new Color(1, 0, 0, 0);
        }
    }

    void Update()
    {
        if (!inZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime);
        UpdateBarVisual();
        UpdateGlow();
        UpdateHeartbeatAudio();
    }

    void UpdateBarVisual()
    {
        if (fillImage == null) return;
        float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate));
        float finalFill = Mathf.Max(normalized, minimumFill);
        fillImage.fillAmount = finalFill;
        fillImage.color = Color.Lerp(Color.green, Color.red, normalized);
        if (bpmText != null)
            bpmText.text = Mathf.RoundToInt(currentRate).ToString() + " BPM";
        if (backgroundImage != null)
            backgroundImage.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
    }

    void UpdateGlow()
    {
        if (redGlows == null || redGlows.Length == 0) return;

        if (inZone)
        {
            float pulse = Mathf.Sin(Time.time * 8f) * 0.5f + 0.5f;
            float alpha = pulse * glowIntensity;

            foreach (Image glow in redGlows)
            {
                if (glow != null)
                    glow.color = new Color(1f, 0.15f, 0.15f, alpha);
            }
        }
        else
        {
            foreach (Image glow in redGlows)
            {
                if (glow != null)
                    glow.color = Color.Lerp(glow.color, new Color(1, 0, 0, 0), Time.deltaTime * 5f);
            }
        }
    }

    void UpdateHeartbeatAudio()
    {
        if (heartbeatSource == null) return;
        if (inZone)
        {
            float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate));
            heartbeatSource.volume = Mathf.Lerp(minVolume, maxVolume, normalized);
            heartbeatSource.pitch = Mathf.Lerp(minPitch, maxPitch, normalized);
        }
        else
        {
            heartbeatSource.volume = Mathf.Lerp(heartbeatSource.volume, 0f, Time.deltaTime * 4f);
        }
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