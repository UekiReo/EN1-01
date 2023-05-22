using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject wallPrefab;
    public GameObject clearText;

    int[,] map;             //レベルデザイン用の配列
    GameObject[,] field;    //ゲーム管理用の配列
    List<GameObject[,]> beforeField = new List<GameObject[,]>();

    // Start is called before the first frame update
    void Start()
    {
        //GameObject instance = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

        Screen.SetResolution(1920, 1080, false);

        map = new int[,] {
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
            {4, 1, 4, 3, 0, 0, 0, 0, 0, 0, 4},
            {4, 0, 4, 4, 0, 0, 0, 4, 0, 0, 4},
            {4, 0, 0, 0, 3, 0, 3, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 2, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 2, 3, 2, 2, 0, 0, 4},
            {4, 0, 0, 0, 0, 4, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 4, 0, 0, 0, 0, 0, 4},
            {4, 0, 0, 0, 0, 0, 0, 0, 0, 4, 4},
            {4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4},
        };
        field = new GameObject
            [
                map.GetLength(0),
                map.GetLength(1)
            ];

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y, x] = Instantiate(
                        playerPrefab,
                        new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0),
                        Quaternion.identity);
                }
                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0),
                        Quaternion.identity);
                }
                if (map[y, x] == 3)
                {
                    Instantiate(
                    goalPrefab,
                    new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0.01f),
                    Quaternion.identity);
                }
                if (map[y, x] == 4)
                {
                    field[y, x] = Instantiate(
                    wallPrefab,
                    new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0.01f),
                    Quaternion.identity);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCleard() != true)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                beforeField.Add(field); //一個前のField情報を記録
                Vector2Int playerIndex = GetPlayerIndex();

                Vector2Int moveTo = new Vector2Int(playerIndex.x - 1, playerIndex.y);

                MoveNumber("Player", playerIndex, moveTo);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                beforeField.Add(field); //一個前のField情報を記録
                Vector2Int playerIndex = GetPlayerIndex();

                Vector2Int moveTo = new Vector2Int(playerIndex.x + 1, playerIndex.y);

                MoveNumber("Player", playerIndex, moveTo);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                beforeField.Add(field); //一個前のField情報を記録
                Vector2Int playerIndex = GetPlayerIndex();

                Vector2Int moveTo = new Vector2Int(playerIndex.x, playerIndex.y - 1);

                MoveNumber("Player", playerIndex, moveTo);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                beforeField.Add(field); //一個前のField情報を記録
                Vector2Int playerIndex = GetPlayerIndex();

                Vector2Int moveTo = new Vector2Int(playerIndex.x, playerIndex.y + 1);

                MoveNumber("Player", playerIndex, moveTo);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            beforeField.Add(field);
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Destroy(field[y, x]);
                }
            }
            field = new GameObject
             [
              map.GetLength(0),
              map.GetLength(1)
             ];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 1)
                    {
                        field[y, x] = Instantiate(
                            playerPrefab,
                            new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0),
                            Quaternion.identity);
                    }
                    if (map[y, x] == 2)
                    {
                        field[y, x] = Instantiate(
                            boxPrefab,
                            new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0),
                            Quaternion.identity);
                    }
                    if (map[y, x] == 3)
                    {
                        Instantiate(
                        goalPrefab,
                        new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0.01f),
                        Quaternion.identity);
                    }
                    if (map[y, x] == 4)
                    {
                        field[y, x] = Instantiate(
                        wallPrefab,
                        new Vector3(x - (map.GetLength(1) / 2.0f), map.GetLength(0) - y - (map.GetLength(0) / 2.0f), 0.01f),
                        Quaternion.identity);
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {

        }

        if (IsCleard())
        {
            clearText.SetActive(true);
        }
        else
        {
            clearText.SetActive(false);
        }
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] == null)
                {
                    continue;
                }

                if (field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {

        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= map.GetLength(1))
        {
            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Wall")
        {
            return false;
        }

        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;

            bool succes = MoveNumber(tag, moveTo, moveTo + velocity);

            if (!succes)
            {
                return false;
            }
        }

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x - (map.GetLength(1) / 2.0f), field.GetLength(0) - moveTo.y - (map.GetLength(0) / 2.0f), 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];

        field[moveFrom.y, moveFrom.x] = null;

        return true;
    }

    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //収納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //収納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }

            }
        }

        //要素数はgoals.Countで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成でなければ条件達成
        return true;
    }
}