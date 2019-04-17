
using UnityEngine;
using UnityEngine.UI;


public class LeaderboardUIControl : MonoBehaviour
{

	public BirbMovement birb;
	public InputField userInput;
	public Text statusText;
	public Button submitButton;
	public bool loading = false;

	public void Start()
	{
		Globals.DebugFlag = true;

		//get the authentication token(s) somehow...
		//e.g. for facebook, check the Unity Facebook SDK at https://developers.facebook.com/docs/unity
		LeaderboardSDKClient.Instance.userID = "1234";
		LeaderboardSDKClient.Instance.username = "Unnamed_Player";

		//check here for more information regarding authentication and authorization in Azure App Service
		//https://azure.microsoft.com/en-us/documentation/articles/app-service-authentication-overview/
	}

	public void updatePlayerName() {
		LeaderboardSDKClient.Instance.username = userInput.text;
	}

	public void submitScore() {
		submitButton.enabled = false;
		CreateScore ();
	}


	public void ClearOutput()
	{
		statusText.text = string.Empty;
	}
		
	public void CreateScore()
	{
		ClearOutput ();
		Score score = new Score();

		score.value = birb.score;

		LeaderboardSDKClient.Instance.CreateScore(score, response =>
			{
				if (response.Status == CallBackResult.Success)
				{
					Debug.Log("Create score completed");
					ListTopScores ();
				}
				else
				{
					Debug.Log(response.Exception.Message);
				}
			});
		Debug.Log("Loading...");
	}

	public void ListTopScores()
	{
		ClearOutput ();
		//get the top 10 scores for all users
		LeaderboardSDKClient.Instance.ListTopScores(10, 0, response =>
			{
				Debug.Log(response.Status);
				if (response.Status == CallBackResult.Success)
				{
					Debug.Log(response);
					Debug.Log(response.Result.Length);
					foreach (var item in response.Result)
					{
						Debug.Log(item.username);
						WriteLine(string.Format("{0}: {1} points", item.username, item.value));
					}
				}
				else
				{
					Debug.Log(response.Exception.Message);
				}
			});
		Debug.Log("Loading...");
	}
		

	public void WriteLine(string s)
	{
		statusText.text += s + "\r\n";
	}

}




