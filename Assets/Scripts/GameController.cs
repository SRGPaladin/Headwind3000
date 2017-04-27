using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {

	float timebetweengroundandquit = 3.0f;
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
	private bool restart;

	//the game has ended!
	public bool ended = false;

	//speeds of spawned enemies
	private const float basicEnemySpeed = 70f;

	public static float currentSimulatedSpeed;

	//for scrolling, texture materials for each primitive
	public Material buildingMaterial;
	public Material floorMaterial;

    void Start()
    {
		restart = false;
		restartText.text = "";
		gameOverText.text = "";
		timer = 0.0f;
        StartCoroutine (SpawnWaves());
		currentSimulatedSpeed = startingSimulatedSpeed;
    }

	private void Update() {
		timerText.text = "Time: " + timer;

		if (restart) 
		{
			if (Input.anyKeyDown) 
			{
				Time.timeScale = 1;
				SceneManager.LoadScene ("StartMenu", LoadSceneMode.Single);
			}
		}

		if (ended) {
			timebetweengroundandquit -= Time.deltaTime;
			if (timebetweengroundandquit < 0) {
				Time.timeScale = 0;
				GameOver();
			}
		}
		else {
			timer += Time.deltaTime;
		}

		//to reflect mesh dimensions
		const float wallSpeedToOffset = 1f/(500f/30f);
		const float floorSpeedToOffset = 1f/(500f/20f);
		buildingMaterial.mainTextureOffset -= new Vector2(currentSimulatedSpeed*wallSpeedToOffset*Time.deltaTime, 0);
		floorMaterial.mainTextureOffset -= new Vector2(0, currentSimulatedSpeed * floorSpeedToOffset * Time.deltaTime);
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
        }
    }

	public void GameOver ()
	{
		//restartText.text = "Press 'R' for Restart";
		gameOverText.text = "Game Over! You lasted: "+timer + "\nPress any key to continue.";
		restart = true;
	}
}
