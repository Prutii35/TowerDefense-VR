using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThingToProtect : MonoBehaviour
{

    public float health = 100f;
    float currentHealth;

    public RectTransform gameOverPanel;
    public TextMeshPro scoreText;

    public GameObject healthBarPrefab;
    GameObject healthBar;
    void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0,5f,0.1f),Quaternion.identity, transform);
    }
    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;

        if(obj.tag == "Enemy")
        {
            currentHealth -= obj.GetComponent<Enemy>().damage;

            //modify health
            Transform pivot = healthBar.transform.Find("HealthyPivot");
            Vector3 scale = pivot.localScale;
            scale.x = Mathf.Clamp(currentHealth / health,0,1);
            pivot.localScale = scale;

            Destroy(obj);

            //check health
            CheackHealth();
        }
    }

    void CheackHealth()
    {
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
            float high = PlayerPrefs.GetFloat("High Score");


            if (high > Score.Amount)
            {
                scoreText.text = "High Score : " + high + '\n' + '\n' + "You scored " + Score.Amount;
                //You need this to save high score across game sessions
                
            }
            else
            {
                scoreText.text = "New High Score : " + Score.Amount;
                PlayerPrefs.SetFloat("High Score", Score.Amount);
            }

            gameOverPanel.gameObject.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}
