using DG.Tweening;
using System;
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
    [Space]
    public GameObject m_first_inst;

    public AudioClip deadsound;
    [Space]
    public RectTransform m_retry_button;
    [Space]
    public Transform startarget;
    private int bonus = 10;

    private int guideind;

    private AudioSource ass;

    public Sprite[] guideimgs;
    [Space]
    public GameObject m_ingame_ui;
    [Space]
    public GameObject m_highscore_panel;
    [Space]
    public List<GameObject> m_highscore_objects;
    [Space]
    public _HScore m_scores;
    [Space]
    public List<BirdKillerColider> m_birdkillers;
    [Space]
    public GameObject m_pause_panel;
    [Space]
    public Text m_text;
    [Space]
    public Button m_paush_button;
    [Space]
    public Text m_music_on_off_text;


    private string m_on_string = "SOUND- ON";
    private string m_off_string = "SOUND- OFF";

    public static UIHandler instance;

    private int m_frame;

    private void Start()
    {

        Time.timeScale = 1;

        if (instance == null)
        {
            instance = this;
        }

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
            //guide.SetActive(true);
        }
        else
        {
            guide.SetActive(false);
        }
        int num = @int % 10;
        bonus = (@int - num) / 10 * 5 + 10;
        startbtn.SetActive(true);
        m_ingame_ui.SetActive(false);
        //slider.gameObject.SetActive(false);


        if (!PlayerPrefs.HasKey("highscore"))
        {
            string s = JsonUtility.ToJson(m_scores);
            PlayerPrefs.SetString("highscore", s);
        }
        else
        {
            Debug.Log("Filled");
            string s = PlayerPrefs.GetString("highscore");
            m_scores = JsonUtility.FromJson<_HScore>(s);
            PlayerPrefs.SetString("highscore", s);
        }

        m_scores.m_scores.Sort((a, b) => b.CompareTo(a));

        Debug.Log(PlayerPrefs.GetString("highscore"));

    }

    private void OnEnable()
    {
        m_paush_button.onClick.AddListener(_Paush);
    }

    private void OnDisable()
    {
        m_paush_button.onClick.RemoveAllListeners();

    }

    private void _Paush()
    {
        m_paush_button.interactable = false;
        BirdManager2 m_bm = FindObjectOfType<BirdManager2>();
        m_bm.m_paused = true;

        if (PlayerPrefs.GetInt("squarebird_ismusic") == 0)
        {
            Debug.Log("Music ON");
            m_music_on_off_text.text = m_on_string;
        }
        else
        {
            Debug.Log("Music OFF");
            m_music_on_off_text.text = m_off_string;
        }

        m_pause_panel.SetActive(true);
        Time.timeScale = 0;
    }

    public void _PlayUnpush()
    {
        m_pause_panel.SetActive(false);
        StartCoroutine(_CountDown());

    }

    IEnumerator _CountDown()
    {
        m_text.gameObject.SetActive(true);
        m_text.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        m_text.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        m_text.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        m_text.gameObject.SetActive(false);
        BirdManager2 m_bm = FindObjectOfType<BirdManager2>();
        m_bm.m_paused = false;
        Time.timeScale = 1;
        m_paush_button.interactable = true;
    }

    public void _Home()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _MusicOnOff()
    {
        if (PlayerPrefs.GetInt("squarebird_ismusic") == 0)
        {
            PlayerPrefs.SetInt("squarebird_ismusic", 1);
            //musicoff.SetActive(true);
            m_music_on_off_text.text = m_off_string;
        }
        else
        {
            PlayerPrefs.SetInt("squarebird_ismusic", 0);
            //musicoff.SetActive(false);
            m_music_on_off_text.text = m_on_string;
        }
        FindObjectOfType<BirdManager2>().UpdateMusicSetting();
        //GameObject.Find("BirdManager").GetComponent<BirdManager2>().UpdateMusicSetting();
    }

    void Update()
    {
        m_frame++;
        if (m_frame >= 15)
        {
            _ResetKillers();
            m_frame = 0;
        }

    }

    public void _ResetKillers()
    {
        foreach (var item in m_birdkillers)
        {
            item._Reset();
        }
    }

    public void _AddStar()
    {
        startxt.text = PlayerPrefs.GetInt("squarebird_coin").ToString();
    }

    public void _OpenHighscore()
    {
        m_highscore_panel.SetActive(true);

        int m_a = 1;

        foreach (var item in m_scores.m_scores)
        {
            m_highscore_objects[m_a - 1].GetComponentInChildren<Text>().text = item.ToString();
            m_a++;
        }
    }

    public void _CloseHighscore()
    {
        m_highscore_panel.SetActive(false);
    }

    public void _SaveHighScore()
    {
        m_scores.m_scores.Sort((a, b) => b.CompareTo(a));

        float num = float.Parse(scoretxt.text);


        if (m_scores.m_scores.Contains(num))
        {
            return;
        }

        Debug.Log(num);
        m_scores.m_scores.Add(num);
        m_scores.m_scores.Sort((a, b) => b.CompareTo(a));
        m_scores.m_scores.RemoveAt(4);
        string s = JsonUtility.ToJson(m_scores);
        PlayerPrefs.SetString("highscore", s);
    }

    public void StartGame()
    {
        isplay = true;
        startbtn.SetActive(false);
        //slider.gameObject.SetActive(true);
        m_ingame_ui.SetActive(true);
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
        //slider.gameObject.SetActive(false);
        m_ingame_ui.SetActive(false);
        winmenu.SetActive(true);
        winmenu.GetComponentInChildren<Text>().text = "Level " + lefttxt.text + " Completed";
        Invoke("WaitAndLoad", 1f);
    }

    private void WaitAndLoad()
    {
        //slider.gameObject.SetActive(false);
        m_ingame_ui.SetActive(false);
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
        //slider.gameObject.SetActive(false);
        m_ingame_ui.SetActive(false);
        reloadmenu.SetActive(true);
        m_retry_button.DOAnchorPosY(-245f, 1f);
        _SaveHighScore();
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _OpenInstruction()
    {
        guideind = -1;
        guide.SetActive(true);
        m_first_inst.SetActive(true);
        guideimg.SetActive(false);
        //Image component = guideimg.GetComponent<Image>();
        //component.sprite = guideimgs[guideind];
        //component.SetNativeSize();
    }

    public void ShowInstruction()
    {
        guideind++;
        Debug.Log(guideind);
        if (guideind == 2)
        {
            guideind = 0;
            guide.SetActive(false);
        }
        else
        {
            guideimg.SetActive(true);
            m_first_inst.SetActive(false);
            Image component = guideimg.GetComponent<Image>();
            component.sprite = guideimgs[guideind];
            component.SetNativeSize();
        }
    }
}

[System.Serializable]
public class _HScore
{
    public List<float> m_scores;
}
