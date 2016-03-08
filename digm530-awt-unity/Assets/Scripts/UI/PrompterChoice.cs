using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PrompterChoice : MonoBehaviour {
	public Button button;
	public Text text;
    public AudioClip clickNoise;

	private StoryChoice choice;

	public void AssignStoryChoice(StoryChoice targetChoice)
	{
		this.choice = targetChoice;
		this.text.text = targetChoice.Text;
	}

	public void OnPress()
	{
        SoundController.PlaySFX(clickNoise);
		StoryTeller.Instance.ChoiceSelected(choice);
	}

    public void Update()
    {
        button.interactable = !Prompter.Prompting;
    }
}
