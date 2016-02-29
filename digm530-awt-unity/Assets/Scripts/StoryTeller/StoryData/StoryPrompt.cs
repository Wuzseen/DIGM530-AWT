﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;

public class StoryPrompt 
{
	public string Name {get; set; }
	public string Prompt { get; set; }

	public bool NarratorOnly { get; set; }

	public string PrefabToSpawn { get; set; }

	public List<StoryChoice> Choices { get; set; }

	public StoryPrompt() 
	{
		this.Name = "DefaultPromptName";
		this.Prompt = "This is a default prompt. Wow!";
		this.NarratorOnly = false;
		this.PrefabToSpawn = null;
		this.Choices = new List<StoryChoice>();
	}

	public bool SpawnPrefab()
	{
		return PrefabToSpawn != null;
	}

	public string Serialize()
	{
		return StoryTeller.Serialize(typeof(StoryPrompt), this);
	}
}
