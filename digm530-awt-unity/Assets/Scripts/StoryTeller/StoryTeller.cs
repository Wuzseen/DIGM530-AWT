using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using System.IO;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class StoryTeller : MonoBehaviour 
{
	public GameObject loadScreen;
	private static readonly fsSerializer _serializer = new fsSerializer();

	public AirConsoleReceiver consoleReceiver;

	public Prompter prompter;

	public string StoryPath = "DefaultStory.json";

	private Story story;

	public static StoryTeller Instance;

	public List<StoryChoice> choices;

	private bool begun = false;

	private StoryPrompt activePrompt;

	public CanvasGroup lastWordsPanel;
	public InputField lastWordInput;

	public Text lastWordPrompt;

	private string lastWords;

    private int cycleCount = 0;

	// Use this for initialization
	void Awake()
	{
		if(Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
		AirConsole.instance.onConnect += AirConsole_instance_onConnect;
	}

	void OnDestroy()
	{
		AirConsole.instance.onConnect -= AirConsole_instance_onConnect;
	}

	void AirConsole_instance_onConnect(int device_id)
	{
		if(begun) 
		{
			return;
		}
		loadScreen.SetActive(false);
		begun = true;
		string json = File.ReadAllText(StoryPath);
		this.story = Deserialize(typeof(Story), json) as Story;
        this.story.ShuffleClues();
		StoryPrompt firstPrompt = this.story.LookupPrompt(story.FirstPromptName);
		activePrompt = firstPrompt;
		this.prompter.MakePrompt(firstPrompt);
		consoleReceiver.SendPrompt(firstPrompt);
		ActivateActive();
	}

	public void NarratorChoice()
	{

	}

	public void DeactivateActive()
	{
		if(activePrompt.Name == "Narrator Intro")
		{
			lastWordsPanel.gameObject.SetActive(false);
			prompter.canvasGroup.alpha = 1f;
			lastWords = lastWordInput.text;
			print("last words = " + lastWords);
		}
	}

	string promptCopy = "";

	public void ActivateActive()
    {
        if (activePrompt.Name == "cluePage")
        {
			print(cycleCount);
            int clue1Index = cycleCount * 2;
            int clue2Index = cycleCount * 2 + 1;
            if (clue1Index >= this.story.Clues.Count)
            {
                string allClues = "You're so close, but all clues have been discovered:\n\n";
                for (int i = 0; i < this.story.Clues.Count; i++)
                {
                    allClues += (this.story.Clues[i] + "\n");
                    allClues += "\n";
                }
                activePrompt.Prompt = allClues;
            }
            else
            {
				if(promptCopy == "")
				{
					promptCopy = activePrompt.Prompt;
				}
				string clue1 = this.story.Clues[clue1Index];
				string clue2 = this.story.Clues[clue2Index];
				print(clue1);
				print(clue2);
                activePrompt.Prompt = string.Format(promptCopy, "\n" + clue1);
                this.prompter.MakePrompt(activePrompt);
                StoryPrompt copy = new StoryPrompt(activePrompt);
				copy.Prompt = string.Format(promptCopy, "\n" + clue2);
                consoleReceiver.SendPrompt(copy);
                cycleCount++;
                return;
            }
        }


        if (activePrompt.Name == "Narrator Intro")
		{
			lastWordsPanel.gameObject.SetActive(true);
			prompter.canvasGroup.alpha = 0f;
		}
		if(activePrompt.FormatIsDeathWord == true)
		{
			activePrompt.Prompt = string.Format(activePrompt.Prompt, lastWords);
		}
        this.prompter.MakePrompt(activePrompt);
        consoleReceiver.SendPrompt(activePrompt);
    }

	public void FirstChoiceIntro()
	{
		if(activePrompt.Name == "Narrator Intro")
		{
			ChoiceSelected(activePrompt.Choices[0]);
		}
	}

	public void ChoiceSelected(StoryChoice choice)
	{
		DeactivateActive();
		StoryPrompt targetPrompt = story.LookupPrompt(choice.TargetPrompt);
		activePrompt = targetPrompt;
		ActivateActive();
	}

	public static string Serialize(Type type, object value) 
	{
		// serialize the data
		fsData data;
		_serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

		// emit the data via JSON
		return fsJsonPrinter.PrettyJson(data);
	}

	public static object Deserialize(Type type, string serializedState) 
	{
		// step 1: parse the JSON data
		fsData data = fsJsonParser.Parse(serializedState);

		// step 2: deserialize the data
		object deserialized = null;
		_serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

		return deserialized;
	}
}
