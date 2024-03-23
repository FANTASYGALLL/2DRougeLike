
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Completeds
{
    using System.Collections.Generic;       //Allows us to use Lists. 
    using UnityEngine.UI;                   //Allows us to use UI.

    public class GameManager : MonoBehaviour
    {
        public float levelStartDelay = 2f;                        //Time to wait before starting level, in seconds.
        public float turnDelay = 0.1f;                            //Delay between each Player turn.
        public int playerFoodPoints = 100;                        //Starting value for Player food points.
        public  static GameManager instance = null;                //Static instance of GameManager which allows it to be accessed by any other script.
        [HideInInspector] public bool playersTurn = true;        //Boolean to check if it's players turn, hidden in inspector but public.


        private BoardManager boardScript;                        //Store a reference to our BoardManager which will set up the level.
        private int level = 1;                                    //Current level number, expressed in game as "Day 1".
        private List<Enemy> enemies;                            //List of all Enemy units, used to issue them move commands.
        private bool enemiesMoving;                                //Boolean to check if enemies are moving.
        private bool doingSetup = true;                         //Boolean to check if we're setting up board, prevent Player from moving during setup.
        private Text levelText;
        private GameObject levelImage;
        private GameObject exitGame;
        private GameObject restartGame;

        private void OnLevelWasLoaded(int index)
        {
            level++;
            
            InitGame();
        }
        //Awake is always called before any Start functions
        void Awake()
        {
            //Check if instance already exists
            if (instance == null)

                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            //Assign enemies to a new List of Enemy objects.
            enemies = new List<Enemy>();

            //Get a component reference to the attached BoardManager script
            boardScript = GetComponent<BoardManager>();

            
            
            //Call the InitGame function to initialize the first level 
            InitGame();
        }

      


        //Initializes the game for each level.
        void InitGame()
        {
            doingSetup = true;

            exitGame = GameObject.Find("ExitGame");
            restartGame = GameObject.Find("ReStart");

            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Day" + level;
            levelImage.SetActive(true);
            //Clear any Enemy objects in our List to prepare for next level.
            Invoke("HideLevelImage", levelStartDelay);


            enemies.Clear();
            HideExitGame();
            //Call the SetupScene function of the BoardManager script, pass it current level number.
            boardScript.SetupScene(level);

            

        }

        private void HideExitGame()
        {
            exitGame.SetActive(false);
            restartGame.SetActive(false);
        }

        private void HideLevelImage()
        {
            levelImage.SetActive(false);
            doingSetup = false;
            
        }

        //Update is called every frame.
        void Update()
        {
            //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
            if (playersTurn || enemiesMoving|| doingSetup)

                //If any of these are true, return and do not start MoveEnemies.
                return;

            //Start moving enemies.
            StartCoroutine(MoveEnemies());
        }

        //Call this to add the passed in Enemy to the List of Enemy objects.
        public void AddEnemyToList(Enemy script)
        {
            //Add Enemy to List enemies.
            enemies.Add(script);
        }


        //GameOver is called when the player reaches 0 food points
        public void GameOver()
        {
            restartGame.SetActive(true);
            exitGame.SetActive(true);
            levelText.text="After"+ level  +"days , you die";
            levelImage.SetActive(true);
            //Disable this GameManager.
            enabled = false;
        }

        public void RestartGame()
        {
            level = 1;
           
            playerFoodPoints = 100;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);


        }

        //Coroutine to move enemies in sequence.
        IEnumerator MoveEnemies()
        {
            //While enemiesMoving is true player is unable to move.
            enemiesMoving = true;

            //Wait for turnDelay seconds, defaults to .1 (100 ms).
            yield return new WaitForSeconds(turnDelay);

            //If there are no enemies spawned (IE in first level):
            if (enemies.Count == 0)
            {
                //Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
                yield return new WaitForSeconds(turnDelay);
            }

            //Loop through List of Enemy objects.
            for (int i = 0; i < enemies.Count; i++)
            {
                //Call the MoveEnemy function of Enemy at index i in the enemies List.
                enemies[i].MoveEnemy();

                //Wait for Enemy's moveTime before moving next Enemy, 
                yield return new WaitForSeconds(enemies[i].moveTime);
            }
            //Once Enemies are done moving, set playersTurn to true so player can move.
            playersTurn = true;

            //Enemies are done moving, set enemiesMoving to false.
            enemiesMoving = false;
        }
    }
}