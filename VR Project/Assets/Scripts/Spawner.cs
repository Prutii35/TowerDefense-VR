using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Spawner : MonoBehaviour
{
    public float initialWaitTime = 5f;
    public float waveWaitTime = 5f;

    public GameObject[] enemyPrefab;

    public Transform firstWaypoint;
    public Transform pathStart1;
    public Transform pathStart2;
    public Transform pathStart3;
    public Transform pathStart4;
    public Transform pathStart5;


    float pPath1 = 0.20f;
    float pPath2 = 0.20f;
    float pPath3 = 0.20f;
    float pPath4 = 0.20f;
    float pPath5 = 0.20f;

    int randomPath;

    public TextMeshPro waveText;

    float extraSpeed = 0f;
    float extraHealth = 0f;

    public string nextLvl = "level #";

    List<Wave> waves = new List<Wave>();
    int currentWave = -1;

    void Awake()
    {
        getWaves();
        StartCoroutine(WaveLoop());
    }

    void getWaves()
    {
        int child = 0;

        //loop through each child obj in spawner
        while(child < transform.childCount)
        {
            //get wave script and move to the next
            waves.Add(transform.GetChild(child).GetComponent<Wave>());
            child++;
        }
    }

    IEnumerator WaveLoop()
    {
        // pause before we start for setup
        yield return new WaitForSeconds(initialWaitTime);

        //loop  through each wave
        while(currentWave < waves.Count)
        {

            randomPath = Random.Range(0, 101);

            float interval1 = pPath1 * 100;
            float interval2 = interval1 + pPath2 * 100;
            float interval3 = interval2 + pPath3 * 100;
            float interval4 = interval3 + pPath4 * 100;

            Debug.Log("Random este " + randomPath);
            Debug.Log("0 - " + interval1 + " - " + interval2 + " - " + interval3 + " - " + interval4 + " - 100");

            if (randomPath < interval1) //path 1
            {
                Debug.Log("Path 1.");
                firstWaypoint = pathStart1;

                if (pPath1 < 0.20f)
                {
                    float split = pPath1 / 4;

                    pPath1 = 0f;
                    pPath2 += split;
                    pPath3 += split;
                    pPath4 += split;
                    pPath5 += split;

                    Debug.Log("Cresc celelalte path-uri cu split = " + split);
                }
                else
                {
                    pPath1 -= 0.20f;
                    pPath2 += 0.05f;
                    pPath3 += 0.05f;
                    pPath4 += 0.05f;
                    pPath5 += 0.05f;
                }
                
            }
            else if(randomPath >= interval1 && randomPath < interval2) //path 2
            {
                

                Debug.Log("Path 2.");
                firstWaypoint = pathStart2;

                if (pPath2 < 0.20f)
                {
                    float split = pPath2 / 4;

                    pPath2 = 0f;
                    pPath1 += split;
                    pPath3 += split;
                    pPath4 += split;
                    pPath5 += split;

                    Debug.Log("Cresc celelalte path-uri cu split = " + split);
                }
                else
                {
                    pPath2 -= 0.20f;
                    pPath1 += 0.05f;
                    pPath3 += 0.05f;
                    pPath4 += 0.05f;
                    pPath5 += 0.05f;
                }
            }
            else if(randomPath >= interval2 && randomPath < interval3) //path 3
            {
                Debug.Log("Path 3.");
                firstWaypoint = pathStart3;

                if (pPath3 < 0.20f)
                {
                    float split = pPath3 / 4;

                    pPath3 = 0f;
                    pPath2 += split;
                    pPath1 += split;
                    pPath4 += split;
                    pPath5 += split;

                    Debug.Log("Cresc celelalte path-uri cu split = " + split);
                }
                else
                {
                    pPath3 -= 0.20f;
                    pPath2 += 0.05f;
                    pPath1 += 0.05f;
                    pPath4 += 0.05f;
                    pPath5 += 0.05f;
                }
            }
            else if (randomPath >= interval3 && randomPath < interval4)//path 4
            {

                Debug.Log("Path 4.");
                firstWaypoint = pathStart4;

                if (pPath4 < 0.20f)
                {
                    float split = pPath4 / 4;

                    pPath4 = 0f;
                    pPath2 += split;
                    pPath3 += split;
                    pPath1 += split;
                    pPath5 += split;

                    Debug.Log("Cresc celelalte path-uri cu split = " + split);
                }
                else
                {
                    pPath4 -= 0.20f;
                    pPath2 += 0.05f;
                    pPath3 += 0.05f;
                    pPath1 += 0.05f;
                    pPath5 += 0.05f;
                }
            }
            else //path 5
            {
                Debug.Log("Path 5.");
                firstWaypoint = pathStart5;

                if (pPath5 < 0.20f)
                {
                    float split = pPath4 / 4;

                    pPath5 = 0f;
                    pPath2 += split;
                    pPath3 += split;
                    pPath1 += split;
                    pPath4 += split;

                    Debug.Log("Cresc celelalte path-uri cu split = " + split);
                }
                else
                {
                    pPath5 -= 0.20f;
                    pPath2 += 0.05f;
                    pPath3 += 0.05f;
                    pPath1 += 0.05f;
                    pPath4 += 0.05f;
                }
            }

            //increase wave counter
            currentWave++;

            if(currentWave != 0)
            {
                extraSpeed += 0.1f;
                extraHealth += 3.5f;
                Debug.Log("Upgraded stats.");
            }

            waveText.text = "Wave " + (currentWave + 1) + " / " + waves.Count;

            //start the wave and wait to finish
            yield return StartCoroutine(startNextWave());
        }

        while(GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
        {
             yield return new WaitForSeconds(1f);
        }

        // all enemies are dead and lvl is finished
        SceneManager.LoadScene(nextLvl);
    }

    IEnumerator startNextWave()
    {
        //start spawning waves segments
        foreach(WaveSegment segment in waves[currentWave].PatternToWaveSegments())
        {
            //spawn enemies
            yield return StartCoroutine(SpawnEnemies(segment.spawns));

            //wait at the end of the segment
            yield return new WaitForSeconds(segment.waitTime);
        }

        //we ve finished the wave and wait between the waves
        yield return new WaitForSeconds(waveWaitTime);
    }

    IEnumerator SpawnEnemies(List<int>enemies)
    {
        //loop through each enemy indicator in pattern
        foreach(int enemy in enemies) // indicele enemy este corespunzator din pattern
        {
            //create the enemy and alocate thier first waypoint
            GameObject newEnemy = Instantiate(enemyPrefab[enemy], transform.position, Quaternion.identity);

            newEnemy.GetComponent<Enemy>().currentWaypoint = firstWaypoint;

            newEnemy.GetComponent<Enemy>().moveSpeed = newEnemy.GetComponent<Enemy>().moveSpeed + extraSpeed;
            newEnemy.GetComponent<Enemy>().health = newEnemy.GetComponent<Enemy>().health + extraHealth;
            newEnemy.SendMessage("setHealthBar");

            //Debug.Log("Health : " + newEnemy.GetComponent<Enemy>().health);
            //Debug.Log("Speed : " + newEnemy.GetComponent<Enemy>().moveSpeed);

            //wait for the next enemy
            yield return new WaitForSeconds(0.5f);
        }
    }
}
