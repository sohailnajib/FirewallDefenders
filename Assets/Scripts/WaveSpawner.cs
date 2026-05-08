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
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            waveNumber++;

            if (GameManager.instance != null)
                GameManager.instance.UpdateWave(waveNumber);

            // Show the educational message before each wave starts
            WaveMessage waveMsg = FindObjectOfType<WaveMessage>();
            if (waveMsg != null)
                yield return StartCoroutine(waveMsg.ShowWaveMessage(waveNumber));

            yield return StartCoroutine(SpawnBugs());

            // Make it harder each wave
            bugsPerWave++;

            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    IEnumerator SpawnBugs()
    {
        for (int i = 0; i < bugsPerWave; i++)
        {
            // Randomise spawn position slightly so bugs don't all stack
            Vector3 spawnPos = spawnPoint.position +
                new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-2f, 2f));

            Instantiate(bugPrefab, spawnPos, Quaternion.identity);

            yield return new WaitForSeconds(0.8f);
        }
    }

    public int GetWaveNumber()
    {
        return waveNumber;
    }
}
