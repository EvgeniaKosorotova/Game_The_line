using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScreen : MonoBehaviour
{

    #region Variables

    [SerializeField] tk2dUIItem pauseButton;
    [SerializeField] tk2dUIItem touchButton;
    [SerializeField] tk2dTextMesh touchText;
    [SerializeField] tk2dTextMesh scope;
    [SerializeField] float time = 0;
    bool isFlagStart = true;
    float totalTimeValue = 0f;
	int flagResetGameGenerator = 0;

    #endregion


    #region Unity lifecycle

    void OnEnable()
	{
		pauseButton.OnClick += PauseButton_OnClick;
		touchButton.OnDown += TouchButton_OnDown;
		touchButton.OnUp += TouchButton_OnUp;
		Restart();
	}


	void OnDisable()
	{
		pauseButton.OnClick -= PauseButton_OnClick;
		touchButton.OnDown -= TouchButton_OnDown;
		touchButton.OnUp -= TouchButton_OnUp;
		Generator.Instance.Scope = time + "";
	}


	void Update()
	{
		if (Time.timeScale == 1)
		{
			totalTimeValue += Time.deltaTime;
			if (totalTimeValue > 0)
			{
				time = totalTimeValue - totalTimeValue % 1;
				scope.text = "Scope\n" + time;
			}
		}
	}

	#endregion


	#region Events handlers

	void PauseButton_OnClick()
	{
		Generator.Instance.IsPause = true;
		GameManager.Instance.CurrentState = GameState.Pause;
		Generator.Instance.StartField = false;
		Time.timeScale = 0;
		isFlagStart = true;
	}


	void TouchButton_OnDown()
	{
		Generator.Instance.MoveCircle = true;
		if (isFlagStart)
		{
			touchText.transform.position = Vector3.one * -10000f;
			Generator.Instance.StartField = isFlagStart;
			Time.timeScale = 1;
			isFlagStart = false;
			Generator.Instance.IsPause = false;
		}
	}


	void TouchButton_OnUp()
	{
		Generator.Instance.MoveCircle = false;
	}

	#endregion


	#region Methods

	void Restart()
	{
		if(flagResetGameGenerator < Generator.Instance.FlagReset) 
		{
			isFlagStart = true;
			touchText.transform.position = Vector3.one / -10000f;
			totalTimeValue = 0f;
			time = 0;
			scope.text = "Scope\n" + time;
			flagResetGameGenerator = Generator.Instance.FlagReset;
		}
	}

	#endregion
}