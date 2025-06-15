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
		var previousPath = context.CurrentPath;
		UpdateContext( context );
		
		context.CurrentDepth++;
		
		while ( _currentChild < Children.Count )
		{
			var status = Children[_currentChild].Execute( context );
			context.LastNodeStatus = status;

			switch ( status )
			{
				case NodeStatus.Running:
					context.CurrentDepth--;
					return NodeStatus.Running;
				case NodeStatus.Success:
					Reset();
					context.CurrentDepth--;
					context.CurrentPath = previousPath;
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
		context.CurrentPath = previousPath;
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
