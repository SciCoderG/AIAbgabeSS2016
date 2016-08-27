using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Threading;
using System.Diagnostics;

public class AIController : MonoBehaviour
{
	private ActionExecutor m_ActionExecutor;

	private LearningBehaviour m_LearningBehaviour;

	public static IteratingUI iteratingUI{ get; set; }

	public static LearningVariablesUI learningVariablesUI{ get; set; }

	private bool m_IsIterating;
	private int m_IterationsTodo;

	private static readonly long MAX_ITERATINGMILLISECONDS = 14L;

	private Stopwatch stopWatch;

	public AIController ()
	{
		m_IsIterating = false;
		stopWatch = new Stopwatch ();
	}

	// Use this for initialization
	void Start ()
	{
		m_ActionExecutor = new ActionExecutor ();

		m_LearningBehaviour = new LearningBehaviour (m_ActionExecutor);
		m_LearningBehaviour.init ();

		FieldManager.aiController = this;

		FieldModifier.createAndAddNewField (0f, 0f, true, 0f);
		FieldModifier.createAndAddNewField (1f, 0f, true, 0f);

		updateLearningVariablesShown ();
		updateNumberOfIterationsText ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_IsIterating) {
			stopWatch.Reset ();
			stopWatch.Start ();
			while (0 < m_IterationsTodo && MAX_ITERATINGMILLISECONDS > stopWatch.ElapsedMilliseconds) {
				m_LearningBehaviour.iterate ();
				m_IterationsTodo--;
			}
			stopWatch.Stop ();

			if (1 > m_IterationsTodo) {
				onStopIterating ();
			}
			updateNumberOfIterationsText ();
		}
	}

	public void onStopIterating ()
	{
		m_IsIterating = false;
		printTable ();
	}

	public void onStartIterating ()
	{
		string sNumIterations = iteratingUI.iterationInput.text;

		int iterations;
		bool bParsedSuccessfull = int.TryParse (sNumIterations, out iterations);
		if (bParsedSuccessfull && !m_IsIterating) {
			m_IsIterating = true;
			m_IterationsTodo = iterations;
		} else if (m_IsIterating) {
			UnityEngine.Debug.Log ("AIController - onStartIterating: we are currently still iterating");
		} else {
			UnityEngine.Debug.Log ("AIController: Couldn't parse NumIteration-Input.");
		}
	}

	public void onSetLearningVariables ()
	{
		float learningRate;
		if (float.TryParse (learningVariablesUI.m_LearningRateInput.text, out learningRate)) {
			learningRate = zeroToOneRangeCheck (learningRate);
			m_LearningBehaviour.m_LearningRate = learningRate;
		}
		float discountRate;
		if (float.TryParse (learningVariablesUI.m_DiscountRateInput.text, out discountRate)) {
			discountRate = zeroToOneRangeCheck (discountRate);
			m_LearningBehaviour.m_DiscountRate = discountRate;
		}
		float randomAction;
		if (float.TryParse (learningVariablesUI.m_RandomActionInput.text, out randomAction)) {
			randomAction = zeroToOneRangeCheck (randomAction);
			m_LearningBehaviour.m_RandomAction = randomAction;
		}
		float randomState;
		if (float.TryParse (learningVariablesUI.m_RandomStateInput.text, out randomState)) {
			randomState = zeroToOneRangeCheck (randomState);
			m_LearningBehaviour.m_RandomState = randomState;
		}

		updateLearningVariablesShown ();
	}



	public void reset ()
	{
		m_LearningBehaviour.reset ();
		updateNumberOfIterationsText ();
		printTable ();
	}

	private void updateNumberOfIterationsText ()
	{
		iteratingUI.numIterationsText.text = "Iterations:\n" + m_LearningBehaviour.m_QTable.m_NumberOfUpdates;
	}

	private void printTable ()
	{
		File.WriteAllText (Application.dataPath + "/QTableOutput/printedQTable.txt", m_LearningBehaviour.m_QTable.ToString ());
	}

	private float zeroToOneRangeCheck (float value)
	{
		if (value < 0f) {
			return 0f;
		}
		if (value > 1f) {
			return 1f;
		}
		return value;
	}

	private void updateLearningVariablesShown ()
	{
		learningVariablesUI.m_LearningRateInput.text = "" + m_LearningBehaviour.m_LearningRate;

		learningVariablesUI.m_DiscountRateInput.text = "" + m_LearningBehaviour.m_DiscountRate;

		learningVariablesUI.m_RandomActionInput.text = "" + m_LearningBehaviour.m_RandomAction;

		learningVariablesUI.m_RandomStateInput.text = "" + m_LearningBehaviour.m_RandomState;
	}
}
