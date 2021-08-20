
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerHealthBar : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private float LerpTimer = 0f;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar = null;
    public Image backHealthBar = null;


    private void Start()
    {
        health = maxHealth;
        
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthUI();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(Random.Range(5, 10));
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            RestoreHealth(Random.Range(5, 10));
        }
    }

    public void UpdateHealthUI()
    {
        Debug.Log(health);

        float fillFront = frontHealthBar.fillAmount;
        float fillBack = backHealthBar.fillAmount;
        float healthFraction = health / maxHealth;

        if (fillBack > healthFraction)
        {
            frontHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;

            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer / chipSpeed;
            
            // its an easing effect
            percentComplete = percentComplete * percentComplete;

            backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = healthFraction;

            LerpTimer += Time.deltaTime;
            float percentComplete = LerpTimer / chipSpeed;

            // its an easing effect
            percentComplete = percentComplete * percentComplete;
            
            frontHealthBar.fillAmount = Mathf.Lerp(fillFront, healthFraction, percentComplete);
        }
        
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        LerpTimer = 0f;
        
    }

    public void RestoreHealth(float healAmount)
    {
        health += healAmount;

        LerpTimer = 0f;
    }
}
