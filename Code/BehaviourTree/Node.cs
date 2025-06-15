namespace Sandbox.BehaviourTree;

/// <summary>
/// Classe de base abstraite pour tous les nœuds de l'arbre de comportement.
/// </summary>
public abstract class Node
{
	/// <summary>
	/// Exécute la logique du nœud et retourne son statut.
	/// </summary>
	/// <returns>Le statut résultant de l'exécution du nœud.</returns>
	public abstract NodeStatus Execute();

	/// <summary>
	/// Réinitialise l'état interne du nœud.
	/// </summary>
	public virtual void Reset() { }
}
