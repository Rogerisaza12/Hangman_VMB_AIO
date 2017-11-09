using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Assets/Score", order = 1)]
public class Score : ScriptableObject
{
    public List<Match> matches = new List<Match>();

    //MEMENTO!
    public void Save(string time, int playerOneErrors, int playerTwoErrors, string winner)
    {
        if (matches.Count == 5)
            matches.Remove(matches[0]);

        matches.Add(new Match(time, playerOneErrors, playerTwoErrors, winner));
    }

    public Match Load(int index)
    {
        if (index >= matches.Count || index < 0)
            return null;
        else
            return matches[index];
    }
}
