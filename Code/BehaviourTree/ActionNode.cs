using System;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud qui exécute une action spécifique et retourne un statut.
/// </summary>
/// <param name="action">La fonction d'action à exécuter qui retourne un NodeStatus.</param>
public class ActionNode( Func<NodeStatus> action ) : Node
{
	/// <summary>
	/// Exécute l'action associée au nœud.
	/// </summary>
	/// <returns>Le statut retourné par l'action.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "ActionNode";
		
		var status = action();
		context.LastNodeStatus = status;
		
		Log.Info( $"{new string( ' ', context.CurrentDepth * 2 )}ActionNode: {status}" );
		return status;
	}
}
