using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public float health = 30f;
    float currentHealth;

    public GameObject healthBarPrefab;
    GameObject healthBar;

    public float worth = 4f;

    public Transform currentWaypoint;
    public float moveSpeed = 2f;
    

    void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0,0.5f,0.25f),Quaternion.identity, transform);
    }

    void Update()
    {
        //move enemy to waypoint
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);

        //check for arive
        if(transform.position.Equals(currentWaypoint.position))
        {
            // check for next waypoint
            if(currentWaypoint.GetComponent<Waypoint>().nextWaypoint != null)
            {
                currentWaypoint = currentWaypoint.GetComponent<Waypoint>().nextWaypoint;
            }
        }
    }

    public void Hurt(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Money.Amount += worth;
            Destroy(gameObject);
        }

        // modify green scale to reflect dmg
        Transform pivot = healthBar.transform.Find("HealthyPivot");
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health,0,1);
        pivot.localScale = scale;
    }

}
