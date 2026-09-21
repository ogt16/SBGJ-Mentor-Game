using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    // generate a random character preset (applied to the CharacterData script attached to the charcter prefab)
    // spawn the created characters

    // Enumerators
    public enum Occupation {Florist, Butcher} // these are all the job roles a character can have
    public enum Preferences {} // these are all the things characters can like/ dislike
    public enum PositiveTrait {}
    public enum NegativeTrait {}

    public GameObject CharacterPrefab;


    [ContextMenu("Generate Character")] // so we can create characters from the editor without having to run in game for testing purposes
    public void GenerateCharacter()
    {
        /*
        Creation rules
            - First Name, and a Last Name
            - Single occupation
            - Star rating- this is from 1-5 with a higher chance for a lower value, but effected on player upgrades
            - 1-2 Likes
            - 1-2 dislikes 
            ? 0-1 positive traits
            ? 0-1 negative traits (player upgrades can negate this)
        */

        GameObject NewCharacter          = Instantiate(CharacterPrefab);
        CharacterData NewCharacterData   = NewCharacter.GetComponent<CharacterData>();

        NewCharacterData.FirstName       = NameData.FirstNames[Random.Range(0, NameData.FirstNames.Length)];
        NewCharacterData.LastName        = NameData.LastNames[Random.Range(0, NameData.LastNames.Length)];


        Debug.Log($"Creating new character:{NewCharacterData.FirstName} {NewCharacterData.LastName}");
    }
}
