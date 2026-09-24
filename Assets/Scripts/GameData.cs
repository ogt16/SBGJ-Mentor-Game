using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    [SerializeField] public List<CharacterData> followers;
    public List<Upgrade> upgrades = new List<Upgrade>();

    //Upgradable Stats + starting values
    public float influenceRadius = 2;
    public float droneSpawnRate = 15;
    public float influenceSpeed = 1;
    public float dayLength = 120;
    public float quality;
    public float passiveFollowerGain = 0;
    public float passiveFollowerFrequency = 60; // No upgrade for this
    public float startingInfluence = 0;
    public float walkSpeed = 10;
    public float skillCheckDifficulty;
    public float skillCheckPerfectReward;
    public float skillCheckRecovery;
    public float skillCheckFrequency;

    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
        
        if (Instance != null && Instance != this) 
        { 
            Destroy(gameObject); 
        } 
        else 
        { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } 

        followers = new List<CharacterData>();
    }
    
    public void AddFollower(CharacterData follower)
    {

        if (!followers.Contains(follower))
        {
            followers.Add(follower);
        }

    }

    public void RemoveFollower(CharacterData follower)
    {
        if (followers.Contains(follower))
        {
            followers.Remove(follower);
        }
    }

    public bool TryUpgrade(Upgrade upgrade)
    {
        if (upgrades.Contains(upgrade)) { return false; }

        upgrades.Add(upgrade);
        ApplyUpgrade(upgrade);
        return true;
    }

    void ApplyUpgrade(Upgrade upgrade)
    {
        switch (upgrade.upgradeStatToChange)
        {
            case UpgradeStatToChange.INFLUENCE_RADIUS:
                influenceRadius += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.DRONE_SPAWN_RATE:
                droneSpawnRate += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.INFLUENCE_SPEED:
                influenceSpeed += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.DAY_LENGTH:
                dayLength += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.QUALITY:
                quality += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.PASSIVE_FOLLOWER_GAIN:
                passiveFollowerGain += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.STARTING_INFLUENCE:
                startingInfluence += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.WALK_SPEED:
                walkSpeed += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.SKILL_CHECK_DIFFICULTY:
                skillCheckDifficulty += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.SKILL_CHECK_PERFECT_REWARD:
                skillCheckPerfectReward += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.SKILL_CHECK_RECOVERY:
                skillCheckRecovery += upgrade.upgradeStatDifference;
                break;
            case UpgradeStatToChange.SKILL_CHECK_FREQUENCY:
                skillCheckFrequency += upgrade.upgradeStatDifference;
                break;
        }
    }

}
