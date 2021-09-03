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

        //移動タイプに応じた移動方法を選択して実行
        SetMoveByMoveType();
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

            //ExpをTotalExpに加算
            GameData.instance.UpdateToatalExp(enemyData.exp);

            //最新のTotalExpを利用して表示更新
            enemyGenerator.PreparateDisplayTotalExp(enemyData.exp);

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

    /// <summary>
    /// 移動タイプに応じた移動方法を選択して実行
    /// </summary>
    private void SetMoveByMoveType()
    {
        //moveTypeで分岐
        switch (enemyData.moveType)
        {
            //Straightの場合
            case MoveType.Straight:
                MoveStraight();
                break;

            case MoveType.Meandering:
                MoveMeandering();
                break;

            case MoveType.Boss_Horizontal:
                MoveHorizontal();
                break;
        }
    }

    /// <summary>
    /// 直進移動
    /// </summary>
    private void MoveStraight()
    {
        Debug.Log("直進");

        transform.DOLocalMoveY(-3000, enemyData.moveDuration);
    }

    /// <summary>
    /// 蛇行移動
    /// </summary>
    private void MoveMeandering()
    {
        Debug.Log("蛇行");

        //左右方向の移動をループ処理することで行ったり来たりさせる。左右の移動幅はランダム、移動間隔は等速
        //-1は無限ループ
        transform.DOLocalMoveX(transform.position.x + Random.Range(200.0f, 400.0f), 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        transform.DOLocalMoveY(-3000, enemyData.moveDuration);
    }

    /// <summary>
    /// ボス・水平移動
    /// </summary>
    private void MoveHorizontal()
    {
        transform.localPosition = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
        transform.DOLocalMoveY(-650, 3.0f).OnComplete(() => 
        { 
            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x + 550, 2.5f).SetEase(Ease.Linear)); //[2]
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x + -550, 5.0f)).SetEase(Ease.Linear);//[3]
            sequence.Append(transform.DOLocalMoveX(transform.localPosition.x, 2.5f).SetEase(Ease.Linear));       //[4]
            sequence.AppendInterval(1.0f).SetLoops(-1, LoopType.Restart);                                        //[5]
        });
    }

}
