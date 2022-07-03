using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class GameManager
{
    public const int MinLevel = 1;
    public const int MaxLevel = 10;

    private readonly static Dictionary<int, TimeSpan> timeLimitPerLevel;

    static GameManager()
    {
        // レベルごとの制限時間
        timeLimitPerLevel = new Dictionary<int, TimeSpan>()
        {
            { MinLevel, TimeSpan.FromSeconds(3) },
            { 2, TimeSpan.FromSeconds(2) },
            { 3, TimeSpan.FromSeconds(1) },
            { 4, TimeSpan.FromSeconds(0.7) },
            { 5, TimeSpan.FromSeconds(0.6) },
            { 6, TimeSpan.FromSeconds(0.5) },
            { 7, TimeSpan.FromSeconds(0.4) },
            { 8, TimeSpan.FromSeconds(0.3) },
            { 9, TimeSpan.FromSeconds(0.2) },
            { MaxLevel, TimeSpan.FromSeconds(0.15) },
        };

        Instance = new GameManager();
    }

    private GameManager() => Reset();

    public static GameManager Instance { get; }

    /// <summary>
    /// 現在のレベルを取得する。
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// クリアレベルを取得する。
    /// </summary>
    public int? ClearLevel { get; private set; }

    /// <summary>
    /// ベストタイムを取得する。
    /// </summary>
    public TimeSpan? BestTime { get; private set; }

    public void Reset()
    {
        Level = 1;
        ClearLevel = null;
        BestTime = null;
    }

    /// <summary>
    /// ゲームのレベルを上げる。
    /// </summary>
    /// <returns>レベルアップ前のレベル。最大レベルだった場合は <see cref="MaxLevel"/> を返す。</returns>
    public int LevelUp()
    {
        int currentLevel = Level;
        int nextLevel = currentLevel + 1;
        if (nextLevel > MaxLevel)
        {
            // 最大レベルに到達していたらレベルアップ不可
            return MaxLevel;
        }

        // レベルアップ
        ClearLevel = currentLevel;
        Level = nextLevel;
        return currentLevel;
    }

    /// <summary>
    /// 指定したレベルの制限時間を返す。
    /// </summary>
    /// <param name="level"></param>
    /// <returns>制限時間。</returns>
    public TimeSpan GetTimeLimit(int level)
    {
        if (!(level is >= MinLevel and <= MaxLevel))
        {
            Util.ThrowArgumentOutOfRangeException(nameof(level), $"レベルは{MinLevel}～{MaxLevel}の範囲で指定してください。");
        }

#if DEVELOPMENT_BUILD
        var oneSecond = TimeSpan.FromSeconds(1);
        return timeLimitPerLevel[level] + oneSecond;
#else
        return timeLimitPerLevel[level];
#endif

    }

    /// <summary>
    /// ベストタイムを更新する。
    /// </summary>
    /// <param name="value"></param>
    /// <returns>ベストタイムを更新できたかどうか。</returns>
    public bool UpdateBestTime(TimeSpan value)
    {
        if (BestTime is TimeSpan bestTime)
        {
            if (value < bestTime)
            {
                // タイムが現在のベストタイムより遅い場合は更新しない
                return false;
            }
        }

        // ベストタイム更新
        BestTime = value;
        return true;
    }
}
