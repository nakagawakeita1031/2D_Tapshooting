﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    [SerializeField, Header("獲得した合計 Exp")]
    private int totalExp;

    [SerializeField, Header("拠点の耐久力")]
    private int durabilityBase;

    [SerializeField, Header("エネミーの最大生成数")]
    private int maxGenerateCountBase;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// TotalExpの更新
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateToatalExp(int exp)
    {
        totalExp += exp;
    }
    /// <summary>
    /// TOtalExpの取得
    /// </summary>
    /// <returns></returns>
    public int GetTotalExp()
    {
        return totalExp;
    }

    /// <summary>
    /// 拠点耐久力の値を取得
    /// </summary>
    /// <returns></returns>
    public int GetDurability()
    {
        return durabilityBase;
    }

    /// <summary>
    /// エネミーの最大生成数の値を取得
    /// </summary>
    /// <returns></returns>
    public int GetMaxGenerateCount()
    {
        return maxGenerateCountBase;
    }
}
