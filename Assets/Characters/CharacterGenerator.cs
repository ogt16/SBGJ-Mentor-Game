using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    // generate a random character preset (applied to the CharacterData script attached to the charcter prefab)
    // spawn the created characters

    // Enumerators
    public enum Occupation {Florist, Butcher, Doctor, Unemployed, Accountant, Plumber, CEO, Firefighter, Chef, Teacher, Writer, Singer, Redditor, Politician, Landlord} // these are all the job roles a character can have
    public enum Preferences {Flowers, Videogames, Books, Painting, Pottery, Yoga, Chess} // these are all the things characters can like/ dislike
    public enum PositiveTrait {Charismatic, Honest, Optimistic, Calm, Obedient, Kind, Friendly, Wise, Efficient, Loyal, Patient}
    public enum NegativeTrait {Lazy, Insomniac, Nosey, Thanatophobic, Foolish, Liar, Jealous, Rebellious, Selfish, Fussy, Needy}

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

        // Randomised Name
        NewCharacterData.FirstName       = NameData.FirstNames[Random.Range(0, NameData.FirstNames.Length)];
        NewCharacterData.LastName        = NameData.LastNames[Random.Range(0, NameData.LastNames.Length)];

        //Occupation
        int NumOfJobs                    = System.Enum.GetNames(typeof(Occupation)).Length;
        NewCharacterData._Occupation     = (Occupation)Random.Range(0,NumOfJobs);

        List<int> PreferencesUsed = new List<int>(); // there shouldnt be repeats in the likes and dislikes list AND there shouldnt be a like in the dislike list

        // CHARACTER LIKES
        int TotalLikes = Random.Range(1,3);
        for(int i = 0; i < TotalLikes; i++)
        {
            bool IsValid = false;
            while(!IsValid)
            {
                // generate an enum id
                // ensure the enum is not in the preferences used list
                int TryIndex = Random.Range(0, System.Enum.GetNames(typeof(Preferences)).Length);
                if(!PreferencesUsed.Contains(TryIndex))
                {
                    // this is valid
                    NewCharacterData.Likes.Add((Preferences) TryIndex);

                    PreferencesUsed.Add(TryIndex);
                    IsValid = true; 
                }
            }
        }

        // CHARACTER DISLIKES
        int TotalDislikes = Random.Range(1,3);
        for(int i = 0; i < TotalLikes; i++)
        {
            bool IsValid = false;
            while(!IsValid)
            {
                // generate an enum id
                // ensure the enum is not in the preferences used list
                int TryIndex = Random.Range(0, System.Enum.GetNames(typeof(Preferences)).Length);
                if(!PreferencesUsed.Contains(TryIndex))
                {
                    // this is valid
                    NewCharacterData.Dislikes.Add((Preferences) TryIndex);

                    PreferencesUsed.Add(TryIndex);
                    IsValid = true; 
                }
            }
        }

        // Virtues
        int VirtueChance = Random.Range(0, 101);
        if(VirtueChance <= 25) // flat 20% chance BUT we can change this to a variable or an influence from upgrades
        {
            NewCharacterData.Virtues.Add((PositiveTrait) Random.Range(0, System.Enum.GetNames(typeof(PositiveTrait)).Length));
        }

        //Flaws
        int FlawChance = Random.Range(0, 101);
        if(FlawChance <= 25) // flat 20% chance BUT we can change this to a variable or an influence from upgrades
        {
            NewCharacterData.Flaws.Add((NegativeTrait) Random.Range(0, System.Enum.GetNames(typeof(NegativeTrait)).Length));
        }

        //Setting randomised colour
        NewCharacterData.SpriteColour = new Color32((byte)Random.Range(1,256), (byte)Random.Range(1,256), (byte)Random.Range(1,256), 255);

        NewCharacterData.InitialiseCharacter();

        Debug.Log($"Creating new character: {NewCharacterData.FirstName} {NewCharacterData.LastName}");
    }
}
