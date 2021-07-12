using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroupGameClear;

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
}
