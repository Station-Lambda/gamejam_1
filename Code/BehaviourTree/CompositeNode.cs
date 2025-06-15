namespace Sandbox.BehaviourTree;

/// <summary>
/// Classe de base abstraite pour les nœuds composites qui peuvent avoir plusieurs enfants.
/// </summary>
public abstract class CompositeNode : Node
{
	/// <summary>
	/// Liste des nœuds enfants de ce nœud composite.
	/// </summary>
	protected readonly List<Node> Children = [];

	/// <summary>
	/// Ajoute un nœud enfant à ce nœud composite.
	/// </summary>
	/// <param name="child">Le nœud enfant à ajouter.</param>
	public void AddChild( Node child )
	{
		Children.Add( child );
	}

	/// <summary>
	/// Réinitialise ce nœud et tous ses enfants.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		foreach ( var child in Children )
		{
			child.Reset();
		}
	}
}
