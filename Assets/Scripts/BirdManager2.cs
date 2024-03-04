using MK.Glow;
using UnityEngine;

public class BirdManager2 : MonoBehaviour
{
	public GameObject[] bird;

	private GameObject topbird;

	private GameObject powerslider;

	private bool isrun;

	private bool iswin;

	private bool once = true;

	private bool ispowerup;

	public GameObject[] egg;

	private BirdBehaviour bb;

	public float speed;

	private float endpointx;

	private float powerstarttime;

	private float t1;

	private float prevposspeedup;

	private float cutoff;

	private UIHandler uihandler;

	public AudioClip[] eggsounds;

	private int selectedbird;

	private int soundinc;

	private int ismusic;

	private AudioSource ass;

	public GameObject bonustxt;

	public GameObject vignette;

	public GameObject haptic;

	private void Start()
	{
		selectedbird = PlayerPrefs.GetInt("squarebird_selectedchar");
		topbird = Object.Instantiate(bird[selectedbird], new Vector3(base.transform.position.x, 0.5f, 0f), Quaternion.identity);
		topbird.transform.SetParent(base.transform);
		bb = topbird.GetComponent<BirdBehaviour>();
		topbird.GetComponent<TrailRenderer>().enabled = true;
		uihandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
		ass = GetComponent<AudioSource>();
		ismusic = PlayerPrefs.GetInt("squarebird_ismusic");
		t1 = Time.time;
		powerslider = GameObject.Find("PowerSlider");
		powerslider.transform.SetParent(topbird.transform);
		powerslider.transform.position = new Vector3(0f, 1.5f, -3f);
		powerslider.SetActive(false);
	}

	public Sprite GetBirdImg(int i)
	{
		return bird[i + 1].GetComponent<SpriteRenderer>().sprite;
	}

	public bool IsPowerActive()
	{
		return ispowerup;
	}

	public void StartGame()
	{
	}

	public void UpdateMusicSetting()
	{
		ismusic = PlayerPrefs.GetInt("squarebird_ismusic");
	}

	private void Update()
	{
		if (uihandler.isplay)
		{
			if (ispowerup)
			{
				cutoff = powerslider.GetComponent<SpriteRenderer>().material.GetFloat("_Cutoff");
			}
			if (Input.GetMouseButtonDown(0) && !bb.isdead)
			{
				isrun = true;
			}
			if (Input.GetMouseButtonUp(0) && isrun && !bb.istouchtop)
			{
				Vector3 position = topbird.transform.position;
				Vector3 position2 = position;
				position.y += 0.75f;
				topbird.transform.position = position;
				GameObject gameObject = Object.Instantiate(egg[selectedbird], position2, Quaternion.identity);
				gameObject.transform.SetParent(base.transform);
				gameObject.GetComponent<Animator>().Play(0);
				if (ismusic == 0)
				{
					ass.clip = eggsounds[soundinc];
					ass.Play();
					if (soundinc == 4)
					{
						soundinc = 0;
					}
					else
					{
						soundinc++;
					}
				}
			}
			if (!bb.isdead)
			{
				base.transform.Translate(Vector2.right * Time.deltaTime * speed);
			}
			if (t1 + 0.5f < Time.time)
			{
				t1 = Time.time;
				uihandler.SetScore();
			}
			if (!ispowerup)
			{
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 9f, Time.deltaTime * 5f);
			}
			else
			{
				cutoff += 0.1f * Time.deltaTime;
				powerslider.GetComponent<SpriteRenderer>().material.SetFloat("_Cutoff", cutoff);
				Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, 10f, Time.deltaTime * 5f);
				if (cutoff >= 0.7f && ispowerup)
				{
					speed = 4.5f;
				}
				if (cutoff >= 0.9f && ispowerup)
				{
					powerslider.GetComponent<SpriteRenderer>().material.SetFloat("_Cutoff", 1f);
					DeActivePowerUpMode();
				}
			}
			uihandler.ChangeSlider(base.transform.position.x);
		}
		if (iswin)
		{
			Vector3 position3 = base.transform.position;
			if (endpointx > position3.x)
			{
				base.transform.position = Vector3.MoveTowards(position3, new Vector3(endpointx - 1f, position3.y, position3.z), Time.deltaTime * speed);
			}
		}
	}

	public void SetBlastBonusScore()
	{
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "+" + uihandler.SetBonusScore();
	}

	public void Win()
	{
		endpointx = GameObject.Find("Home(Clone)").transform.GetChild(0).transform.position.x - 1f;
		uihandler.isplay = false;
		iswin = true;
		if (once)
		{
			once = false;
			PlayerPrefs.SetInt("squarebird_levelno", PlayerPrefs.GetInt("squarebird_levelno") + 1);
			StartCoroutine(uihandler.GoToNextLevel(GameObject.Find("Home(Clone)").transform.position.x));
		}
	}

	public void Perfect1()
	{
		speed = 4.2f;
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "Perfect x1";
		Haptic();
	}

	public void Perfect2()
	{
		speed = 4.5f;
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "Perfect x2";
		Haptic();
	}

	public void Perfect3()
	{
		speed = 4.7f;
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "Perfect x3";
		Haptic();
	}

	public void Perfect4()
	{
		speed = 5f;
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "Perfect x4";
		Haptic();
	}

	private void Haptic()
	{
		haptic.SetActive(true);
		Vector3 position = Camera.main.transform.position;
		position.y -= 0.05f;
		Camera.main.transform.position = position;
		Invoke("ResetHaptic", 0.1f);
	}

	private void ResetHaptic()
	{
		haptic.SetActive(false);
		Vector3 position = Camera.main.transform.position;
		position.y += 0.05f;
		Camera.main.transform.position = position;
	}

	public void ActivePowerUpMode()
	{
		ispowerup = true;
		powerslider.GetComponent<SpriteRenderer>().material.SetFloat("_Cutoff", 0.001f);
		GameObject gameObject = Object.Instantiate(bonustxt);
		gameObject.transform.SetParent(base.transform);
		gameObject.GetComponent<TextMesh>().text = "Perfect x5 \n Fever";
		speed = 6f;
		bb.BirdAnimSpeed(0.05f);
		vignette.SetActive(true);
		powerslider.SetActive(true);
		powerslider.transform.GetChild(1).gameObject.SetActive(true);
		powerslider.transform.GetChild(2).gameObject.SetActive(true);
		GameObject.Find("Main Camera").GetComponent<MKGlowFree>().enabled = true;
		powerslider.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
		//if (PlayerPrefs.GetInt("squarebird_isvibrate") == 0)
		//{
		//	Handheld.Vibrate();
		//}
	}

	public void DeActivePowerUpMode()
	{
		ispowerup = false;
		speed = 3.8f;
		bb.BirdAnimSpeed(0.1f);
		vignette.SetActive(false);
		powerslider.transform.GetChild(2).gameObject.SetActive(false);
		GameObject.Find("Main Camera").GetComponent<MKGlowFree>().enabled = false;
		powerslider.transform.localScale = new Vector3(1f, 1f, 1f);
		powerslider.SetActive(false);
	}
}
