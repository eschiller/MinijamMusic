using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public bool initializePlayersAndCams = false;
    public int players;
    public Transform player1;
    public Transform player2;
    private Dictionary<string, GameObject> allEnemies;

    public GameObject p1cam;
    public GameObject p1coopcam;
    public GameObject p2coopcam;
    public bool cameraFollowsPlayer = false;

    public GameManager gameMgr;

    private GameObject cam1;
    private GameObject cam2;


    public LevelManager(){
        allEnemies = new Dictionary<string, GameObject>();
    }

    // Use this for initialization
    void Start () {
        if (initializePlayersAndCams) {
            this.InitPlayersAndCams();
        }

        gameMgr = GameObject.Find("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        CheckForWin();
        CheckForLoss();
        UpdateEnemies();
    }


    public void SpawnEnemy(string eName, GameObject ePrefab, Vector3 eLocation) {
        GameObject newEnemy = Instantiate(ePrefab, eLocation, Quaternion.identity);
        allEnemies.Add(eName, newEnemy);
    }

    public int GetEnemyCount(){
        return allEnemies.Count;
    }

    private void UpdateEnemies(){
        foreach(var enemyKey in allEnemies.Keys) {
            if (allEnemies[enemyKey] == null) {
                allEnemies.Remove(enemyKey);
            }
        }
    }

    public void CheckForWin() {
        if (GameObject.Find("SpiderBoss")  == null) {
            if (gameMgr != null)
            {
                gameMgr.WinGame();
            }
        }

    }

    public void CheckForLoss()
    {
        //Simple win state of checking of all enemies are dead
        if (GameObject.FindGameObjectsWithTag("Player").Length == 0)
        {
            if (gameMgr != null)
            {
                gameMgr.LoseGame();
            }
        }
    }


    public void InitPlayersAndCams()
    {
        // Check if number of players were were set in player prefs (this may
        // have happened during the title screen). If not, check if the variable
        // was set in inspector, and if not, assume that it's 1.
        if (PlayerPrefs.GetInt("playerSelection", -1) != -1)
        {
            players = PlayerPrefs.GetInt("playerSelection");
        }
        else if (players == 0)
        {
            players = 1;
        }

        // if just one player, make a full screen camera and assign it to created player1
        if (players == 1)
        {
            player1 = Instantiate(player1, new Vector3(0, 0, -1), Quaternion.identity) as Transform;

            cam1 = (GameObject)Instantiate(p1cam, new Vector3(0, 0, -10), Quaternion.identity);

            if (cam1 == null)
            {
                Debug.Log("Couldn't create cam1");
            }

            if (cameraFollowsPlayer)
            {
                CameraFollow camfollow_script = cam1.GetComponent<CameraFollow>();

                if (camfollow_script == null)
                {
                    Debug.Log("Couldn't get theCameraFollow component");
                }

                camfollow_script.myPlay = player1;
            }

            //if 2 players, create two cameras and assign them to players 1 and 2
        }
        else if (players == 2)
        {
            player1 = Instantiate(player1, new Vector3(-32, 0, -1), Quaternion.identity) as Transform;
            player2 = Instantiate(player2, new Vector3(32, 0, -1), Quaternion.identity) as Transform;

            cam1 = (GameObject)Instantiate(p1coopcam, new Vector3(0, 0, -10), Quaternion.identity);
            cam2 = (GameObject)Instantiate(p2coopcam, new Vector3(0, 0, -10), Quaternion.identity);

            if (cameraFollowsPlayer)
            {
                CameraFollow camfollow_script1 = cam1.GetComponent<CameraFollow>();
                CameraFollow camfollow_script2 = cam2.GetComponent<CameraFollow>();

                camfollow_script1.myPlay = player1;
                camfollow_script2.myPlay = player2;
            }
        }
    }
}
