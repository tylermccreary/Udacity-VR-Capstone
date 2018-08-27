using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool menuScene;
    public bool movementAllowed;
    public float stunTime;
    private int playRound;
    private int menuRound;
    public Player[] players;
    public GameObject ball;
    public Player mainPlayer;
    
    private float gameClock;
    private float endClock;
    private int blueScore;
    private int orangeScore;
    public GameObject[] blueRespawns;
    public GameObject[] orangeRespawns;
    public Goal[] goals;
    public Text blueClock;
    public Text orangeClock;
    public Text blueScoreEast;
    public Text blueScoreWest;
    public Text orangeScoreEast;
    public Text orangeScoreWest;
    public GameObject ballSpawnNeutral;
    public GameObject ballSpawnOrange;
    public GameObject ballSpawnBlue;
    public Renderer floorBoundsRenderer;
    public Material floorBoundsMaterial;
    public Material floorOutOfBoundsMaterial;
    public AudioSource dynamicAudioSource;
    public AudioClip outOfBounds;
    public AudioClip crowdGoalClip;
    public AudioClip buzzerClip;
    public AudioClip beepClip;
    public AudioSource gameClockAudioSource;
    public GameObject menu;
    public Text winnerDisplay1;
    public Text winnerDisplay2;

    private int clockBeeps;

    private bool goalTriggered;
    private bool outTriggered;

    public enum GameStateEnum { Prep, Play, Goal, End, Idle }
    public GameStateEnum gameState;

    void Start()
    {
        playRound = 1;
        gameClock =  60;
        endClock = 10;
        movementAllowed = false;
        clockBeeps = 10;
        goalTriggered = false;
        outTriggered = false;
    }

    void Update()
    {
        if (gameState == GameStateEnum.Play)
        {
            gameClock -= Time.deltaTime;
            if (gameClock < clockBeeps && clockBeeps > 1)
            {
                gameClockAudioSource.Play();
                clockBeeps--;
            }
            if (gameClock <= 0)
            {
                DisplayWinner();
                gameState = GameStateEnum.End;
                dynamicAudioSource.clip = buzzerClip;
                dynamicAudioSource.Play();
            }
            else
            {
                UpdateClocks(gameClock);
            }
        }
        else if (gameState == GameStateEnum.End)
        {
            endClock -= Time.deltaTime;
            if (endClock <= 0)
            {
                SteamVR_LoadLevel.Begin("VikingMenu");
            }
            //UpdateClocks(endClock);
        }
    }

    public void TriggerGoal(Goal goal)
    {
        if (!goalTriggered && !outTriggered)
        {
            goalTriggered = true;
            if (gameClock > 0)
            {
                gameState = GameStateEnum.Goal;
                dynamicAudioSource.clip = crowdGoalClip;
                dynamicAudioSource.time = 4.3f;
                dynamicAudioSource.Play();
                if (goal.team == "blue")
                {
                    orangeScore++;
                    orangeScoreEast.text = orangeScore.ToString();
                    orangeScoreWest.text = orangeScore.ToString();
                }
                else
                {
                    blueScore++;
                    blueScoreEast.text = blueScore.ToString();
                    blueScoreWest.text = blueScore.ToString();
                }
                StartCoroutine(Reset(goal.team));
            }
        }
    }

    private IEnumerator Reset(string teamColor)
    {
        gameState = GameStateEnum.Goal;
        yield return new WaitForSeconds(5.0f);
        dynamicAudioSource.Stop();
        gameState = GameStateEnum.Play;
        int blueCount = 0;
        int orangeCount = 0;
        foreach (Player player in players)
        {
            if (player.team == "blue")
            {
                player.gameObject.transform.position = blueRespawns[(blueScore + orangeScore + blueCount) % 3].transform.position;
                blueCount++;
            }
            else
            {
                player.gameObject.transform.position = orangeRespawns[(blueScore + orangeScore + orangeCount) % 3].transform.position;
                orangeCount++;
            }
        }

        
        foreach (Goal goal in goals)
        {
            goal.ResetGoal();
        }
        
        ball.transform.parent = null;
        ball.GetComponent<Rigidbody>().isKinematic = true;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        if (teamColor == "blue")
        {
            ball.transform.position = ballSpawnBlue.transform.position;
        } else if (teamColor == "orange")
        {
            ball.transform.position = ballSpawnOrange.transform.position;
        } else
        {
            ball.transform.position = ballSpawnNeutral.transform.position;
        }
        ball.gameObject.layer = LayerMask.NameToLayer("Ball");
        floorBoundsRenderer.material = floorBoundsMaterial;
        goalTriggered = false;
        outTriggered = false;
    }

    public void TriggerOutOfBounds ()
    {
        if (!outTriggered && ! goalTriggered)
        {
            outTriggered = true;
            dynamicAudioSource.clip = outOfBounds;
            dynamicAudioSource.Play();
            floorBoundsRenderer.material = floorOutOfBoundsMaterial;
            StartCoroutine(Reset("neutral"));
        }
    }

    private void UpdateClocks(float time)
    {
        if (blueClock != null && orangeClock != null)
        {
            int seconds = Mathf.FloorToInt(time % 60);
            string secondsString = seconds.ToString();
            if (seconds < 10)
            {
                secondsString = "0" + secondsString;
            }
            string clock = Mathf.FloorToInt(time / 60) + ":" + secondsString;
            blueClock.text = clock;
            orangeClock.text = clock;
        }
    }

    private void DisplayWinner()
    {
        if (blueScore > orangeScore)
        {
            winnerDisplay1.text = "Blue Team Wins!";
            winnerDisplay2.text = "Blue Team Wins!";
        } else if (blueScore < orangeScore)
        {
            winnerDisplay1.text = "Orange Team Wins!";
            winnerDisplay2.text = "Orange Team Wins!";
        } else
        {
            winnerDisplay1.text = "Draw";
            winnerDisplay2.text = "Draw";
        }
        winnerDisplay1.gameObject.SetActive(true);
        winnerDisplay2.gameObject.SetActive(true);
    }

    public void RecognizeSpeech(string keyword) 
    {
        switch (keyword)
        {
            case "open":
                PassToPlayer();
                break;
            case "shoot":
                ShootCommand();
                break;
        }
    }
    

    private void PassToPlayer()
    {
        if (ball.transform.parent != null)
        {
            AIDummyController dummyWithBall = ball.transform.parent.parent.gameObject.GetComponent<AIDummyController>();
            if (dummyWithBall != null)
            {
                dummyWithBall.PassToPlayer(mainPlayer.gameObject);
            }
        }
    }

    private void ShootCommand()
    {
        if (ball.transform.parent != null)
        {
            AIDummyController ai = ball.transform.parent.parent.gameObject.GetComponent<AIDummyController>();
            if (ai != null)
            {
                if (ai.gameObject.GetComponent<Player>().team == mainPlayer.team)
                {
                    ai.Shoot();
                }
            }
        }
    }

    public void Resume()
    {
        menu.SetActive(false);
        menuScene = false;
    }

    public void PauseGame()
    {
        menu.SetActive(true);
        menuScene = true;
    }
}


