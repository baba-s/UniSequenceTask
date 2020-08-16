using System;
using System.Collections;
using UnityEngine;

namespace Kogane
{
	/// <summary>
	/// ログ出力機能付きの SingleTask を管理するクラス
	/// </summary>
	public sealed class SingleTaskWithLog : ISequenceTask
	{
		//==============================================================================
		// デリゲート
		//==============================================================================
		public delegate void OnParentCallback( string parentName );

		public delegate void OnChildCallback( string parentName, string childName );

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
		public static OnParentCallback OnStartParent  { get; set; } = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
		public static OnParentCallback OnFinishParent { get; set; } = parentName => Debug.Log( $"[SingleTask]「{parentName}」終了" );
		public static OnChildCallback  OnStartChild   { get; set; } = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
		public static OnChildCallback  OnFinishChild  { get; set; } = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了" );

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SingleTaskWithLog() : this( false )
		{
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		public SingleTaskWithLog( bool isReuse )
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
					task
					(
						() =>
						{
							OnFinishChild?.Invoke( m_name, text );
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
			m_task.Play
			(
				() =>
				{
					OnFinishParent?.Invoke( m_name );
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