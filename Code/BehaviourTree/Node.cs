namespace Sandbox.BehaviourTree;

/// <summary>
/// Classe de base abstraite pour tous les nœuds de l'arbre de comportement.
/// </summary>
public abstract class Node
{
	/// <summary>
	/// Le nom du nœud pour le debug.
	/// </summary>
	public virtual string Name => GetType().Name;
	
	/// <summary>
	/// Exécute la logique du nœud et retourne son statut.
	/// </summary>
	/// <param name="context">Le contexte d'exécution de l'arbre.</param>
	/// <returns>Le statut résultant de l'exécution du nœud.</returns>
	public abstract NodeStatus Execute( BehaviourTreeContext context );

	/// <summary>
	/// Réinitialise l'état interne du nœud.
	/// </summary>
	public virtual void Reset() { }
	
	/// <summary>
	/// Met à jour le chemin dans le contexte.
	/// </summary>
	protected void UpdateContext( BehaviourTreeContext context, string additionalInfo = "" )
	{
		context.LastExecutedNode = this;
		var info = string.IsNullOrEmpty( additionalInfo ) ? Name : $"{Name}({additionalInfo})";
		
		if ( context.CurrentDepth == 0 )
		{
			context.CurrentPath = info;
		}
		else
		{
			var separator = context.CurrentPath.EndsWith( "/" ) ? "" : "/";
			context.CurrentPath += $"{separator}{info}";
		}
	}
}
