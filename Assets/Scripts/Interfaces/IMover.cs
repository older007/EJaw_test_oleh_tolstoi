using UnityEngine;

namespace Interfaces
{
    public interface IMover
    {
        bool CanMove { get; }
        float Timer { get; set; }
        Vector3 StartPost { get; set; }
        Vector3 EndPos { get; set; }
    }
}