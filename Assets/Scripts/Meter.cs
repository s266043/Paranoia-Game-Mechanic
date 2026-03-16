using UnityEngine;
using UnityEngine.UI;
public class HeartRateManager : MonoBehaviour
{
    public Slider hrSlider;
    public float baseRate = 80f;
    public float maxRate = 180f;
    public float decreaseSpeed = 8f;
    private float currentRate;
    private bool inDangerZone = false;
    void Start()
    {
        currentRate = baseRate;
        hrSlider.value = currentRate;
    }
    void Update()
    {
        if (!inDangerZone)
            currentRate = Mathf.Lerp(currentRate, baseRate, decreaseSpeed * Time.deltaTime);
        hrSlider.value = currentRate;
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