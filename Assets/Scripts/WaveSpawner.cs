using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public GameObject bugPrefab;
    public Transform spawnPoint;
    public float timeBetweenWaves = 10f;
    public int bugsPerWave = 3;

    private int waveNumber = 0;

    void Start()
    {
        // Start spawning using coroutine - as taught in slides
        StartCoroutine(SpawnWaves());
    }

    // Coroutine for wave spawning - as taught in slides
    IEnumerator SpawnWaves()
    {
        // Wait before first wave
        yield return new WaitForSeconds(2f);

        while (true)
        {
            waveNumber++;
            if (GameManager.instance != null)
                GameManager.instance.UpdateWave(waveNumber);
            Debug.Log("Wave " + waveNumber + " starting!");

            // Show educational message
                WaveMessage waveMsg = FindObjectOfType<WaveMessage>();
                if (waveMsg != null)
                    yield return StartCoroutine(waveMsg.ShowWaveMessage(waveNumber));


            // Spawn bugs for this wave
            yield return StartCoroutine(SpawnBugs());

            // Increase difficulty each wave
            bugsPerWave += 1;

            // Wait before next wave
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnBugs()
    {
        for (int i = 0; i < bugsPerWave; i++)
        {
            // Spawn bug at spawn point with random offset
            Vector3 spawnPos = spawnPoint.position + 
                new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-2f, 2f));
            
            // Instantiate the bug prefab - as taught in slides
            Instantiate(bugPrefab, spawnPos, Quaternion.identity);

            // Wait between each bug spawn
            yield return new WaitForSeconds(0.8f);
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }
}