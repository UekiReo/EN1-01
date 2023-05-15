using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    // 消失までの時間
    private float lifeTime;

    // 残り時間
    private float leftLifeTime;

    // 移動量
    private Vector3 velocity;

    // 初期scale
    private Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        // 消失までの時間
        lifeTime = 0.3f;

        // 残り時間の初期化
        leftLifeTime = lifeTime;

        // 現在のscaleを記憶
        defaultScale = transform.localScale;

        // ランダムで決まる移動量
        float maxVelocity = 5;

        // 各方向へランダムで飛ばす
        velocity = new Vector3
        (Random.Range(-maxVelocity, maxVelocity), Random.Range(-maxVelocity, maxVelocity), 0);
    }

    // Update is called once per frame
    void Update()
    {
        leftLifeTime -= Time.deltaTime;

        transform.position += velocity * Time.deltaTime;

        transform.localScale = Vector3.Lerp(new Vector3(0, 0, 0), defaultScale, leftLifeTime / lifeTime);

        if (leftLifeTime <= 0) { Destroy(gameObject); }
    }
}