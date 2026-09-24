
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    Slider daySlider;

    private void Start()
    {
        drones = new List<GameObject>();
        daySlider = FindAnyObjectByType<Slider>();
        //Apply Day Length upgrades
        dayLimit = GameData.Instance.dayLength;
        daySlider.maxValue = dayLimit;
        daySlider.value = dayLimit;
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
        daySlider.value = dayLimit;
        daySlider.handleRect.transform.Rotate(0, 0, -0.05f);
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
        newDrone.GetComponent<CharacterData>().transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject.transform.Find("InfluenceMeter").gameObject.GetComponent<Slider>().value = newDrone.GetComponent<CharacterData>().influence;
        newDrone.transform.position = SelectSpawnPosition();
        drones.Add(newDrone);
    }

    void SpawnNewPassiveFollower()
    {
        GameObject newFollower = characterGenerator.GenerateCharacter();

        GameData.Instance.AddFollower(newFollower.GetComponent<CharacterData>());
        newFollower.GetComponent<CharacterData>().influence = GameData.Instance.startingInfluence;

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
