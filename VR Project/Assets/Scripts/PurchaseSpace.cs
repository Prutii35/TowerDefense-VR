using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseSpace : MonoBehaviour
{

    public Text moneyText;

    public GameObject basicTowerPrefab;
    GameObject boughtTower;

    void Update()
    {
        moneyText.text = "Money $" + Money.Amount;

        if(boughtTower != null)
        {
            MovePurchasedTower();
            checkForWall();
        }
    }

    void MovePurchasedTower()
    {
        //move the tower in front of camera
        boughtTower.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 5;
    }

    public void BuyBasicTower()
    {
        Debug.Log("aloooooooooooo");
        //check if u have enough money, check if u bought a tower
        if(Money.Amount < 40 || boughtTower != null){
            return;
        }

        // spawn a tower
        boughtTower =  Instantiate(basicTowerPrefab, transform.position, Quaternion.identity);

        // make tower transparent
        Color color =  boughtTower.GetComponent<Renderer>().material.color;
        color.a = 0.5f;
        boughtTower.GetComponent<Renderer>().material.color = color;

        //take away moeny
        Money.Amount -= 40;

    }

    void checkForWall()
    {
        //create our raycast
        Ray raycast = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
        RaycastHit hit;

        //debug
        Debug.DrawRay(raycast.origin,raycast.direction * 100);

        //check if raycast hits
        if(Physics.Raycast(raycast,out hit))
        {
            //did we hit a wall
            if(hit.collider.gameObject.tag == "Wall")
            {
                //put the tower on the wall
                boughtTower.transform.position = hit.collider.gameObject.transform.position + new Vector3(0,0.75f,0);


                //check if u click the button

                if(Input.GetMouseButtonDown(0)){

                    //take away wall tag
                    hit.collider.gameObject.tag = "Untagged";

                    // make tower transparent
                    Color color =  boughtTower.GetComponent<Renderer>().material.color;
                    color.a = 1f;
                    boughtTower.GetComponent<Renderer>().material.color = color;

                    //enable script
                    boughtTower.GetComponent<Tower>().enabled = true;

                    boughtTower = null;

                }

              
            }


        }
    }
}
