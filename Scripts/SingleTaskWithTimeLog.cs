using System;
using System.Collections;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// 処理時間のログ出力機能付きの SingleTask を管理するクラス
	/// </summary>
	public sealed class SingleTaskWithTimeLog : ISequenceTask
	{
		//==============================================================================
		// デリゲート
		//==============================================================================
		public delegate void OnStartParentCallback( string parentName );

		public delegate void OnFinishParentCallback( string parentName, float elapsedTime );

		public delegate void OnStartChildCallback( string parentName, string childName );

		public delegate void OnFinishChildCallback( string parentName, string childName, float elapsedTime );

		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly SingleTask m_task = new SingleTask();

		//==============================================================================
		// 変数
		//==============================================================================
		private string m_name = string.Empty;

		//==============================================================================
		// デリゲート(static)
		//==============================================================================
		public static OnStartParentCallback  OnStartParent	{ get; set; } = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
		public static OnFinishParentCallback OnFinishParent	{ get; set; } = ( parentName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒" );
		public static OnStartChildCallback   OnStartChild	{ get; set; } = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
		public static OnFinishChildCallback  OnFinishChild	{ get; set; } = ( parentName, childName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒" );

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
					task
					(
						() =>
						{
							OnFinishChild?.Invoke( m_name, text, Time.realtimeSinceStartup - startTime );
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
			m_task.Play
			(
				() =>
				{
					OnFinishParent?.Invoke( m_name, Time.realtimeSinceStartup - startTime );
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