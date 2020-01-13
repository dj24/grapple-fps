using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Provides global access to core game instances.
    // e.g GameManager.Player can be called from anywhere to retrieve player controller

	private static GameManager _instance;

	public static GameManager Instance { get { return _instance; } }

	public static PlayerController Player
	{
		get
		{
			return GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		}
	}

	public static Text DebugText
	{
		get
		{
			return GameObject.FindWithTag("DebugNumber").GetComponent<Text>();
		}
	}

	private void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			_instance = this;
		}
	}
}
