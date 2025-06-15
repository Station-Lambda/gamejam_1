using Sandbox.BehaviourTree;

namespace Sandbox;

public class AiCharacter : Component
{
	private Node _behaviourTree;
	private BehaviourTreeContext _context;
	
	/// <summary>
	/// Affiche le debug du behaviour tree au-dessus du personnage.
	/// </summary>
	[Property] public bool ShowDebug { get; set; } = true;

	protected override void OnStart()
	{
		_behaviourTree = BuildBehaviourTree();
		_context = new BehaviourTreeContext();
	}

	protected override void OnUpdate()
	{
		// Exécuter l'arbre de comportement
		var status = _behaviourTree.Execute( _context );
		
		// Afficher le debug si activé
		if ( ShowDebug && _context.LastExecutedNode != null )
		{
			var debugText = $"{_context.CurrentPath}\nStatus: {_context.LastNodeStatus}";
			Gizmo.Draw.ScreenText( debugText, WorldPosition + Vector3.Up * 100, "Consolas", 12, TextFlag.Center );
		}
	}
	
	private static Node BuildBehaviourTree()
	{
		var root = new SelectorNode();
        
		return root;
	}
}
