using System.Collections;
using System.Collections.Generic;
 
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
	public bool isplay;

	public bool isnotify = true;

	public Text scoretxt;

	public Text lefttxt;

	public Text righttxt;

	public Text reloadscoretxt;

	public Text reloadleveltxt;

	public Text reloadbesttxt;

	public Text startxt;

	public Slider slider;

	public GameObject reloadmenu;

	public GameObject winmenu;

	public GameObject instruction;

	public GameObject guide;

	public GameObject startbtn;

	public GameObject guideimg;

	public AudioClip deadsound;

	private int bonus = 10;

	private int guideind;

	private AudioSource ass;

	public Sprite[] guideimgs;

	private void Start()
	{
		if (PlayerPrefs.GetInt("squarebird_tmpscore") == 0)
		{
			scoretxt.text = "0";
		}
		else
		{
			scoretxt.text = PlayerPrefs.GetInt("squarebird_tmpscore").ToString();
		}
		int @int = PlayerPrefs.GetInt("squarebird_levelno");
		startxt.text = PlayerPrefs.GetInt("squarebird_coin").ToString();
		lefttxt.text = @int.ToString();
		righttxt.text = (@int + 1).ToString();
		ass = GetComponent<AudioSource>();
		 
		if (@int <= 1)
		{
			guideind = 0;
			guide.SetActive(true);
		}
		else
		{
			guide.SetActive(false);
		}
		int num = @int % 10;
		bonus = (@int - num) / 10 * 5 + 10;
		startbtn.SetActive(true);
		slider.gameObject.SetActive(false);
	}

	private void Update()
	{
	}

	public void StartGame()
	{
		isplay = true;
		startbtn.SetActive(false);
		slider.gameObject.SetActive(true);
	}

	public void SetScore()
	{
		int num = int.Parse(scoretxt.text);
		num++;
		if (num > PlayerPrefs.GetInt("squarebird_best"))
		{
			PlayerPrefs.SetInt("squarebird_best", num);
		}
		scoretxt.text = num.ToString();
		PlayerPrefs.SetInt("squarebird_tmpscore", num);
	}

	public int SetBonusScore()
	{
		int num = int.Parse(scoretxt.text);
		num += bonus;
		if (num > PlayerPrefs.GetInt("squarebird_best"))
		{
			PlayerPrefs.SetInt("squarebird_best", num);
		}
		scoretxt.text = num.ToString();
		PlayerPrefs.SetInt("squarebird_tmpscore", num);
		return bonus;
	}

	public void SetStarScore()
	{
		int num = int.Parse(startxt.text);
		num++;
		PlayerPrefs.SetInt("squarebird_coin", num);
		startxt.text = num.ToString();
	}

	public void SetSlider(float maxvalue)
	{
		slider.maxValue = maxvalue;
	}

	public void ChangeSlider(float v)
	{
		slider.value = v;
	}

	public IEnumerator GoToNextLevel(float endpoint)
	{
		int len = 0;
		 
		if (GameObject.FindGameObjectsWithTag("Egg").Length != 0)
		{
			GameObject[] eggs = GameObject.FindGameObjectsWithTag("Egg");
			List<GameObject> homeeggs = new List<GameObject>();
			for (int j = 0; j < eggs.Length; j++)
			{
				if (endpoint < eggs[j].transform.position.x)
				{
					homeeggs.Add(eggs[j]);
				}
			}
			len = homeeggs.Count;
			for (int i = 0; i < homeeggs.Count; i++)
			{
				yield return new WaitForSeconds(0.5f);
				homeeggs[i].GetComponent<EggBehaviour>().DestroyEgg();
			}
		}
		Invoke("Wait", (float)len * 0.2f);
	}

	private void Wait()
	{
		slider.gameObject.SetActive(false);
		winmenu.SetActive(true);
		winmenu.GetComponentInChildren<Text>().text = "Level " + lefttxt.text + " Completed";
		Invoke("WaitAndLoad", 1f);
	}

	private void WaitAndLoad()
	{
		slider.gameObject.SetActive(false);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ShowReloadMenu()
	{
		 
		if (PlayerPrefs.GetInt("squarebird_ismusic") == 0)
		{
			ass.clip = deadsound;
			ass.Stop();
			ass.Play();
		}
		isplay = false;
		reloadscoretxt.text = scoretxt.text;
		reloadleveltxt.text = lefttxt.text;
		reloadbesttxt.text = PlayerPrefs.GetInt("squarebird_best").ToString();
		PlayerPrefs.SetInt("squarebird_tmpscore", 0);
		Invoke("Wait2", 1f);
	}

	private void Wait2()
	{
		slider.gameObject.SetActive(false);
		reloadmenu.SetActive(true);
	}

	public void ReloadGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ShowInstruction()
	{
		guideind++;
		if (guideind == 3)
		{
			guideind = 0;
			guide.SetActive(false);
		}
		else
		{
			Image component = guideimg.GetComponent<Image>();
			component.sprite = guideimgs[guideind];
			component.SetNativeSize();
		}
	}
}
