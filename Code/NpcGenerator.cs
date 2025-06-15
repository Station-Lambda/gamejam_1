using System;
using System.Collections.Generic;

namespace Sandbox;

/// <summary>
/// Simple NPC profile generator for MVP.
/// </summary>
public static class NpcGenerator
{
	private static readonly Random Random = new Random();
	
	// Name banks
	private static readonly string[] MaleNames = 
	{
		"James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph",
		"Thomas", "Charles", "Christopher", "Daniel", "Matthew", "Anthony", "Mark",
		"Donald", "Steven", "Kenneth", "Andrew", "Joshua", "Kevin", "Brian", "George"
	};
	
	private static readonly string[] FemaleNames = 
	{
		"Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan",
		"Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Betty", "Helen", "Sandra",
		"Donna", "Carol", "Ruth", "Sharon", "Michelle", "Laura", "Dorothy", "Amy"
	};
	
	// Simple trait tags
	private static readonly string[] TraitTags = 
	{
		"Friendly", "Anxious", "Confident", "Shy", "Curious", "Stubborn",
		"Optimistic", "Pessimistic", "Creative", "Logical", "Emotional", "Stoic",
		"Trusting", "Suspicious", "Energetic", "Lazy", "Brave", "Cowardly",
		"Generous", "Selfish", "Patient", "Impatient", "Honest", "Deceptive"
	};
	
	// Simple memories
	private static readonly string[] MemoryTemplates = 
	{
		"Lost a pet when I was young",
		"Had a strange dream last week",
		"Saw something unexplainable near the old church",
		"My best friend moved away years ago",
		"Won a competition in high school",
		"Got lost in the forest once",
		"Helped a stranger last month",
		"Found an old photo album in the attic",
		"Learned to cook from my grandmother",
		"Had a car accident five years ago",
		"Traveled to Europe once",
		"Met someone famous at a coffee shop",
		"Witnessed a meteor shower",
		"Got food poisoning at a restaurant",
		"Saved money for something special",
		"Broke my arm falling off a bike",
		"Had a prophetic dream that came true",
		"Lost my wallet on the subway",
		"Received an anonymous letter",
		"Saw a ghost in an old building"
	};
	
	// Emotional states
	private static readonly string[] Emotions = 
	{
		"Content", "Anxious", "Happy", "Sad", "Angry", "Fearful",
		"Excited", "Bored", "Hopeful", "Frustrated", "Curious", "Confused",
		"Peaceful", "Restless", "Nostalgic", "Lonely", "Grateful", "Worried"
	};
	
	/// <summary>
	/// Generates a simple profile for an NPC.
	/// </summary>
	public static void GenerateProfile( NpcProfile profile )
	{
		// Generate name
		if ( string.IsNullOrEmpty( profile.Name ) || profile.Name == "Unknown" )
		{
			var names = profile.Gender == Gender.Male ? MaleNames : FemaleNames;
			profile.Name = names[Random.Next( names.Length )];
		}
		
		// Generate 2-3 trait tags
		profile.Traits.Clear();
		int traitCount = Random.Next( 2, 4 );
		var usedTraits = new HashSet<int>();
		
		for ( int i = 0; i < traitCount; i++ )
		{
			int index;
			do
			{
				index = Random.Next( TraitTags.Length );
			} while ( usedTraits.Contains( index ) );
			
			usedTraits.Add( index );
			profile.Traits.Add( TraitTags[index] );
		}
		
		// Generate 3-4 simple memories
		profile.Memory.Clear();
		int memoryCount = Random.Next( 3, 5 );
		var usedMemories = new HashSet<int>();
		
		for ( int i = 0; i < memoryCount; i++ )
		{
			int index;
			do
			{
				index = Random.Next( MemoryTemplates.Length );
			} while ( usedMemories.Contains( index ) );
			
			usedMemories.Add( index );
			profile.Memory.Add( MemoryTemplates[index] );
		}
		
		// Generate 1 current emotion
		profile.CurrentEmotion = Emotions[Random.Next( Emotions.Length )];
	}
	
	/// <summary>
	/// Gets a random emotion.
	/// </summary>
	public static string GetRandomEmotion()
	{
		return Emotions[Random.Next( Emotions.Length )];
	}
	
	/// <summary>
	/// Gets a random trait.
	/// </summary>
	public static string GetRandomTrait()
	{
		return TraitTags[Random.Next( TraitTags.Length )];
	}
}
