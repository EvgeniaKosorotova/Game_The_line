using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameOverScreen : MonoBehaviour {

	#region Variables

	[SerializeField] tk2dUIItem tryAgainButton;
	[SerializeField] tk2dTextMesh scope;
	[SerializeField] tk2dTextMesh bestScope;
	int best = 0;


	#endregion


	#region Properties

	public string Scope
	{
		set
		{ 
			scope.text = "Scope\n" + value;
		}
	}

	#endregion


	#region Unity lifecycle

	void OnEnable()
	{
		tryAgainButton.OnClick += TryAgainButton_OnClick;
		Scope = Generator.Instance.Scope;
		GetBestTime();
	}


	void OnDisable()
	{
		tryAgainButton.OnClick -= TryAgainButton_OnClick;
	}

	#endregion


	#region Events handlers

	void TryAgainButton_OnClick()
	{
		Generator.Instance.Restart();
		GameManager.Instance.CurrentState = GameState.Game;
	}

	#endregion


	#region Private

	void GetBestTime()
	{
		best = PlayerPrefs.GetInt("Best");
		if (best < int.Parse(Generator.Instance.Scope))
		{
			PlayerPrefs.SetInt("Best", int.Parse(Generator.Instance.Scope));
			best = PlayerPrefs.GetInt("Best");
		}
		bestScope.text = "Best\n" + best;
	}

	#endregion
}
