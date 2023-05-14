using System.Collections;
using System.Collections.Generic;
//using System.Runtime.InteropServices.WindowsRuntime;
//using UnityEditor.Timeline;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //プレイヤーの位置を取得
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
    //数字の移動
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動先が範囲外なら移動不可
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0))
        {
            return false;
        }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1))
        {
            return false;
        }

        //移動先に箱が居たら
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            Vector2Int velocity = moveTo - moveFrom;
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            if (!success)
            {
                return false;
            }
        }

        field[moveFrom.y, moveFrom.x].transform.position = new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);
        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;

    }

    //クリア表示
    bool IsCleard()
    {
        List<Vector2Int> goals = new List<Vector2Int>();
        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                if (map[y, x] == 3)
                {
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                return false;
            }
        }
        return true;
    }

    public GameObject playerPrehub;
    public GameObject BoxPrehub;
    public GameObject goalPrehub;

    // 追記
    public GameObject ClearText;

    //配列
    int[,] map;
    GameObject[,] field;

    // Start is called before the first frame update
    void Start()
    {


        //配列の実態の作成と初期化
        map = new int[,]
        {
            {0,0,1,0,0 },
            {0,0,0,0,0,},
            {0,3,2,3,0 },
            {0,2,3,2,0 },
            {0,0,0,0,0 }
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
                    field[y, x] = Instantiate
                       (
                            playerPrehub,
                            new Vector3(x, map.GetLength(0) - y, 0),
                            Quaternion.identity
                       );
                }

                if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate
                     (
                          BoxPrehub,
                          new Vector3(x, map.GetLength(0) - y, 0),
                          Quaternion.identity
                     );
                }

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //右移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //位置取得
            Vector2Int playerIndex = GetPlayerIndex();
            //数字の移動
            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(1, 0));
            //もしクリアしていたら
        }

        //左移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector2Int playerIndex = GetPlayerIndex();
            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(-1, 0));
            //もしクリアしていたら
            if (IsCleard())
            {
                Debug.Log("Clear!!");
                ClearText.SetActive(true);
            }
        }

        //上移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //位置取得
            Vector2Int playerIndex = GetPlayerIndex();
            //数字の移動
            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(0, -1));
            //もしクリアしていたら
            if (IsCleard())
            {
                Debug.Log("Clear!!");
                ClearText.SetActive(true);
            }
        }

        //下移動
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //位置取得
            Vector2Int playerIndex = GetPlayerIndex();
            //数字の移動
            MoveNumber("player", playerIndex, playerIndex + new Vector2Int(0, 1));
            //もしクリアしていたら
            if (IsCleard())
            {
                Debug.Log("Clear!!");
                ClearText.SetActive(true);
            }
        }
    }
}