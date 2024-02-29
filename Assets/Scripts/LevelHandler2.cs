using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelHandler2 : MonoBehaviour
{
    private int landcount = 5;

    public GameObject startland;

    private Vector3 startpoint;

    public List<GameObject> lands;


    private GameObject land;

    public GameObject home;

    public GameObject spoint;

    public GameObject gradientbg;

    private int levelno;

    private int prevlevelno;

    public string[] levelcodes;

    private string[] charunlockcode;

    private string levelcode = string.Empty;

    public UIHandler uihandler;

    public GameObject notifymenu;

    public GameObject newbird;

    private int newcharid;

    public Color[] topcolor;

    public Color[] midcolor;

    public Color[] botcolor;
    [Space]
    public int m_test_no;
    [Space]
    public List<GameObject> m_newland_objects;


    private void Awake()
    {
        if (!PlayerPrefs.HasKey("squarebird_levelno"))
        {
            PlayerPrefs.SetInt("squarebird_levelno", 1);
        }
        if (!PlayerPrefs.HasKey("squarebird_levelcodex"))
        {
            PlayerPrefs.SetString("squarebird_levelcodex", "0;0");
        }
    }

    private void Start()
    {
        levelno = PlayerPrefs.GetInt("squarebird_levelno");
        levelcodes = PlayerPrefs.GetString("squarebird_levelcodex").Split(';');

        Debug.Log(PlayerPrefs.GetString("squarebird_levelcodex"));

        prevlevelno = int.Parse(levelcodes[0]);

        if (levelno <= 5)
        {
            landcount = 5;
        }
        else if (levelno <= 10)
        {
            landcount = 10;
        }
        else if (levelno <= 20)
        {
            landcount = levelno;
        }
        else if (levelno <= 50)
        {
            landcount = 20;
        }
        else if (levelno >= 51)
        {
            landcount = 25;
        }

        if (prevlevelno < levelno)
        {
            prevlevelno = levelno;
            land = startland;
            startpoint = new Vector3(6f, land.transform.position.y, land.transform.position.z);
            for (int i = 0; i < landcount; i++)
            {
                GameObject gameObject = Object.Instantiate(GetNextLand2(), startpoint, Quaternion.identity);
                startpoint.x += gameObject.GetComponent<LandBehaviour2>().landwidth;
                land = gameObject;
            }
            levelcode = levelcode.Substring(1, levelcode.Length - 1);
            levelcode = prevlevelno + ";" + levelcode;
            PlayerPrefs.SetString("squarebird_levelcodex", levelcode);
        }
        else if (prevlevelno == levelno)
        {
            land = startland;
            startpoint = new Vector3(6f, land.transform.position.y, land.transform.position.z);
            string[] array = levelcodes[1].Split(',');
            for (int j = 0; j < array.Length; j++)
            {
                //GameObject gameObject2 = Object.Instantiate(lands[int.Parse(array[j])], startpoint, Quaternion.identity);
                GameObject gameObject2 = Object.Instantiate(_GetLandObject(int.Parse(array[j])), startpoint, Quaternion.identity);
                startpoint.x += gameObject2.GetComponent<LandBehaviour2>().landwidth;
                land = gameObject2;
            }
        }
        Object.Instantiate(home, startpoint, Quaternion.identity);
        Material material = gradientbg.GetComponent<MeshRenderer>().material;
        int num = (levelno - 1) % 10;
        material.SetColor("_ColorTop", topcolor[num]);
        material.SetColor("_ColorMid", midcolor[num]);
        material.SetColor("_ColorBot", botcolor[num]);
        gradientbg.GetComponent<MeshRenderer>().material = material;
        gradientbg.transform.localScale = new Vector3(Mathf.Abs(startpoint.x - spoint.transform.position.x + 15f), 22f, 1f);
        gradientbg.transform.position = new Vector3((gradientbg.transform.localScale.x - 15f) / 2f, 0.5f, 5f);
        uihandler.SetSlider(Mathf.Abs(startpoint.x - startland.transform.position.x));
        int[] array2 = new int[22]
        {
            10, 20, 30, 40, 50, 60, 70, 80, 90, 100,
            110, 120, 130, 140, 150, 160, 170, 180, 190, 200,
            210, 220
        };
        charunlockcode = PlayerPrefs.GetString("squarebird_charunlock").Split(',');
        string text = string.Empty;
        for (int k = 0; k < array2.Length; k++)
        {
            if (array2[k] <= levelno && int.Parse(charunlockcode[k]) == 0)
            {
                uihandler.isnotify = false;
                PlayerPrefs.SetInt("squarebird_notify", 1);
                notifymenu.SetActive(true);
                newbird.GetComponent<Image>().sprite = GameObject.Find("BirdManager").GetComponent<BirdManager2>().GetBirdImg(k);
                charunlockcode[k] = "1";
                newcharid = k + 1;
            }
            text = text + "," + charunlockcode[k];
        }
        text = text.Substring(1, text.Length - 1);
        PlayerPrefs.SetString("squarebird_charunlock", text);
    }

    public void SelectNewChar()
    {
        uihandler.isnotify = false;
        PlayerPrefs.SetInt("squarebird_selectedchar", newcharid);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void CancelNotifyMenu()
    {
        uihandler.isnotify = true;
        notifymenu.SetActive(false);
    }

    private GameObject _GetLandObject(int m_index)
    {
        Debug.Log(m_index);
        return m_newland_objects[m_index];
        //return lands[m_index];
    }

    private GameObject GetNextLand2()
    {
        int min = 0;
        int max = lands.Count;



        if (levelno >= 1 && levelno <= 5)
        {
            min = 0;
            max = 2;
        }
        if (levelno >= 6 && levelno <= 20)
        {
            min = 3;
            max = 8;
        }
        if (levelno >= 21)
        {
            min = 0;
            max = lands.Count;
        }
        int index = Random.Range(min, max);
        //int m_a = Random.Range(0, m_newland_objects.Count);
        levelcode = levelcode + "," + index;
        return m_newland_objects[index];
        //return lands[index];
    }

    #region Genrate Level
#if UNITY_EDITOR
    public List<GameObject> m_genratedObjects;
    [Space]
    public int m_land_count_test;
    public void _GenrateLevelNow()
    {
        _DestroyLevelNow();
        m_genratedObjects.Clear();

        land = startland;
        startpoint = new Vector3(6f, land.transform.position.y, land.transform.position.z);
        for (int i = 0; i < m_land_count_test; i++)
        {
            GameObject gameObject = Object.Instantiate(_NewGameNext(), startpoint, Quaternion.identity);
            startpoint.x += gameObject.GetComponent<LandBehaviour2>().landwidth;
            land = gameObject;
            m_genratedObjects.Add(gameObject);
        }
        m_genratedObjects.Add(Object.Instantiate(home, startpoint, Quaternion.identity));
    }

    private GameObject _NewGameNext()
    {
        return m_newland_objects[m_test_no];
        //return lands[index];
    }

    public void _DestroyLevelNow()
    {
        foreach (var item in m_genratedObjects)
        {
            DestroyImmediate(item);
        }
    }


#endif
    #endregion
}
