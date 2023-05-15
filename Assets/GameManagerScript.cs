using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    //�N���X�̒��A���\�b�h�̊O�ɏ������Ƃɒ���
    //�Ԃ�l�̌^�ɒ���

    ////�����̏o��
    //void PrintArray()
    //{
    //    //�ǉ��A������̐錾�Ə�����
    //    string debugText = "";
    //    for (int i = 0; i < map.Length; i++)
    //    {
    //        //�ύX�A������Ɍ������Ă���
    //        debugText += map[i].ToString() + ",";
    //    }
    //    // ����������������o��
    //    Debug.Log(debugText);
    //}

    //�v���C���[�̈ʒu�̎擾
    Vector2Int GetPlayerIndex()
    {
        //�v�f����map.Length�Ŏ擾
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

    //�v���C���[�̈ړ�
    bool MoveNumber(string tag, Vector2Int moveFrom, Vector2Int moveTo)
    {
        //�ړ��悪�͈͊O�Ȃ�ړ��s��
        if (moveTo.y < 0 || moveTo.y >= field.GetLength(0)) { return false; }
        if (moveTo.x < 0 || moveTo.x >= field.GetLength(1)) { return false; }

        //�ړ���ɔ�(2)��������
        if (field[moveTo.y, moveTo.x] != null && field[moveTo.y, moveTo.x].tag == "Box")
        {
            //�ǂ̕����ֈړ����邩���Z�o
            Vector2Int velocity = moveTo - moveFrom;
           
            bool success = MoveNumber(tag, moveTo, moveTo + velocity);
            //���������ړ����s������B�v���C���[�̈ړ������s
            if (!success) { return false; }
        }

        //�v���C���[�A���ւ�炸�̈ړ�����
        field[moveFrom.y, moveFrom.x].transform.position =
            new Vector3(moveTo.x, field.GetLength(0) - moveTo.y, 0);

        field[moveTo.y, moveTo.x] = field[moveFrom.y, moveFrom.x];
        field[moveFrom.y, moveFrom.x] = null;
        return true;
    }

    bool IsCleard()
    {
        //Vector2Int�^�̉ϒ��z��̍쐬
        List<Vector2Int> goals = new List<Vector2Int>();

        for (int y = 0; y < map.GetLength(0); y++)
        {
            for (int x = 0; x < map.GetLength(1); x++)
            {
                //�i�[�ꏊ���ۂ��𔻒f
                if (map[y, x] == 3)
                {
                    //�i�[�ꏊ�̃C���f�b�N�X���T���Ă���
                    goals.Add(new Vector2Int(x, y));
                }
            }
        }

        //�v�f����goals.Cunt�Ŏ擾
        for (int i = 0; i < goals.Count; i++)
        {
            GameObject f = field[goals[i].y, goals[i].x];
            if (f == null || f.tag != "Box")
            {
                //��ł������Ȃ�������������B��
                return false;
            }
        }
        //�������B���o�Ȃ���Ώ����B��
        return true;
    }

    public GameObject playerPrefab;
    public GameObject boxPrefab;
    public GameObject goalPrefab;
    public GameObject particlePrefab;

    public GameObject clearText;

    //�z��̐錾
    int[,] map;//���x���f�U�C���p�̔z��
    GameObject[,] field;//�Q�[���Ǘ��p�̔z��

    // Start is called before the first frame update
    void Start()
    {

        Screen.SetResolution(1920, 1080, false);

        //�z��̍쐬�Ə�����
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
        //�v���C���[�̈ʒu�̎擾
        Vector2Int playerMoveFrom = GetPlayerIndex();
        Vector2Int playerMoveTo = playerMoveFrom + new Vector2Int(0, 0);

        //�N���A���ĂȂ��Ƃ��ړ��ł���
        if (IsCleard() != true)
        {
            //�E�L�[���������Ƃ�
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(1, 0);
                //�v���C���[�̈ړ�
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //���L�[���������Ƃ�
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(-1, 0);
                //�v���C���[�̈ړ�
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //��L�[���������Ƃ�
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(0, -1);
                //�v���C���[�̈ړ�
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //���L�[���������Ƃ�
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                playerMoveTo = playerMoveFrom + new Vector2Int(0, 1);
                //�v���C���[�̈ړ�
                MoveNumber("Player", playerMoveFrom, playerMoveTo);
            }
            //���̑�
            else { }
        }
        //�N���A������
        else if (IsCleard())
        {
            //�Q�[���I�u�W�F�N�g��SetActive���\�b�h���g���L����
            clearText.SetActive(true);
        }
    }
}
