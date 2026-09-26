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

    // Initial spawn rates of the occupations. These can be changed from upgrades
    private Dictionary<Occupation, int> JobSpawnRates = new Dictionary<Occupation, int>
    {
        [Occupation.Florist]     = 1,
        [Occupation.Butcher]     = 1,
        [Occupation.Doctor]      = 1,
        [Occupation.Unemployed]  = 2,
        [Occupation.Accountant]  = 1,
        [Occupation.Plumber]     = 1,
        [Occupation.CEO]         = 1,
        [Occupation.Firefighter] = 1,
        [Occupation.Chef]        = 1,
        [Occupation.Teacher]     = 1,
        [Occupation.Writer]      = 1,
        [Occupation.Singer]      = 1,
        [Occupation.Redditor]    = 1,
        [Occupation.Politician]  = 1,
        [Occupation.Landlord]    = 1,
    };


    [ContextMenu("Generate Character")] // so we can create characters from the editor without having to run in game for testing purposes
    public GameObject GenerateCharacter()
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
        Character NewCharacterData   = NewCharacter.GetComponent<Character>();

        // Randomised Name
        NewCharacterData.data.FirstName       = NameData.FirstNames[Random.Range(0, NameData.FirstNames.Length)];
        NewCharacterData.data.LastName        = NameData.LastNames[Random.Range(0, NameData.LastNames.Length)];

        //Occupation (influenced by spawn weights dictionary)
        List<Occupation> occupations     = new List<Occupation>();
        foreach(KeyValuePair<Occupation, int> Job in JobSpawnRates)
        {
            for(int i = 0; i < Job.Value; i ++)
            {
                occupations.Add(Job.Key);
            }
        }

        NewCharacterData.data._Occupation     = occupations[Random.Range(0, occupations.Count)];
        List<int> PreferencesUsed        = new List<int>(); // there shouldnt be repeats in the likes and dislikes list AND there shouldnt be a like in the dislike list

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
                    NewCharacterData.data.Likes.Add((Preferences) TryIndex);

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
                    NewCharacterData.data.Dislikes.Add((Preferences) TryIndex);

                    PreferencesUsed.Add(TryIndex);
                    IsValid = true; 
                }
            }
        }

        // Virtues
        int VirtueChance = Random.Range(0, 101);
        if(VirtueChance <= GameData.Instance.quality) // flat 25% chance BUT we can change this to a variable or an influence from upgrades
        {
            NewCharacterData.data.Virtues.Add((PositiveTrait) Random.Range(0, System.Enum.GetNames(typeof(PositiveTrait)).Length));
        }

        //Flaws
        int FlawChance = Random.Range(0, 101);
        if(FlawChance <= GameData.Instance.quality) // flat 25% chance BUT we can change this to a variable or an influence from upgrades
        {
            NewCharacterData.data.Flaws.Add((NegativeTrait) Random.Range(0, System.Enum.GetNames(typeof(NegativeTrait)).Length));
        }

        //Setting randomised colour
        NewCharacterData.data.hairColour = RandomColour();
        NewCharacterData.data.shirtColour = RandomColour();
        NewCharacterData.data.shoeColour = RandomColour();
        NewCharacterData.data.skinColour = SkinColour();
        NewCharacterData.data.hairStyle = Random.Range(0, 16);

        // setup the information panel and all that
        NewCharacterData.InitialiseCharacter();

        Debug.Log($"Creating new character: {NewCharacterData.data.FirstName} {NewCharacterData.data.LastName}");
        return NewCharacter;
    }

    Color RandomColour()
    {
        return new Color(Random.Range(0, 256) / 255f, Random.Range(0, 256) / 255f, Random.Range(0, 256) / 255f, 1);
    }

    // May have slight skew towards generating white skin tones blame AI
    Color SkinColour()
    {
        return new Color(Random.Range(180f, 231f) / 255f, Random.Range(200f, 256f) / 255f, Random.Range(50f, 100f) / 255f, 1);
    }
}
