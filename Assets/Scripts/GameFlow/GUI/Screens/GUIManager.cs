using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : SingletonMonoBehaviour<GUIManager> 
{
    #region Variables

    [SerializeField] GameObject menuScreen;
	[SerializeField] GameObject gameScreen;
	[SerializeField] GameObject gameOverScreen;
	[SerializeField] GameObject generator;

    #endregion


	#region Unity lifecycle

	void OnEnable()
	{
		generator.SetActive (true);
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
		GameManager.Instance.CurrentState = GameState.Game;
		Generator.Instance.Circle.SetActive(true);
	}


	void OnDisable()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

	#endregion


    #region Private Methods

	void MenuScreenChange()
	{
		gameOverScreen.SetActive(false);
		gameScreen.SetActive(false);
		menuScreen.SetActive(true);
	}


	void GameScreenChange()
	{
		menuScreen.SetActive(false);
		gameOverScreen.SetActive(false);
		gameScreen.SetActive(true);
	}


	void GameOverScreenChange()
	{
		menuScreen.SetActive(false);
		gameScreen.SetActive(false);
		gameOverScreen.SetActive(true);
	}

    #endregion


	#region Events Handlers

	void GameManager_OnGameStateChanged(GameState state)
	{
		switch (state)
		{
		case GameState.Game:
			GameScreenChange();
			break;
		case GameState.Pause:
			MenuScreenChange();
			break;
		case GameState.GameOver:
			GameOverScreenChange();
			break;
		}
	}

	#endregion
}
