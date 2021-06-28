using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    
    [SerializeField,Header("エネミーのプレファブ")]
    private GameObject enemyPrefab;

    //基準値
    [Header("基準値")]
    private float timer;
    //目標値
    [Header("目標値")]
    public int preparateTimer;

    //生成したエネミーの数をカウントする為の変数
    private int generateCount;

    private GameManager gameManager;

    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    /// <summary>
    /// エネミー生成の準備
    /// </summary>
    private void PreparateGenerateEnemy()
    {
        timer += Time.deltaTime;

        //生成時間に到達したら
        if (timer >= preparateTimer)
        {
            timer = 0;

            //enemy生成
            GenerateEnemy(); 
        }
    }
    /// <summary>
    /// エネミーの生成
    /// </summary>
    private void GenerateEnemy()
    {
        //クローン生成
        //エネミーオブジェクトにアタッチされているEnemyControllerスクリプトの情報を取得し変数に代入
        //EnemyControllerスクリプトのSetUpEnemyメソッドを実行
        Instantiate(enemyPrefab, transform, false).GetComponent<EnemyContoroller>().SetUpEnemy();
    }

    private void Update()
    {
        if (!gameManager.isGameUp)
        {
            //エネミー生成の準備
            PreparateGenerateEnemy();
        }

    }
}
