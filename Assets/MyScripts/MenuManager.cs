using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {
    public GameManager gameManager;
    public GameObject tutorialButton;
    public GameObject playButton;
    public GameObject backButton;
    public GameObject AIButton;
    public GameObject MultiplayerButton;

	public void TutorialClick()
    {
        SteamVR_LoadLevel.Begin("Tutorial");
    }

    public void PlayClick()
    {
        tutorialButton.SetActive(false);
        playButton.SetActive(false);
        backButton.SetActive(true);
        AIButton.SetActive(true);
        MultiplayerButton.SetActive(true);
    }

    public void BackClick()
    {
        tutorialButton.SetActive(true);
        playButton.SetActive(true);
        backButton.SetActive(false);
        AIButton.SetActive(false);
        MultiplayerButton.SetActive(false);
    }

    public void AIClick()
    {
        SteamVR_LoadLevel.Begin("SmallGame");
    }

    public void XLClick()
    {
        SteamVR_LoadLevel.Begin("AIGameScene");
    }

    public void Resume()
    {
        gameManager.Resume();
    }
}
