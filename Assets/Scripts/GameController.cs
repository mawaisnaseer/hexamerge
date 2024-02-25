using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public List<Draggable> stackPrefabs;
    public List<Transform> spawnPoints;
    public List<Draggable> spawnedObjects;
    public int refillCount = 3;

    public static Action<Draggable> OnSpawnStackPlaced;
    public HexaBoard hexaBoard;

    void Start()
    {
        // Spawn initial objects
        foreach (Transform spawnPoint in spawnPoints)
        {
            SpawnStack(spawnPoint);
        }
    }

    void SpawnStack(Transform spawnPoint)
    {
        // Select a random stack prefab
        Draggable stackPrefab = stackPrefabs[UnityEngine.Random.Range(0, stackPrefabs.Count)];

        // Instantiate the stack prefab at the spawn point
        Draggable spawnedStack = Instantiate(stackPrefab, spawnPoint.position, spawnPoint.transform.rotation);

        // Add the spawned stack to the list of spawned objects
        spawnedObjects.Add(spawnedStack);

        // Subscribe to the stack emptied event
        OnSpawnStackPlaced += HandleStackEmptied;
    }

    void HandleStackEmptied(Draggable stack)
    {
        // Remove the emptied stack from the spawned objects list
        spawnedObjects.Remove(stack);

        if (hexaBoard.AllBoardFilled())
        {
            UIController.uIController.levelFailed.gameObject.SetActive(true);
        }

        // Check if the spawned stack count is empty
        if (spawnedObjects.Count == 0)
        {
            // Refill the spawn points
            RefillSpawnPoints();
        }
    }

    void RefillSpawnPoints()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            // Spawn a new stack at the selected spawn point
            SpawnStack(spawnPoint);
        }


    }
}
