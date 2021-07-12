using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed;
    public int enemyHP;
    [Header("Enemyの攻撃力")]
    public int enemyPower;

    [SerializeField]
    private Slider slider;

    private int minHP = 0;
    private int maxHP;

    //ボスの判定用。trueならBoss、falseならBoss以外
    private bool isBoss;

    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private GameObject bulletEffectPrefab;

    /// <summary>
    /// Enemyの設定
    /// </summary>
    /// <param name="isBoss"></param>
    // Start is called before the first frame update
    public void SetUpEnemy(bool isBoss = false)
    {
        //引数で受け取った情報を変数に代入してスクリプト内で利用できる状態にする
        this.isBoss = isBoss;

        if (!this.isBoss)
        {
            //エネミーのx軸(左右)の位置をゲーム画面に収まる範囲でランダムな位置に変更
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-630, 630), transform.localPosition.y, 0);
        }
        else
        {
            //Bossの位置を徐々に下方向に変更
            transform.DOLocalMoveY(transform.localPosition.y - 500, 3.0f);

            //BOssのサイズを大きくする
            transform.localScale = Vector3.one * 4.0f;

            //Hpゲージの位置を高い位置にする
            slider.transform.localPosition = new Vector3(0, 60, 0);

            //hpを３倍にする
            enemyHP *= 3;
        }

        //ゲーム開始時点のHPの値を最大値として代入
        maxHP = enemyHP;

        //HPゲージの表示更新
        DisplayHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBoss)
        {
            //このスクリプトがアタッチされているゲームオブジェクトを徐々に移動させる
            transform.Translate(0, moveSpeed, 0);
        }

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

                //Bulletヒット演出用エフェクトの生成
                GenerateBulletEffect(collision.gameObject.transform);
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

            //BOssの場合
            if (isBoss)
            {
                //Boss討伐済みの状態にする
                enemyGenerator.SwitchBossDestroyed(true);
            }

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

    private void GenerateBulletEffect(Transform bulletTran)
    {
        GameObject effect = Instantiate(bulletEffectPrefab, bulletTran, false);

        effect.transform.SetParent(transform);

        Destroy(effect, 3.0f);

    }

    /// <summary>
    /// Enemyの追加設定
    /// </summary>
    /// <param name="enemyGenerator"></param>
    public void AdditionalSetUpEnemy(EnemyGenerator enemyGenerator)
    {
        //引数で受け取った情報を変数に代入してスクリプト内で利用できるようにする
        this.enemyGenerator = enemyGenerator;

        Debug.Log("追加設定完了");
    }
}
