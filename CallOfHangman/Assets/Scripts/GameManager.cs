using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private Player playerPrefab;
    [SerializeField]
    private PlayerAI playerAIPrefab;

    public int gameMode // 0. No game selected, 1. Singleplayer, 2. Local Multiplayer
    {
        get; private set;
    }

    public int turn
    {
        get; private set;
    }

    public int playerInTurn // 0. Player 1, 1. Player 2
    {
        get; private set;
    }

    public Player[] players
    {
        get; private set;
    }
    //Players indexer
    public Player this[int i]
    {
        get
        {
            return players[i];
        }
        private set
        {
            players[i] = value;
        }
    }

    //Singleton!
    public static GameManager Singleton
    {
        get; private set;
    }

    private void Awake()
    {
        if (Singleton != null)
            DestroyImmediate(gameObject);
        else
            Singleton = this;
    }

    private void Start()
    {
        SuscribeToGameModes();
    }

    public void SetupSingleplayer()
    {
        gameMode = 1;

        CreatePlayerWithAI();

        SuscribeToSingleplayerEvents();

        UIFacade.Singleton.singleplayer.Setup();
        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.singleplayer.SetActiveSingleplayer(true);
    }

    public void SetupLocalMultiplayer()
    {
        gameMode = 2;

        CreatePlayers();

        SuscribeToLocalMultiplayerEvents();

        UIFacade.Singleton.localMultiplayer.Setup();
        UIFacade.Singleton.SetActiveMainMenu(false);
        UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayer(true);
    }

    public void NextTurn()
    {
        turn++;

        if (playerInTurn == 0)
            Observer.Singleton.PlayerOneEndsTurn();
        else
            Observer.Singleton.PlayerTwoEndsTurn();

        playerInTurn = turn % 2;
    }

    private void CreatePlayers()
    {
        players = new Player[2];

        players[0] = Instantiate(playerPrefab) as Player;
        players[0].SetIndex(0);
        players[1] = Instantiate(playerPrefab) as Player;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void CreatePlayerWithAI()
    {
        players = new Player[2];

        players[0] = Instantiate(playerPrefab) as Player;
        players[0].SetIndex(0);

        PlayerAI playerAI = Instantiate(playerAIPrefab) as PlayerAI;

        players[1] = playerAI;
        players[1].SetIndex(1);

        players[1].gameObject.SetActive(false);
    }

    private void SuscribeToGameModes()
    {
        //Subscribing to the game modes
        Observer.Singleton.onSingleplayer += SetupSingleplayer;
        Observer.Singleton.onLocalMultiplayer += SetupLocalMultiplayer;
    }

    private void SuscribeToSingleplayerEvents()
    {
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterSingleplayer += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnterSingleplayer += NextTurn;
        Observer.Singleton.onWordInputFieldEnterSingleplayer += UIFacade.Singleton.singleplayer.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += NextTurn;
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += UIFacade.Singleton.singleplayer.ClearInputFields;
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += UIFacade.Singleton.singleplayer.UpdateLetterSectionColor;

        //AI!
        PlayerAI playerAI = players[1].GetComponent<PlayerAI>();

        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterSingleplayer += playerAI.SelectWord;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterSingleplayer += playerAI.Play;
    }

    private void SuscribeToLocalMultiplayerEvents()
    {
        //Subscribing to the word input field events
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += SetPlayerWord;
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += NextTurn;
        Observer.Singleton.onWordInputFieldEnterLocalMultiplayer += UIFacade.Singleton.localMultiplayer.ClearInputFields;
        //Subscribing to the letter input field events
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += CheckForCharOnRivalPlayerWord;
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += NextTurn;
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += UIFacade.Singleton.localMultiplayer.ClearInputFields;
        Observer.Singleton.onLetterInputFieldEnterLocalMultiplayer += UIFacade.Singleton.localMultiplayer.UpdateLetterSectionColor;
    }

    //SET PLAYER WORD CASES!

    public void SetPlayerWord()
    {
        switch (gameMode)
        {
            case 1:
                SetPlayerWordCaseSingleplayer();
                break;

            case 2:
                SetPlayerWordCaseLocalMultiplayer();
                break;
        }
    }

    private void SetPlayerWordCaseSingleplayer()
    {
        players[0].SetWord(UIFacade.Singleton.singleplayer.currentInputFieldText);

        UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(0, false);
        UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(1, true);

        StartCoroutine(ExecuteTimer());
    }

    private void SetPlayerWordCaseLocalMultiplayer()
    {
        players[playerInTurn].SetWord(UIFacade.Singleton.localMultiplayer.currentInputFieldText);

        if (turn == 0)
            UIFacade.Singleton.localMultiplayer.playerTurnWordScreen.text =
                string.Format("Player 1 close your eyes, Player 2 select a word.");
        if (turn == 1)
        {
            UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(0, false);
            UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(1, true);

            StartCoroutine(ExecuteTimer());
        }
    }

    //CHECK FOR CHAR ON RIVAL PLAYER WORD CASES!

    public void CheckForCharOnRivalPlayerWord()
    {
        switch (gameMode)
        {
            case 1:
                CheckForCharOnRivalPlayerWordCaseSingleplayer();
                break;

            case 2:
                CheckForCharOnRivalPlayerWordCaseLocalMultiplayer();
                break;

            default:
                return;
        }
    }

    public void CheckForCharOnRivalPlayerWordAI(char letter)
    {
        char playedChar = letter;

        //Is the char selected alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        int otherPlayer = (playerInTurn + 1) % 2;

        Dictionary<int, char> correctChars = players[otherPlayer].CheckForCharsInWord(playedChar);

        //Are not matched chars in the word ???
        if (correctChars.Count == 0)
            UIFacade.Singleton.singleplayer.UpdateErrors(playerInTurn);
        else
            UIFacade.Singleton.singleplayer.UpdateSuccess(playerInTurn, correctChars);

        if (players[playerInTurn].errorsCount >= 10)
        {
            GameOver(otherPlayer);
            return;
        }

        if (players[playerInTurn].sucessCount == players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
            return;
        }
    }

    private void CheckForCharOnRivalPlayerWordCaseSingleplayer()
    {
        char playedChar = UIFacade.Singleton.singleplayer.currentInputFieldText[0];

        //Is the char selected alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        int otherPlayer = (playerInTurn + 1) % 2;

        Dictionary<int, char> correctChars = players[otherPlayer].CheckForCharsInWord(playedChar);

        //Are not matched chars in the word ???
        if (correctChars.Count == 0)
        {
            UIFacade.Singleton.singleplayer.UpdateErrors(playerInTurn);
        }
        else
        {
            UIFacade.Singleton.singleplayer.UpdateSuccess(playerInTurn, correctChars);

            AudioManager.Singleton.PlayEffect("Success");
        }

        if (players[playerInTurn].errorsCount >= 10)
        {
            GameOver(otherPlayer);
            return;
        }

        if (players[playerInTurn].sucessCount == players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
            return;
        }
    }

    private void CheckForCharOnRivalPlayerWordCaseLocalMultiplayer()
    {
        char playedChar = UIFacade.Singleton.localMultiplayer.currentInputFieldText[0];

        //Is the char selected alredy played ???
        if (players[playerInTurn].playedChars.Contains(playedChar))
            return;
        else
            players[playerInTurn].playedChars.Add(playedChar);

        int otherPlayer = (playerInTurn + 1) % 2;

        Dictionary<int, char> correctChars = players[otherPlayer].CheckForCharsInWord(playedChar);

        //Are not matched chars in the word ???
        if (correctChars.Count == 0)
        {
            UIFacade.Singleton.localMultiplayer.UpdateErrors(playerInTurn);
        }
        else
        {
            UIFacade.Singleton.localMultiplayer.UpdateSuccess(playerInTurn, correctChars);

            AudioManager.Singleton.PlayEffect("Success");
        }

        if (players[playerInTurn].errorsCount >= 10)
        {
            GameOver(otherPlayer);
            return;
        }

        if (players[playerInTurn].sucessCount == players[otherPlayer].wordCharsArray.Length)
        {
            GameOver(playerInTurn);
            return;
        }
    }

    //GAME OVER CASES!

    public void GameOver(int winner)
    {
        switch (gameMode)
        {
            case 1:
                GameOverCaseSingleplayer(winner);
                break;

            case 2:
                GameOverCaseLocalMultiplayer(winner);
                break;

            default:
                break;
        }
    }

    private void GameOverCaseSingleplayer(int winner)
    {
        if (UIFacade.Singleton.singleplayer.timer.isWatching)
            UIFacade.Singleton.singleplayer.timer.StopWatch();

        UIFacade.Singleton.singleplayer.SaveMatchData(winner);

        UIFacade.Singleton.singleplayer.SetWinnerScreen(winner);
        UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(1, false);
        UIFacade.Singleton.singleplayer.SetActiveSingleplayerScreen(2, true);
    }

    private void GameOverCaseLocalMultiplayer(int winner)
    {
        if (UIFacade.Singleton.localMultiplayer.timer.isWatching)
            UIFacade.Singleton.localMultiplayer.timer.StopWatch();

        UIFacade.Singleton.localMultiplayer.SaveMatchData(winner);

        UIFacade.Singleton.localMultiplayer.SetWinnerScreen(winner);
        UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(1, false);
        UIFacade.Singleton.localMultiplayer.SetActiveLocalMultiplayerScreen(2, true);
    }

    private IEnumerator ExecuteTimer()
    {
        yield return null;

        switch (gameMode)
        {
            case 1:
                UIFacade.Singleton.singleplayer.timer.StartWatch();
                break;

            case 2:
                UIFacade.Singleton.localMultiplayer.timer.StartWatch();
                break;

            default:
                break;
        }
    }
}
