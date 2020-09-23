using UnityEngine;

namespace Hospital.HospitalNurse
{
    public interface ILightTriggerable
    {
        void OnTriggered(GameObject triggerSource);
    }
}