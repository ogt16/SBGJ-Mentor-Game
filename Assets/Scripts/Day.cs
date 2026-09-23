
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
    public float DayLimit;
    [SerializeField] Player player;

    private void Start()
    {
        drones = new List<GameObject>();
        //Apply Day Length upgrades
        DayLimit = player.dayLength;
        player.passiveFollowerTimer = 60;
        while (drones.Count < droneQuota)
        {
            SpawnNewDrone();
        }
    }

    private void Update()
    {
        DayLimit -= Time.deltaTime;
        if (DayLimit < 0)
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

        while (drones.Count < droneQuota)
        {
            SpawnNewDrone();
        }

        while (player.passiveFollowerCount>=1)
        {
            player.passiveFollowerCount--;
            SpawnNewPassiveFollower();
        }
    }

    void SpawnNewDrone()
    {
        GameObject newDrone = characterGenerator.GenerateCharacter();
        newDrone.transform.Find("InformationPanel").gameObject.SetActive(false);
        //Apply Starting Influence upgrades to Drones
        newDrone.GetComponent<CharacterData>().influence = player.startingInfluence;
        newDrone.transform.position = SelectSpawnPosition();
        drones.Add(newDrone);
    }

    void SpawnNewPassiveFollower()
    {
        GameObject newFollower = characterGenerator.GenerateCharacter();
        newFollower.GetComponent<CharacterData>().influence = 100;
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
