using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
	#region Variables

	[SerializeField] tk2dUIItem continueButton;
	[SerializeField] tk2dUIItem tryAgainButton;

	#endregion


	#region Unity lifecycle

	void OnEnable()
	{
		tryAgainButton.OnClick += TryAgainButton_OnClick;
		continueButton.OnClick += ContinueButton_OnClick;
	}


	void OnDisable()
	{
		continueButton.OnClick -= ContinueButton_OnClick;
		tryAgainButton.OnClick -= TryAgainButton_OnClick;
	}

	#endregion


	#region Events handlers

	void TryAgainButton_OnClick()
	{
		Generator.Instance.Restart();
		GameManager.Instance.CurrentState = GameState.Game;
	}


	void ContinueButton_OnClick()
	{
		GameManager.Instance.CurrentState = GameState.Game;
	}

	#endregion
}
