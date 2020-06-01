using System;
using System.Collections;
using Kogane.Internal;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// 処理時間や GC 発生回数のログ出力機能付きの SingleTask を管理するクラス
	/// </summary>
	public sealed class SingleTaskWithProfiler : ISequenceTask
	{
		//==============================================================================
		// デリゲート
		//==============================================================================
		public delegate void OnStartParentCallback( string parentName );

		public delegate void OnFinishParentCallback( string parentName, float elapsedTime, int gcCount );

		public delegate void OnStartChildCallback( string parentName, string childName );

		public delegate void OnFinishChildCallback( string parentName, string childName, float elapsedTime, int gcCount );

		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly SingleTask m_task = new SingleTask();

		//==============================================================================
		// 変数
		//==============================================================================
		private string m_name = string.Empty;

		//==============================================================================
		// イベント(static)
		//==============================================================================
		public static event OnStartParentCallback  OnStartParent;
		public static event OnFinishParentCallback OnFinishParent;
		public static event OnStartChildCallback   OnStartChild;
		public static event OnFinishChildCallback  OnFinishChild;

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// タスクを追加します
		/// </summary>
		public void Add( string text, Action<Action> task )
		{
			m_task.Add
			(
				onNext =>
				{
					OnStartChild?.Invoke( m_name, text );
					var startTime = Time.realtimeSinceStartup;
					var gcWatcher = new GCWatcher();
					gcWatcher.Start();
					task
					(
						() =>
						{
							gcWatcher.Stop();
							OnFinishChild?.Invoke( m_name, text, Time.realtimeSinceStartup - startTime, gcWatcher.Count );
							onNext();
						}
					);
				}
			);
		}

		/// <summary>
		/// タスクを実行します
		/// </summary>
		public void Play( string text, Action onCompleted )
		{
			m_name = text;

			OnStartParent?.Invoke( m_name );
			var startTime = Time.realtimeSinceStartup;
			var gcWatcher = new GCWatcher();
			gcWatcher.Start();
			m_task.Play
			(
				() =>
				{
					gcWatcher.Stop();
					OnFinishParent?.Invoke( m_name, Time.realtimeSinceStartup - startTime, gcWatcher.Count );
					onCompleted?.Invoke();
				}
			);
		}

		/// <summary>
		/// コレクションを反復処理する列挙子を返します
		/// </summary>
		public IEnumerator GetEnumerator()
		{
			throw new NotImplementedException();
		}
	}
}