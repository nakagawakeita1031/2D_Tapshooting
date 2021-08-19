using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "ItemDataSO", menuName = "ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public List<ItemData> itemDataList = new List<ItemData>();

    [Serializable]
    public class ItemData
    {
        public int no;
        public Sprite ItemSprite;
        public String ItemName;
    }
}
