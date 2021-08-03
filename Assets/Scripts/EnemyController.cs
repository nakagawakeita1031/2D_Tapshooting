using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(CapsuleCollider2D))]
public class EnemyController : MonoBehaviour
{
    [Header("エネミーのデータ情報")]
    public EnemyDataSO.EnemyData enemyData; //SetUpEnemyメソッドにて引数として受け取ったエネミーのデータを代入する。これにより振る舞いが変わる

    [SerializeField]
    private Image imgEnemy;

    public float moveSpeed;
   
    //TODO後で削除する
    [Header("Enemyの攻撃力")]
    public int enemyPower;

    [SerializeField]
    private Slider slider;

    private int minHP = 0;
    private int maxHP;

    private int enemyHp;

    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private GameObject bulletEffectPrefab;

    /// <summary>
    /// Enemyの設定
    /// </summary>
    /// <param name="isBoss"></param>
    // Start is called before the first frame update
    public void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
    {
        //引数で受け取った情報を変数に代入してスクリプト内で利用できる状態にする
        this.enemyData = enemyData;

        if (this.enemyData.enemyType != EnemyType.Boss)
        {
            //エネミーのx軸(左右)の位置をゲーム画面に収まる範囲でランダムな位置に変更
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-630, 630), transform.localPosition.y, 0);
        }
        else
        {
            //Bossの位置を徐々に下方向に変更
            transform.DOLocalMoveY(transform.localPosition.y - 600, 3.0f);

            //BOssのサイズを大きくする
            transform.localScale = Vector3.one * 7.0f;

            //Hpゲージの位置を高い位置にする
            slider.transform.localPosition = new Vector3(0, 60, 0);
        }

        //画像をEnemyDataの画像にする(ここでエネミーごとの画像に変更する)
        imgEnemy.sprite = this.enemyData.enemySprite;

        //ゲーム開始時点のHPの値を最大値として代入
        maxHP = this.enemyData.hp;

        enemyHp = maxHP;

        //HPゲージの表示更新
        DisplayHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyData.enemyType != EnemyType.Boss)
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
        enemyHp -= bullet.bulletPower; //Bulletスクリプトが管理しているpublic情報であるbulletPower変数を利用する

        //Hpの値の上限・下限を確認して範囲内に制限
        enemyHp = Mathf.Clamp(enemyHp, minHP, maxHP);

        //Hpゲージの表示更新
        DisplayHpGauge();

        //体力が０以下になったら
        if (enemyHp <= 0)
        {
            enemyHp = 0;

            //BOssの場合
            if (enemyData.enemyType == EnemyType.Boss)
            {
                //Boss討伐済みの状態にする
                enemyGenerator.SwitchBossDestroyed(true);
            }

            //Enemy破壊
            Destroy(this.gameObject);
        }
        else
        {
            Debug.Log("残り HP :" + enemyHp);
        }
    }

    /// <summary>
    /// HPゲージの表示更新
    /// </summary>
    private void DisplayHpGauge()
    {
        //Hpゲージを現在値に合わせて制御
        slider.DOValue((float)enemyHp / maxHP, 0.25f);
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
