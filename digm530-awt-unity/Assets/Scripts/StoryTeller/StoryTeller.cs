using UnityEngine;
using System;
using System.Collections;
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

	private bool begun = false;
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
		StoryPrompt firstPrompt = this.story.LookupPrompt(story.FirstPromptName);
		this.prompter.MakePrompt(firstPrompt);
		consoleReceiver.SendPrompt(firstPrompt);
	}

	public void ChoiceSelected(StoryChoice choice)
	{
		StoryPrompt targetPrompt = story.LookupPrompt(choice.TargetPrompt);
		this.prompter.MakePrompt(targetPrompt);
		consoleReceiver.SendPrompt(targetPrompt);
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
