namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud décorateur qui inverse le résultat de son nœud enfant.
/// Success devient Failure et vice versa. Running reste Running.
/// </summary>
/// <param name="child">Le nœud enfant dont le résultat sera inversé.</param>
public class InverterNode( Node child ) : Node
{
	/// <summary>
	/// Exécute le nœud enfant et inverse son résultat.
	/// </summary>
	/// <returns>Failure si l'enfant retourne Success, Success si l'enfant retourne Failure, Running sinon.</returns>
	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		context.LastExecutedNode = this;
		context.CurrentPath = "InverterNode";
		context.CurrentDepth++;
		
		var status = child.Execute( context );
		context.CurrentDepth--;

		var invertedStatus = status switch
		{
			NodeStatus.Success => NodeStatus.Failure,
			NodeStatus.Failure => NodeStatus.Success,
			_ => status
		};
		
		context.LastNodeStatus = invertedStatus;
		Log.Info( $"{new string( ' ', context.CurrentDepth * 2 )}InverterNode: {status} -> {invertedStatus}" );
		return invertedStatus;
	}

	/// <summary>
	/// Réinitialise le nœud enfant.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		child.Reset();
	}
}
