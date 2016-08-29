using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// UI class managing the Inputs and Outputs corresponding to the AI-learning-variables-UI
/// </summary>
public class LearningVariablesUI : MonoBehaviour
{

	public InputField m_LearningRateInput, m_DiscountRateInput, m_RandomActionInput, m_RandomStateInput;

	public LearningVariablesUI ()
	{
		AIController.learningVariablesUI = this;
	}
}
