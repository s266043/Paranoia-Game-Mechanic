using UnityEngine;
using UnityEngine.UI;
public class HeartRateManager : MonoBehaviour
{
    [Header("Settings")]
    public Slider heartSlider;
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float decaySpeed = 8f;
    private float currentRate;
    private bool inDangerZone = false;
    void Start()
    {
        currentRate = baseRate;
        heartSlider.value = currentRate;
    }
    void Update()
    {
        if (!inDangerZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decaySpeed * Time.deltaTime);
        heartSlider.value = currentRate;
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