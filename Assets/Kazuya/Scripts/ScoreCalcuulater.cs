using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    public int totalScore = 0;
    public int rareBonus = 0;

    public System.Action<int, int> OnScoreCalculated; // DisplayScoresへ通知する用

    private void OnTriggerEnter2D(Collider2D other)
    {
        int score = 0;
        int rare = 0;

        switch (other.tag)
        {
            case "nEye": score += 1; break;
            case "tEye": score += 2; break;
            case "nred": score += 1; rare += 50; break;
            case "tred": score += 2; rare += 70; break;
            case "nblue": score += 1; rare += 100; break;
            case "tblue": score += 2; rare += 120; break;
        }

        totalScore += score + rare;
        rareBonus += rare;

        Debug.Log($"Hit: {other.tag} → Score +{score} / Rare +{rare}");

        // スコアが更新されたらイベントで通知
        OnScoreCalculated?.Invoke(totalScore, rareBonus);
    }

    public void ResetScore()
    {
        totalScore = 0;
        rareBonus = 0;
    }
}
