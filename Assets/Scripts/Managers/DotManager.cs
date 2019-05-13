using System.Linq;
using Core.DependencyManager;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class DotManager : MonoBehaviour
    {
        private Transform[] Dots => transform.Cast<Transform>().ToArray();

        public Vector3 GetPosition(int i)
        {
            return Dots[i].position;
        }

        private void Awake()
        {
            DI.Add<DotManager>(this);
            
            ValidateDots();
        }

        private void ValidateDots()
        {
            var dots = GetComponentsInChildren<Dot>();

            for (var i = 0; i < dots.Length; i++)
            {
                var dot = dots[i];
                var index = i + 1;
                
                if (index == dots.Length)
                {
                    index = 0;
                }
                
                dot.SetIndex(index);
            }
        }

        private void OnTransformChildrenChanged()
        {
            ShowLines();
        }

        [ContextMenu("Draw")]
        private void ShowLines()
        {
            Debug.DrawLine(Vector3.zero, Vector3.up, Color.red);

            for (var i = 0; i < Dots.Length; i++)
            {
                Vector3 from;
                Vector3 to;
                
                if (i == Dots.Length - 1)
                {
                    from = Dots[Dots.Length - 1].position;
                    to = Dots[0].position;
                }
                else
                {
                    from = Dots[i].position;
                    to = Dots[i + 1].position;
                }
                
                Debug.DrawLine(from, to, Color.red, 10f);
            }
        }
    }
}
