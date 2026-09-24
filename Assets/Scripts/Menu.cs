
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Image unpressedImage;
    [SerializeField] Image pressedImage;
    SceneAsset DayScene;

    float defaultScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        defaultScale = unpressedImage.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        unpressedImage.transform.localScale = new Vector3(defaultScale + (Mathf.Sin(Time.time) * 0.025f), defaultScale + (Mathf.Sin(Time.time) * 0.025f), 1);
        pressedImage.transform.localScale = new Vector3(defaultScale + (Mathf.Sin(Time.time) * 0.025f), defaultScale + (Mathf.Sin(Time.time) * 0.025f), 1);
    }

    public void PlayButtonPressed()
    {
        unpressedImage.gameObject.SetActive(false);
        pressedImage.gameObject.SetActive(true);
        SceneManager.LoadScene("Day");
    }
}
