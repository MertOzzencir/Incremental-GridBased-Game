using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Deposits spawnableObject;
    [SerializeField] private int totalSpawnAmount;
    [SerializeField] private float spawnTimer;
    [SerializeField] private Vector2Int spawnArea;
    private float refTimer;
    private int spawnedObjectTimer;
    private Deposits nextObject;
    private List<Vector3Int> savedSpawnPositions = new List<Vector3Int>();
    private List<Vector3Int> spawnPositions = new List<Vector3Int>();
    void Awake()
    {
        for (int i = Mathf.FloorToInt(transform.position.z - spawnArea.y / 2); i < Mathf.FloorToInt(transform.position.z + spawnArea.y / 2); i++)
        {
            for (int j = Mathf.FloorToInt(transform.position.x - spawnArea.x / 2); j < Mathf.FloorToInt(transform.position.x + spawnArea.x / 2); j++)
            {
                savedSpawnPositions.Add(new Vector3Int(j, 0, i));
                spawnPositions.Add(new Vector3Int(j, 0, i));
            }
        }
        ReadyNextObject();
    }
    void Start()
    {
        GridSystem.Instance.OnDeletedGrid += CheckPotantialPosition;
    }

    void Update()
    {
        if (savedSpawnPositions.Count == 0 || spawnedObjectTimer >= totalSpawnAmount)
            return;


        refTimer += Time.deltaTime;
        if (refTimer > spawnTimer)
        {
            Vector3Int spawnPosition = GetSpawnPoint();
            GridSystem.Instance.CheckPoint(spawnPosition, new GridSpec(nextObject), out bool isValid);
            savedSpawnPositions.Remove(spawnPosition);
            if (isValid)
            {
                refTimer = 0;
                spawnedObjectTimer++;
                nextObject.GetGameObject().SetActive(true);
                Quaternion randomRotation = Quaternion.Euler(new Vector3(nextObject.Orientation.transform.rotation.x,Random.Range(0,180),nextObject.Orientation.transform.rotation.z));
                nextObject.Orientation.transform.rotation =randomRotation;
                nextObject.Placed();
                ReadyNextObject();
            }
        }
    }
    private void CheckPotantialPosition(List<Vector3Int> deletedPoints)
    {
        foreach (var a in deletedPoints)
        {
            if (spawnPositions.Contains(a))
            {
                savedSpawnPositions.Add(a);
                spawnedObjectTimer--;
            }

        }
    }
    [ContextMenu("Get Point")]
    private Vector3Int GetSpawnPoint()
    {
        return savedSpawnPositions[Random.Range(0, savedSpawnPositions.Count)];
        //Debug.Log(spawnPoint);
    }

    private void ReadyNextObject()
    {
        nextObject = Instantiate(spawnableObject);
        nextObject.GetGameObject().SetActive(false);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnArea.x, 1, spawnArea.y));
    }


}
