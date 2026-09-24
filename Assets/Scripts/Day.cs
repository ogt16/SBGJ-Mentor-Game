
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Day : MonoBehaviour
{
    [SerializeField] CharacterGenerator characterGenerator;
    [SerializeField] List<CircleCollider2D> spawnPoints;

    int droneQuota = 5;
    List<GameObject> drones;
    
    [SerializeField] Player player;

    float passiveFollowerCount = 0;
    float passiveFollowerTimer = GameData.Instance.passiveFollowerFrequency;
    float droneSpawnTimer = GameData.Instance.droneSpawnRate;
    float dayLimit;

    private void Start()
    {
        drones = new List<GameObject>();
        //Apply Day Length upgrades
        dayLimit = GameData.Instance.dayLength;
        while (drones.Count < droneQuota)
        {
            SpawnNewDrone();
        }
    }

    private void Update()
    {
        passiveFollowerTimer -= Time.deltaTime;
        if (passiveFollowerTimer < 0)
        {
            passiveFollowerTimer = GameData.Instance.passiveFollowerFrequency;
            passiveFollowerCount += GameData.Instance.passiveFollowerGain;
        }

        droneSpawnTimer -= Time.deltaTime;
        if (droneSpawnTimer < 0 && drones.Count < droneQuota)
        {
            SpawnNewDrone();
            droneSpawnTimer = GameData.Instance.droneSpawnRate;
        }
        dayLimit -= Time.deltaTime;
        if (dayLimit < 0)
        {
            GoToNight();
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

        while (passiveFollowerCount>=1)
        {
            passiveFollowerCount--;
            SpawnNewPassiveFollower();
        }
    }

    void SpawnNewDrone()
    {
        GameObject newDrone = characterGenerator.GenerateCharacter();
        newDrone.transform.Find("InformationPanel").gameObject.SetActive(false);
        //Apply Starting Influence upgrades to Drones
        newDrone.GetComponent<CharacterData>().influence = GameData.Instance.startingInfluence;
        newDrone.transform.position = SelectSpawnPosition();
        drones.Add(newDrone);
    }

    void SpawnNewPassiveFollower()
    {
        GameObject newFollower = characterGenerator.GenerateCharacter();
        newFollower.GetComponent<CharacterData>().influence = GameData.Instance.startingInfluence;
        GameData.Instance.AddFollower(newFollower);
        newFollower.gameObject.SetActive(false);

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
