using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClickController : MonoBehaviour {

    public Image buttonImage;
    public Color hoverColor;
    public Color unHoverColor;
    public Color clickColor;
    public AudioClip selectClip;
    public MenuManager menuManager;
    public enum ButtonEnum { Tutorial, Play, Back, AI, Leave, Resume, Small};
    public ButtonEnum buttonEnum;
    
    void Start () {
        buttonImage.color = unHoverColor;
	}

    public void Hover ()
    {
        buttonImage.color = hoverColor;
    }

    public void UnHover ()
    {
        buttonImage.color = unHoverColor;
    }

    public void Click ()
    {
        buttonImage.color = clickColor;
        if (buttonEnum == ButtonEnum.Tutorial)
        {
            menuManager.TutorialClick();
        } else if (buttonEnum == ButtonEnum.Play)
        {
            menuManager.PlayClick();
        } else if (buttonEnum == ButtonEnum.Back)
        {
            menuManager.BackClick();
        } else if (buttonEnum == ButtonEnum.AI)
        {
            menuManager.XLClick();
        } else if (buttonEnum == ButtonEnum.Leave)
        {
            SteamVR_LoadLevel.Begin("VikingMenu");
        } else if (buttonEnum == ButtonEnum.Resume)
        {
            menuManager.Resume();
        }
        else if (buttonEnum == ButtonEnum.Small)
        {
            menuManager.AIClick();
        }
        buttonImage.color = unHoverColor;
    }
}
