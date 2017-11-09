using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Networking
{
    public class PlayerNetworking : NetworkBehaviour
    {
        public int index;

        public string word;

        public int errorsCount;

        public int sucessCount
        {
            get; private set;
        }

        public bool isFinished
        {
            get; private set;
        }

        public char[] wordCharsArray;

        public List<char> charsSelected = new List<char>();
        //Word chars indexer
        public char this[int i]
        {
            get
            {
                return wordCharsArray[i];
            }
            private set
            {
                wordCharsArray[i] = value;
            }
        }

        private void Start()
        {
            if (isServer)
                SetUpServerPlayers();

            else
                SetUpClientPlayers();
        }

        //RPC & COMMANDS!
        [Command]
        public void CmdUpdateIndex(int index)
        {
            RpcUpdateIndex(index);
        }

        [ClientRpc]
        public void RpcUpdateIndex(int index)
        {
            this.index = index;
        }

        [Command]
        public void CmdSetWord(string word)
        {
            RpcSetWord(word);
        }

        [ClientRpc]
        public void RpcSetWord(string word)
        {
            this.word = word;

            wordCharsArray = word.ToCharArray();

            if (index == 0)
            {
                for (int i = UIFacade.Singleton.playerOneEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerOneEmptyTexts[i].gameObject.SetActive(false);
            }
            else
            {
                for (int i = UIFacade.Singleton.playerTwoEmptyTexts.Length - 1; i > wordCharsArray.Length - 1; i--)
                    UIFacade.Singleton.playerTwoEmptyTexts[i].gameObject.SetActive(false);
            }

            Debug.Log(string.Format("Player {0} word: {1}", index, word));
        }

        [Command]
        public void CmdSetErrorsServer(int errorsCount)
        {
            RpcSetErrorsServer(errorsCount);
        }

        [ClientRpc]
        public void RpcSetErrorsServer(int errorsCount)
        {
            this.errorsCount = errorsCount;

            isFinished = true;
        }

        public void SetIndex(int index)
        {
            CmdUpdateIndex(index);

            if (isLocalPlayer)
                GameManagerNetworking.Singleton.SetLocalPlayerSettings(index);

            switch (index)
            {
                case 0:
                    Observer.Singleton.onPlayerTwoEndsTurn += Turn;
                    Observer.Singleton.onPlayerOneEndsTurn += EndTurn;
                    break;
                case 1:
                    Observer.Singleton.onPlayerOneEndsTurn += Turn;
                    Observer.Singleton.onPlayerTwoEndsTurn += EndTurn;
                    break;
                default:
                    return;
            }

            Debug.Log(string.Format("Player {0} created!", index));
        }

        public void SetWord(string word)
        {
            CmdSetWord(word);

            if (isLocalPlayer)
            {
                StartCoroutine(ChekForWordsInPlayers());

                UIFacade.Singleton.SetActiveLocalMultiplayerScreen(0, false);
                UIFacade.Singleton.SetActiveLocalMultiplayerScreen(2, true);
            }

            Debug.Log(string.Format("Player {0} word: {1}", index, word));
        }

        public Dictionary<int, char> CheckForCharsInWord(char inputChar)
        {
            Dictionary<int, char> charsInWord = new Dictionary<int, char>();

            for (int i = 0; i < wordCharsArray.Length; i++)
            {
                if (inputChar == wordCharsArray[i])
                    charsInWord.Add(i, wordCharsArray[i]);
            }

            return charsInWord;
        }

        public void IncreaseSuccessCount()
        {
            sucessCount++;
        }

        public void IncreaseErrorsCount()
        {
            errorsCount++;
        }

        public IEnumerator ChekForWordsInPlayers()
        {
            while (true)
            {
                if (GameManagerNetworking.Singleton.players[0].word != "" && GameManagerNetworking.Singleton.players[1].word != "")
                {
                    UIFacade.Singleton.SetActiveLocalMultiplayerScreen(0, false);
                    UIFacade.Singleton.SetActiveLocalMultiplayerScreen(2, false);
                    UIFacade.Singleton.SetActiveLocalMultiplayerScreen(1, true);

                    break;
                }

                yield return new WaitForSeconds(1f);
            }
        }

        private void SetUpServerPlayers()
        {
            if (isLocalPlayer)
                GameManagerNetworking.Singleton.SetHostPlayerOnline(gameObject);
            else
                GameManagerNetworking.Singleton.SetPlayerTwoClient(gameObject);
        }

        private void SetUpClientPlayers()
        {
            if (isLocalPlayer)
                GameManagerNetworking.Singleton.SetPlayerTwoClient(gameObject);
            else
                GameManagerNetworking.Singleton.SetHostPlayerOnline(gameObject);
        }

        private void Turn()
        {
            if (GameManagerNetworking.Singleton.onlineTurn > 1)
            {
                if (index == 0)
                    UIFacade.Singleton.playersWordsObject[1].SetActive(true);
                else
                    UIFacade.Singleton.playersWordsObject[0].SetActive(true);
            }
        }

        private void EndTurn()
        {
            if (GameManagerNetworking.Singleton.onlineTurn > 1)
            {
                if (index == 0)
                    UIFacade.Singleton.playersWordsObject[1].SetActive(false);
                else
                    UIFacade.Singleton.playersWordsObject[0].SetActive(false);
            }
        }
    }
}
