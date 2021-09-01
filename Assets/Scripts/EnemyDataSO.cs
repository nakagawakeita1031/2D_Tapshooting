﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDataList = new List<EnemyData>();

    [Serializable]
    public class EnemyData
    {
        public int no;                //エネミーの通し番号
        public int hp;　　　　　　　　//エネミーのHp
        public int power;　　　　　　 //エネミーの攻撃力
        public Sprite enemySprite;　　//エネミーの画像
        public EnemyType enemyType;　 //エネミーのタイプ
        public int exp;
        public float moveDuration;    //拠点までの移動時間
        public MoveType moveType;     //移動方法の種類
    }
}
