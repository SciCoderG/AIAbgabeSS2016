using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class AIController : MonoBehaviour
{
	public InputField m_NumIterationInput;

	public Text scrollView;

	private Board m_CurrentBoard;


	private LearningBehaviour m_LearningBehaviour;

	public AIController ()
	{


	}

	// Use this for initialization
	void Start ()
	{
		m_CurrentBoard = new Board ();

		m_LearningBehaviour = new LearningBehaviour (m_CurrentBoard);
		m_LearningBehaviour.init ();
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
		Debug.Log (Application.persistentDataPath);
		File.WriteAllText (Application.dataPath + "/QTableOutput/printedQTable.txt", m_LearningBehaviour.m_QTable.ToString ());
	}

	public void onSaveBoard ()
	{
		m_CurrentBoard.save ("test1");
	}

	public void onLoadBoard ()
	{
		m_CurrentBoard.load ("test1");
	}

	public void onChangeBoard ()
	{
		GameObject fieldObject = GameObject.Instantiate (Resources.Load ("Prefabs/FieldPrefab", typeof(GameObject))) as GameObject;
		Field field = fieldObject.GetComponent<Field> ();
		fieldObject.transform.position = new Vector3 (1f, 0f, 0f);
		m_CurrentBoard.m_ExistingFields.Add (field);
	}
}
