using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EditFieldUI : MonoBehaviour
{

	public Text m_CurrentRewardText;
	public InputField m_NewRewardField;
	public Toggle m_AccessibleToggle;

	private Field m_CurrentField;

	public EditFieldUI ()
	{
		FieldModifier.editFieldUI = this;
	}

	public void onNewFieldClicked (Field field)
	{
		m_CurrentRewardText.text = "Reward:\n" + field.m_Reward;
		m_NewRewardField.text = "";
		m_AccessibleToggle.isOn = field.m_IsAccessible;

		m_CurrentField = field;
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

		m_CurrentField.m_Reward = newReward;
		// adjust EditFieldUI
		m_CurrentRewardText.text = "Reward:\n" + newReward;
		m_NewRewardField.text = "";

		// get accessible
		m_CurrentField.m_IsAccessible = m_AccessibleToggle.isOn;
	}
}
