using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{

    // �����܂ł̎���
    private float lifeTime;

    // �c�莞��
    private float leftLifeTime;

    // �ړ���
    private Vector3 velocity;

    // ����scale
    private Vector3 defaultScale;

    // Start is called before the first frame update
    void Start()
    {
        // �����܂ł̎���
        lifeTime = 0.3f;

        // �c�莞�Ԃ̏�����
        leftLifeTime = lifeTime;

        // ���݂�scale���L��
        defaultScale = transform.localScale;

        // �����_���Ō��܂�ړ���
        float maxVelocity = 5;

        // �e�����փ����_���Ŕ�΂�
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