using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class LeaderboardSDKClient : MonoBehaviour
{
	public string Url;
	public static LeaderboardSDKClient Instance;

	[HideInInspector]
	public string userID;

	[HideInInspector]
	public string username;

	void Awake()
	{
		Instance = this;
		Utilities.ValidateForNull(Url);
	}


	//POST https://functionURL/api/scores
	public void CreateScore(Score instance, Action<CallbackResponse<User>> oncreateScoreCompleted)
	{
		instance._id = Instance.userID;
		instance.username = Instance.username;
		Utilities.ValidateForNull(instance, oncreateScoreCompleted);
		StartCoroutine(PostScoreInternal(instance, oncreateScoreCompleted));
	}
		
	//GET https://functionURL/api/scores/top/:count
	public void ListTopScores(int count, int skipCount, Action<CallbackResponse<Score[]>> callback)
	{
		Debug.Log ("starting");
		Utilities.ValidateForNull(callback);
		StartCoroutine(GetStuffArray<Score>("/scores/top/" + count, skipCount, callback));
		Debug.Log ("Done");
	}
		

	private IEnumerator PostScoreInternal(Score instance, Action<CallbackResponse<User>> onInsertCompleted)
	{
		string json = JsonUtility.ToJson(instance);
	
		using (UnityWebRequest www = Utilities.BuildScoresAPIWebRequest(GetLeaderboardsAPIURL() + "scores",
			HttpMethod.Post.ToString(), json, userID, username))
		{
			yield return www.SendWebRequest();
			if (Globals.DebugFlag) Debug.Log(www.responseCode);
		
			CallbackResponse<User> response = new CallbackResponse<User>();

			if (Utilities.IsWWWError(www))
			{
				if (Globals.DebugFlag) Debug.Log(www.error);

				Utilities.BuildResponseObjectOnFailure(response, www);
			}

			else if (www.downloadHandler != null)  //all OK
			{
				//let's get the new object that was created
				try
				{
					User newObject = JsonUtility.FromJson<User>(www.downloadHandler.text);
					if (Globals.DebugFlag) Debug.Log("new object is " + newObject.ToString());
					response.Status = CallBackResult.Success;
					response.Result = newObject;
				}
				catch (Exception ex)
				{
					response.Status = CallBackResult.DeserializationFailure;
					response.Exception = ex;
				}
			}
			onInsertCompleted(response);
			www.Dispose ();
		}
	}

	private IEnumerator GetStuffSingle<T>(string url, Action<CallbackResponse<T>> callback)
	{
		using (UnityWebRequest www = Utilities.BuildScoresAPIWebRequest
			(GetLeaderboardsAPIURL() + url, HttpMethod.Get.ToString(), null, userID, username))
		{
		yield return www.Send();
		if (Globals.DebugFlag) Debug.Log(www.responseCode);
			CallbackResponse<T> response = new CallbackResponse<T>();
			if (Utilities.IsWWWError(www))
			{
				if (Globals.DebugFlag) Debug.Log(www.error);
				Utilities.BuildResponseObjectOnFailure(response, www);
			}
			else
			{
				try
				{
					T data = JsonUtility.FromJson<T>(www.downloadHandler.text);
					response.Result = data;
					response.Status = CallBackResult.Success;
				}
				catch (Exception ex)
				{
					response.Status = CallBackResult.DeserializationFailure;
					response.Exception = ex;
				}
			}
			callback(response);
			www.Dispose ();
		}
	}

	private IEnumerator GetStuffArray<T>(string url, int skipCount, Action<CallbackResponse<T[]>> callback)
	{
		string fullurl = GetLeaderboardsAPIURL() + url;
		if (skipCount < 0)
			throw new ArgumentException("skipCount cannot be less than zero");
		else if (skipCount > 0)
			fullurl += "?skip=" + skipCount;

		using (UnityWebRequest www = Utilities.BuildScoresAPIWebRequest
			(fullurl, HttpMethod.Get.ToString(), null, userID, username))
		{
			yield return www.SendWebRequest();
			if (Globals.DebugFlag) Debug.Log(www.responseCode);
			var response = new CallbackResponse<T[]>();
			if (Utilities.IsWWWError(www))
			{
				if (Globals.DebugFlag) Debug.Log(www.error);
				Utilities.BuildResponseObjectOnFailure(response, www);
			}
			else
			{
				try
				{
					Debug.Log(www.downloadHandler.text);
					T[] data = JsonHelper.GetJsonArray<T>(www.downloadHandler.text);
					response.Result = data;
					response.Status = CallBackResult.Success;
				}
				catch (Exception ex)
				{
					response.Status = CallBackResult.DeserializationFailure;
					response.Exception = ex;
				}
			}
			callback(response);
			www.Dispose ();
		}
	}

	private string GetLeaderboardsAPIURL()
	{
		return string.Format("{0}/api/", Url);
	}


}
