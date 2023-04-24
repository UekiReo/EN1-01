using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject playerPrehub;
    int[,] map; // レベルデザイン用の配列
    GameObject[,] field; // ゲーム管理用の配列
    int playerIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
       

        //配列
        map = new int[,]
        {
            {0,0,0,0,0},
            {0,0,1,0,0},
            {0,0,0,0,0},
        };
        field = new GameObject
            [
              map.GetLength(0),
              map.GetLength(1)
            ];

        // 変更。二重for文で二次元配列の情報を出力
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 1)
                {
                    field[y,x] = Instantiate(
                        playerPrehub,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
            }
        }
        //PrintArray();
    }

    Vector2Int GetPlayerIndex()
    {
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (field[y, x] == null) { continue; }
                if (field[y, x].tag == "Player") { return new Vector2Int(x, y); }
            }
        }
        return new Vector2Int(-1, -1);
    }

    void PrintArray()
    {
        //string debugText = "";
        //for (int i = 0; i < map.Length; i++)
        //{
        //    //要素を結合
        //    debugText += map[i].ToString() + ",";
        //}
        //Debug.Log(debugText);
    }

   

    bool MoveNumber(int number, Vector2Int moveFrom, Vector2Int moveTo)
    {

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

        if (moveTo.y < 0 || moveTo.y >= map.GetLength(0))
        {
            return false;
        }

        if (moveTo.x < 0 || moveTo.x >= map.GetLength(1))
        {
            return false;
        }

        if (field[moveTo.y,moveTo.x] != null && field[moveTo.y,moveTo.x].tag == "Box")
        {
            Vector2Int Velocity = moveTo - moveFrom;

            bool success = MoveNumber(2, moveTo, moveTo + Velocity);
            if (!success)
            {
                return false;
            }
        }

        map[moveTo.y, moveTo.x] = number;
        map[moveTo.y, moveTo.x] = 0;
        return true;
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.RightArrow))
    //    {
    //        int playerIndex = GetPlayerIndex();
    //        MoveNumber(1, playerIndex, playerIndex + 1);
    //        PrintArray();
    //    }
    //    if (Input.GetKeyDown(KeyCode.LeftArrow))
    //    {
    //        int playerIndex = GetPlayerIndex();
    //        MoveNumber(1, playerIndex, playerIndex - 1);
    //        PrintArray();
    //    }
    //}
}
