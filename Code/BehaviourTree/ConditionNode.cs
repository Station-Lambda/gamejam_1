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
	public override NodeStatus Execute()
	{
		return condition() ? NodeStatus.Success : NodeStatus.Failure;
	}
}
