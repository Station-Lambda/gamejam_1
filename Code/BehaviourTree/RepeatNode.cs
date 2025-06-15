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
	public override NodeStatus Execute()
	{
		// Répétition infinie
		if ( repeatCount == -1 )
		{
			child.Execute();
			return NodeStatus.Running;
		}

		// Répétition avec limite
		while ( _currentCount < repeatCount )
		{
			var status = child.Execute();
			
			if ( status == NodeStatus.Running )
				return NodeStatus.Running;
			
			_currentCount++;
		}

		// Toutes les répétitions sont terminées
		_currentCount = 0;
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