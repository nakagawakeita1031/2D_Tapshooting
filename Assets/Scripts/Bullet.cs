using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Bullet : MonoBehaviour
{
    [Header("バレット速度")]
    public float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Rigidbody2D>().AddForce(transform.right * 300);

        //Debug.Log("発射");

    }

    // Update is called once per frame
    void Update()
    {
     if (Input.GetMouseButtonDown(0))
        {
            //ShotBullet();

            Debug.Log("左クリック確認");
        }

        
    }
    /// <summary>
    /// バレット制御
    /// </summary>
    public void ShotBullet(Vector3 direction)
    {
        //バレットの移動処理
        //発射方向と速度を変更
        GetComponent<Rigidbody2D>().AddForce(direction * bulletSpeed);

        Debug.Log("発射速度：" + bulletSpeed);

        //5秒後にbullet破壊
        Destroy(this.gameObject, 5.0f);
    }

}
