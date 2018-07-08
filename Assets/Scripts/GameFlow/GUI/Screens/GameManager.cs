using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
	Game = 0,
	Pause = 1,
	GameOver = 2
}
		

public class GameManager : SingletonMonoBehaviour<GameManager> 
{
	#region Variables

	public static event System.Action<GameState> OnGameStateChanged;
	GameState currentState;

	#endregion


	#region Property

	public GameState CurrentState
	{
		get 
		{
			return currentState;
		}
		set
		{
			if (currentState != value) 
			{
				currentState = value;
				if (OnGameStateChanged != null) 
				{
					OnGameStateChanged(currentState);
				}
			}
		}
	}

	#endregion
}
