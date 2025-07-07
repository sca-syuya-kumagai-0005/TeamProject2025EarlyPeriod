using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ScoreCalculator : MonoBehaviour
{
    public int totalScore = 0;
    public int rareBonus = 0;
    public int totalEyes = 0;

    public int odokashiCount = 0;
    public int bibiriCount = 0;
    public int rareEnemyCount = 0;
    public int BonusPoint = 0;

    public System.Action<ScoreCalculator> OnScoreUpdated;

    HashSet<GameObject> EyeParents = new();

    // 各オバケごとのタグ履歴（オバケ1体につき複数目のタグを保持）
    private Dictionary<GameObject, HashSet<string>> parentTagMap = new();
    public void CalculateScoreLikeMouse(Collider2D scoringArea)
    {
        // 初期化
        ResetScore();

        // 目のコライダー全体を取得
        Collider2D[] colliders = GameObject.FindObjectsOfType<Collider2D>();

        Dictionary<GameObject, (int nCount, int tCount)> EyeCount = new();
        HashSet<GameObject> EyeParents = new();

        int nEye = 0, tEye = 0, nRed = 0, tRed = 0, nBlue = 0, tBlue = 0;

        foreach (var col in colliders)
        {
            string[] validTags = { "nEye", "tEye", "nred", "tred", "nblue", "tblue" };
            if (!validTags.Contains(col.tag)) continue;

            if (!IsFullyInside(scoringArea.bounds, col.bounds)) continue;

            GameObject parentObj = col.transform.parent != null ? col.transform.parent.gameObject : null;
            if (parentObj != null)
            {
                EyeParents.Add(parentObj);
                if (!EyeCount.ContainsKey(parentObj)) EyeCount[parentObj] = (0, 0);
            }

            switch (col.tag)
            {
                case "nEye":
                    nEye++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1 + 1, EyeCount[parentObj].Item2);
                    break;
                case "tEye":
                    tEye++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1, EyeCount[parentObj].Item2 + 1);
                    break;
                case "nred":
                    nRed++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1 + 1, EyeCount[parentObj].Item2);
                    break;
                case "nblue":
                    nBlue++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1 + 1, EyeCount[parentObj].Item2);
                    break;
                case "tred":
                    tRed++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1, EyeCount[parentObj].Item2 + 1);
                    break;
                case "tblue":
                    tBlue++;
                    if (parentObj != null)
                        EyeCount[parentObj] = (EyeCount[parentObj].Item1, EyeCount[parentObj].Item2 + 1);
                    break;
            }
        }

        // 合計目数
        int normal = nEye + nRed + nBlue;
        int threaten = tEye + tRed + tBlue;
        totalEyes = normal + threaten;

        // 通常スコア
        totalScore += (normal / 2) * 2 + (normal % 2);
        totalScore += (threaten / 2) * 5 + (threaten % 2 == 1 ? 2 : 0);

        // ボーナス
        BonusPoint = GetBonusPoint(totalEyes);
        totalScore += BonusPoint;

        int nRarebonus = 0;
        int tRarebonus = 0;

        nRarebonus += GetRareBonus(nRed, 50);
        nRarebonus += GetRareBonus(nBlue, 100);
        tRarebonus += GetRareBonus(tRed, 70);
        tRarebonus += GetRareBonus(tBlue, 120);
        rareBonus = nRarebonus + tRarebonus;
        totalScore += rareBonus;

        // 種類分類（脅かし or ビビリ）
        foreach (var parent in EyeParents)
        {
            if (!parentTagMap.ContainsKey(parent)) parentTagMap[parent] = new();

            var (nCount, tCount) = EyeCount[parent];
            bool isOdokashi = tCount > 0;

            if (isOdokashi) odokashiCount++;
            else bibiriCount++;

            // 全てがレア目かどうか
            if (nCount + tCount > 0 &&
                parentTagMap[parent].All(t => t.Contains("red") || t.Contains("blue")))
            {
                rareEnemyCount++;
            }
        }

        OnScoreUpdated?.Invoke(this); // UIへ通知
    }


    bool IsFullyInside(Bounds outer, Bounds inner)
    {
        return outer.Contains(inner.min) && outer.Contains(inner.max);
    }

    int GetBonusPoint(int eyes)
    {
        return eyes switch
        {
            3 => 5,
            4 => 10,
            5 => 20,
            6 => 50,
            7 => 100,
            8 => 250,
            9 => 300,
            10 => 500,
            _ => 0,
        };
    }

    int GetRareBonus(int count, int baseScore)
    {
        if (count == 0) return 0;
        if (count <= 2) return baseScore;
        if (count <= 4) return baseScore * 2;
        if (count <= 6) return baseScore * 3;
        if (count <= 8) return baseScore * 4;
        if (count <= 10) return baseScore * 5;
        return baseScore * 6;
    }


    public void ResetScore()
    {
        totalScore = 0;
        rareBonus = 0;
        totalEyes = 0;
        odokashiCount = 0;
        bibiriCount = 0;
        rareEnemyCount = 0;
        parentTagMap.Clear();
    }
}
