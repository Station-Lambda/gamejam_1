using System;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud qui exécute une action spécifique et retourne un statut.
/// </summary>
public class ActionNode : Node
{
	private readonly Func<NodeStatus> _action;
	private readonly string _actionName;
	
	public ActionNode( Func<NodeStatus> action, string actionName = null )
	{
		_action = action;
		_actionName = actionName ?? "Action";
	}
	
	public override string Name => _actionName;
	
	/// <summary>
	/// Exécute l'action associée au nœud.
	/// </summary>
	/// <returns>Le statut retourné par l'action.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		UpdateContext( context );
		
		var status = _action();
		context.LastNodeStatus = status;
		
		return status;
	}
}
