using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandlerHome : MonoBehaviour
{
	public GameObject[] charbtn;

	public Sprite[] charlockimg;

	public Sprite[] charunlockimg;

	private string[] charunlockcode;

	public GameObject msgbox;

	public GameObject musicoff;

	public GameObject vibrateoff;

	public GameObject charmenu;

	private void Awake()
	{
		if (!PlayerPrefs.HasKey("squarebird_best"))
		{
			PlayerPrefs.SetInt("squarebird_best", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_selectedchar"))
		{
			PlayerPrefs.SetInt("squarebird_selectedchar", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_tmpscore"))
		{
			PlayerPrefs.SetInt("squarebird_tmpscore", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_charunlock"))
		{
			PlayerPrefs.SetString("squarebird_charunlock", "0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0");
		}
		if (!PlayerPrefs.HasKey("squarebird_notify"))
		{
			PlayerPrefs.SetInt("squarebird_notify", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_ismusic"))
		{
			PlayerPrefs.SetInt("squarebird_ismusic", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_isvibrate"))
		{
			PlayerPrefs.SetInt("squarebird_isvibrate", 0);
		}
		if (!PlayerPrefs.HasKey("squarebird_coin"))
		{
			PlayerPrefs.SetInt("squarebird_coin", 0);
		}
	}

	private void Start()
	{
		msgbox.SetActive(false);
		if (PlayerPrefs.GetInt("squarebird_notify") == 1)
		{
			msgbox.SetActive(true);
			PlayerPrefs.SetInt("squarebird_notify", 0);
		}
		else
		{
			msgbox.SetActive(false);
		}
		musicoff.SetActive(false);
		if (PlayerPrefs.GetInt("squarebird_ismusic") == 0)
		{
			musicoff.SetActive(false);
		}
		else
		{
			musicoff.SetActive(true);
		}
		vibrateoff.SetActive(false);
		if (PlayerPrefs.GetInt("squarebird_isvibrate") == 0)
		{
			vibrateoff.SetActive(false);
		}
		else
		{
			vibrateoff.SetActive(true);
		}
		charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');
		UnlockCharacter();
	}

	public void SetMusic()
	{
		if (PlayerPrefs.GetInt("squarebird_ismusic") == 0)
		{
			PlayerPrefs.SetInt("squarebird_ismusic", 1);
			musicoff.SetActive(true);
		}
		else
		{
			PlayerPrefs.SetInt("squarebird_ismusic", 0);
			musicoff.SetActive(false);
		}
		GameObject.Find("BirdManager").GetComponent<BirdManager2>().UpdateMusicSetting();
	}

	public void SetVibrate()
	{
		if (PlayerPrefs.GetInt("squarebird_isvibrate") == 0)
		{
			PlayerPrefs.SetInt("squarebird_isvibrate", 1);
			vibrateoff.SetActive(true);
		}
		else
		{
			PlayerPrefs.SetInt("squarebird_isvibrate", 0);
			vibrateoff.SetActive(false);
		}
	}

	public void UnlockCharacter()
	{
		if (charbtn.Length > charunlockcode.Length)
		{
			int num = charbtn.Length - charunlockcode.Length;
			string text = string.Empty;
			for (int i = 0; i < num; i++)
			{
				text += ",0";
			}
			PlayerPrefs.SetString("squarebird_charunlock", PlayerPrefs.GetString("squarebird_charunlock") + text);
			charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');
		}
		for (int j = 0; j < charbtn.Length; j++)
		{
			if (int.Parse(charunlockcode[j]) == 1)
			{
				charbtn[j].GetComponent<Button>().enabled = true;
				charbtn[j].GetComponent<Image>().sprite = charunlockimg[j];
			}
			else
			{
				charbtn[j].GetComponent<Button>().enabled = false;
				charbtn[j].GetComponent<Image>().sprite = charlockimg[j];
			}
		}
	}

	public void SelectCharacter(int charid)
	{
		PlayerPrefs.SetInt("squarebird_selectedchar", charid);
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void HideCharMenu()
	{
		charmenu.SetActive(false);
	}

	public void ShowCharMenu()
	{
		charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');
		UnlockCharacter();
		charmenu.SetActive(true);
	}
}
