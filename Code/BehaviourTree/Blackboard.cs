using System.Collections.Generic;

namespace Sandbox.BehaviourTree;

/// <summary>
/// Système de stockage partagé pour les données entre les nœuds de l'arbre de comportement.
/// Permet aux nœuds de partager des informations et des états.
/// </summary>
public class Blackboard
{
	private readonly Dictionary<string, object> _data = new();

	/// <summary>
	/// Définit une valeur dans le blackboard.
	/// </summary>
	/// <typeparam name="T">Le type de la valeur.</typeparam>
	/// <param name="key">La clé pour identifier la valeur.</param>
	/// <param name="value">La valeur à stocker.</param>
	public void Set<T>( string key, T value )
	{
		_data[key] = value;
	}

	/// <summary>
	/// Récupère une valeur du blackboard.
	/// </summary>
	/// <typeparam name="T">Le type de la valeur à récupérer.</typeparam>
	/// <param name="key">La clé de la valeur.</param>
	/// <returns>La valeur si elle existe, sinon la valeur par défaut du type.</returns>
	public T Get<T>( string key )
	{
		if ( _data.TryGetValue( key, out var value ) && value is T typedValue )
		{
			return typedValue;
		}
		
		return default;
	}

	/// <summary>
	/// Tente de récupérer une valeur du blackboard.
	/// </summary>
	/// <typeparam name="T">Le type de la valeur à récupérer.</typeparam>
	/// <param name="key">La clé de la valeur.</param>
	/// <param name="value">La valeur récupérée si elle existe.</param>
	/// <returns>True si la valeur existe et est du bon type, false sinon.</returns>
	public bool TryGet<T>( string key, out T value )
	{
		if ( _data.TryGetValue( key, out var objValue ) && objValue is T typedValue )
		{
			value = typedValue;
			return true;
		}
		
		value = default;
		return false;
	}

	/// <summary>
	/// Vérifie si une clé existe dans le blackboard.
	/// </summary>
	/// <param name="key">La clé à vérifier.</param>
	/// <returns>True si la clé existe, false sinon.</returns>
	public bool Contains( string key )
	{
		return _data.ContainsKey( key );
	}

	/// <summary>
	/// Supprime une valeur du blackboard.
	/// </summary>
	/// <param name="key">La clé de la valeur à supprimer.</param>
	/// <returns>True si la valeur a été supprimée, false si elle n'existait pas.</returns>
	public bool Remove( string key )
	{
		return _data.Remove( key );
	}

	/// <summary>
	/// Efface toutes les données du blackboard.
	/// </summary>
	public void Clear()
	{
		_data.Clear();
	}
}