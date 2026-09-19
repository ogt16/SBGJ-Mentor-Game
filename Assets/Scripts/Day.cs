using UnityEngine;
using UnityEngine.SceneManagement;

public class Day : MonoBehaviour
{
    public void GoToNight()
    {
        SceneManager.LoadScene("Night");
    }
}
