using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class DefenceBase : MonoBehaviour
{
    [Header("拠点の耐久値")]
    public int durability;

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

        Debug.Log("残りの耐久値" + durability);

        //TODO 耐久値が0以下になっていないか確認

        //TODO 耐久値が0以下なら、ゲームオーバーの判定を行う
    }


}
