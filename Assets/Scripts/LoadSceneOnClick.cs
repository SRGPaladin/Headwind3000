using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void LoadByIndex(int sceneIndex)
	{
		Time.timeScale = 1;
		SceneManager.LoadScene (sceneIndex, LoadSceneMode.Single);
	}
}
