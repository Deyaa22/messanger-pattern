using System;
using System.Collections.Generic;

namespace Messanger;

/// <summary>
/// Subscribe, Unsubscribe, and Fire actions with global action names.
/// </summary>
/// <typeparam name="T">Type of parameter will be used in action</typeparam>
public class Messanger<T>
{
	private Dictionary<string, Action<T>> _actions = new Dictionary<string, Action<T>>();

	/// <summary>
	/// Set 'one' action to 'one' action name.
	/// </summary>
	/// <param name="actionName">Action name</param>
	public Action<T> this[string actionName]
	{
		set
		{
			if (value == null)
			{
				if (_actions.ContainsKey(actionName))
					_actions[actionName] = null;
				return;
			}

			if (_actions.ContainsKey(actionName))
				_actions.Remove(actionName);

			SubscribeAction(actionName, value);
		}
	}

	/// <summary>
	/// Subscribe Action with name.
	/// </summary>
	/// <param name="actionName">Action name</param>
	/// <param name="action">Action - Method</param>
	public void SubscribeAction(string actionName, Action<T> action)
	{
		if (!_actions.ContainsKey(actionName))
			_actions[actionName] = action;
		else
			_actions[actionName] += action;
	}

	/// <summary>
	/// Unsubscribe Action with name.
	/// </summary>
	/// <param name="actionName">Action name</param>
	/// <param name="action">Action - Method</param>
	public void UnsubscribeAction(string actionName, Action<T> action)
	{
		if (!_actions.ContainsKey(actionName) || _actions[actionName] == null)
			return;

		_actions[actionName] -= action;
	}

	/// <summary>
	/// Invoke subscribed actions with action name.
	/// </summary>
	/// <param name="actionName">Action name</param>
	/// <param name="actionParameter">Action parameter</param>
	/// <exception cref="ArgumentException">Thrown if action name not subscribed</exception>
	public void FireAction(string actionName, T actionParameter)
	{
		if (!_actions.ContainsKey(actionName))
			throw new ArgumentException("Action with name [{actionName}] is not subscribed !!");

		_actions[actionName].Invoke(actionParameter);
	}
}