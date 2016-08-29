using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// UI class managing the Inputs and Outputs corresponding to the number-of-iterations-UI
/// </summary>
public class IteratingUI : MonoBehaviour
{

	public InputField iterationInput;
	public Text numIterationsText;

	public IteratingUI ()
	{
		AIController.iteratingUI = this;
	}
}
