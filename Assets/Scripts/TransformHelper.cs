using UnityEngine;

public class TransformHelper
{
    private static  Transform temporaryObjectContainerTran;

    /// <summary>
    /// temporaryObjectContainerTran変数のプロパティ
    /// </summary>
    public static Transform TemporaryObjectContainerTran
    {
        set
        {
            temporaryObjectContainerTran = value;
        }
        get
        {
            return temporaryObjectContainerTran;
        }
    }

    /// <summary>
    /// temporaryObjectContainerTranに情報をセット
    /// </summary>
    /// <param name="newTran"></param>
    public static void SetTemporaryObjectContainerTran(Transform newTran)
    {
        temporaryObjectContainerTran = newTran;

        Debug.Log("temporaryObjectContainerTran変数に位置情報をセット完了");
    }

    public static Transform GetTemporaryObjectContainerTran()
    {
        return temporaryObjectContainerTran;
    }
}
