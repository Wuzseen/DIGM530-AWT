using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleReceiver : MonoBehaviour {
	private int narratorId;

	// Use this for initialization
	void Start () 
	{
		AirConsole.instance.onMessage += AirConsole_instance_onMessage;
		AirConsole.instance.onConnect += AirConsole_instance_onConnect;
	}

	void OnDestroy () 
	{
		AirConsole.instance.onMessage -= AirConsole_instance_onMessage;
		AirConsole.instance.onConnect -= AirConsole_instance_onConnect;
	}

	void AirConsole_instance_onConnect (int device_id)
	{
		this.narratorId = device_id;

		print(device_id);
	}

	void AirConsole_instance_onMessage (int from, JToken data)
	{
		narratorId = from;
		print((string)data);
	}

	public void SendPrompt(StoryPrompt aPrompt) {
		print(aPrompt.Serialize());
		AirConsole.instance.Broadcast(aPrompt.Serialize());
	}
}
