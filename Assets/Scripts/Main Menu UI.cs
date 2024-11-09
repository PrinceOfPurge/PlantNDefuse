using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public Button playButton;
    public Button quitButton;

    // Start is called before the first frame update
    void Start()
    {
        playButton.onClick.AddListener(OnPlayButtonClick);
        quitButton.onClick.AddListener(OnQuitButtonClick);
    }

    void OnPlayButtonClick()
    {
        SceneManager.LoadScene("AudioLoadScreen");
    }

    void OnQuitButtonClick()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
