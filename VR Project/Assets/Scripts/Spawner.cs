using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public float initialWaitTime = 5f;
    public float waveWaitTime = 5f;

    public GameObject[] enemyPrefab;
    public Transform firstWaypoint;

    public Text waveTest;

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
            //increase wave counter
            currentWave++;
            waveTest.text = "Wave " + (currentWave + 1) + "/" + waves.Count;

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

            //wait for the next enemy
            yield return new WaitForSeconds(0.5f);
        }
    }
}
