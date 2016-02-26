using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleReceiver : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		AirConsole.instance.onMessage += AirConsole_instance_onMessage;
	}

	void OnDestroy () 
	{
		AirConsole.instance.onMessage -= AirConsole_instance_onMessage;
	}

	void AirConsole_instance_onMessage (int from, JToken data)
	{
		AirConsole.instance.Message(from, "Full of pixels");

	}

	// Update is called once per frame
	void Update () {

	}
}
