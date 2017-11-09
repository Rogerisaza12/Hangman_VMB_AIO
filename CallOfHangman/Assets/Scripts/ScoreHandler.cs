using UnityEngine;
using UnityEngine.UI;

public class ScoreHandler : MonoBehaviour {

    [SerializeField]
    private Score scoreAsset;

    [Space(10f)]

    [SerializeField]
    private Text[] matchData;
    
    //Singleton!
    public static ScoreHandler Singleton
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
        UpdateMatchTexts();
    }

    public void UpdateMatchTexts()
    {
        for (int i = 0; i < matchData.Length; i++)
        {
            if (i < scoreAsset.matches.Count)
            {
                Match actualMatch = scoreAsset.matches[i];

                matchData[i].text = string.Format("{0}. {1}, P1 errors: {2}, P2 errors: {3}, Winner: {4}",
                    (i + 1).ToString(),
                    actualMatch.time,
                    actualMatch.playerOneErrors,
                    actualMatch.playerTwoErrors,
                    actualMatch.winner);
            }
            else
                matchData[i].gameObject.SetActive(false);
        }
    }
}
