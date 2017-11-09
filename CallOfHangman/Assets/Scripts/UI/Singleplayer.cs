using UnityEngine.UI;

public class Singleplayer : UserInterface {

    public Button letterButton;

    public void SetActiveSingleplayer(bool state)
    {
        gameModeScreen.SetActive(state);
    }

    public void SetActiveSingleplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > screens.Length - 1)
            return;

        screens[screen].SetActive(state);
    }

    public void SetActiveLetterSection(bool state)
    {
        letterButton.interactable = state;
        letterInputField.interactable = state;
    }

    public override void SetWinnerScreen(int winner)
    {
        int losser = (winner + 1) % 2;

        if (winner == 0)
            playerWinner.text = "PLAYER 1 WIN!";
        else
            playerWinner.text = "PLAYER AI WIN!";

        playersWords[winner].text = GameManager.Singleton.players[winner].word;
        playersWords[losser].text = GameManager.Singleton.players[losser].word;

        playersErrors[0].text = playerOneErrors.text;
        playersErrors[1].text = playerTwoErrors.text;
    }
}
