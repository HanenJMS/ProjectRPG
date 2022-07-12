using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1,99)]
        [SerializeField] int startLevel = 1;
        [SerializeField] CharacterClass characterClass;
        [SerializeField] Progression progression = null;
        int currentLevel = 0;
        private void Start()
        {
            currentLevel = GetLevel();
            Experience exp = GetComponent<Experience>();
            exp.OnExperenceGained += UpdateLevel;

        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel)
            {
                currentLevel = newLevel;
            }
        }
        public int GetLevel()
        {
            if(currentLevel < 1)
            {
                currentLevel = CalculateLevel();
            }
            return currentLevel;
        }
        public float GetStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, startLevel);
        }
        public int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();
            float currentExp = exp.GetExperiencePoints();
            if (exp == null) return startLevel;
            int levelProgression = progression.GetLevels(Stat.ExperienceToLevel, characterClass);
            for(int levels = 1; levels <= levelProgression; levels++)
            {
                float expToLevel = progression.GetStat(Stat.ExperienceToLevel, characterClass, levels);
                if (currentExp < expToLevel)
                {
                    return levels;
                }
            }
            return levelProgression + 1;
        }
    }
}
