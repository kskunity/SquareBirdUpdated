using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIHandlerHome : MonoBehaviour
{
    public GameObject[] charbtn;

    public Sprite[] charlockimg;

    public Sprite[] charunlockimg;
    [Space]
    public GameObject[] m_coinsdesc_text;
    //public GameObject[] m_selectbutton;

    public string[] charunlockcode;
    [Space]
    public int[] char_pricing;
    [Space]
    public GameObject notifymenu;
    public Image newbird;
    [Space]

    public GameObject msgbox;

    public GameObject musicoff;

    public GameObject vibrateoff;

    public GameObject charmenu;
    [Space]
    public Text m_cointext;


    private int m_currunt_ch_no;
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
        //UnlockCharacter();
    }


#if UNITY_EDITOR
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerPrefs.SetInt("squarebird_coin", 1000);
            m_cointext.text = PlayerPrefs.GetInt("squarebird_coin").ToString("00");
        }
    }

#endif

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


    public void _CloseSelectPanel()
    {
        notifymenu.SetActive(false);
    }

    public void SelectCharacter(int charid)
    {
        m_currunt_ch_no = charid - 1;
        Debug.Log(m_currunt_ch_no);
        Debug.Log(charunlockcode[m_currunt_ch_no]);

        int score = PlayerPrefs.GetInt("squarebird_coin");
        string m_s = charunlockcode[m_currunt_ch_no];

        switch (m_s)
        {
            case "0":
                if (score >= char_pricing[m_currunt_ch_no])
                {
                    Debug.Log("_BuyProcess");
                    _BuyProcess(m_currunt_ch_no);
                }
                break;
            case "1":
                PlayerPrefs.SetInt("squarebird_selectedchar", charid);
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
        }
    }


    public void _BuyProcess(int m_no)
    {
        m_currunt_ch_no = m_no;
        notifymenu.SetActive(true);
        newbird.sprite = charunlockimg[m_no];
    }

    public void _Buy()
    {
        charunlockcode[m_currunt_ch_no] = "1";
        string text = string.Empty;
        for (int k = 0; k < charunlockcode.Length; k++)
        {
            text = text + "," + charunlockcode[k];
        }

        int score = PlayerPrefs.GetInt("squarebird_coin");
        score = score- char_pricing[m_currunt_ch_no];
        PlayerPrefs.SetInt("squarebird_coin", score);

        m_cointext.text = PlayerPrefs.GetInt("squarebird_coin").ToString("00");


        text = text.Substring(1, text.Length - 1);
        PlayerPrefs.SetString("squarebird_charunlock", text);
        charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');
        _CloseSelectPanel();
        PlayerPrefs.SetInt("squarebird_selectedchar", m_currunt_ch_no + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void HideCharMenu()
    {
        charmenu.SetActive(false);
    }

    public void ShowCharMenu()
    {
        charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');


        m_cointext.text = PlayerPrefs.GetInt("squarebird_coin").ToString("00");
        //UnlockCharacter();
        _UnlockBoughtCh();
        charmenu.SetActive(true);
    }


    public void _UnlockBoughtCh()
    {
        for (int j = 0; j < charbtn.Length; j++)
        {
            if (int.Parse(charunlockcode[j]) == 1)
            {
                charbtn[j].GetComponent<Image>().sprite = charunlockimg[j];
                m_coinsdesc_text[j].SetActive(false);
            }
            else
            {
                charbtn[j].GetComponent<Image>().sprite = charunlockimg[j];
                //charbtn[j].GetComponent<Image>().sprite = charlockimg[j];
            }
        }
    }
}
