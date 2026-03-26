using UnityEngine;
using UnityEngine.UI;

public class UManager : MonoBehaviour
{
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float decaySpeed = 6f;
    public float minimumFill = 0.12f;
    public Image fillImage;
    public Image backgroundImage;
    private float currentRate;
    private bool inZone = false;

    void Start()
    {
        currentRate = baseRate;
    }

    void Update()
    {
        if (!inZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime);
        UpdateBarVisual();
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