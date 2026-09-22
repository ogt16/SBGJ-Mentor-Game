using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CultController : MonoBehaviour
{
    // this is used in the night scene for sacrificing the cult members and having an outcome to the ritual.
    // essentially the backend of the night loop. will need hooking into the UI created

    public Upgrade[] RitualUpgrades; // all the scriptable objects for the ritual results
    private GameObject[] RitualStorage = new GameObject[5];


    // Add member to the ritual functionality
    public void AddFollowerToRitual(GameObject FollowerReference, int SlotID)
    {
        RitualStorage[SlotID] = FollowerReference; // add the follower to the ritual
    }

    public void RemoveFollowerFromRitual(int SlotID)
    {
        RitualStorage[SlotID] = null;
    }

    // make sure the conditions are satisfied before proceeding with the ritual results

    [ContextMenu("Debug Test Ritual Results")]
    public void TrySubmitRitual()
    {
        for(int i = 0; i < 5; i++)
        {
            GameObject MakeFollowerDebug = GetComponent<CharacterGenerator>().GenerateCharacter();
            RitualStorage[i] = MakeFollowerDebug;
        }

        foreach(GameObject follower in RitualStorage){if(follower == null){return;}}

        // All the slots are filled
        CompleteRitual();
    }

    /*  Complete Ritual

        - remove the members from the cult (they have been SACRIFICED)
        - reward the player if a crafting recipe for an upgrade has been completed

    */
    private Dictionary<CharacterGenerator.Occupation, int> ListToDictOcc(List<CharacterGenerator.Occupation> RequirementList)
    {
        Dictionary<CharacterGenerator.Occupation, int> CreatedDictionary = new Dictionary<CharacterGenerator.Occupation, int>();

        foreach(CharacterGenerator.Occupation RequirementType in RequirementList)
        {
            if(CreatedDictionary.ContainsKey(RequirementType))
            {
                int CachedValue = CreatedDictionary[RequirementType];
                CreatedDictionary.Remove(RequirementType);
                CreatedDictionary.Add(RequirementType, CachedValue + 1);
            }
            else
            {
                CreatedDictionary.Add(RequirementType, 0);
            }
        }

        return CreatedDictionary;
    }
    private Dictionary<CharacterGenerator.PositiveTrait, int> ListToDictVirtue(List<CharacterGenerator.PositiveTrait> RequirementList)
    {
        Dictionary<CharacterGenerator.PositiveTrait, int> CreatedDictionary = new Dictionary<CharacterGenerator.PositiveTrait, int>();

        foreach(CharacterGenerator.PositiveTrait RequirementType in RequirementList)
        {
            if(CreatedDictionary.ContainsKey(RequirementType))
            {
                int CachedValue = CreatedDictionary[RequirementType];
                CreatedDictionary.Remove(RequirementType);
                CreatedDictionary.Add(RequirementType, CachedValue + 1);
            }
            else
            {
                CreatedDictionary.Add(RequirementType, 0);
            }
        }

        return CreatedDictionary;
    }
    private Dictionary<CharacterGenerator.NegativeTrait, int> ListToDictFlaw(List<CharacterGenerator.NegativeTrait> RequirementList)
    {
        Dictionary<CharacterGenerator.NegativeTrait, int> CreatedDictionary = new Dictionary<CharacterGenerator.NegativeTrait, int>();

        foreach(CharacterGenerator.NegativeTrait RequirementType in RequirementList)
        {
            if(CreatedDictionary.ContainsKey(RequirementType))
            {
                int CachedValue = CreatedDictionary[RequirementType];
                CreatedDictionary.Remove(RequirementType);
                CreatedDictionary.Add(RequirementType, CachedValue + 1);
            }
            else
            {
                CreatedDictionary.Add(RequirementType, 0);
            }
        }

        return CreatedDictionary;
    }

    private void CompleteRitual()
    {
        Debug.Log("Starting ritual");
        List<Upgrade> UpgradesRewarded = new List<Upgrade>();

        // extract the data required for the combinations
        // go through all the upgrades and see if any of the conditions are fulfilled
        List<CharacterGenerator.Occupation> OccupationRequirementsFulfilled  = new List<CharacterGenerator.Occupation>(); // this is all the occupations from the ritual
        List<CharacterGenerator.PositiveTrait> PositiveTraitsFulfilled       = new List<CharacterGenerator.PositiveTrait>();
        List<CharacterGenerator.NegativeTrait> NegativeTraitsFulfilled       = new List<CharacterGenerator.NegativeTrait>();

        foreach(GameObject Follower in RitualStorage)
        {
            CharacterData FollowerData = Follower.GetComponent<CharacterData>();

            OccupationRequirementsFulfilled.Add(FollowerData._Occupation);

            if(FollowerData.Virtues.Count > 0)
            {
                foreach(CharacterGenerator.PositiveTrait Virtue in FollowerData.Virtues)
                {
                    PositiveTraitsFulfilled.Add(Virtue);
                }
            }

            if(FollowerData.Flaws.Count > 0)
            {
                foreach(CharacterGenerator.NegativeTrait Flaw in FollowerData.Flaws)
                {
                    NegativeTraitsFulfilled.Add(Flaw);
                }
            }
        }

        /* 
            conditions are stored as a list currently but to determine conditions this would be easier as a dictionary with numbers of occurences
            for example:
            List version: Florist, Florist, Butcher, CEO, Florist
            Dictionary Version: [Florist] = 3, [Butcher] = 1, [CEO] = 1
            --> Easier to tell if conditions have been satisfied 
        */

        // convert the lists to dictionaries to compare with the upgrade conditions
        Dictionary<CharacterGenerator.Occupation, int> OccupationResults = ListToDictOcc(OccupationRequirementsFulfilled);
        Dictionary<CharacterGenerator.PositiveTrait, int> VirtueResults  = ListToDictVirtue(PositiveTraitsFulfilled);
        Dictionary<CharacterGenerator.NegativeTrait, int> FlawResults    = ListToDictFlaw(NegativeTraitsFulfilled);


        // go through all the upgrades and see if any of the conditions are satisfied
        foreach(Upgrade RitualUpgrade in RitualUpgrades)
        {
            bool validator = true;

            // Check occupation conditions for upgrade
            if(RitualUpgrade.OccupationRequirements.Count > 0)
            {
                Dictionary<CharacterGenerator.Occupation, int> OccupationConditions = ListToDictOcc(RitualUpgrade.OccupationRequirements);
                // iterate through each enum condition that influences the upgrade being complete
                foreach(KeyValuePair<CharacterGenerator.Occupation, int> Condition in OccupationConditions)
                {
                    if(OccupationResults.ContainsKey(Condition.Key))
                    {
                        if(OccupationResults[Condition.Key] < Condition.Value){validator = false; break;}
                    }
                    else{validator = false; break;}
                }
            }
            // if its made it past the occupation conditions satisfied then check the next set (positive and negative traits)

            // Check positive traits conditions for upgrade
            if(RitualUpgrade.VirtueRequirements.Count > 0)
            {
                Dictionary<CharacterGenerator.PositiveTrait, int> VirtueConditions = ListToDictVirtue(RitualUpgrade.VirtueRequirements);
                // iterate through each enum condition that influences the upgrade being complete
                foreach(KeyValuePair<CharacterGenerator.PositiveTrait, int> Condition in VirtueConditions)
                {
                    if(VirtueResults.ContainsKey(Condition.Key))
                    {
                        if(VirtueResults[Condition.Key] < Condition.Value){validator = false; break;}
                    }
                    else{validator = false; break;}
                }
            }

            // Check negative traits conditions for upgrade
            if(RitualUpgrade.FlawRequirements.Count > 0)
            {
                Dictionary<CharacterGenerator.NegativeTrait, int> FlawConditions = ListToDictFlaw(RitualUpgrade.FlawRequirements);
                // iterate through each enum condition that influences the upgrade being complete
                foreach(KeyValuePair<CharacterGenerator.NegativeTrait, int> Condition in FlawConditions)
                {
                    if(FlawResults.ContainsKey(Condition.Key))
                    {
                        if(FlawResults[Condition.Key] < Condition.Value){validator = false; break;}
                    }
                    else{validator = false; break;}
                }
            }

            // if all conditions are satisfied then reward the upgrade!
            if(validator)
            {
                Debug.Log($"Conditions satisfied for {RitualUpgrade}!");
                UpgradesRewarded.Add(RitualUpgrade);
            }
        }

        if(UpgradesRewarded.Count > 0)
        {
            foreach(Upgrade _upgrade in UpgradesRewarded)
            {
                Debug.Log($"Rewarding player with {_upgrade.DisplayName}");
            }
        }

    }

}
