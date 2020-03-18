using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public Transform target;

    public float speed = 10f;
    public float damage = 5f;

    void Update()
    {
        //check if enemy exist
        if(target != null){
            // move to targe pos
            transform.position = Vector3.MoveTowards(transform.position, target.position,speed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    void OnTriggerEnter(Collider other)
    {
        GameObject obj = other.gameObject;
        //check if enemy
        if(obj.tag == "Enemy")
        {
            obj.SendMessage("Hurt",damage);
            Destroy(gameObject);
        }
        else if(obj.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
