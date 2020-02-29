using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

	public static float adsSpeed = 20f;

    public static PlayerController Player
	{
		get
		{
			return GameObject.FindWithTag("Player").GetComponent<PlayerController>();
		}
	}

	 public static Transform Canvas
	{
		get
		{
			return GameObject.Find("/Canvas").transform;
		}
	}
	

	public static InputHandler Input
	{
		get
		{
			return Camera.main.GetComponent<InputHandler>();
		}
	}

	public static CameraController PlayerCamera
	{
		get
		{
			return Camera.main.GetComponent<CameraController>();
		}
	}

	public static WeaponController CurrentWeapon
	{
		get
		{
			return GameManager.Player.GetComponentInChildren<WeaponController>();
		}
	}

	public static Text DebugText
	{
		get
		{
			var text = GameObject.FindWithTag("DebugNumber");
			if(text != null){
				return text.GetComponent<Text>();
			}
			return null;
		}
	}

    public static RopeController Rope
    {
        get
        {
            GameObject rope = GameObject.FindWithTag("GrappleRope");
            if (!rope)
            {
                return null;
            }
            return rope.GetComponent<RopeController>();
        }
    }

	public static AudioController Audio
	{
		get
        {
            return Camera.main.GetComponent<AudioController>();
        }
	}

	public static ParticleSystem SpeedLines
	{
		get
        {
            return GameObject.Find("Speed Lines").GetComponent<ParticleSystem>();
        }
	}

    private void Awake()
	{
        Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		// Time. timeScale = 0.25f;
        // QualitySettings.vSyncCount = 0;  // VSync must be disabled
        // Application.targetFrameRate = 59;
        
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
