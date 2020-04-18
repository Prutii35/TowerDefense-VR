using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range = 3f;
    public float fireRate = 1f;

    public GameObject bulletPrefab;
    public Transform barrelExit;

    Transform target;
    float fireCounter = 0;

    bool stillInRange = false;

    void Update() 
    {
        
        findNextTarget();
        
        if (target != null)
        {
            aimAtTarget();
            shoot();

            //check who is in range
            int layerMask = 1 << 9;
            Collider[] enemies = Physics.OverlapSphere(transform.position, range, layerMask);

            if (enemies.Length > 0)
            {
                stillInRange = false;
                foreach (Collider enemy in enemies)
                {
                    if (target == enemy) stillInRange = true;
                }

                if (stillInRange == false)
                {
                    target = null;
                }
            }
        }
    }

    void findNextTarget()
    {
        //check who is in range
        int layerMask = 1 << 9;
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, layerMask);

        //check if we are in range
        if(enemies.Length > 0)
        {
            //asume the first enemy is closest
            target = enemies[0].gameObject.transform;

            //loop through all the enemise
            foreach(Collider enemy in enemies)
            {
                //calculate the distance of the enemy to the tower
                float distance = Vector3.Distance(transform.position,enemy.transform.position);

                //see who is closer
                if(distance < Vector3.Distance(transform.position, target.position))
                {
                    target = enemy.gameObject.transform;
                }
            }
        }
        else 
        {// if no enemies
            target = null;
        }
    }

    void aimAtTarget()
    {
        // look at our target
        Vector3 lookPos = target.position - transform.position;
        lookPos.y = 0;

        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    void shoot()
    {
        if(fireCounter <= 0)
        {
            GameObject newBullet =  Instantiate(bulletPrefab,barrelExit.position,Quaternion.identity);
            newBullet.GetComponent<Bullet>().target = target;
            fireCounter = fireRate;
        }
        else
        {
            fireCounter -= Time.deltaTime;
        }
    }

}
