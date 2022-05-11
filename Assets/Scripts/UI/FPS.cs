using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPS : MonoBehaviour
{
	string label = "";
	public float count;
	public Text FPSText;

	IEnumerator Start()
	{
		GUI.depth = 2;
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				count = (1 / Time.deltaTime);
				label = "FPS :" + Mathf.Round(count);
				FPSText.text = label;
			}
			else
			{
				label = "Pause";
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

}
