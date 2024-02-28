using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
	private bool ismove;

	public Transform startarget;

	private UIHandler uihandler;

	private void Start()
	{
		uihandler = GameObject.Find("UIHandler").GetComponent<UIHandler>();
	}

	private void Update()
	{
		if (ismove)
		{
			if (Mathf.Abs(Vector2.Distance(startarget.position, base.transform.position)) < 0.5f)
			{
				uihandler.SetStarScore();
				ismove = false;
				Object.Destroy(base.gameObject);
			}
			else
			{
				base.transform.position = Vector3.MoveTowards(base.transform.position, startarget.position, 18f * Time.deltaTime);
			}
		}
	}

	public void MoveStar()
	{
		ismove = true;
	}
}
