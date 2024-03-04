using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdKillerColider : MonoBehaviour
{

    public bool m_top;


    private Camera m_cam;

    void Start()
    {
        m_cam= Camera.main;
        Vector3 bottomLeft = m_cam.ScreenToWorldPoint(Vector2.zero);
        Vector3 topRight = m_cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        if (m_top)
        {
            transform.DOMoveY(topRight.y+0.5f, 0f);
        }
        else
        {
            transform.DOMoveY(bottomLeft.y-0.5f, 0f);

        }

        Debug.Log(bottomLeft);
        Debug.Log(topRight);


    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);

        switch (other.gameObject.tag)
        {
            case "Bird":
                BirdBehaviour bird = FindObjectOfType<BirdBehaviour>();
                BirdFrontCollider bf = FindObjectOfType<BirdFrontCollider>();
                bf.once = false;
                bird.GetComponent<BirdBehaviour>().BirdDead();
                if (m_top)
                {
                    gameObject.SetActive(false);
                }
                //other.gameObject.SetActive(false);
                break;

            case "Egg":
                other.gameObject.SetActive(false);
                break;
        }
    }
}
