using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    public Button resumeButton;
    public Button mainMenuButton;
    public Button SoundSetting;
    public Image pauseMenuHolder;
    public GameObject VolumeControl;
    public Button BackButton;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(OnResumeButtonClick);
        mainMenuButton.onClick.AddListener(OnMainMenuButtonClick);
        SoundSetting.onClick.AddListener(OnSoundSettings);
        BackButton.onClick.AddListener(OnBackButton);
    }

    void OnResumeButtonClick()
    {
        pauseMenuHolder.gameObject.SetActive(false);
        Debug.Log("get rid of pause menu");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void OnBackButton()
    {
        VolumeControl.gameObject.SetActive(false);
    }

    void OnSoundSettings()
    {
        VolumeControl.gameObject.SetActive(true);
    }

    void OnMainMenuButtonClick()
    {
        SceneManager.LoadScene("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenuHolder.gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
