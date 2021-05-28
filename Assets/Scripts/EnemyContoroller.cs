using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoroller : MonoBehaviour
{
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //このスクリプトがアタッチされているゲームオブジェクトを徐々に移動させる
        transform.Translate(0, moveSpeed, 0);

        //Enemyの位置が一定値を超えたら
        if (transform.localPosition.y < -1500f)
        {
            
            Destroy(gameObject);
        }
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            DestroyObjects(collision);
        }
       
    }

    /// <summary>
    /// バレットとエネミーの破壊処理
    /// </summary>
    /// <param name="collision"></param>
    private void DestroyObjects(Collider2D collision)
    {
        //侵入判定確認
        Debug.Log("侵入したオブジェクト名：" + collision.gameObject.tag);

        //Bulletゲームオブジェクトを破壊
        Destroy(collision.gameObject);

        //Enemy破壊
        Destroy(this.gameObject);
    }
}
