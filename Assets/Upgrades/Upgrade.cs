using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Scriptable Objects/Ritual Upgrade")]
public class Upgrade : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;
    public string RewardText;

    [Header("Requirements")]
    public List<CharacterGenerator.Occupation> OccupationRequirements    = new List<CharacterGenerator.Occupation>();
    public List<CharacterGenerator.PositiveTrait> VirtueRequirements     = new List<CharacterGenerator.PositiveTrait>();
    public List<CharacterGenerator.NegativeTrait> FlawRequirements       = new List<CharacterGenerator.NegativeTrait>();

}
