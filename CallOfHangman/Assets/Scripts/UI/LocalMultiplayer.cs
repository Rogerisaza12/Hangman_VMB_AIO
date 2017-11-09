public class LocalMultiplayer : UserInterface {

    public void SetActiveLocalMultiplayer(bool state)
    {
        gameModeScreen.SetActive(state);
    }

    public void SetActiveLocalMultiplayerScreen(int screen, bool state)
    {
        if (screen < 0 || screen > screens.Length - 1)
            return;

        screens[screen].SetActive(state);
    }
}
