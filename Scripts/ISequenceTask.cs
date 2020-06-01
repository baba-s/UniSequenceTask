using System;
using System.Collections;

namespace Kogane
{
	/// <summary>
	/// タスクのインターフェイス
	/// </summary>
	public interface ISequenceTask : IEnumerable
	{
		/// <summary>
		/// タスクを追加します
		/// </summary>
		void Add( string text, Action<Action> task );
		
		/// <summary>
		/// タスクを実行します
		/// </summary>
		void Play( string text, Action onCompleted );
	}

	/// <summary>
	/// ISequenceTask 型の拡張メソッドを管理するクラス
	/// </summary>
	public static class ISequenceTaskExt
	{
		/// <summary>
		/// タスクを追加します
		/// </summary>
		public static void Add( this ISequenceTask self, Action<Action> task )
		{
			self.Add( string.Empty, task );
		}
		
		/// <summary>
		/// タスクを実行します
		/// </summary>
		public static void Play( this ISequenceTask self )
		{
			self.Play( onCompleted: null );
		}
		
		/// <summary>
		/// タスクを実行します
		/// </summary>
		public static void Play( this ISequenceTask self, Action onCompleted )
		{
			self.Play( string.Empty, onCompleted );
		}
		
		/// <summary>
		/// タスクを実行します
		/// </summary>
		public static void Play( this ISequenceTask self, string text )
		{
			self.Play( text, null );
		}
	}
}