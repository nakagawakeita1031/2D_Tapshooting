using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenceBase : MonoBehaviour
{
    [Header("拠点の耐久値")]
    public int durability;

    [SerializeField]
    private Text txtDurability;

    private int minDurability = 0; //耐久値の最小を代入
    private int maxDurability; //耐久値の最大を代入

    void Start()
    {
        //ゲームの開始時点の耐久値を最大値として代入する
        maxDurability = durability;

        DisplayDurability();

        //TODO ゲージの表示を耐久値に合わせて更新する
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.TryGetComponent(out EnemyContoroller enemyContoroller))
            {
                UpdateDurability(enemyContoroller);
            }
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// 耐久力の更新
    /// </summary>
    /// <param name="enemyContoroller"></param>
    private void UpdateDurability(EnemyContoroller enemyContoroller)
    {
        durability -= enemyContoroller.enemyPower;

        //耐久値を上限・下限値の範囲内に収まるかを確認し、それを超えた場合には上限・制限する
        durability = Mathf.Clamp(durability, minDurability, maxDurability);

        Debug.Log("残りの耐久値" + durability);

        DisplayDurability();


        //TODO ゲージの表示を耐久値に合わせて更新

        //TODO 耐久値が0以下になっていないか確認

        //TODO 耐久値が0以下なら、ゲームオーバーの判定を行う
    }

    /// <summary>
    /// 耐久値の表示更新
    /// </summary>
    private void DisplayDurability()
    {
        //画面に耐久値を現在 / 最大値の形式で表示する
        txtDurability.text = durability + "/" + maxDurability;
    }

}
