
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] Image unpressedImage;
    [SerializeField] Image hoverImage;
    [SerializeField] Image pressedImage;

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
        hoverImage.transform.localScale = new Vector3(defaultScale + (Mathf.Sin(Time.time) * 0.025f), defaultScale + (Mathf.Sin(Time.time) * 0.025f), 1);
    }

    public void PlayButtonPressed()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/sounds/button press");
        unpressedImage.gameObject.SetActive(false);
        hoverImage.gameObject.SetActive(false);
        pressedImage.gameObject.SetActive(true);
        SceneManager.LoadScene("Day");
    }

    public void PlayButtonHover()
    {
        unpressedImage.gameObject.SetActive(false);
        hoverImage.gameObject.SetActive(true);
    }

    public void PlayButtonEndHover()
    {
        hoverImage.gameObject.SetActive(false);
        unpressedImage.gameObject.SetActive(true);
    }
}
