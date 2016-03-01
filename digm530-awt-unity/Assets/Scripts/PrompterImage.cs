using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class PrompterImage : MonoBehaviour 
{
	public Image img;
	public CanvasGroup cg;

	public Sprite LoadSprite(string spriteName)
	{
		return Resources.Load<Sprite>("Sprites/" + spriteName);
	}

	public void LoadFromPrompt(StoryPrompt prompt)
	{
		cg.alpha = 1f;
		Sprite s = LoadSprite(prompt.ImageName);
		if(s == null) {
			throw new System.Exception("Sprite not found for prompt " + prompt.Name);
		}
		img.sprite = s;
	}

	public void DisableImg()
	{
		cg.alpha = 0f;
	}
}
