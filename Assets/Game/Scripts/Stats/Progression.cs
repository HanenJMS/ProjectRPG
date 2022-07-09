using UnityEngine;

namespace RPG.Stats
{
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
    public class Progression : ScriptableObject
    {
        [SerializeField] ProgressionCharacterClass[] characterClasses = null;
        public float GetHealth(CharacterClass characterClass, int level)
        {
            foreach(ProgressionCharacterClass progressionClass in characterClasses)
            {
                if (progressionClass.GetCharacterClass() == characterClass)
                {
                    return progressionClass.GetHealth(level);
                }
            }
            return 0f;
        }

        [System.Serializable]
        class ProgressionCharacterClass
        {
            [SerializeField] CharacterClass characterClass;
            [SerializeField] float[] health;
            public CharacterClass GetCharacterClass()
            {
                return characterClass;
            }
            public float GetHealth(int level)
            {
                return health[level-1];
            }
        }
    }
}