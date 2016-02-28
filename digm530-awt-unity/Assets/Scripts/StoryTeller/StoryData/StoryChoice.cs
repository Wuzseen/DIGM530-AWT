using UnityEngine;
using System.Collections;
using FullSerializer;

public class StoryChoice 
{
	public string Text { get; set; }

	public string TargetPrompt { get; set; }

	public StoryChoice () 
	{
		this.Text = "A choice";
		this.TargetPrompt = "DefaultPrompt";
	}
}