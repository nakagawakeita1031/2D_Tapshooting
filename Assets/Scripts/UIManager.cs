using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroupGameClear;

    [SerializeField]
    private CanvasGroup CanvasGroupGameOver;

    [SerializeField]
    private Text txtGameOver;

    [SerializeField]
    private Text txtDurability;

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text txtTotalExp;
    
    /// <summary>
    /// ゲームクリア表示を隠す
    /// </summary>
    public void HideGameClearSet()
    {
        //GameClearSetゲームオブジェクトの透明度を0にして見えなくする
        canvasGroupGameClear.alpha = 0;
    }

    /// <summary>
    /// ゲームクリアを表示
    /// </summary>
    public void DisplayGameClearSet()
    {
        //GameClearSetゲームオブジェクトの透明度を徐々に1にしてゲームクリア表示
        canvasGroupGameClear.DOFade(1.0f, 0.25f);
    }

    public void HideGameOverSet()
    {
        CanvasGroupGameOver.alpha = 0;
    }

    public void DisplayGameOverSet()
    {
        //GameOverSetゲームオブジェクトの透明度を徐々に１にしてゲームオーバー表示
        CanvasGroupGameOver.DOFade(1.0f, 1.0f);

        //ゲーム画面に表示する文字列を用意して代入
        string text = "Game Over";

        //DOTweenのDOTextメソッドを利用して文字列を1文字ずつ順番じモナ次表示時間で表示
        txtGameOver.DOText(text, 1.5f).SetEase(Ease.Linear);
    }

    /// <summary>
    /// 耐久値の表示更新
    /// </summary>
    public void DisplayDurability(int durability, int maxDurability)
    {
        //画面に耐久値を現在 / 最大値の形式で表示する
        txtDurability.text = durability + "/" + maxDurability;

        //ゲージの表示を耐久値に合わせて更新(最初はduability / maxduarbilityの結果が1.0ｆになるので、ゲージは最大値になる)
        slider.DOValue((float)durability / maxDurability, 0.25f);
    }

    /// <summary>
    /// TotalExpの表示更新
    /// </summary>
    /// <param name="totalExp"></param>
    public void UpdateDisplayTotalExp(int totalExp)
    {
        txtTotalExp.text = totalExp.ToString();
    }
}
