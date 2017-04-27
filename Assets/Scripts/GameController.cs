using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

    public GameObject hazard;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
	
	public float startingSimulatedSpeed;

	public GUIText timerText;
	public GUIText restartText;
	public GUIText gameOverText;

	private float timer;
	private bool gameOver;
	private bool restart;

	//speeds of spawned enemies
	private const float basicEnemySpeed = 70f;

	public static float currentSimulatedSpeed;

	//for scrolling, texture materials for each primitive
	public Material buildingMaterial;
	public Material floorMaterial;

    void Start()
    {
		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		timer = 0.0f;
        StartCoroutine (SpawnWaves());
		currentSimulatedSpeed = startingSimulatedSpeed;
    }

	private void Update() {

		timer += Time.deltaTime;
		timerText.text = "Time: " + timer;

		//to reflect mesh dimensions
		const float wallSpeedToOffset = 1f/(500f/30f);
		const float floorSpeedToOffset = 1f/(500f/20f);
		buildingMaterial.mainTextureOffset -= new Vector2(currentSimulatedSpeed*wallSpeedToOffset*Time.deltaTime, 0);
		floorMaterial.mainTextureOffset -= new Vector2(0, currentSimulatedSpeed * floorSpeedToOffset * Time.deltaTime);

		if (restart) 
		{
			if (Input.GetKeyDown (KeyCode.R)) 
			{
				SceneManager.UnloadSceneAsync("gameplay");
				SceneManager.LoadScene ("StartMenu", LoadSceneMode.Additive);
			}
		}
	}

	IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds (startWait);
        while (true)
        {
			
            for (int i = 0; i < hazardCount; i++)
            {
				//spawn new enemy
                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-spawnValues.x, spawnValues.x),
					transform.position.y + Random.Range(-spawnValues.y, spawnValues.y), transform.position.z);
                Quaternion spawnRotation = Quaternion.identity;
				GameObject enemy = Instantiate(hazard, spawnPosition, spawnRotation);

				//set the new enemy's speed; for now, only spawning basic enemies
				enemy.GetComponent<Rigidbody>().velocity = transform.forward * -(currentSimulatedSpeed + basicEnemySpeed);
				yield return new WaitForSeconds(spawnWait);
            }
				
			//make the next wave more difficult
			hazardCount += 2;
			spawnWait *= 0.9f;
			currentSimulatedSpeed += 0.2f*startingSimulatedSpeed;

            yield return new WaitForSeconds (waveWait);

			if (gameOver) 
			{
				Time.timeScale = 0;
				gameOverText.text = "Game Over! Survive Time: " + timer + "s";
				restartText.text = "Press 'R' for Restart";
				restart = true;
				break;
			}
        }
    }

	public void GameOver ()
	{
		gameOver = true;
	}
}
