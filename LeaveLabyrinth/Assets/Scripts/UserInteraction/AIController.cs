using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class AIController : MonoBehaviour
{
	public InputField m_NumIterationInput;

	public Text scrollView;

	private ActionExecutor m_ActionExecutor;

	private LearningBehaviour m_LearningBehaviour;

	public AIController ()
	{


	}

	// Use this for initialization
	void Start ()
	{
		m_ActionExecutor = new ActionExecutor ();

		m_LearningBehaviour = new LearningBehaviour (m_ActionExecutor);
		m_LearningBehaviour.init ();

		FieldModifier.createAndAddNewField (0f, 0f, true, 0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void onStartIterating ()
	{
		string sNumIterations = m_NumIterationInput.text;

		int iterations;
		bool bParsedSuccessfull = int.TryParse (sNumIterations, out iterations);
		if (bParsedSuccessfull) {
			m_LearningBehaviour.iterate (iterations);
		} else {
			Debug.Log ("AIController: Couldn't parse NumIteration-Input.");
		}
		File.WriteAllText (Application.dataPath + "/QTableOutput/printedQTable.txt", m_LearningBehaviour.m_QTable.ToString ());
	}


}
