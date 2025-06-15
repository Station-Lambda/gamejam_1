namespace Sandbox.BehaviourTree;

/// <summary>
/// Politique de succès pour un nœud parallèle.
/// </summary>
public enum ParallelPolicy
{
	/// <summary>
	/// Succès si au moins un enfant réussit.
	/// </summary>
	RequireOne,
	
	/// <summary>
	/// Succès si tous les enfants réussissent.
	/// </summary>
	RequireAll
}

/// <summary>
/// Nœud qui exécute tous ses enfants en parallèle.
/// Le résultat dépend de la politique choisie.
/// </summary>
/// <param name="successPolicy">La politique pour déterminer le succès du nœud.</param>
/// <param name="failurePolicy">La politique pour déterminer l'échec du nœud.</param>
public class ParallelNode( ParallelPolicy successPolicy = ParallelPolicy.RequireAll, ParallelPolicy failurePolicy = ParallelPolicy.RequireOne ) : CompositeNode
{
	/// <summary>
	/// Exécute tous les nœuds enfants en parallèle.
	/// </summary>
	/// <returns>Success/Failure selon les politiques, Running si des enfants sont encore en cours.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "ParallelNode";
		
		context.CurrentDepth++;
		
		var successCount = 0;
		var failureCount = 0;
		var runningCount = 0;

		foreach ( var child in Children )
		{
			var status = child.Execute( context );
			
			switch ( status )
			{
				case NodeStatus.Success:
					successCount++;
					break;
				case NodeStatus.Failure:
					failureCount++;
					break;
				case NodeStatus.Running:
					runningCount++;
					break;
			}
		}

		context.CurrentDepth--;
		NodeStatus result;

		// Vérifier les conditions d'échec en premier
		if ( failurePolicy == ParallelPolicy.RequireOne && failureCount > 0 )
			result = NodeStatus.Failure;
		else if ( failurePolicy == ParallelPolicy.RequireAll && failureCount == Children.Count )
			result = NodeStatus.Failure;
		// Vérifier les conditions de succès
		else if ( successPolicy == ParallelPolicy.RequireOne && successCount > 0 )
			result = NodeStatus.Success;
		else if ( successPolicy == ParallelPolicy.RequireAll && successCount == Children.Count )
			result = NodeStatus.Success;
		// Si des enfants sont encore en cours
		else if ( runningCount > 0 )
			result = NodeStatus.Running;
		// Par défaut, échec si aucune condition n'est remplie
		else
			result = NodeStatus.Failure;

		context.LastNodeStatus = result;
		return result;
	}
}