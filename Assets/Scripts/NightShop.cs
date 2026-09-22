using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NightShop : MonoBehaviour
{
    public void GoBackToDay()
    {
        SceneManager.LoadScene("Day");
    }
}
