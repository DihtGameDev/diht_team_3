using System;
using UnityEngine;

namespace Hospital.HospitalNurse
{
    internal abstract class NurseState
    {

        public virtual void Start(HospitalNurseController nurseController) {}
        
        public abstract NurseState Tick(HospitalNurseController nurseController);
        
        public virtual void End(HospitalNurseController nurseController) {}
    }

    internal class IdleState : NurseState
    {
        private static NurseState instance;
        
        private IdleState() {}

        public override void Start(HospitalNurseController nurseController)
        {
            base.Start(nurseController);
            nurseController.quest();
            nurseController.speed = nurseController.walkSpeed;
            nurseController.visualDetecter.distanceOfViewing = nurseController.visualDetecter.calmDistanceOfViewing;
        }

        public override NurseState Tick(HospitalNurseController nurseController)
        {
            if (nurseController.isMarshallVisible)
            {
                if (nurseController.marshallController.isBeingChased)
                {
                    nurseController.Annoyment = HospitalNurseController.RUSHING_TRESHHOLD;
                }
                if (nurseController.Annoyment < HospitalNurseController.RUSHING_TRESHHOLD)
                {
                    nurseController.Annoyment += (1.1f - Vector2.Distance(
                        nurseController.transform.position,
                        nurseController.marshall.transform.position
                    ) / nurseController.visualDetecter.distanceOfViewing) * 4f;
                }
                else
                {
                    return RushingState.GetInstance();
                }
            }
            else
            {
                var waitingTime = nurseController.waitingOnDefaultPoint;
                var destination = nurseController.defaultTrajectory[
                    nurseController.pointerOfDefMove % nurseController.defaultTrajectory.Count
                ];
                
                if (nurseController.Annoyment >= HospitalNurseController.WANDERING_TRESHHOLD && nurseController.lastDistractor != nurseController.marshall)
                {
                    waitingTime = nurseController.waitingOnUndefaultPoint;
                    destination = nurseController.lastDistractor.transform.position;
                }
                else
                {
                    nurseController.Annoyment -= 0.3f;
                }

                if (nurseController.target != destination)
                {
                    nurseController.currentWaitingOnPoint = 0;
                    nurseController.target = destination;
                }
                
                if (nurseController.currentWaitingOnPoint > waitingTime)
                {
                    nurseController.Annoyment -= 0.5f;
                    if (nurseController.Annoyment < 0.01f && nurseController.isCameToTarget)
                    {
                        nurseController.pointerOfDefMove++;
                    }
                }
                else if (nurseController.isCameToTarget)
                {
                    nurseController.currentWaitingOnPoint += Time.deltaTime;
                }
            }
            return instance;
        }

        public static NurseState GetInstance()
        {
            return instance ?? (instance = new IdleState());
        }
    }

    internal class RushingState : NurseState
    {
        
        private static NurseState instance;
        
        private RushingState() {}
        
        public override void Start(HospitalNurseController nurseController)
        {
            base.Start(nurseController);
            nurseController.StartCoroutine(nurseController.glitch(0.2f));
            nurseController.exclam();
            nurseController.marshallController.number_of_rushers++;
            nurseController.speed = nurseController.runSpeed;
            nurseController.visualDetecter.distanceOfViewing = nurseController.visualDetecter.angryDistanceOfViewing;
        }

        public override NurseState Tick(HospitalNurseController nurseController)
        {
            nurseController.target = nurseController.lastDistractor.transform.position;
            return !nurseController.isMarshallVisible ? RunawayState.GetInstance() : instance;
        }

        public override void End(HospitalNurseController nurseController)
        {
            base.End(nurseController);
            nurseController.marshallController.number_of_rushers--;
        }

        public static NurseState GetInstance()
        {
            return instance ?? (instance = new RushingState());
        }
    }
    
    internal class RunawayState : NurseState
    {
        private static NurseState instance;

        private static readonly float RUNAWAY_STATE_DURATION = 2f;
        private float currentRunawayStateDuration = 0f;
        
        private RunawayState() {}

        public override void Start(HospitalNurseController nurseController)
        {
            base.Start(nurseController);
            currentRunawayStateDuration = 0f;
        }

        public override NurseState Tick(HospitalNurseController nurseController)
        {
            if (nurseController.isMarshallVisible)
            {
                return RushingState.GetInstance();
            }
            
            nurseController.target = nurseController.lastDistractor.transform.position;

            if (currentRunawayStateDuration < RUNAWAY_STATE_DURATION)
            {
                currentRunawayStateDuration += Time.deltaTime;
            }
            else
            {
                return IdleState.GetInstance();
            }
            
            return instance;
        }

        public static NurseState GetInstance()
        {
            return instance ?? (instance = new RunawayState());
        }
    }
}