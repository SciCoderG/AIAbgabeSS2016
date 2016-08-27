using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LearningVariablesUI : MonoBehaviour
{

	public InputField m_LearningRateInput, m_DiscountRateInput, m_RandomActionInput, m_RandomStateInput;

	public LearningVariablesUI ()
	{
		AIController.learningVariablesUI = this;
	}
}
