using System.Collections;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
	private RaycastHit2D hit;

	private RaycastHit2D downhit;

	private RaycastHit2D uphit;

	private RaycastHit2D righthit;

	public bool isdead;
	public bool m_won;

	public bool istouchtop;

	private Rigidbody2D body;

	private BirdManager2 bm;

	public GameObject groundparticle;

	public Sprite[] birdidleframe;

	private float birdanimspeed = 0.1f;

	private int perfectcount;

	public LineRenderer laser;

	public static BirdBehaviour instance;

    private void Start()
	{
		instance = this;
		body = GetComponent<Rigidbody2D>();
		//bm = GameObject.Find("BirdManager").GetComponent<BirdManager2>();
		bm = FindObjectOfType<BirdManager2>();	
		StartCoroutine(BirdIdle());
	}

	private void Update()
	{
		downhit = Physics2D.Raycast(base.transform.position - new Vector3(0f, 0.6f, 0f), Vector3.down, 0.5f);
		uphit = Physics2D.Raycast(base.transform.position + new Vector3(0f, 0.5f, 0f), Vector3.up, 0.3f);
		if (uphit.collider == null)
		{
			istouchtop = false;
		}
		else if (uphit.collider.tag.Equals("TopBlock") || uphit.collider.tag.Equals("Block") || uphit.collider.tag.Equals("TopCover"))
		{
			istouchtop = true;
		}
		else
		{
			istouchtop = false;
		}
		if (downhit.collider == null)
		{
			body.gravityScale = 4f;
			groundparticle.SetActive(false);
		}
		else
		{
			if (downhit.collider.tag.Equals("BottomBlock") || downhit.collider.tag.Equals("Block"))
			{
				if (!isdead)
				{
					groundparticle.SetActive(true);
				}
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
		//if (bm.IsPowerActive())
		//{
		//	righthit = Physics2D.Raycast(base.transform.position + new Vector3(0.8f, 0f, 0f), Vector3.right, 4f);
		//	Debug.DrawRay(base.transform.position + new Vector3(0.8f, 0f, 0f), Vector3.right + new Vector3(4f, 0f, 0f), Color.blue);
		//	if (righthit.collider == null)
		//	{
		//		laser.gameObject.SetActive(false);
		//	}
		//	else if (righthit.collider.tag.Equals("Block"))
		//	{
		//		laser.gameObject.SetActive(true);
		//		laser.SetPosition(0, base.transform.position);
		//		laser.SetPosition(1, righthit.collider.transform.position);
		//		righthit.collider.GetComponent<BlockBehaviour>().BlastBlock();
		//	}
		//}
		//else
		//{
		//	laser.gameObject.SetActive(false);
		//}
		if (!isdead)
		{
			base.transform.position = new Vector3(bm.transform.position.x, base.transform.position.y, base.transform.position.z);
		}
		if (isdead)
		{
			body.constraints = RigidbodyConstraints2D.None;
			body.AddTorque(2f);
		}
	}

	public void BirdDead()
	{
		if (!bm.IsPowerActive())
		{
			isdead = true;
			base.transform.SetParent(GameObject.Find("Parent").transform);
			groundparticle.SetActive(false);
			body.AddForceAtPosition((Vector2.left + Vector2.up) * 2f, base.transform.position, ForceMode2D.Impulse);
			base.transform.position -= new Vector3(0.25f, 0f, 0f);
			GetComponent<SpriteRenderer>().sprite = birdidleframe[3];
			//if (PlayerPrefs.GetInt("squarebird_isvibrate") == 0)
			//{
			//	Handheld.Vibrate();
			//}
			GameObject.Find("UIHandler").GetComponent<UIHandler>().ShowReloadMenu();
			laser.gameObject.SetActive(false);
		}
	}

	private IEnumerator BirdIdle()
	{
		int fid = 0;
		int[] frameseq = new int[4] { 0, 1, 2, 1 };
		while (!isdead)
		{
			GetComponent<SpriteRenderer>().sprite = birdidleframe[frameseq[fid]];
			fid++;
			if (fid == frameseq.Length)
			{
				fid = 0;
			}
			yield return new WaitForSeconds(birdanimspeed);
		}
	}

	public void BirdAnimSpeed(float animspeed)
	{
		birdanimspeed = animspeed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Star"))
		{
			other.gameObject.GetComponent<StarBehaviour>().MoveStar();
		}
		if (other.gameObject.tag.Equals("Perfect") && !bm.IsPowerActive())
		{
			Debug.Log("Prefect colision here");

			perfectcount++;
			if (perfectcount == 1)
			{
				other.gameObject.SetActive(false);
				bm.Perfect1();
			}
			else if (perfectcount == 2)
			{
				other.gameObject.SetActive(false);
				bm.Perfect2();
			}
			else if (perfectcount == 3)
			{
				other.gameObject.SetActive(false);
				bm.Perfect3();
			}
			else if (perfectcount == 4)
			{
				other.gameObject.SetActive(false);
				bm.Perfect4();
			}
			else if (perfectcount == 5)
			{
				Debug.Log("Disabled Powerslider here");
				//perfectcount = 0;
                return;
				other.gameObject.SetActive(false);
				bm.ActivePowerUpMode();
			}
		}
	}

	public void _ResetPrefect()
	{
        perfectcount = 0;
    }
}
