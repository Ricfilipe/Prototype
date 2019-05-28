using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public GameObject text;
    public GameObject slider;
    private Text textSlider;
    private Slider realSlider;

    private float num;
    // Start is called before the first frame update
    void Start()
    {
        textSlider = text.GetComponent<Text>();
        realSlider = slider.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        textSlider.text = "Number of Waves: "+ realSlider.value;
        num = realSlider.value;
    }

    public void Play()
    {
        string path = "Assets/config.txt";
        System.IO.File.WriteAllText(path, "" + (num));
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit(0);
    }
}
