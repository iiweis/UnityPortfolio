using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ゲームの結果を表す。
/// </summary>
public enum ResultState
{
    /// <summary>
    /// 結果無し。ゲームが始まっていない。
    /// </summary>
    None,

    /// <summary>
    /// ゲーム成功。
    /// </summary>
    Success,

    /// <summary>
    /// ゲーム失敗。
    /// </summary>
    Failure,
}
