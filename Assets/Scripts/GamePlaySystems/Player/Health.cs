

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RedWraith.Player
{

    public class Health : MonoBehaviour
    {
        [SerializeField] private GameObject healthBarContainer;
        [SerializeField] private HealthBarSlider healthBarVisuals;

        [SerializeField] private int currentHealth;
        [SerializeField] private int maxHealth;

        [SerializeField] private Animator anim;
        private bool isPlayerDead = false;

        private void Start()
        {
            CurrentHealth = MaxHealth;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                if (isPlayerDead)
                    return;

                currentHealth = value;

                healthBarVisuals.SetHealth(CurrentHealth);
            }
        }

        public int MaxHealth
        {
            get => maxHealth;
            private set => maxHealth = value;
        }


        public void TakeDamage(int damage)
        {
            if (isPlayerDead)
                return;


            CurrentHealth -= damage;
        }


        public void HealDamage(int healingValue)
        {
            if (isPlayerDead)
                return;

            CurrentHealth += healingValue;

            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }


        }


        private void Update()
        {
            // if (isPlayerDead)
            //     return;

            if (CurrentHealth <= 0 && !isPlayerDead)
            {
                isPlayerDead = true;

                StartCoroutine(GameOver());
            }




            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                CurrentHealth = 0;
            }
        }


        IEnumerator GameOver()
        {
            yield return new WaitForSeconds(4f);

            SceneManager.LoadScene("GameOver");
        }

    }

}
