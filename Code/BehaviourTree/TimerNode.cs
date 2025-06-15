using System;

namespace Sandbox.BehaviourTree;

public class TimerNode : Node
{
	private readonly float _duration;
	private readonly Func<float> _durationFunc;
	private float _startTime;
	private bool _isRunning;
	private float _currentDuration;

	public TimerNode( float duration )
	{
		_duration = duration;
	}

	public TimerNode( Func<float> durationFunc )
	{
		_durationFunc = durationFunc;
	}

	public override NodeStatus Execute( BehaviourTreeContext context )
	{
		if ( !_isRunning )
		{
			_startTime = Time.Now;
			_isRunning = true;
			_currentDuration = _durationFunc?.Invoke() ?? _duration;
		}

		var elapsed = Time.Now - _startTime;
		var remaining = _currentDuration - elapsed;
		UpdateContext( context, $"{remaining:F1}s" );
		
		if ( elapsed >= _currentDuration )
		{
			_isRunning = false;
			context.LastNodeStatus = NodeStatus.Success;
			return NodeStatus.Success;
		}

		context.LastNodeStatus = NodeStatus.Running;
		return NodeStatus.Running;
	}

	public override void Reset()
	{
		base.Reset();
		_isRunning = false;
		_startTime = 0;
		_currentDuration = 0;
	}
}