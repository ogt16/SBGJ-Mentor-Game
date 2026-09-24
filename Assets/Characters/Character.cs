using UnityEngine;
using System.Collections.Generic;
using TMPro;

public struct CharacterData
{
    public string FirstName;
    public string LastName;
    public CharacterGenerator.Occupation _Occupation;
    public List<CharacterGenerator.Preferences> Likes;
    public List<CharacterGenerator.Preferences> Dislikes;
    public List<CharacterGenerator.PositiveTrait> Virtues;
    public List<CharacterGenerator.NegativeTrait> Flaws;

    // Cosmetics
    public int hairStyle;
    public Color hairColour;
    public Color skinColour;
    public Color shirtColour;
    public Color shoeColour;

    


    public static bool operator ==(CharacterData c1, CharacterData c2)
    {
        return c1.Equals(c2);
    }

    public static bool operator !=(CharacterData c1, CharacterData c2)
    {
        return !c1.Equals(c2);
    }
}




public class Character : MonoBehaviour
{
    public CharacterData data = new CharacterData();

    public float influence;

    // Sprites
    [SerializeField] SpriteRenderer shirtSprite;
    [SerializeField] SpriteRenderer shoesSprite;
    [SerializeField] SpriteRenderer skinSprite;
    [SerializeField] List<GameObject> hairStyles;

    SpriteRenderer chosenHairStyle;

    private void Awake()
    {
        data.Likes = new List<CharacterGenerator.Preferences>();
        data.Dislikes = new List<CharacterGenerator.Preferences>();
        data.Virtues = new List<CharacterGenerator.PositiveTrait>();
        data.Flaws = new List<CharacterGenerator.NegativeTrait>();

    }

    public void InitialiseCharacter()
    {
        // Cosmetics
        shirtSprite.color = data.shirtColour;
        shoesSprite.color = data.shoeColour;
        skinSprite.color = data.skinColour;
        hairStyles[data.hairStyle].SetActive(true);
        chosenHairStyle = hairStyles[data.hairStyle].GetComponent<SpriteRenderer>();
        chosenHairStyle.color = data.hairColour;

        // set information panel
        GameObject InformationPanel = transform.Find("InformationPanel").gameObject.transform.Find("Canvas").gameObject;

        // setting the name
        InformationPanel.transform.Find("Name").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{data.FirstName} {data.LastName}");
        InformationPanel.transform.Find("Job").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{data._Occupation}");

        if(data.Likes.Count > 1){InformationPanel.transform.Find("Likes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Likes: {data.Likes[0]}, {data.Likes[1]}");}
        else{InformationPanel.transform.Find("Likes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Likes: {data.Likes[0]}");}

        if(data.Dislikes.Count > 1){InformationPanel.transform.Find("Dislikes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {data.Dislikes[0]}, {data.Dislikes[1]}");}
        else{InformationPanel.transform.Find("Dislikes").gameObject.GetComponent<TextMeshProUGUI>().SetText($"Dislikes: {data.Dislikes[0]}");}

        if(data.Virtues.Count > 0){InformationPanel.transform.Find("Virtue").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{data.Virtues[0]}");}
        else{InformationPanel.transform.Find("Virtue").gameObject.GetComponent<TextMeshProUGUI>().SetText("");}

        if(data.Flaws.Count > 0){InformationPanel.transform.Find("Flaw").gameObject.GetComponent<TextMeshProUGUI>().SetText($"{data.Flaws[0]}");}
        else{InformationPanel.transform.Find("Flaw").gameObject.GetComponent<TextMeshProUGUI>().SetText("");}

    }


}