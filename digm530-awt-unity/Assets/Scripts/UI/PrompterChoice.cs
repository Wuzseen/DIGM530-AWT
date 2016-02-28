using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrompterChoice : MonoBehaviour {
	public Button button;
	public Text text;

	private StoryChoice choice;

	public void AssignStoryChoice(StoryChoice targetChoice)
	{
		this.choice = targetChoice;
		this.text.text = targetChoice.Text;
	}

	public void OnPress()
	{
		StoryTeller.Instance.ChoiceSelected(choice);
	}
}
