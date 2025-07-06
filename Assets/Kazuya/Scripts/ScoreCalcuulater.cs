using UnityEngine;
using System.Collections.Generic;

public class ScoreCalculator : MonoBehaviour
{
    public int totalScore = 0;
    public int rareBonus = 0;
    public int totalEyes = 0;

    public int odokashiCount = 0;
    public int bibiriCount = 0;
    public int rareEnemyCount = 0;

    public System.Action<ScoreCalculator> OnScoreUpdated;

    HashSet<string> odokashiTags = new() { "nEye", "tEye", "tred", "tblue" };

    // �e�I�o�P���Ƃ̃^�O�����i�I�o�P1�̂ɂ������ڂ̃^�O��ێ��j
    private Dictionary<GameObject, HashSet<string>> parentTagMap = new();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == null || other.transform.root == null) return;

        string tag = other.tag;
        GameObject parentObj = other.transform.root.gameObject;

        int score = 0;
        int rare = 0;

        switch (tag)
        {
            case "nEye": score += 1; break;
            case "tEye": score += 2; break;
            case "nred": score += 1; rare += 50; break;
            case "tred": score += 2; rare += 70; break;
            case "nblue": score += 1; rare += 100; break;
            case "tblue": score += 2; rare += 120; break;
        }

        if (score == 0) return;

        totalScore += score + rare;
        rareBonus += rare;
        totalEyes++;

        if (tag.Contains("red") || tag.Contains("blue"))
            rareEnemyCount++;

        // �ڂ̃^�O���I�o�P���ƂɋL�^
        if (!parentTagMap.ContainsKey(parentObj))
            parentTagMap[parentObj] = new HashSet<string>();

        parentTagMap[parentObj].Add(tag);

        // ���ނ�����
        RecountTypes();

        // UI�֒ʒm
        OnScoreUpdated?.Invoke(this);
    }

    void RecountTypes()
    {
        odokashiCount = 0;
        bibiriCount = 0;

        foreach (var kv in parentTagMap)
        {
            bool isOdokashi = false;

            foreach (string tag in kv.Value)
            {
                if (odokashiTags.Contains(tag))
                {
                    isOdokashi = true;
                    break;
                }
            }

            if (isOdokashi) odokashiCount++;
            else bibiriCount++;
        }
    }

    public void ResetScore()
    {
        totalScore = 0;
        rareBonus = 0;
        totalEyes = 0;
        rareEnemyCount = 0;
        odokashiCount = 0;
        bibiriCount = 0;

        parentTagMap.Clear();
    }
}
