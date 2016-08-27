using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IteratingUI : MonoBehaviour
{

	public InputField iterationInput;
	public Text numIterationsText;

	public IteratingUI ()
	{
		AIController.iteratingUI = this;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
