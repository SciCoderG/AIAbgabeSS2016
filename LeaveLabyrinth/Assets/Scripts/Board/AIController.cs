using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Connects the AI-User-Interface to the AI-Logic and the rest of the environment
/// </summary>
public class AIController : MonoBehaviour
{

	public static IteratingUI iteratingUI{ get; set; }

	public static LearningVariablesUI learningVariablesUI{ get; set; }

	private ActionExecutor m_ActionExecutor;

	private QualityChangeBuffer m_QualityChangeBuffer;

	private LearningBehaviour m_LearningBehaviour;

	private bool m_IsIterating;
	private int m_IterationsTodo;
	private int m_IterationsToQualityFlush;

	private static readonly long MAX_ITERATINGMILLISECONDS = 14L;
	private static readonly int MAX_ITERATIONS_TO_FLUSH_QCHANGEBUFFER = 1000;

	private Stopwatch m_StopWatch;

	private GameObject m_AIRepresentation;

	public AIController ()
	{
		m_IsIterating = false;
		m_StopWatch = new Stopwatch ();
	}

	// Use this for initialization
	void Start ()
	{
		m_AIRepresentation = GameObject.Instantiate (Resources.Load ("Prefabs/AIRepresentation")) as GameObject;

		m_ActionExecutor = new ActionExecutor ();
		m_QualityChangeBuffer = new QualityChangeBuffer ();

		m_LearningBehaviour = new LearningBehaviour (m_ActionExecutor, m_QualityChangeBuffer);
		m_LearningBehaviour.init ();

		FieldManager.aiController = this;

		string[] boardFileNames = SaveLoadManager.getBoardFileNames ();
		if (null != boardFileNames && boardFileNames.Length > 0) {
			FieldManager.load (boardFileNames [0]);
		} else {
			// create Sample-Szenario from the lecutre
			FieldModifier.createAndAddNewField (0f, 0f, true, 0f);
			FieldModifier.createAndAddNewField (1f, 0f, true, 0f);
			FieldModifier.createAndAddNewField (2f, 0f, true, 0f);
			FieldModifier.createAndAddNewField (3f, 0f, true, -1f);

			FieldModifier.createAndAddNewField (0f, 1f, true, 0f);
			FieldModifier.createAndAddNewField (1f, 1f, false, 0f);
			FieldModifier.createAndAddNewField (2f, 1f, true, 0f);
			FieldModifier.createAndAddNewField (3f, 1f, true, 0f);

			FieldModifier.createAndAddNewField (0f, 2f, true, 0f);
			FieldModifier.createAndAddNewField (1f, 2f, true, 0f);
			FieldModifier.createAndAddNewField (2f, 2f, true, 0f);
			FieldModifier.createAndAddNewField (3f, 2f, true, 1f);
		}

		updateLearningVariablesShown ();
		updateNumberOfIterationsText ();

		updateAIRepresentationPosition (m_LearningBehaviour.m_CurrentState);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_IsIterating) {
			m_StopWatch.Reset ();
			m_StopWatch.Start ();
			// only iterate for a max of MAX_ITERATINGMILLISECONDS (usually 14ms), to avoid unresponsiveness
			while (0 < m_IterationsTodo && MAX_ITERATINGMILLISECONDS > m_StopWatch.ElapsedMilliseconds) {
				m_LearningBehaviour.iterate ();
				m_IterationsTodo--;
				m_IterationsToQualityFlush++;
			}
			m_StopWatch.Stop ();

			if (1 > m_IterationsTodo) {
				onStopIterating ();
			}
			if (MAX_ITERATIONS_TO_FLUSH_QCHANGEBUFFER <= m_IterationsToQualityFlush) {
				FieldModifier.updateQualityToState (m_QualityChangeBuffer);
				updateAIRepresentationPosition (m_LearningBehaviour.m_CurrentState);
				m_IterationsToQualityFlush = 0;
			}
			updateNumberOfIterationsText ();
		}
	}

	/// <summary>
	/// Stops the current iteration of the Q-Learning Algorithm
	/// </summary>
	public void onStopIterating ()
	{
		m_IsIterating = false;
		m_IterationsToQualityFlush = 0;
		FieldModifier.updateQualityToState (m_QualityChangeBuffer);
		updateAIRepresentationPosition (m_LearningBehaviour.m_CurrentState);
		updateNumberOfIterationsText ();
		printTable ();
	}

	/// <summary>
	/// Retrieves number of iterations from UI
	/// </summary>
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

	/// <summary>
	/// Retrieves and updates changes of the learning variables for the Q-Learning Algorithm from the UI
	/// </summary>
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

	/// <summary>
	/// Reset everything the AI learned and corresponding UI 
	/// </summary>
	public void reset ()
	{
		m_LearningBehaviour.reset ();
		updateNumberOfIterationsText ();
		onUseCurrentFieldSelectionAsAICurrentState ();
		printTable ();
	}

	/// <summary>
	/// Changes current AI-State to the currently selected field
	/// </summary>
	public void onUseCurrentFieldSelectionAsAICurrentState ()
	{
		Field currentFieldSelection = FieldModifier.currentlySelectedField;
		if (null != currentFieldSelection) {
			updateAIRepresentationPosition (currentFieldSelection);
			m_LearningBehaviour.m_CurrentState = FieldManager.getStateFromField (currentFieldSelection);
		}
	}

	public void onShowAIRepresentation ()
	{
		m_AIRepresentation.SetActive (true);
	}

	public void onHideAIRepresentation ()
	{
		m_AIRepresentation.SetActive (false);
	}

	private void updateNumberOfIterationsText ()
	{
		iteratingUI.numIterationsText.text = "Iterations:\n" + m_LearningBehaviour.m_QTable.m_NumberOfUpdates;
	}

	private void printTable ()
	{
		File.WriteAllText (Application.dataPath + "/QTableOutput/printedQTable.txt", m_LearningBehaviour.m_QTable.ToString ());
	}

	/// <summary>
	/// Convenience method. Now that I think about it, this is just a Mathf.clamp()....
	/// </summary>
	/// <returns>Clamped value</returns>
	/// <param name="value">Value.</param>
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

	/// <summary>
	/// Updates the learning variables shown in the UI.
	/// </summary>
	private void updateLearningVariablesShown ()
	{
		learningVariablesUI.m_LearningRateInput.text = "" + m_LearningBehaviour.m_LearningRate;

		learningVariablesUI.m_DiscountRateInput.text = "" + m_LearningBehaviour.m_DiscountRate;

		learningVariablesUI.m_RandomActionInput.text = "" + m_LearningBehaviour.m_RandomAction;

		learningVariablesUI.m_RandomStateInput.text = "" + m_LearningBehaviour.m_RandomState;
	}

	/// <summary>
	/// Updates the AI-representation position to the specified field
	/// </summary>
	/// <param name="field">Field.</param>
	private void updateAIRepresentationPosition (Field field)
	{
		// the longer this takes, the uglier everything gets. sorry :(
		if (null != field && null != m_AIRepresentation) {
			Renderer fieldRenderer = field.GetComponent<Renderer> ();
			Renderer aiRenderer = m_AIRepresentation.GetComponent < Renderer> ();
			if (null != fieldRenderer && null != aiRenderer) {
				Vector3 offset = new Vector3 (
					                 0f, 
					                 fieldRenderer.bounds.extents.y + aiRenderer.bounds.extents.y, 
					                 0f);
				m_AIRepresentation.transform.position = field.transform.position + offset;
			} else {
				UnityEngine.Debug.Log ("AIController - updateAIRepresentationPosition: Either Field or AIRepresentation doesn't have a Renderer.");
				onHideAIRepresentation ();
			}
		} else if (null == m_AIRepresentation) {
			UnityEngine.Debug.Log ("Couln't load AI Representation");
		}
	}

	/// <summary>
	/// Updates the AI representation position to the specified state --> the corresponding field.
	/// </summary>
	/// <param name="state">State.</param>
	private void updateAIRepresentationPosition (uint state)
	{
		updateAIRepresentationPosition (FieldManager.getFieldFromState (state));
	}
}
