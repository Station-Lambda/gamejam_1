

using Sandbox.AiIntegration;

namespace Sandbox.UI;

public partial class AiMenu : PanelComponent
{
	public List<string> Conversations;
	
	protected override int BuildHash() => System.HashCode.Combine( Conversations );
	
	public void SelectConversation(string conversation)
	{
		Log.Info( conversation );
	}
}
