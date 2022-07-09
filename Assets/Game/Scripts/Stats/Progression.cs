using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        public float GetStat(Stat stat, CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.GetCharacterClass() != characterClass) continue;
                foreach(ProgressionStat progressionStat in progressionClass.stats)
                {
                    if (progressionStat.GetStat() != stat) continue;
                    if (progressionStat.HasNoLevelProgression(level)) continue;
                    return progressionStat.GetLevel(level);
                }
            }
            return 0f;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            public ProgressionStat[] stats;
            public CharacterClass GetCharacterClass()
            {
                return characterClass;
            }
            public float GetHealth(int level)
            {
                return 1;//health[level-1];
            }
        }
        [System.Serializable]
        class ProgressionStat
        {   
            [SerializeField] Stat stat;
            [SerializeField] float[] levels;
            public Stat GetStat()
            {
                return stat;
            }
            public float GetLevel(int level)
            {
                return levels[level-1];
            }
            public bool HasNoLevelProgression(int level)
            {
                return levels.Length < level;
            }
        }
    }
}