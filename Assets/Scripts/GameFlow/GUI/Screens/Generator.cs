using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Generator : SingletonMonoBehaviour<Generator> 
{
    #region Variables

	[SerializeField] GameObject block;
	[SerializeField] GameObject bonus;
	[SerializeField] GameObject circle;
	[SerializeField] GameObject timing;
	[SerializeField] tk2dTextMesh timingText;
	ObjectPool poolBlock;
    ObjectPool poolBonus;
	const float circle_x = 320f;
	const float circle_y = 400f;
	const float circle_z = 0.31f;
	const float xx = 45.7f;
	const float yy = 81.11868f;
	const float zz = 0.31f;
	const float speed = 160f;
	const int numberCells = 7;
	[SerializeField] int flagReset = 0;
	[SerializeField] bool isMoveCircle;
    [SerializeField] bool isStart = false;
	[SerializeField] string scope;
	[SerializeField] bool isPause = false;
    int iterator = 7;
    int next = 3;
	const string tagBlock = "Block";
	const string tagBonus = "BonusBlock";
	List<List<GameObject>> table = new List<List<GameObject>>();

    #endregion


    #region Properties

	public GameObject Circle
	{
		get
		{
			return circle;
		}
	}


	public bool IsPause
	{
		get
		{ 
			return isPause;
		}
		set
		{ 
			isPause = value;
		}
	}


	public string Scope
	{
		get
		{ 
			return scope;
		}
		set
		{
			scope = value;
		}
	}


	public GameObject Timing
	{
		get
		{
			return timing;
		}
	}


	public string TimingText
	{
		set
		{ 
			timingText.text = value;
		}
	}


    public int FlagReset
    {
        get
        {
            return flagReset;
        }
        set
        {
            flagReset = value;
        }
    }


    public bool MoveCircle
    {
        get
        {
            return isMoveCircle;
        }
        set
        {
            isMoveCircle = value;
        }
    }


    public bool StartField
    {
        get
        {
            return isStart;
        }
        set
        {
            isStart = value;
        }
    }

    #endregion


    #region Unity lifecycle

    void Start() 
	{
		poolBlock = PoolManager.Instance.PoolForObject(block);
        poolBonus = PoolManager.Instance.PoolForObject(bonus);
		circle.transform.position = new Vector3(circle_x,circle_y,circle_z);
		BuildStart();
	}


	void Update() 
	{
		if (isMoveCircle) 
		{
			CircleChange();
		}
		if (table[2][0].transform.position.y <= yy*3.1)
		{
			AddRange();
			table.RemoveAt(0);
		}
		for (int i = 0; i < table.Count; i++)
		{
			for (int j = 0; j < numberCells; j++)
			{
				if (table[i][j] != null)
				{
					Vector3 vector = new Vector3(0,-1,0);
					table[i][j].transform.Translate(vector * speed * Time.deltaTime);
					StartField = true;
				}
			}
		}
	}

    #endregion
	

	#region Methods

    public void Restart()
	{
		flagReset += 1;
		for (int i = 0; i < table.Count; i++)
		{
			for (int j = 0; j < numberCells; j++)
			{
				if (table[i][j] != null)
				{
					if (table[i][j].CompareTag(tagBlock))
					{
						poolBlock.Push(table[i][j]);
					} 
					else
					{
						if (table[i][j].CompareTag(tagBonus))
						{
							poolBonus.Push(table[i][j]);
						}
					}
				}
			}
		}
		table.RemoveRange(0, table.Count);
		iterator = 7;
		next = 3;
		BuildStart();
		circle.transform.position = new Vector3(circle_x,circle_y,circle_z);
	}


	void CircleChange()
	{
		if (Input.GetKey(KeyCode.Mouse0) && Time.timeScale != 0) 
		{
			circle.transform.position = new Vector3(Input.mousePosition.x, circle_y, circle_z);
		}
	}


	GameObject CreateNewBlock(float x = xx, float y = yy, float z = zz)
	{
        GameObject gameObject = poolBlock.Pop();
        gameObject.transform.position = new Vector3(x, y, z);
        return gameObject;
	}


	void BuildStart()
	{
		int numberY = 1;
		int counter = 7;
		for (int j = 0; j < 8; j++)
		{
			List<GameObject> range = new List<GameObject>();
			int numberX = 1;
			if (j > 0 && j % 2 == 0)
			{
				counter--;
			}
			int indentation = 7 - counter;
			for (int q = 0; q < indentation; q++)
			{
				range.Add(CreateNewBlock(x:xx * numberX, y:yy * numberY));
				numberX += 2;
			}
			for (int q = 0; q < (7 - indentation * 2); q++)
			{
				range.Add(null);
				numberX += 2;
			}
			for (int q = 0; q < indentation; q++)
			{
				range.Add(CreateNewBlock(x:xx * numberX, y:yy * numberY));
				numberX += 2;
			}
			table.Add(range);
			numberY += 2;
		}
		Time.timeScale = 0;
	}
    

    void AddRange()
    {
        List<GameObject> line = new List<GameObject>();
		for (int j = 0; j < numberCells; j++)
		{
            if (table[0][j] != null)
            {
                if (table[0][j].CompareTag("Block"))
                {
                    poolBlock.Push(table[0][j]);
                }
                else
                {
					if(table[0][j].CompareTag(tagBonus))
                    {
                        poolBonus.Push(table[0][j]);
                    }
                }
            }
		}
        if (iterator % 2 != 0)
        {
            line = GenerateLabyrinth();
        }
        else
        {
			for (int i = 0; i < numberCells; i++)
            {
                if (i == next)
                {
                    if (iterator % 12 == 0)
                    {
                        float x_new = xx * 1 + xx * 2 * i;
                        float y_new = yy * 15;
                        GameObject bonus = poolBonus.Pop();
                        bonus.transform.position = new Vector3(x_new, y_new, zz);
                        line.Add(bonus);
                    }
                    else
                    {
                        line.Add(null);
                    }
                }
                else
                {
                    float x_new = xx * 1 + xx * 2 * i;
                    float y_new = yy * 15;
                    GameObject g_object = CreateNewBlock(x_new, y_new, zz);
                    line.Add(g_object);
                }
            }
        }
        iterator++;
        table.Add(line);
    }


	List<GameObject> GenerateLabyrinth()
	{
		List<GameObject> range = new List<GameObject> { null, null, null, null, null, null, null };
		int value = UnityEngine.Random.Range (2, numberCells - 2);
		bool isCheck = false;
		int begin = 1;
		int left = next - 0;
		int right = numberCells - 1 - next;
		do
		{
			isCheck = false;
			if (left <= 1)
			{
				begin = 1;
				next = value;
			}
			else
			{
				if (left >= value)
				{
					begin = next - value + 1;
					next = begin;
				}
				else
				{
					if (right <= 1)
					{
						begin = numberCells - 1 - value;
						next = numberCells - 2;
					}
					else
					{
						if (right >= value)
						{
							begin = next;
							next = next + value - 1;
						}
						else
						{
							value--;
							isCheck = true;
						}
					}
				}
			}
		}
		while (isCheck);
		for (int i = 0; i < numberCells; i++)
		{
			if (value > 0 && i >= begin)
			{
				value--;
			}
            else
            {
                float x_new = xx * 1 + xx * 2 * i;
                float y_new = yy * 15;
                GameObject g_object = CreateNewBlock(x_new, y_new, zz);
                range[i] = g_object;
            }
        }
		return range;
	}
		
    #endregion
}
