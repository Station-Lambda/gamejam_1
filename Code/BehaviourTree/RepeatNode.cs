namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud décorateur qui répète l'exécution de son enfant un certain nombre de fois ou indéfiniment.
/// </summary>
/// <param name="child">Le nœud enfant à répéter.</param>
/// <param name="repeatCount">Le nombre de répétitions (-1 pour infini).</param>
public class RepeatNode( Node child, int repeatCount = -1 ) : Node
{
	private int _currentCount = 0;

	/// <summary>
	/// Exécute le nœud enfant selon le nombre de répétitions spécifié.
	/// </summary>
	/// <returns>Running tant que les répétitions ne sont pas terminées, Success quand toutes les répétitions sont complétées.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "RepeatNode";
		
		Log.Info( $"{new string( ' ', context.CurrentDepth * 2 )}RepeatNode: Repeat {_currentCount}/{(repeatCount == -1 ? "∞" : repeatCount.ToString())}" );
		context.CurrentDepth++;

		// Répétition infinie
		if ( repeatCount == -1 )
		{
			child.Execute( context );
			context.CurrentDepth--;
			context.LastNodeStatus = NodeStatus.Running;
			return NodeStatus.Running;
		}

		// Répétition avec limite
		while ( _currentCount < repeatCount )
		{
			var status = child.Execute( context );
			
			if ( status == NodeStatus.Running )
			{
				context.CurrentDepth--;
				context.LastNodeStatus = NodeStatus.Running;
				return NodeStatus.Running;
			}
			
			_currentCount++;
		}

		// Toutes les répétitions sont terminées
		Log.Info( $"{new string( ' ', context.CurrentDepth * 2 )}RepeatNode: Completed all {repeatCount} repetitions" );
		_currentCount = 0;
		context.CurrentDepth--;
		context.LastNodeStatus = NodeStatus.Success;
		return NodeStatus.Success;
	}

	/// <summary>
	/// Réinitialise le compteur de répétitions et le nœud enfant.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		_currentCount = 0;
		child.Reset();
	}
}