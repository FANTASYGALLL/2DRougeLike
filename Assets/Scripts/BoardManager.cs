using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Completeds
{
    public class BoardManager : MonoBehaviour
    {
        [Serializable]
        public class Count
        {
            public int minimum;
            public int maximum;

            public Count(int min, int max)
            {
                minimum = min;
                maximum = max;
            }
        }

        public int columns = 8;
        public int rows = 8;
        public Count wallCount = new Count(5, 9);
        public Count foodCount = new Count(1, 5);
        public GameObject exit;
        public GameObject[] floorTiles;
        public GameObject[] wallTiles;
        public GameObject[] foodTiles;
        public GameObject[] enemyTiles;
        public GameObject[] outWallTiles;

        private Transform boardHolder;
        private List<Vector3> gridPositions = new List<Vector3>();

        void InitialiseList()//清空列表并逐一向列表中加用于生成Tiles的vector
        {
            gridPositions.Clear();
            for (int x = 1; x < columns - 1; x++)
            {
                for (int y = 1; y < rows - 1; y++)
                {
                    gridPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }

        void BoardSetup()//生成地板和外墙
        {
            boardHolder = new GameObject("Borad").transform;
            for (int x = -1; x < columns + 1; x++)
            {
                for (int y = -1; y < rows + 1; y++)
                {
                    GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                    if (x == -1 || x == columns || y == -1 || y == rows)
                        toInstantiate = outWallTiles[Random.Range(0, outWallTiles.Length)];
                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    instance.transform.SetParent(boardHolder);

                }
            }
        }

        Vector3 RandomPosition()//生成物品的位置
        {
            int randomIndex = Random.Range(0, gridPositions.Count);
            Vector3 randomPosition = gridPositions[randomIndex];
            gridPositions.RemoveAt(randomIndex);
            return randomPosition;
        }

        void LayoutObjectAtRandom(GameObject[] titleArray, int minimum, int maximum)
        {
            int objectCount = Random.Range(minimum, maximum);
            for (int i = 0; i < objectCount; i++)
            {
                Vector3 randomPosition = RandomPosition();
                GameObject titleChoice = titleArray[Random.Range(0, titleArray.Length)];
                Instantiate(titleChoice, randomPosition, Quaternion.identity);
            }
        }

        public void SetupScene(int level)
        {
            BoardSetup();
            InitialiseList();
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
            LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.minimum);
            int enemyCount = (int)Math.Log(level, 2f);
            LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
            Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
        }

    }
}