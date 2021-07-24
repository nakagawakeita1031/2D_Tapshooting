using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenceBase : MonoBehaviour
{
    [Header("拠点の耐久値")]
    public int durability;

    private int minDurability = 0; //耐久値の最小を代入
    private int maxDurability; //耐久値の最大を代入

    private GameManager gameManager;

    [SerializeField]
    private GameObject enemyAttackEffectPrefab;

    /// <summary>
    /// DefenseBaseの設定
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpDefenceBase(GameManager gameManager)
    {
        //引数を利用して、gameManagerスクリプトの情報を受け取って用意しておいた変数に代入
        this.gameManager = gameManager;

        //ゲームの開始時点の耐久値を最大値として代入する
        maxDurability = durability;

        gameManager.uIManager.DisplayDurability(durability, maxDurability);

        //TODO ゲージの表示を耐久値に合わせて更新する
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {

            if (collision.TryGetComponent(out EnemyController enemyContoroller))
            {
                UpdateDurability(enemyContoroller);

            }

            //エネミーの攻撃演出用のエフェクト生成
            GenerateEnemyAttackEffect(collision.gameObject.transform);

            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    /// <param name="enemyContoroller"></param>
    private void UpdateDurability(EnemyController enemyContoroller)
    {
        durability -= enemyContoroller.enemyPower;

        //耐久値を上限・下限値の範囲内に収まるかを確認し、それを超えた場合には上限・制限する
        durability = Mathf.Clamp(durability, minDurability, maxDurability);

        Debug.Log("残りの耐久値" + durability);

        gameManager.uIManager.DisplayDurability(durability, maxDurability);


        //耐久値が0以下になっていないか確認
        if (durability <= 0 && gameManager.isGameUp == false)
        {
            Debug.Log("Game Over");

            //耐久値が0以下なら、ゲームオーバーの判定を行う
            gameManager.SwitchGameUp(true);

            gameManager.PreparateGameOver();
        }


    }

    /// <summary>
    /// エネミーが拠点に侵入した際の攻撃演出用のエフェクト生成
    /// </summary>
    /// <param name="enemyTran"></param>
    private void GenerateEnemyAttackEffect(Transform enemyTran)
    {
        GameObject enemyAttackEffect = Instantiate(enemyAttackEffectPrefab, enemyTran, false);

        //enemyAttackEffect.transform.SetParent(TransformHelper.GetTemporaryObjectContainerTran());

        //生成されたエフェクトをTemporaryObjectContainerTranの子オブジェトにする(引数にTemporaryObjectContainerTranプロパティを利用)
        enemyAttackEffect.transform.SetParent(TransformHelper.TemporaryObjectContainerTran);

        Destroy(enemyAttackEffect, 3.0f);
    }
}
