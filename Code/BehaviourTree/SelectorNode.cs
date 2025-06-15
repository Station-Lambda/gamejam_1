namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud sélecteur qui exécute ses enfants dans l'ordre jusqu'à ce qu'un succède.
/// Retourne Success dès qu'un enfant réussit, Failure si tous échouent.
/// </summary>
public class SelectorNode : CompositeNode
{
	private int _currentChild = 0;

	/// <summary>
	/// Exécute les nœuds enfants dans l'ordre jusqu'à ce qu'un succède.
	/// </summary>
	/// <returns>Success si un enfant réussit, Running si un enfant est en cours, Failure si tous échouent.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "SelectorNode";
		
		context.CurrentDepth++;
		
		while ( _currentChild < Children.Count )
		{
			var status = Children[_currentChild].Execute( context );

			switch ( status )
			{
				case NodeStatus.Running:
					context.CurrentDepth--;
					context.LastNodeStatus = NodeStatus.Running;
					return NodeStatus.Running;
				case NodeStatus.Success:
					Reset();
					context.CurrentDepth--;
					context.LastNodeStatus = NodeStatus.Success;
					return NodeStatus.Success;
				case NodeStatus.Failure:
				case NodeStatus.Invalid:
				default:
					_currentChild++;
					break;
			}
		}

		Reset();
		context.CurrentDepth--;
		context.LastNodeStatus = NodeStatus.Failure;
		return NodeStatus.Failure;
	}

	/// <summary>
	/// Réinitialise l'état du sélecteur.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		_currentChild = 0;
	}
}
