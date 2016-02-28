using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;

public class Prompter : MonoBehaviour {
	public Text promptField;
	public float TimePerLetter = .15f;

	public RectTransform choiceRoot;

	public GameObject PrompterChoicePrefab;

	private List<GameObject> prompterChoices;

	public UnityEvent onPromptStart;
	public UnityEvent onPromptEnd;

	void Awake()
	{
		prompterChoices = new List<GameObject>();
		MakePrompt(new StoryPrompt());
	}

	void CreateChoices(StoryPrompt prompt)
	{
		foreach(Transform choice in choiceRoot) 
		{
			Destroy(choice.gameObject);
		}
		foreach(StoryChoice choice in prompt.Choices)
		{
			GameObject prompterPrefabInstance = Instantiate(PrompterChoicePrefab);
			prompterPrefabInstance.transform.SetParent(choiceRoot,false);
		}
	}

	public void MakePrompt(StoryPrompt prompt)
	{
		StartCoroutine(DoPrompt(prompt));
		CreateChoices(prompt);
	}

	IEnumerator DoPrompt(StoryPrompt prompt)
	{
		string PromptText = prompt.Prompt;
		onPromptStart.Invoke();
		float duration = PromptText.Length * TimePerLetter;

		int promptLength = 0;
		while(promptLength < PromptText.Length - 1)
		{
			promptField.text = PromptText.Substring(0, promptLength);
			promptLength++;
			yield return new WaitForSeconds(TimePerLetter);
		}
		promptField.text = PromptText;
		onPromptEnd.Invoke();
	}
}
