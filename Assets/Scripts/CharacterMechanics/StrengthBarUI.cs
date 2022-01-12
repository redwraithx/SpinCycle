using UnityEngine;
using UnityEngine.UI;

public class StrengthBarUI : MonoBehaviour
{
    public Slider strengthBarSlider = null;
    private bool isIncreasing;
    private bool isStrengthBarTimeEnable = false;
    private bool canGainStrength = false;
    private bool isStrengthBarRegenTimeEnable = false;
    public float initialDelayTime = 1f;
    public float delayTimer = 0f;

    private void Awake()
    {
        if (!strengthBarSlider)
            strengthBarSlider = GetComponentInChildren<Slider>();

        delayTimer = initialDelayTime;
    }

    private void Update()
    {
        //if(isIncreasing)
        //{
        //    strengthBarSlider.value += Time.deltaTime;

        //    if (strengthBarSlider.value == strengthBarSlider.maxValue)
        //        isIncreasing = false;
        //}
        //else
        //{
        //    strengthBarSlider.value -= Time.deltaTime;

        //    if (strengthBarSlider.value == strengthBarSlider.minValue)
        //        isIncreasing = true;
        //}

        if (strengthBarSlider.value == strengthBarSlider.maxValue && Input.GetMouseButtonDown(0))
        {
            strengthBarSlider.value = strengthBarSlider.minValue;

            isStrengthBarTimeEnable = true;
        }

        if (isStrengthBarRegenTimeEnable)
        {
            delayTimer -= Time.deltaTime;

            if (delayTimer <= 0f)
            {
                canGainStrength = true;
                delayTimer = initialDelayTime;
                isStrengthBarRegenTimeEnable = false;
            }
        }

        if (canGainStrength)
        {
            strengthBarSlider.value += Time.deltaTime;

            if (strengthBarSlider.value >= strengthBarSlider.maxValue)
            {
                canGainStrength = false;
            }
        }

        if (isStrengthBarTimeEnable)
        {
            if (Input.GetMouseButtonUp(0))
            {
                isStrengthBarRegenTimeEnable = true;
                isStrengthBarTimeEnable = true;
            }
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Player"))
    //    {
    //    }
    //}
}