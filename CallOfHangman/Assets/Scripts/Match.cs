[System.Serializable]
public class Match
{
	public string time;
    public int playerOneErrors;
    public int playerTwoErrors;
    public string winner;

    public Match(string time, int playerOneErrors, int playerTwoErrors, string winner)
    {
        this.time = time;
        this.playerOneErrors = playerOneErrors;
        this.playerTwoErrors = playerTwoErrors;
        this.winner = winner;
    }
}
