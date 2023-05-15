using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //クラスの中、メソッドの外に書くことに注意
    //返り値の型に注意

    ////文字の出力
    //void PrintArray()
    //{
    //    //追加、文字列の宣言と初期化
    //    string debugText = "";
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        //変更、文字列に結合していく
    //        debugText += map[i].ToString() + ",";
    //    }
    //    // 結合した文字列を出力
    //    Debug.Log(debugText);
    //}

    //プレイヤーの位置の取得
    Vector2Int GetPlayerIndex()
    {
        //要素数はmap.Lengthで取得
        for (int y = 0; y < field.GetLength(0); y++)
        {
            for (int x = 0; x < field.GetLength(1); x++)
            {
                if (field[y, x] != null && field[y, x].tag == "Player")
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }

    //プレイヤーの移動
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //移動先が範囲外なら移動不可
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //移動先に箱(2)がいたら
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //どの方向へ移動するかを算出
            Vector2Int velocity = moveTo - moveFrom;
           
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            //もし箱が移動失敗したら。プレイヤーの移動も失敗
            if (!success) { return false; }
        }

        //プレイヤー、箱関わらずの移動処理
        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        //Vector2Int型の可変長配列の作成
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //格納場所か否かを判断
                if (map[y, x] == 3)
                {
                    //格納場所のインデックスを控えておく
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //要素数はgoals.Cuntで取得
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //一つでも箱がなかったら条件未達成
                return false;
            }
        }
        //条件未達成出なければ条件達成
        return true;
    }

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;

    public GameObject clearText;

    //配列の宣言
    int[,] map;//レベルデザイン用の配列
    GameObject[,] field;//ゲーム管理用の配列

    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1920, 1080, false);

        //配列の作成と初期化
        map = new int[,] {
            { 0, 0, 0, 0, 0},
            { 0, 3, 1, 3, 0},
            { 0, 0, 2, 0, 0},
            { 0, 2, 3, 2, 0},
            { 0, 0, 0, 0, 0},
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
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                else if (map[y, x] == 2)
                {
                    field[y, x] = Instantiate(
                        boxPrefab,
                        new Vector3(x, map.GetLength(0) - y, 0),
                        Quaternion.identity
                        );
                }
                else if (map[y, x] == 3)
                {
                    field[y, x] = Instantiate(
                        goalPrefab,
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
        //プレイヤーの位置の取得
        Vector2Int playerMoveFrom = GetPlayerIndex();
        Vector2Int playerMoveTo = playerMoveFrom + new Vector2Int(0, 0);

        //クリアしてないとき移動できる
        if (IsCleard() != true)
        {
            //右キーを押したとき
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(1, 0);
                //プレイヤーの移動
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //左キーを押したとき
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(-1, 0);
                //プレイヤーの移動
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //上キーを押したとき
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(0, -1);
                //プレイヤーの移動
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //下キーを押したとき
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(0, 1);
                //プレイヤーの移動
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //その他
            else { }
        }
        //クリアした時
        else if (IsCleard())
        {
            //ゲームオブジェクトのSetActiveメソッドを使い有効化
            clearText.SetActive(true);
        }
    }
}
