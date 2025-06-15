namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud séquence qui exécute ses enfants dans l'ordre jusqu'à ce qu'un échoue.
/// Retourne Success si tous les enfants réussissent, Failure dès qu'un enfant échoue.
/// </summary>
public class SequenceNode : CompositeNode
{
	private int _currentChild = 0;

	/// <summary>
	/// Exécute les nœuds enfants dans l'ordre jusqu'à ce qu'un échoue.
	/// </summary>
	/// <returns>Success si tous les enfants réussissent, Running si un enfant est en cours, Failure si un enfant échoue.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "SequenceNode";
		
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
				case NodeStatus.Failure:
					Reset();
					context.CurrentDepth--;
					context.LastNodeStatus = NodeStatus.Failure;
					return NodeStatus.Failure;
				case NodeStatus.Success:
				case NodeStatus.Invalid:
				default:
					_currentChild++;
					break;
			}
		}

		Reset();
		context.CurrentDepth--;
		context.LastNodeStatus = NodeStatus.Success;
		return NodeStatus.Success;
	}

	/// <summary>
	/// Réinitialise l'état de la séquence.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		_currentChild = 0;
	}
}
