using UnityEngine;

public class BirdFrontCollider : MonoBehaviour
{
	public GameObject bird;

	private bool once = true;

	private BirdManager2 bm;

	private void Start()
	{
		bm = GameObject.Find("BirdManager").GetComponent<BirdManager2>();
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Block") && once)
		{
			if (bm.IsPowerActive())
			{
				other.gameObject.GetComponent<BlockBehaviour>().BlastBlock();
				return;
			}
			bird.GetComponent<BirdBehaviour>().BirdDead();
			once = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Block") && once)
		{
			if (bm.IsPowerActive())
			{
				other.gameObject.GetComponent<BlockBehaviour>().BlastBlock();
				return;
			}
			bird.GetComponent<BirdBehaviour>().BirdDead();
			once = false;
		}
	}
}
