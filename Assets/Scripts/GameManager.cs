using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;              
     void Start()
     {
		 
		// This locks the cursor
		Cursor.lockState = CursorLockMode.Locked;
		
		// If you unlock the cursor, but its still invisible, try this:
		Cursor.visible = false;

         //Check if instance already exists
         if (instance == null)
             
             //if not, set instance to this
             instance = this;
         
         //If instance already exists and it's not this:
         else if (instance != this)
             
             //Then destroy this. This enforces our singleton pattern, 
             // meaning there can only ever be one instance of a GameManager.
             Destroy(gameObject);    
         
         //Sets this to not be destroyed when reloading scene / Switching scenes
         DontDestroyOnLoad(gameObject); // VERY IMPORTANT
         
     }     

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

	public static Volume PostProcess
	{
		get
		{
			return GameObject.Find("Post Process Volume").GetComponent<Volume>();
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
}
