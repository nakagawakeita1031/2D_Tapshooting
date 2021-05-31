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
        //侵入判定処理
        if (collision.gameObject.tag == "Enemy")
        {
            durability -= 10;

            Debug.Log("残りの耐久値：" + durability);

            Destroy(collision.gameObject);
        }
    }


}
