using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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

    public static RopeController Rope
    {
        get
        {
            return GameObject.FindWithTag("GrappleRope").GetComponent<RopeController>();
        }
    }

    private void Awake()
	{
        
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;
        
        if (Instance != null && Instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			Instance = this;
		}
	}
}
