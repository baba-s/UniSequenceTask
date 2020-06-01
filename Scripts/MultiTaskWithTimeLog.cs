using System;
using System.Collections;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// 処理時間のログ出力機能付きの MultiTask を管理するクラス
	/// </summary>
	public sealed class MultiTaskWithTimeLog : ISequenceTask
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
		private readonly MultiTask m_task = new MultiTask();

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
				onEnded =>
				{
					OnStartChild?.Invoke( m_name, text );
					var startTime = Time.realtimeSinceStartup;
					task
					(
						() =>
						{
							OnFinishChild?.Invoke( m_name, text, Time.realtimeSinceStartup - startTime );
							onEnded();
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