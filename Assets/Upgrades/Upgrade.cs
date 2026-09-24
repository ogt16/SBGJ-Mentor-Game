using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum UpgradeStatToChange
{
    INFLUENCE_RADIUS,
    DRONE_SPAWN_RATE,
    INFLUENCE_SPEED,
    DAY_LENGTH,
    QUALITY,
    PASSIVE_FOLLOWER_GAIN,
    STARTING_INFLUENCE,
    WALK_SPEED,
    SKILL_CHECK_DIFFICULTY,
    SKILL_CHECK_PERFECT_REWARD,
    SKILL_CHECK_RECOVERY,
    SKILL_CHECK_FREQUENCY
}

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Ritual Upgrade")]
public class Upgrade : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;
    public string RewardText;
    public UpgradeStatToChange upgradeStatToChange;
    public float upgradeStatDifference;

    [Header("Requirements")]
    public List<CharacterGenerator.Occupation> OccupationRequirements    = new List<CharacterGenerator.Occupation>();
    public List<CharacterGenerator.PositiveTrait> VirtueRequirements     = new List<CharacterGenerator.PositiveTrait>();
    public List<CharacterGenerator.NegativeTrait> FlawRequirements       = new List<CharacterGenerator.NegativeTrait>();

}
