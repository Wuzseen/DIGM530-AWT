using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;
using System.Collections.Generic;

public class Prompter : MonoBehaviour {
	public CanvasGroup canvasGroup { get; set; }
	public Text promptField;
	public float TimePerLetter = .15f;

	public RectTransform choiceRoot;

	public GameObject PrompterChoicePrefab;

	public GameObject fateAwaits;

	private List<PrompterChoice> prompterChoices;

	public UnityEvent onPromptStart;
	public UnityEvent onPromptEnd;

	public PrompterImage imgPrompt;

    public static bool Prompting { get; private set; }

	void Awake()
	{
		canvasGroup = this.GetComponent<CanvasGroup>();
		prompterChoices = new List<PrompterChoice>();
	}

	void CreateChoices(StoryPrompt prompt)
	{
		prompterChoices = new List<PrompterChoice>();
		foreach(Transform choice in choiceRoot) 
		{
			Destroy(choice.gameObject);
		}
		foreach(StoryChoice choice in prompt.Choices)
		{
			GameObject prompterPrefabInstance = Instantiate(PrompterChoicePrefab);
			prompterPrefabInstance.transform.SetParent(choiceRoot,false);
			PrompterChoice pc = prompterPrefabInstance.GetComponent<PrompterChoice>();
			pc.AssignStoryChoice(choice);
			prompterChoices.Add(pc);
		}
	}

	public void MakePrompt(StoryPrompt prompt)
	{
        StopCoroutine("DoPrompt");
		StartCoroutine(DoPrompt(prompt));
	}

	public void SelectChoiceByIndex(int index) 
	{
		PrompterChoice choiceFab = prompterChoices[index];
		choiceFab.OnPress();
	}

	IEnumerator DoPrompt(StoryPrompt prompt)
	{
		
		onPromptStart.Invoke();
		CreateChoices(prompt);
		if(prompt.ImageName != null)
		{
			imgPrompt.LoadFromPrompt(prompt);
		}
		else
		{
			imgPrompt.DisableImg();
		}

        if(prompt.AudioName != null)
        {
            prompt.PlayAudio();
        }

		if(prompt.NarratorOnly) 
		{
			fateAwaits.SetActive(true);
		}
		else
		{
			fateAwaits.SetActive(false);
			string PromptText = prompt.Prompt;
			float duration = PromptText.Length * TimePerLetter;

			int promptLength = 0;
            Prompting = true;
            bool exit = false;
			while(promptLength < PromptText.Length - 1)
			{
				promptField.text = PromptText.Substring(0, promptLength);
				promptLength++;
                float t = 0f;
                while (t < TimePerLetter)
                {
                    t += Time.deltaTime;
                    exit = Input.GetKeyDown(KeyCode.Space);
                    if (exit)
                        break;
                    yield return null;
                }
                if (exit)
                    break;
            }
			promptField.text = PromptText;
		}
        Prompting = false;
		onPromptEnd.Invoke();
	}
}
