using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ゲーム終了判定値")]
    public bool isGameUp;//ゲーム終了がtrue

    [SerializeField]
    private DefenceBase defenceBase;
    
    [SerializeField]
    private PlayerController playerController;
   
    [SerializeField]
    private EnemyGenerator enemyGenerator;

    [SerializeField]
    private Transform temporaryObjectContainerTran;

    // Start is called before the first frame update
    void Start()
    {
        //ゲーム終了の判定を、「ゲームが終了していない(未終了)状態 = false」に切り替える(false = ゲーム終了していない状態として運用する)
        SwitchGameUp(false);

        //DefenceBaseスクリプトに用意した、DefenceBaseの設定を行うためのSetUpDefenceBaseメソッドを呼び出す。
        //引数としてGameManagerの情報を渡す
        defenceBase.SetUpDefenceBase(this);

        playerController.SetUpplayer(this);

        enemyGenerator.SetUpEnemyGenerator(this);

        //TransformHelperスクリプトのtemporaryObjectContainerTran変数に情報を渡す
        //TransformHelper.SetTemporaryObjectContainerTran(temporaryObjectContainerTran);

        //TransformHelperスクリプトのtemporaryObjectContainerTranプロパティにtemporaryObjectContainerTran変数の情報を代入する
        TransformHelper.TemporaryObjectContainerTran = temporaryObjectContainerTran;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ゲーム終了状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchGameUp(bool isSwitch)
    {
        isGameUp = isSwitch;
    }
}
