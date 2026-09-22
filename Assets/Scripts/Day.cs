using UnityEngine;
using UnityEngine.SceneManagement;

public class Day : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void GoToNight()
    {
        SceneManager.LoadScene("Night");
    }
}
