using UnityEngine;
using System.Collections.Generic;
using TMPro;


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

    public float influence = 0;

    public void InitialiseCharacter()
    {
        transform.Find("Square").transform.gameObject.GetComponent<SpriteRenderer>().color = SpriteColour;

        // set information panel
        GameObject InformationPanel = transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject;

        // setting the name
        InformationPanel.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{FirstName} {LastName}");
        InformationPanel.transform.Find("Job").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{_Occupation}");

        if(Likes.Count > 1){InformationPanel.transform.Find("Likes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Likes: {Likes[0]}, {Likes[1]}");}
        else{InformationPanel.transform.Find("Likes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Likes: {Likes[0]}");}

        if(Dislikes.Count > 1){InformationPanel.transform.Find("Dislikes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {Dislikes[0]}, {Dislikes[1]}");}
        else{InformationPanel.transform.Find("Dislikes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {Dislikes[0]}");}

        if(Virtues.Count > 0){InformationPanel.transform.Find("Virtue").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{Virtues[0]}");}
        else{InformationPanel.transform.Find("Virtue").gameObject.GetComponent<TextMeshProUGUI>().SetText("");}

        if(Flaws.Count > 0){InformationPanel.transform.Find("Flaw").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{Flaws[0]}");}
        else{InformationPanel.transform.Find("Flaw").gameObject.GetComponent<TextMeshProUGUI>().SetText("");}

    }


}