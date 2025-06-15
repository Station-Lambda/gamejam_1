using Sandbox.BehaviourTree;

namespace Sandbox;

public class AiCharacter : Component
{
	/// <summary>
	/// Affiche le debug du behaviour tree au-dessus du personnage.
	/// </summary>
	[Property] public bool ShowDebug { get; set; } = true;
	
	private Node _behaviourTree;
	private BehaviourTreeContext _context;

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
			Log.Info( "Debug" );
			var debugText = $"{_context.CurrentPath}\nStatus: {_context.LastNodeStatus}";
			Log.Info( debugText );
			var tr = Transform.World;
			tr.Position += Vector3.Up * 50;
			tr.Rotation = tr.Rotation.RotateAroundAxis( Vector3.Forward, 90 );
			Gizmo.Draw.WorldText( debugText, tr );
		}
	}
	
	private Node BuildBehaviourTree()
	{
		var root = new SelectorNode();
        
		root.AddChild( new ActionNode(BaseAction));
		
		return root;
	}

	private NodeStatus BaseAction()
	{
		Log.Info( "Ok" );
		return NodeStatus.Success;
	}
}
