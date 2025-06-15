namespace Sandbox.BehaviourTree;

/// <summary>
/// Contexte d'exécution pour l'arbre de comportement.
/// Permet de suivre l'état actuel et de partager des données entre les nœuds.
/// </summary>
public class BehaviourTreeContext
{
	/// <summary>
	/// Le dernier nœud qui a été exécuté.
	/// </summary>
	public Node LastExecutedNode { get; set; }
	
	/// <summary>
	/// Le chemin actuel dans l'arbre (pour le debug).
	/// </summary>
	public string CurrentPath { get; set; } = "";
	
	/// <summary>
	/// Le statut du dernier nœud exécuté.
	/// </summary>
	public NodeStatus LastNodeStatus { get; set; }
	
	/// <summary>
	/// Le blackboard pour partager des données entre les nœuds.
	/// </summary>
	public Blackboard Blackboard { get; } = new();
	
	/// <summary>
	/// Profondeur actuelle dans l'arbre (pour le debug).
	/// </summary>
	public int CurrentDepth { get; set; } = 0;
}