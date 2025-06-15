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
	public override NodeStatus Execute()
	{
		var status = child.Execute();

		return status switch
		{
			NodeStatus.Success => NodeStatus.Failure,
			NodeStatus.Failure => NodeStatus.Success,
			_ => status
		};
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
