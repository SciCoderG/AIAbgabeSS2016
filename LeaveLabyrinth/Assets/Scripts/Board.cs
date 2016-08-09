using UnityEngine;
using System.Collections;


public class Board : MonoBehaviour{


	private Field[,] m_BoardMatrix;
	private float spacing = 0.1f;


	public Board() : this(4,3){
		
	}

	public Board(int x, int y){
		m_BoardMatrix = new Field[x, y];

		// initialize Board to zero
		for(int i = 0; i < m_BoardMatrix.GetLength(0); i++){
			for(int j = 0; j < m_BoardMatrix.GetLength(1); j++){
				// Make the Field as a primitive, because we want to see the Board in the Editor, not just after starting
				GameObject fieldObject = GameObject.CreatePrimitive (PrimitiveType.Cube);
				fieldObject.transform.SetParent (this.transform);
				fieldObject.transform.localPosition = new Vector3 (i + i * spacing, 0, j + j * spacing);

				m_BoardMatrix[i,j] = fieldObject.AddComponent<Field> ();
			}
		}


	}
		

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
