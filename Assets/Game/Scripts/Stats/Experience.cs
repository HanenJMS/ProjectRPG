using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        [SerializeField] float experiencePoints = 0;
        //public delegate void ExperenceGainedDelegate();
        public event Action OnExperenceGained;
        public void GainExperience(float experience)
        {
            experiencePoints += experience;
            OnExperenceGained();
        }
        public float GetExperiencePoints()
        {
            return experiencePoints;
        }
        public object CaptureState()
        {
            return experiencePoints;
        }
        public void RestoreState(object state)
        {
            experiencePoints = (float)state;
        }
    }
}
