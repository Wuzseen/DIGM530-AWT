using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;

public class Story 
{
	public string Title { get; set; }
    public List<string> Clues { get; set; }
	public List<StoryPrompt> Prompts { get; set; }
	public string FirstPromptName { get; set; }

	private Dictionary<string, StoryPrompt> PromptLookupDictionary;

	public Story() 
	{
		this.Title = "Default Story";
		this.Prompts = new List<StoryPrompt>();
		this.FirstPromptName = "DefaultPrompt";
        this.Clues = new List<string>();
	}

	public StoryPrompt LookupPrompt(string promptName)
	{
		if(PromptLookupDictionary == null) 
		{
			PromptLookupDictionary = new Dictionary<string, StoryPrompt>();
			for(int i = 0; i < this.Prompts.Count; i++) 
			{
				if(PromptLookupDictionary.ContainsKey(this.Prompts[i].Name))
				{
					throw new System.Exception("Duplicate key found in story: " + this.Prompts[i].Name);
				}
				PromptLookupDictionary.Add(this.Prompts[i].Name, this.Prompts[i]);
			}
		}

		return PromptLookupDictionary[promptName];
	}

    public void ShuffleClues()
    {
        for(int i = 0; i < Clues.Count; i++)
        {
            string t = Clues[i];
            int randomIndex = Random.Range(i, Clues.Count);
            Clues[i] = Clues[randomIndex];
            Clues[randomIndex] = t;
        }
    }
}
