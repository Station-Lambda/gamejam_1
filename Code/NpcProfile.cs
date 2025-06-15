using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sandbox;

public enum Gender
{
	Male,
	Female
}

public sealed class NpcProfile : Component
{
	[Property] public string Name { get; set; } = "";
	
	[Property] public Gender Gender { get; set; } = Gender.Male;
	
	[Property] public List<string> Traits { get; set; } = [];
	
	[Property] public List<string> Memory { get; set; } = [];
	
	[Property] public string CurrentEmotion { get; set; } = "";
	
	protected override void OnStart()
	{
		base.OnStart();

		if ( string.IsNullOrEmpty( Name ) || Name == "Unknown" )
		{
			NpcGenerator.GenerateProfile( this );
		}
	}
	
	public override string ToString()
	{
		var sb = new StringBuilder();
		sb.AppendLine( $"=== {Name} ({Gender}) ===" );
		sb.AppendLine( $"Emotion: {CurrentEmotion}" );
		sb.AppendLine( $"Traits: {string.Join( ", ", Traits )}" );
		sb.AppendLine( $"Memories: {Memory.Count}" );
		if ( Memory.Count == 0 )
		{
			return sb.ToString();
		}

		foreach ( var mem in Memory )
		{
			sb.AppendLine( $"  - {mem}" );
		}
		return sb.ToString();
	}
}
