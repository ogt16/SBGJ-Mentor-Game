using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    public List<GameObject> followers;

    //Upgradable Stats + starting values
    public float influenceRadius;
    public float droneSpawnRate;
    public float influenceSpeed;
    public float dayLength;
    public float quality;
    public float passiveFollowerGain;
    public float passiveFollowerFrequency;
    public float startingInfluence;
    public float walkSpeed;
    public float skillCheckDifficulty;
    public float skillCheckPerfectReward;
    public float skillCheckRecovery;
    public float skillCheckFrequency;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
    }
    
    public void AddFollower(GameObject follower)
    {
        if (follower.CompareTag("Character"))
        {
            if (!followers.Contains(follower))
            {
                followers.Add(follower);
            }
        }
    }

    public void RemoveFollower(GameObject follower)
    {
        if (followers.Contains(follower))
        {
            followers.Remove(follower);
        }
    }

}
