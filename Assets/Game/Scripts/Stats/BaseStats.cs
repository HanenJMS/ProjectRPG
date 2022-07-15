
using GameDevTV.Utils;
using System;
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
        
        [SerializeField] GameObject levelUpParticleEffect;
        LazyValue<int> currentLevel;
        [SerializeField] bool shouldUseModifiers = false;
        public event Action OnLevelUp;
        Experience exp;
        private void Awake()
        {
            exp = GetComponent<Experience>();
            currentLevel = new LazyValue<int>(CalculateLevel);
        }
        private void Start()
        {
            currentLevel.ForceInit();

        }
        private void OnEnable()
        {
            if(exp != null)
            {
                exp.OnExperenceGained += UpdateLevel;
            }
        }
        private void OnDisable()
        {
            if (exp != null)
            {
                exp.OnExperenceGained -= UpdateLevel;
            }
        }
        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if(newLevel > currentLevel.value)
            {
                currentLevel.value = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        private void LevelUpEffect()
        {
            Instantiate(levelUpParticleEffect, this.gameObject.transform);
        }

        public int GetLevel()
        {
            return currentLevel.value;
        }
        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + GetPercentageModifier(stat)/100);
        }

        private float GetPercentageModifier(Stat stat)
        {
            float percentageModifier = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float percentageBonus in provider.GetPercentageModifier(stat))
                {
                    percentageModifier += percentageBonus;
                }
            }
            return percentageModifier;
        }

        private float GetBaseStat(Stat stat)
        {
            return progression.GetStat(stat, characterClass, currentLevel.value);
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!shouldUseModifiers) return 0;
            float damage = 0f;
            foreach(IModifierProvider provider in GetComponents<IModifierProvider>())
            {
                foreach (float modifier in provider.GetAdditiveModifer(stat))
                {
                    damage += modifier;
                }
            }
            return damage;
        }

        private int CalculateLevel()
        {
            Experience exp = GetComponent<Experience>();
            float currentExp = exp.GetExperiencePoints();
            if (exp == null) return startLevel;
            int levelProgression = progression.GetLevels(Stat.ExperienceToLevel, characterClass);
            for(int levels = 1; levels < levelProgression; levels++)
            {
                float expToLevel = progression.GetStat(Stat.ExperienceToLevel, characterClass, levels);
                if (currentExp < expToLevel)
                {
                    return levels;
                }
            }
            return levelProgression;
        }
    }
}
