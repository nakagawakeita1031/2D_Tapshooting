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

    [Header("エネミーの最大生成数")]
    public int maxGenerateCount;

    [Header("エネミーの生成完了管理用")]
    public bool isGenerateEnd;

    [Header("ボス討伐管理用")]
    public bool isBossDestroyed;

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

            //生成したエネミーの数をカウントアップ
            generateCount++;

            Debug.Log("生成したエネミーの数：" + generateCount);

            if (generateCount >= maxGenerateCount)
            {
                isGenerateEnd = true;

                Debug.Log("生成完了");

                //ボスの生成
                StartCoroutine(GenerateBoss());
            }
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
        if (isGenerateEnd)
        {
            return;
        }

        if (!gameManager.isGameUp)
        {
            //エネミー生成の準備
            PreparateGenerateEnemy();
        }

    }

    private IEnumerator GenerateBoss()
    {
        //TODO ボス出現の警告演出

        yield return new WaitForSeconds(1.0f);

        //TODO ボス討伐(仮)
        SwitchBossDestroyed(true);
    }

    public void SwitchBossDestroyed(bool isSwitch)
    {
        isBossDestroyed = isSwitch;

        Debug.Log("ボス討伐");

        //TODO ゲームクリアの準備

    }
}
