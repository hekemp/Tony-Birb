using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GManager : MonoBehaviour {

	private bool _isGameOver;
	private static GManager _instance;
	public Button playAgain;
	public Button returnToMenu;
	public Image gameOverPicture;

	public static GManager Instance {
		get {
			return _instance;
		}
	}

	public bool IsGameOver {
		get {
			return _isGameOver;
		}
		set {
			_isGameOver = value;
			if (value) {
				playAgain.gameObject.SetActive (true);
				returnToMenu.gameObject.SetActive (true);
				gameOverPicture.enabled = true;
			}
		}
	}

	// Use this for initialization
	void Start () {
		_isGameOver = false;
		if (Instance != null) {
			Destroy (this);
		} else {
			_instance = this;
		}

		gameOverPicture.enabled = false;
		playAgain.gameObject.SetActive (false);
		returnToMenu.gameObject.SetActive (false);
		playAgain.onClick.AddListener (RestartGame);
		returnToMenu.onClick.AddListener (exitToMenu);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void RestartGame() {
		SceneManager.LoadSceneAsync (1);
	}

	void exitToMenu() {
		SceneManager.LoadSceneAsync (0);
	}
}
