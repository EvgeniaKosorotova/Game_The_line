using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Circle : MonoBehaviour 
{
    #region Variables

    string action = "decrease";
    bool isWorkTimer = false;
	float totalTimeValue = 0f;
	float timeToAction = 7f;
	float time = 7f;
	float timeOutSave = 0f;
	const string bonus1 = "decrease";
	const string bonus2 = "invulnerability";
	const string gameOver = "gameOver";
	const string gameOverWithBonus = "gameOverWithBonus";
	const string tagBlock = "Block";
	const string tagBonus = "BonusBlock";

    #endregion


    #region Unity lifecycle

	void Update()
	{
		if (isWorkTimer)
		{
			if (action != gameOver)
			{
				Generator.Instance.Timing.SetActive(true);
				float timeOut = totalTimeValue - totalTimeValue % 1;
				if (timeOut > timeOutSave)
				{
					timeOutSave = timeOut;
					time -= 1;
					Generator.Instance.TimingText = time + " sec";
				}
			}
			if (!Generator.Instance.IsPause)
			{
				totalTimeValue += Time.unscaledDeltaTime;
			}
			if (totalTimeValue > timeToAction)
			{
				if (action == bonus1)
				{
//					transform.localScale = new Vector3(transform.localScale.x / 0.7f, transform.localScale.y / 0.7f, transform.localScale.z / 0.7f);
					transform.localScale = Vector3.one / 0.7f;
					action = bonus2;
				} 
				else 
				{
					if (action == bonus2)
					{
						action = bonus1;
					}
				}
				if (action == gameOver)
				{
					GameManager.Instance.CurrentState = GameState.GameOver;
					action = bonus1;
				}
				if (action == gameOverWithBonus)
				{
					GameManager.Instance.CurrentState = GameState.GameOver;
					action = bonus1;
					transform.localScale = Vector3.one / 0.7f;
				}
				totalTimeValue = 0f;
				Generator.Instance.Timing.SetActive(false);
				time = 7f;
				isWorkTimer = false;
			}
		} 
		else
		{
			Generator.Instance.Timing.SetActive(false);
			time = 7f;
			Generator.Instance.TimingText = time + " sec";
			timeOutSave = 0f;
			totalTimeValue = 0f;
		}
	}


    void OnCollisionEnter(Collision collision)
    {
		if (collision.gameObject.CompareTag(tagBlock) && !isWorkTimer)
		{
			Time.timeScale = 0;
            action = gameOver;
            timeToAction = 1f;
			isWorkTimer = true;
        }
        else
        {
			if (collision.gameObject.CompareTag(tagBonus) && !isWorkTimer)
            {
				Vector3 vector = collision.transform.position;
				collision.transform.Translate(Vector3.one * -10000);
                timeToAction = 7f;
				isWorkTimer = true;
            }
        }
		if (isWorkTimer) 
		{
			ActionBonus(collision);
		}
    }

    #endregion


    #region Methods

    void ActionBonus(Collision collision)
    {
		if (action == bonus2)
        {
			collision.transform.Translate(Vector3.one * -10000);
        }
        else
        {
			if (action == bonus1)
            {
				if (collision.gameObject.CompareTag(tagBlock)) 
				{
					Time.timeScale = 0;
					action = gameOverWithBonus;
					totalTimeValue = 0f;
					timeToAction = 1f;
					isWorkTimer = true;
				} 
				else 
				{
					transform.localScale = Vector3.one * 0.7f;
				}
			}
        }
    }
    
    #endregion
}
