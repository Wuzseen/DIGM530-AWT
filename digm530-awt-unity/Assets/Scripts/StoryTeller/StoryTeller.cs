using UnityEngine;
using System;
using System.Collections;
using FullSerializer;
using System.IO;

public class StoryTeller : MonoBehaviour 
{
	private static readonly fsSerializer _serializer = new fsSerializer();

	public Prompter prompter;

	public string StoryPath = "DefaultStory.json";

	private Story story;

	public static StoryTeller Instance;
	// Use this for initialization
	void Awake()
	{
		if(Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}
		Instance = this;
	}

	void Start () 
	{
		string json = File.ReadAllText(StoryPath);
		this.story = Deserialize(typeof(Story), json) as Story;
		this.prompter.MakePrompt(this.story.LookupPrompt(story.FirstPromptName));
	}

	public void ChoiceSelected(StoryChoice choice)
	{
		this.prompter.MakePrompt(story.LookupPrompt(choice.TargetPrompt));
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
