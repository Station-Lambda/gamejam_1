using System;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Nœud sélecteur qui exécute ses enfants dans un ordre aléatoire jusqu'à ce qu'un réussisse.
/// </summary>
public class RandomSelectorNode : CompositeNode
{
	private readonly List<int> _shuffledIndices = [];
	private int _currentIndex = 0;
	private bool _needsShuffle = true;

	/// <summary>
	/// Exécute les nœuds enfants dans un ordre aléatoire jusqu'à ce qu'un réussisse.
	/// </summary>
	/// <returns>Success si un enfant réussit, Running si un enfant est en cours, Failure si tous échouent.</returns>
	public override NodeStatus Execute()
	{
		if ( _needsShuffle )
		{
			ShuffleIndices();
			_needsShuffle = false;
		}

		while ( _currentIndex < _shuffledIndices.Count )
		{
			var childIndex = _shuffledIndices[_currentIndex];
			var status = Children[childIndex].Execute();

			switch ( status )
			{
				case NodeStatus.Running:
					return NodeStatus.Running;
				case NodeStatus.Success:
					ResetState();
					return NodeStatus.Success;
				case NodeStatus.Failure:
				case NodeStatus.Invalid:
				default:
					_currentIndex++;
					break;
			}
		}

		ResetState();
		return NodeStatus.Failure;
	}

	/// <summary>
	/// Mélange les indices des enfants pour un ordre aléatoire.
	/// </summary>
	private void ShuffleIndices()
	{
		_shuffledIndices.Clear();
		for ( var i = 0; i < Children.Count; i++ )
		{
			_shuffledIndices.Add( i );
		}

		// Fisher-Yates shuffle
		for ( var i = _shuffledIndices.Count - 1; i > 0; i-- )
		{
			var j = Random.Shared.Int( 0, i );
			(_shuffledIndices[i], _shuffledIndices[j]) = (_shuffledIndices[j], _shuffledIndices[i]);
		}
	}

	/// <summary>
	/// Réinitialise l'état interne du nœud.
	/// </summary>
	private void ResetState()
	{
		_currentIndex = 0;
		_needsShuffle = true;
	}

	/// <summary>
	/// Réinitialise l'état du sélecteur aléatoire.
	/// </summary>
	public override void Reset()
	{
		base.Reset();
		ResetState();
	}
}
