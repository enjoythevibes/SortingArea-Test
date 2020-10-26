using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TestGame.Conveyor.Belt
{
    public partial class ConveyorBelt
    {
        [System.Serializable]
        private class ConveyorBeltCargoMover
        {
            [SerializeField] private Transform startPointTransform = default;
            [SerializeField] private Transform endPointTransform = default;
            [SerializeField] private float conveyorMovingSpeed = 1f;
            private WaitForSeconds waitBeforeFreeEndPoint = new WaitForSeconds(1.5f);
            private bool waitBeforeFree = false;
            private List<Entities.CargoEntity> cargosOnBelt;

            public bool StartSlotFilled { private set; get; }
            public bool EndSlotFilled { private set; get; }
            public bool Moving { private set; get; }

            public void Initialize()
            {
                cargosOnBelt = new List<Entities.CargoEntity>();                
            }

            public void AddCargoOnStartSlot(Entities.CargoEntity cargoEntity)
            {
                if (StartSlotFilled) return;
                cargoEntity.CargoTransform.position = startPointTransform.position;
                cargoEntity.CargoTransform.rotation = startPointTransform.rotation;
                cargosOnBelt.Add(cargoEntity);
                StartSlotFilled = true;
            }

            public void RemoveCargoFromEndPoint()
            {
                if (!EndSlotFilled) return;
                var cargo = cargosOnBelt[0];
                cargosOnBelt.RemoveAt(0);
                EndSlotFilled = false;
                cargo.DestroyObject();
            }

            public void ClearBelt()
            {
                for (int i = cargosOnBelt.Count - 1; i >= 0 ; i--)
                {
                    var cargo = cargosOnBelt[i];
                    cargosOnBelt.RemoveAt(i);
                    cargo.DestroyObject();
                }
                EndSlotFilled = false;
                StartSlotFilled = false;
            }

            public Entities.CargoEntity TakeCargoFromEndSlot(ConveyorBelt conveyorBeltBase)
            {
                if (!EndSlotFilled) return null;
                var cargo = cargosOnBelt[0];
                waitBeforeFree = true;
                EndSlotFilled = false;
                conveyorBeltBase.StartCoroutine(WaitAndClearEndSlot());
                return cargo;
            }

            private IEnumerator WaitAndClearEndSlot()
            {
                yield return waitBeforeFreeEndPoint;
                cargosOnBelt.RemoveAt(0);
                waitBeforeFree = false;
                yield break;
            }

            public Entities.CargoEntity PeekCargoFromEndPoint()
            {
                if (!EndSlotFilled) return null;
                var cargo = cargosOnBelt[0];
                return cargo;
            }

            public void Tick(float deltaTime)
            {
                if (!waitBeforeFree)
                    Moving = true;
                else
                    Moving = false;
                var cargosCount = cargosOnBelt.Count;
                if (cargosCount == 0) return;
                for (int i = 0; i < cargosCount; i++)
                {
                    var currentCargoEntity = cargosOnBelt[i];
                    if (i > 0)
                    {
                        var distanceToNext = (currentCargoEntity.CargoTransform.position - cargosOnBelt[i - 1].CargoTransform.position).sqrMagnitude;
                        var cargoWidth = currentCargoEntity.Width;
                        if (distanceToNext < cargoWidth * cargoWidth)
                        {
                            if (i == cargosCount - 1)
                                Moving = false;
                            continue;
                        }
                    }
                    if (waitBeforeFree) continue;                    
                    currentCargoEntity.CargoTransform.position = Vector3.MoveTowards(currentCargoEntity.CargoTransform.position, endPointTransform.position, deltaTime * conveyorMovingSpeed);
                }
                var lastCargoWidth = cargosOnBelt[cargosCount - 1].Width;
                if ((cargosOnBelt[cargosCount - 1].CargoTransform.position - startPointTransform.position).sqrMagnitude > lastCargoWidth * lastCargoWidth)
                {
                    StartSlotFilled = false;
                }
                if ((cargosOnBelt[0].CargoTransform.position - endPointTransform.position).sqrMagnitude < 0.001f)
                {
                    if (!waitBeforeFree)
                        EndSlotFilled = true;
                }
            }
        }
    }
}