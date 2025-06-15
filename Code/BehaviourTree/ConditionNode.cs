using System;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud qui évalue une condition et retourne Success ou Failure.
/// </summary>
/// <param name="condition">La fonction de condition à évaluer.</param>
public class ConditionNode( Func<bool> condition ) : Node
{
	/// <summary>
	/// Exécute la condition et retourne Success si vrai, Failure sinon.
	/// </summary>
	/// <returns>Success si la condition est vraie, Failure sinon.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "ConditionNode";
		
		var result = condition();
		var status = result ? NodeStatus.Success : NodeStatus.Failure;
		context.LastNodeStatus = status;
		
		return status;
	}
}
