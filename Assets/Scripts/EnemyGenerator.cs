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

    [Header("エネミーのスクリプタブル・オブジェクト")]
    public EnemyDataSO enemyDataSO;

    //Normalタイプのエネミーのデータだけ代入されているList Debug用にpublic
    public List<EnemyDataSO.EnemyData> normalEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public List<EnemyDataSO.EnemyData> bossEnemyDatas = new List<EnemyDataSO.EnemyData>();

    public void SetUpEnemyGenerator(GameManager gameManager)
    {
        this.gameManager = gameManager;

        //引数で指定したエネミーのタイプリストを作成
        normalEnemyDatas = GetEnemyTypeList(EnemyType.Normal);

        bossEnemyDatas = GetEnemyTypeList(EnemyType.Boss);
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
    private void GenerateEnemy(EnemyType enemyType = EnemyType.Normal)
    {
        //ランダムな値を代入する為の変数を宣言
        int randomEnemyNo;

        //EnemyDataを代入するための変数を宣言
        EnemyDataSO.EnemyData enemyData = null;

        //EnemyTypeに合わせて生成するエネミーの種類を決定し、そのエネミーの種類ごとのリストからランダムなEnemyDataを取得
        switch (enemyType)
        {
            case EnemyType.Normal:
                randomEnemyNo = Random.Range(0, normalEnemyDatas.Count);
                enemyData = normalEnemyDatas[randomEnemyNo];
                break;
            case EnemyType.Boss:
                randomEnemyNo = Random.Range(0, bossEnemyDatas.Count);
                enemyData = bossEnemyDatas[randomEnemyNo];
                break;
        }

        //クローン生成
        //エネミーオブジェクトにアタッチされているEnemyControllerスクリプトの情報を取得し変数に代入
        //EnemyControllerスクリプトのSetUpEnemyメソッドを実行
        GameObject enemySetObj = Instantiate(enemyPrefab, transform, false);

        EnemyController enemyController = enemySetObj.GetComponent<EnemyController>();

        //EnemyControllerスクリプトのSetUpEnemyメソッドを実行する(Startメソッドの代わりになる処理)
        enemyController.SetUpEnemy(enemyData);

        //追加設定を行う
        enemyController.AdditionalSetUpEnemy(this);
    }

    void Update()
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

    /// <summary>
    /// Bossの生成
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateBoss()
    {
        //TODO ボス出現の警告演出

        yield return new WaitForSeconds(1.0f);

        //Boss生成
        GenerateEnemy(EnemyType.Boss);
    }

    /// <summary>
    /// Boss討伐状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchBossDestroyed(bool isSwitch)
    {
        isBossDestroyed = isSwitch;

        Debug.Log("ボス討伐");

        //Boss討伐に合わせて、ゲーム終了の状態に切り替える
        gameManager.SwitchGameUp(isBossDestroyed);

        //ゲームクリアの準備
        gameManager.PreparateGameClear();
    }

    /// <summary>
    /// 引数で指定されたエネミーの種類のListを作成し、作成した値を戻す
    /// </summary>
    /// <param name="enemyType"></param>
    /// <returns></returns>
    private List<EnemyDataSO.EnemyData> GetEnemyTypeList(EnemyType enemyType)
    {
        List<EnemyDataSO.EnemyData> enemyDatas = new List<EnemyDataSO.EnemyData>();

        //引数のタイプのエネミーのデータだけを抽出してenemyDatasリストにEnemyDataを追加してListを作成していく
        for (int i = 0; i < enemyDataSO.enemyDataList.Count; i++)
        {
            if (enemyDataSO.enemyDataList[i].enemyType == enemyType)
            {
                enemyDatas.Add(enemyDataSO.enemyDataList[i]);
            }
        }

        //抽出処理の結果を戻す
        return enemyDatas;
    }

    /// <summary>
    /// TotalExpの表示更新準備
    /// </summary>
    /// <param name="exp"></param>
    public void PreparateDisplayTotalExp(int exp)
    {
        //GameManagerスクリプトからUIManagerスクリプトのUpdateDisplayTotalExpメソッドを実行する
        gameManager.uIManager.UpdateDisplayTotalExp(GameData.instance.GetTotalExp());

        //TODO 引数のexp変数は後々利用する
    }
}
