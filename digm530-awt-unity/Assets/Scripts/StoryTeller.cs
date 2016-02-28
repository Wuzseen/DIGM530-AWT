using UnityEngine;
using System;
using System.Collections;
using FullSerializer;
using System.IO;

public class StoryTeller : MonoBehaviour 
{
	private static readonly fsSerializer _serializer = new fsSerializer();

	public string StoryPath = "DefaultStory.json";
	// Use this for initialization
	void Start () {
		Story s = new Story();
		string storyJson = Serialize(typeof(Story), s);
		File.WriteAllText(StoryPath, storyJson);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static string Serialize(Type type, object value) {
		// serialize the data
		fsData data;
		_serializer.TrySerialize(type, value, out data).AssertSuccessWithoutWarnings();

		// emit the data via JSON
		return fsJsonPrinter.PrettyJson(data);
	}

	public static object Deserialize(Type type, string serializedState) {
		// step 1: parse the JSON data
		fsData data = fsJsonParser.Parse(serializedState);

		// step 2: deserialize the data
		object deserialized = null;
		_serializer.TryDeserialize(data, type, ref deserialized).AssertSuccessWithoutWarnings();

		return deserialized;
	}
}
