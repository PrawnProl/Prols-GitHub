using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _npcPrefab;

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

        if(_timeUntilSpawn <= 0)
        {
            Instantiate(_npcPrefab, transform.position, Quaternion.identity);
            SetTimeUntilSpawn();
        }
    }

    private void SetTimeUntilSpawn()
    {
        _timeUntilSpawn = Random.Range(_minimumSpawnTime, _maximumSpawnTime);
    }
}
