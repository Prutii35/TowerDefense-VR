using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public float range;
    public float fireRate = 1f;

    public GameObject bulletPrefab;
    public Transform barrelExit;

    Transform target;
    float fireCounter = 0;

    bool stillInRange = false;

    public int segments = 100;
    LineRenderer line;

    void Awake()
    {
        line = gameObject.GetComponent<LineRenderer>();

        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;
        CreatePoints();

    }

    void Update() 
    {
        
        findNextTarget();
        

        if (target != null)
        {
            stillInRange = false;
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

    void CreatePoints()
    {
        float x;
        float y;
        float z;

        float angle = 20f;

        for (int i = 0; i < (segments + 1); i++)
        {
            if(gameObject.tag == "ScoutTower")
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * (range * 3.5f + 3);
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * (range * 3.5f + 3);
            }
            else if(gameObject.tag == "BigCannon")
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * (range + 4);
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * (range + 4);
            }
            else if (gameObject.tag == "SmallCannon")
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * (range + 2.5f);
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * (range + 2.5f);
            }
            else // Cannon Tower
            {
                x = Mathf.Sin(Mathf.Deg2Rad * angle) * (range + 12);
                y = Mathf.Cos(Mathf.Deg2Rad * angle) * (range + 12);
            }

            Vector3 vector = Quaternion.Euler(90, 0, 0) * new Vector3(x, y, 0) ;
            line.SetPosition(i, vector);

            angle += (360f / segments);
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
