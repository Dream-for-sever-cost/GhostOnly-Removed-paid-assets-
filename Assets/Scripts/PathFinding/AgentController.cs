using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    private StatController Stat;

    void Awake()
    {
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        Stat = GetComponent<StatController>();
    }

    public void Initialize(float range, float speed)
    {
        agent.stoppingDistance = range - 0.05f;
        agent.speed = speed;
    }

    public void Move(Transform target)
    {
        if (target == null) return;

        agent.isStopped = false;
        agent.speed = Stat.Stats[StatType.MoveSpeed].Value;
        agent.SetDestination(target.position);
    }

    public bool GetDirection() => agent.velocity.x > 0;

    public void Stop()
    {
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.ResetPath();
    }

    public void ToggleEnable(bool onoff)
    {
        agent.enabled = onoff;
    }
}