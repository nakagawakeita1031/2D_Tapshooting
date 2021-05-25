using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject bulletPrefab;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //マウスの左クリックを押したら
        if (Input.GetMouseButtonDown(0))
        {
            //画面をタップ(クリック)した位置をカメラのスクリーン座標の情報を通じてワールド座標に変換
            Vector3 tapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log("タップした位置情報：" + tapPos);

            //方向を計算(マウスクリックの位置からキャラの位置を減算する)
            Vector3 direction = tapPos - transform.position;

            //方向の情報から、不要なZ成分(Z軸情報)の除去を行う
            direction = Vector3.Scale(direction, new Vector3(1, 1, 0));

            //正規化処理を行い、単位ベクトルとする(方向の情報は持ちつつ、距離によって速度差をなくして一定化する)
            direction = direction.normalized;


            Debug.Log("正規化処理後の方向：" + direction)
                ;

            //バレット生成
            GenerateBullet(direction);
        }
    }
    /// <summary>
    /// バレット生成
    /// </summary>
    private void GenerateBullet(Vector3 direction)
    {
        //bulletPrefab変数の値のクローンを生成し、戻り値をbulletObj変数に代入。
        //生成位置はPlayerSetゲームオブジェクトの子オブジェクトに指定
        GameObject bulletObj = Instantiate(bulletPrefab, transform);

        //bulletObj変数(Bulletゲームオブジェクトが代入されている)にアタッチされているBulletスクリプトの
        //情報を取得しShotBulletメソッドに処理を行うように命令
        bulletObj.GetComponent<Bullet>().ShotBullet(direction);
    }
}
