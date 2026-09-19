using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NightShop : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI followerCountLabel;

    void Start()
    {
        UpdateFollowerCount();
    }
    public void BuyInfluenceRangeUpgrade()
    {
        if (GameData.Instance.cultMembers >= 1)
        {
            GameData.Instance.cultMembers -= 1;
            GameData.Instance.influenceRadius += 0.5f;
            UpdateFollowerCount();
        }
        else
        {
            Debug.Log("Not enough followers to buy range upgrade");
        }
    }

    public void BuyInfluencePowerUpgrade()
    {
        if (GameData.Instance.cultMembers >= 1)
        {
            GameData.Instance.cultMembers -= 1;
            GameData.Instance.influencePower += 10;
            UpdateFollowerCount();
        }
        else
        {
            Debug.Log("Not enough followers to buy power upgrade");
        }
    }

    void UpdateFollowerCount()
    {
        followerCountLabel.text = "Followers: " + GameData.Instance.cultMembers.ToString();
    }

    public void GoBackToDay()
    {
        SceneManager.LoadScene("Day");
    }
}
