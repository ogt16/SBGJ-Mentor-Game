using UnityEngine;
using System.Collections.Generic;


public class CharacterData : MonoBehaviour
{
    // contains all the generated data for the character
    public string FirstName;
    public string LastName;
    public CharacterGenerator.Occupation _Occupation;
    public Color SpriteColour;
    public List<CharacterGenerator.Preferences> Likes        = new List<CharacterGenerator.Preferences>();
    public List<CharacterGenerator.Preferences> Dislikes     = new List<CharacterGenerator.Preferences>();
    public List<CharacterGenerator.PositiveTrait> Virtues    = new List<CharacterGenerator.PositiveTrait>();
    public List<CharacterGenerator.NegativeTrait> Flaws      = new List<CharacterGenerator.NegativeTrait>();

}
