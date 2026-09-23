using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CultController : MonoBehaviour
{
    // this is used in the night scene for sacrificing the cult members and having an outcome to the ritual.
    // essentially the backend of the night loop. will need hooking into the UI created

    public Upgrade[] RitualUpgrades; // all the scriptable objects for the ritual results

    [Header("UI Elements")]
    public GameObject InformationPane;
    public GameObject FollowerContainer;
    public GameObject FollowerCardPrefab;

    // Private fields
    private GameObject[] RitualStorage = new GameObject[5];

    public void ClearPreviousCarousel(int Quantity)
    {
        // Clear the previous carousel data and cards
        int PreviousFollowerCarousel = FollowerContainer.transform.childCount;
        for(int i = 0; i < PreviousFollowerCarousel; i++)
        {
            Destroy(FollowerContainer.transform.GetChild(i).transform.gameObject);
        }
    }

    public void SetupFollowerCards(List<CharacterData> FollowerData)
    {
        // apply the saved follower data to the cards
        ClearPreviousCarousel(FollowerData.Count);

        for(int i = 0; i < FollowerData.Count; i++)
        {
            // Create the card itself
            // Apply the correct data to the card

            GameObject NewCard       = Instantiate(FollowerCardPrefab);
            CharacterData CardData   = NewCard.GetComponent<CharacterData>();

            // In hindsight i imagine there is a much better way to map these values
            CardData.FirstName       = FollowerData[i].FirstName;
            CardData.LastName        = FollowerData[i].LastName;
            CardData._Occupation     = FollowerData[i]._Occupation;
            CardData.Likes           = FollowerData[i].Likes;
            CardData.Dislikes        = FollowerData[i].Dislikes;
            CardData.Virtues         = FollowerData[i].Virtues;
            CardData.Flaws           = FollowerData[i].Flaws;
            CardData.SpriteColour    = FollowerData[i].SpriteColour;

            // parent to the carousel itself
            NewCard.transform.SetParent(FollowerContainer.transform);

            NewCard.transform.localPosition                      = new Vector2(NewCard.GetComponent<RectTransform>().rect.width + (i * 150), 0);
            NewCard.GetComponent<UnityEngine.UI.Image>().color   = CardData.SpriteColour;

            // let the follower card access this manager so it can tell it when a card is being hovered
            NewCard.GetComponent<DragComponent>().ControllerReference = this;
        }
    }

    [ContextMenu("Debug Create Follower Carousel")]
    public void DebugRitual()
    {
        // create dummy followers and add them to the carousel to be used in the ritual
        int Quantity = Random.Range(5,8);
        List<CharacterData> Followers = new List<CharacterData>();

        for(int i = 0; i < Quantity; i++)
        {
            GameObject NewFollower = GetComponent<CharacterGenerator>().GenerateCharacter();
            Followers.Add(NewFollower.GetComponent<CharacterData>());
        }

        SetupFollowerCards(Followers);
    }

    public void UpdateInformationPane(CharacterData FollowerData)
    {
        InformationPane.transform.Find("Name").GetComponent<TextMeshProUGUI>().SetText($"{FollowerData.FirstName} {FollowerData.LastName}");
        InformationPane.transform.Find("Occupation").GetComponent<TextMeshProUGUI>().SetText($"{FollowerData._Occupation}");

        // likes and dislikes
        if(FollowerData.Likes.Count > 1){InformationPane.transform.Find("Likes").GetComponent<TextMeshProUGUI>().SetText($"Likes: {FollowerData.Likes[0]}, {FollowerData.Likes[1]}");}
        else{InformationPane.transform.Find("Likes").GetComponent<TextMeshProUGUI>().SetText($"Likes: {FollowerData.Likes[0]}");}

        if(FollowerData.Dislikes.Count > 1){InformationPane.transform.Find("Dislikes").GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {FollowerData.Dislikes[0]}, {FollowerData.Dislikes[1]}");}
        else{InformationPane.transform.Find("Dislikes").GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {FollowerData.Dislikes[0]}");}
        
        if(FollowerData.Virtues.Count > 0){InformationPane.transform.Find("Virtue").GetComponent<TextMeshProUGUI>().SetText($"{FollowerData.Virtues[0].ToString().ToUpper()}");}
        else{{InformationPane.transform.Find("Virtue").GetComponent<TextMeshProUGUI>().SetText($"");}}

        if(FollowerData.Flaws.Count > 0){InformationPane.transform.Find("Flaw").GetComponent<TextMeshProUGUI>().SetText($"{FollowerData.Flaws[0].ToString().ToUpper()}");}
        else{{InformationPane.transform.Find("Flaw").GetComponent<TextMeshProUGUI>().SetText($"");}}
        
    }


    // Add member to the ritual functionality
    public void AddFollowerToRitual(GameObject FollowerReference, int SlotID)
    {
        RitualStorage[SlotID] = FollowerReference; // add the follower to the ritual
        Debug.Log($"Adding {FollowerReference.GetComponent<CharacterData>().FirstName} {FollowerReference.GetComponent<CharacterData>().LastName } to the ritual!");
    }

    public void RemoveFollowerFromRitual(int SlotID)
    {
        RitualStorage[SlotID] = null;
        Debug.Log($"Slot {SlotID} now empty");
    }

    // make sure the conditions are satisfied before proceeding with the ritual results

    public void TrySubmitRitual()
    {
        foreach(GameObject follower in RitualStorage)
        {
            if(follower == null)
            {
                Debug.Log("All slots are not filled");
                return;
            }
        }

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
            if(validator){UpgradesRewarded.Add(RitualUpgrade);}
        }

        // realistically the player will get 0-2 upgrades, i cant imagine there being a combination of characters that unlocks more than 2 upgrades
        if(UpgradesRewarded.Count > 0)
        {
            foreach(Upgrade _upgrade in UpgradesRewarded)
            {
                Debug.Log($"Rewarding player with {_upgrade.DisplayName}");

                // this is the block of code to actually deal with unlocking the upgrades
                /* 
                    If upgrade is NOT unlocked yet:
                        Add the details to the ritual book that shows the recipe

                    if the upgrade IS unlocked already:
                        Does it stack effect?
                         Does it upgrade to the next tier?
                          Does it unlock the recipe for the next tier?
                */
            }
        }
        else
        {
            //the player has sacrificed 5 followers for no reason!
            // i think a sad trumpet should play here honestly
        }

    }

}
