using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LogoTable : MonoBehaviour {

	private static LogoTable instance = null;
	public static LogoTable Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<LogoTable>();
			return instance;
		}
	}

	public enum LogoLetter { T, b, e }

	[System.Serializable]
	public class LogoLine
	{
		public LogoLetter letter1;
		public string letter2;
		public Color color;
	}

	public List<LogoLine> logoLines = new List<LogoLine>()
	{
		new LogoLine(){letter1 = LogoLetter.T, letter2 = "a", color = Color.red},
		new LogoLine(){letter1 = LogoLetter.b, letter2 = "l", color = Color.green},
		new LogoLine(){letter1 = LogoLetter.e, letter2 = "", color = Color.blue},
	};

	void OnGUI ()
	{
		GUILayout.Label ("Select the Logo Table scene object to visualize the table in the editor");
	}

}
