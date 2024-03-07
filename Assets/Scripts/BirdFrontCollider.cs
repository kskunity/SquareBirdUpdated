using UnityEngine;

public class BirdFrontCollider : MonoBehaviour
{
    public GameObject bird;

    [HideInInspector]
    public bool once = true;

    private BirdManager2 bm;

    private void Start()
    {
        //bm = GameObject.Find("BirdManager").GetComponent<BirdManager2>();
        bm = FindObjectOfType<BirdManager2>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (once)
        {
            switch (other.gameObject.tag)
            {
                case "Block":
                    _MakeDead();
                    break;
                case "BottomKiller":
                    _MakeDead();
                    break;
                case "TopBlock":
                    _MakeDead();
                    break;
            }
        }

        //if (other.gameObject.tag.Equals("Block") || other.gameObject.tag.Equals("BottomKiller")&& once)
        //{
        //	if (bm.IsPowerActive())
        //	{
        //		other.gameObject.GetComponent<BlockBehaviour>().BlastBlock();
        //		return;
        //	}
        //	bird.GetComponent<BirdBehaviour>().BirdDead();
        //	once = false;
        //}
    }

    void _MakeDead()
    {
        return;
        bird.GetComponent<BirdBehaviour>().BirdDead();
        once = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {


        if (once)
        {
            switch (other.gameObject.tag)
            {
                case "Block":
                    _MakeDead();
                    break;
                case "BottomKiller":
                    _MakeDead();
                    break;
                case "TopBlock":
                    _MakeDead();
                    break;
            }
        }


        //      if (other.gameObject.tag.Equals("Block")|| other.gameObject.tag.Equals("BottomKiller")  && once)
        //{
        //	if (bm.IsPowerActive())
        //	{
        //		other.gameObject.GetComponent<BlockBehaviour>().BlastBlock();
        //		return;
        //	}
        //	bird.GetComponent<BirdBehaviour>().BirdDead();
        //	once = false;
        //}
    }
}
