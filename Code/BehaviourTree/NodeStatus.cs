namespace Sandbox.BehaviourTree;

/// <summary>
/// Représente l'état d'exécution d'un nœud dans l'arbre de comportement.
/// </summary>
public enum NodeStatus
{
	/// <summary>
	/// Le nœud s'est exécuté avec succès.
	/// </summary>
	Success,
	
	/// <summary>
	/// Le nœud a échoué lors de son exécution.
	/// </summary>
	Failure,
	
	/// <summary>
	/// Le nœud est toujours en cours d'exécution.
	/// </summary>
	Running,
	
	/// <summary>
	/// Le nœud est dans un état invalide.
	/// </summary>
	Invalid
}
