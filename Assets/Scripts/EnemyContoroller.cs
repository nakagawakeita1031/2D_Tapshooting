using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyContoroller : MonoBehaviour
{
    public float moveSpeed;
    public int enemyHP;


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
            
            DestroyBullet(collision);

            //侵入してきたコライダー(Bulletゲームオブジェクト)にBulletスクリプトがアタッチされていたら
            //取得してbullet変数に代入しif文内の処理を実行
            if (collision.gameObject.TryGetComponent(out Bullet bullet))
            {
                UpdateHP(bullet);
            }
        }
       
    }

    /// <summary>
    /// バレットとエネミーの破壊処理
    /// </summary>
    /// <param name="collision"></param>
    private void DestroyBullet(Collider2D collision)
    {
        //侵入判定確認
        Debug.Log("侵入したオブジェクト名：" + collision.gameObject.tag);


        //Bulletゲームオブジェクトを破壊
        Destroy(collision.gameObject);

    }

    private void UpdateHP(Bullet bullet) //受け取る情報用の引数を追加
    {
        //Enemyの体力を減らす
        enemyHP -= bullet.bulletPower; //Bulletスクリプトが管理しているpublic情報であるbulletPower変数を利用する

        //体力が０以下になったら
        if (enemyHP <= 0)
        {
            enemyHP = 0;

            //Enemy破壊
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("残り HP :" + enemyHP);
        }
    }
}
