using UnityEngine;

public class BlockBehaviour : MonoBehaviour
{
	private GameObject topcover;

	private void Start()
	{
		if (base.gameObject.tag.Equals("TopBlock"))
		{
			topcover = Object.Instantiate(GameObject.Find("TopCover"));
			Vector3 position = base.transform.position;
			position.y -= GetComponent<BoxCollider2D>().size.y / 2f;
			topcover.transform.position = position;
			topcover.transform.localScale = new Vector3(GetComponent<BoxCollider2D>().size.x, 0.2f, 1f);
		}
	}

	public void BlastBlock()
	{
		GetComponent<BoxCollider2D>().enabled = false;
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<ParticleSystem>().Play();
		GameObject.Find("BirdManager").GetComponent<BirdManager2>().SetBlastBonusScore();
	}
}
