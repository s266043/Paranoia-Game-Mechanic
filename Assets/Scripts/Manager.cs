using UnityEngine;
using UnityEngine.UI;
public class Manager : MonoBehaviour
{
    [Header("Heart Rate Bar")]
    public Image fillImage;
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float decaySpeed = 8f;
    private float currentRate;
    private bool inDangerZone = false;
    void Start()
    {
        currentRate = baseRate;

        if (fillImage == null)
        {
            Debug.LogError("Manager: Fill Image is NULL! Drag the 'Fill' child into the slot below.");
            return;
        }
        UpdateBarVisual();
        Debug.Log("Manager started - bar ready");
    }
    void Update()
    {
        if (fillImage == null) return;

        if (!inDangerZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime);

        UpdateBarVisual();
    }
    void UpdateBarVisual()
    {
        if (fillImage == null) return;
        float normalized = Mathf.Clamp01((currentRate - baseRate) / (maxRate - baseRate));
        fillImage.fillAmount = normalized;
        fillImage.color = Color.Lerp(Color.green, Color.red, normalized);
    }
    public void IncreaseRate(float amount)
    {
        currentRate = Mathf.Min(currentRate + amount, maxRate);
        inDangerZone = true;
    }

    public void ExitDangerZone()
    {
        inDangerZone = false;
    }
}