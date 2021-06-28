using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyContoroller : MonoBehaviour
{
    public float moveSpeed;
    public int enemyHP;
    [Header("Enemyの攻撃力")]
    public int enemyPower;

    [SerializeField]
    private Slider slider;

    private int minHP = 0;
    private int maxHP;

    // Start is called before the first frame update
    public void SetUpEnemy()
    {
        //エネミーのx軸(左右)の位置をゲーム画面に収まる範囲でランダムな位置に変更
        transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-630, 630), transform.localPosition.y, 0);
        
        //ゲーム開始時点のHPの値を最大値として代入
        maxHP = enemyHP;

        //HPゲージの表示更新
        DisplayHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
        //このスクリプトがアタッチされているゲームオブジェクトを徐々に移動させる
        transform.Translate(0, moveSpeed, 0);

        //Enemyの位置が一定値を超えたら
        if (transform.localPosition.y < -2500f)
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
    /// <summary>
    /// Hpの更新処理とエネミーの破壊確認処理
    /// </summary>
    /// <param name="bullet"></param>
    private void UpdateHP(Bullet bullet) //受け取る情報用の引数を追加
    {
        //Enemyの体力を減らす
        enemyHP -= bullet.bulletPower; //Bulletスクリプトが管理しているpublic情報であるbulletPower変数を利用する

        //Hpの値の上限・下限を確認して範囲内に制限
        enemyHP = Mathf.Clamp(enemyHP, minHP, maxHP);

        //Hpゲージの表示更新
        DisplayHpGauge();

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

    /// <summary>
    /// HPゲージの表示更新
    /// </summary>
    private void DisplayHpGauge()
    {
        //Hpゲージを現在値に合わせて制御
        slider.DOValue((float)enemyHP / maxHP, 0.25f);
    }
}
