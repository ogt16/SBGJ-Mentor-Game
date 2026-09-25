using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }
    [SerializeField] public List<CharacterData> followers;
    public List<Upgrade> upgrades = new List<Upgrade>();

    //Upgradable Stats + starting values
    public float influenceRadius = 2;
    public float droneSpawnRate = 15;
    public float influenceSpeed = 0.5f;
    public float dayLength = 120;
    public float quality = 25;
    public float passiveFollowerGain = 0;
    public float passiveFollowerFrequency = 60; // No upgrade for this
    public float startingInfluence = 0;
    public float walkSpeed = 10;
    public float skillCheckDifficulty = 0.15f;  //Target width
    public float skillCheckPerfectReward = 20;  //Influence gain from hitting
    public float skillCheckRecovery = 10;  //Lose less Influence
    public float skillCheckFrequency = 9500;  //Percentage chance for Skill Check spawn





    // Audio

    float volume = 0.75f;
    bool first = false;

    public FMODUnity.EventReference daymusic;
    public FMODUnity.EventReference nightmusic;

    FMOD.Studio.EventInstance daym;
    FMOD.Studio.EventInstance nightm;

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

    private void Update()
    {


        if (!first)
        {
            daymusic = FMODUnity.EventReference.Find("event:/music/day");
            nightmusic = FMODUnity.EventReference.Find("event:/music/night");
            daym = FMODUnity.RuntimeManager.CreateInstance(daymusic);
            nightm = FMODUnity.RuntimeManager.CreateInstance(nightmusic);
            daym.setVolume(volume);
            nightm.setVolume(volume);

            Debug.Log("Go");
            daym.start();
            nightm.start();
            first = true;
            nightm.setPaused(false);
            daym.setPaused(true);
        }

        FMOD.Studio.PLAYBACK_STATE state;
        FMOD.Studio.PLAYBACK_STATE state1;
        daym.getPlaybackState(out state);
        nightm.getPlaybackState(out state1);

        if (state != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
        daym.start();
        if (state1 != FMOD.Studio.PLAYBACK_STATE.PLAYING) 
        nightm.start();

        string name = SceneManager.GetActiveScene().name;
        if (name == "Day")
        {
            nightm.setPaused(true);
            daym.setPaused(false);
        }
        else if (name == "Night")
        {
            nightm.setPaused(false);
            daym.setPaused(true);
        }
        
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
