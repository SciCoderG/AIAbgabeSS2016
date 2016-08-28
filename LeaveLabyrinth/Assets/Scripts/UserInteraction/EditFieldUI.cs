using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditFieldUI : MonoBehaviour
{

	public Text m_CurrentStateText;
	public Text m_CurrentQualityText;

	public Text m_CurrentRewardText;
	public InputField m_NewRewardField;
	public Toggle m_AccessibleToggle;

	private Field m_CurrentField;

	private string m_StandardMessage = "Please select a field.";

	public EditFieldUI ()
	{
		FieldModifier.editFieldUI = this;
	}

	public void onNewFieldClicked (Field field, string currentState)
	{
		m_CurrentStateText.text = "State:\n" + currentState;
		if (field.M_IsAccessible) {
			m_CurrentQualityText.text = "Quality:\n" + field.getCurrentQuality ().ToString ("0.000000");
		} else {
			m_CurrentQualityText.text = "Quality:\n-";
		}

		m_CurrentRewardText.text = "Reward:\n" + field.M_Reward;
		m_NewRewardField.text = "";
		m_AccessibleToggle.isOn = field.M_IsAccessible;

		m_CurrentField = field;
	}

	public void onUnselect ()
	{
		m_CurrentStateText.text = m_StandardMessage;
		m_CurrentQualityText.text = m_StandardMessage;
		m_CurrentRewardText.text = m_StandardMessage;
		m_NewRewardField.text = "";
		m_AccessibleToggle.isOn = true;
		m_CurrentField = null;
	}

	public void onApplyEditField ()
	{
		if (null == m_CurrentField) {
			return;
		}
		// get new reward
		float newReward = 0f; 
		bool parsedSuccessfully = float.TryParse (m_NewRewardField.text, out newReward);
		if (!parsedSuccessfully) {
			Debug.Log ("EditFieldUI: onApplyEditField - could not parse new reward input successfully");
		}

		m_CurrentField.M_Reward = newReward;
		// adjust EditFieldUI
		m_CurrentRewardText.text = "Reward:\n" + newReward;
		m_NewRewardField.text = "";

		// get accessible
		m_CurrentField.M_IsAccessible = m_AccessibleToggle.isOn;
	}
}
