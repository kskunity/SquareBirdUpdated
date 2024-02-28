using UnityEngine;

public class EggBehaviour : MonoBehaviour
{
	private RaycastHit2D hit;

	private RaycastHit2D downhit;

	private Rigidbody2D body;

	public GameObject groundparticle;

	private BirdManager2 bm;

	private void Start()
	{
		body = GetComponent<Rigidbody2D>();
		bm = GameObject.Find("BirdManager").GetComponent<BirdManager2>();
	}

	private void Update()
	{
		downhit = Physics2D.Raycast(base.transform.position - new Vector3(0f, 0.6f, 0f), Vector3.down, 0.5f);
		if (downhit.collider == null)
		{
			body.gravityScale = 4f;
			groundparticle.SetActive(false);
			return;
		}
		if (downhit.collider.tag.Equals("BottomBlock") || downhit.collider.tag.Equals("Block"))
		{
			groundparticle.SetActive(true);
		}
		else if (downhit.collider.tag.Equals("Finish"))
		{
			bm.Win();
		}
		else
		{
			groundparticle.SetActive(false);
		}
		body.gravityScale = 1f;
	}

	public void EggDead()
	{
		base.transform.SetParent(GameObject.Find("Parent").transform);
		if (bm.IsPowerActive())
		{
			GetComponent<SpriteRenderer>().enabled = false;
			GetComponent<ParticleSystem>().Play();
			Object.Instantiate(GameObject.Find("Star"), base.transform.position, Quaternion.identity).GetComponent<StarBehaviour>().MoveStar();
		}
	}

	public void DestroyEgg()
	{
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<ParticleSystem>().Play();
		GetComponent<BoxCollider2D>().enabled = false;
		Object.Instantiate(GameObject.Find("Star"), base.transform.position, Quaternion.identity).GetComponent<StarBehaviour>().MoveStar();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Star"))
		{
			other.gameObject.GetComponent<StarBehaviour>().MoveStar();
		}
	}
}
