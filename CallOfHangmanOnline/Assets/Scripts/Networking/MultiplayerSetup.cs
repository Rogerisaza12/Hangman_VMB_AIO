using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerSetup : MonoBehaviour {

	public NetworkManager myNetworkManager;

	public void StartBeingHosting()
	{
		myNetworkManager.StopClient();
		myNetworkManager.StartHost();

        UIFacade.Singleton.SetActiveOnlineMultiplayer(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);
	}

	public void StartBeingClient()
	{
		myNetworkManager.StopHost();
		myNetworkManager.StopClient();
		myNetworkManager.StartClient();

        UIFacade.Singleton.SetActiveOnlineMultiplayer(false);
        UIFacade.Singleton.SetActiveLocalMultiplayer(true);
    }
}
