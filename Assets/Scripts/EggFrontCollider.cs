using UnityEngine;

public class EggFrontCollider : MonoBehaviour
{
	public GameObject egg;

	private bool once = true;

	private void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Block"))
		{
			if (once)
			{
				egg.GetComponent<EggBehaviour>().EggDead();
				once = false;
			}
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Block"))
		{
			if (once)
			{
				egg.GetComponent<EggBehaviour>().EggDead();
				once = false;
			}
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}
