using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class AIController : MonoBehaviour
{
	public InputField m_NumIterationInput;

	public Text scrollView;

	public GameObject m_Board;

	private LearningBehaviour m_LearningBehaviour;

	public AIController ()
	{


	}

	// Use this for initialization
	void Start ()
	{
		Board board = m_Board.GetComponent<Board> ();

		m_LearningBehaviour = new LearningBehaviour (board);
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
}
