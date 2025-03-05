using UnityEngine;
using System.Collections.Generic;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _npcPrefabs; // List of NPC prefabs to choose from

    [SerializeField]
    private float _minimumSpawnTime;

    [SerializeField]
    private float _maximumSpawnTime;

    private float _timeUntilSpawn;

    void Awake()
    {
        SetTimeUntilSpawn();
    }

    void Update()
    {
        _timeUntilSpawn -= Time.deltaTime;

        if (_timeUntilSpawn <= 0)
        {
            SpawnNPC();
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }

    private void SpawnNPC()
    {
        if (_npcPrefabs == null || _npcPrefabs.Count == 0)
        {
            Debug.LogError("No NPC prefabs assigned to the spawner!");
            return;
        }

        // Randomly select a prefab from the list
        GameObject randomPrefab = _npcPrefabs[Random.Range(0, _npcPrefabs.Count)];

        // Instantiate the selected prefab
        Instantiate(randomPrefab, transform.position, Quaternion.identity);
    }
}