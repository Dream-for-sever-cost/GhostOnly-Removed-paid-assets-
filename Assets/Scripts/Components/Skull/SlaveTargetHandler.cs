using UnityEngine;

namespace Component.Slave
{
    public class SlaveTargetHandler : MonoBehaviour
    {
        //todo migrate to weapon
        [field: SerializeField] public LayerMask Target { get;  set; }
    }
}