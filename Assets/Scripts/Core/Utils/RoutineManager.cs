using System;
using System.Collections;
using System.Collections.Generic;
using Core.DependencyManager;
using UnityEngine;

namespace Core.Utils
{
    [ExecuteInEditMode]
    public class RoutineManager : MonoBehaviour
    {
        private readonly Dictionary<Guid, IEnumerator> routines = new Dictionary<Guid, IEnumerator>();
            
        [ContextMenu("Init")]
        private void Awake()
        {
            DI.Add<RoutineManager>(this);
        }

        public bool IsWorking(Guid routineId)
        {
            return routines.ContainsKey(routineId);
        }

        public Coroutine RoutineStart(IEnumerator iEnumerator)
        {
            var routineId = Guid.NewGuid();
            
            routines.Add(routineId, iEnumerator);
            
            var routine = StartCoroutine(RoutineController(routineId));

            return routine;
        }

        public Guid RoutineStart(IEnumerator iEnumerator, Action onComplete)
        {
            var routineId = Guid.NewGuid();
            
            routines.Add(routineId, iEnumerator);
            
            StartCoroutine(RoutineController(routineId, onComplete));

            return routineId;
        }

        public void RoutineStop(Guid routine)
        {
            if (!routines.ContainsKey(routine))
            {
                return;
            }

            StopCoroutine(routines[routine]);
        }

        private IEnumerator RoutineController(Guid routineId, Action onComplete = null)
        {
            yield return StartCoroutine(routines[routineId]);

            onComplete.InvokeSafe();
            
            routines.Remove(routineId);
        }
    }
}