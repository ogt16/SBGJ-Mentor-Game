
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Day : MonoBehaviour
{
    [SerializeField] CharacterGenerator characterGenerator;
    [SerializeField] List<CircleCollider2D> spawnPoints;

    int droneQuota = 5;
    List<GameObject> drones;

    private void Start()
    {
        drones = new List<GameObject>();

        while (drones.Count < droneQuota)
        {
            SpawnNewDrone();
        }
    }

    private void FixedUpdate()
    {
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject drone in drones)
        {
            if (drone.activeSelf == false)
            {
                toRemove.Add(drone);
            }
        }

        foreach (GameObject drone in toRemove)
        {
            drones.Remove(drone);
        }

        while (drones.Count < droneQuota)
        {
            SpawnNewDrone();
        }
    }

    void SpawnNewDrone()
    {
        GameObject newDrone = characterGenerator.GenerateCharacter();
        newDrone.transform.position = SelectSpawnPosition();
        drones.Add(newDrone);
    }

    Vector2 SelectSpawnPosition()
    {
        if (spawnPoints.Count == 0) { return Vector2.zero; }

        CircleCollider2D spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
        Vector2 unitRandom = Random.insideUnitCircle;
        unitRandom *= spawnPoint.radius * spawnPoint.transform.localScale;
        unitRandom += spawnPoint.offset;

        return unitRandom + new Vector2(spawnPoint.transform.position.x, spawnPoint.transform.position.y);
        
    }

    public void GoToNight()
    {
        SceneManager.LoadScene("Night");
    }
}
