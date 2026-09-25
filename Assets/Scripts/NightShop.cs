using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NightShop : MonoBehaviour
{
    public void GoBackToDay()
    {
        FMODUnity.RuntimeManager.PlayOneShot("time change");
        SceneManager.LoadScene("Day");
    }
}
