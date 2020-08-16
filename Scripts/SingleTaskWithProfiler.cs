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
		private readonly SingleTask m_task;

		//==============================================================================
		// 変数
		//==============================================================================
		private string m_name = string.Empty;

		//==============================================================================
		// デリゲート(static)
		//==============================================================================
		public static OnStartParentCallback  OnStartParent	{ get; set; } = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
		public static OnFinishParentCallback OnFinishParent	{ get; set; } = ( parentName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
		public static OnStartChildCallback   OnStartChild	{ get; set; } = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
		public static OnFinishChildCallback  OnFinishChild	{ get; set; } = ( parentName, childName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SingleTaskWithProfiler() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SingleTaskWithProfiler( bool isReuse )
		{
			m_task = new SingleTask( isReuse );
		}

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