using System;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud qui évalue une condition et retourne Success ou Failure.
/// </summary>
public class ConditionNode : Node
{
	private readonly Func<bool> _condition;
	private readonly string _conditionName;
	
	public ConditionNode( Func<bool> condition, string conditionName = null )
	{
		_condition = condition;
		_conditionName = conditionName ?? condition.Method?.Name ?? "Condition";
	}
	
	public override string Name => _conditionName;
	
	/// <summary>
	/// Exécute la condition et retourne Success si vrai, Failure sinon.
	/// </summary>
	/// <returns>Success si la condition est vraie, Failure sinon.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		UpdateContext( context );
		
		var result = _condition();
		var status = result ? NodeStatus.Success : NodeStatus.Failure;
		context.LastNodeStatus = status;
		
		return status;
	}
}
