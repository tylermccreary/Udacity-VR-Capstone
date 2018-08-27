using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlacer
{
    public GameObject wallShowPrefab;
    public GameObject wallShowErrorPrefab;
    public GameObject wallPrefab;
    public float gridWidth;

    private Player player;
    private bool wallShowing;
    private bool wallError;

    void Update()
    {
        UpdateWallShowingPrefab();
    }

    void ShowWall()
    {
        if (player.wallsAvailable > 0)
        {
            wallShowing = true;
        }
        else
        {
            //showErrorUI
            //makeErrorNoise
        }
    }

    void HideWall()
    {
        wallShowPrefab.SetActive(false);
        wallShowErrorPrefab.SetActive(false);
        wallShowing = false;
    }

    void PlaceWall()
    {
        if (!wallError)
        {
            //PlaceWall()
            //placeSoundEffect
            wallShowing = false;
        }
    }

    void UpdateWallShowingPrefab()
    {
        //find square player is in

        //find wallStart the the player is facing
        //
    }
}
