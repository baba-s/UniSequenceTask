using System;
using System.Collections;

namespace Kogane
{
	/// <summary>
	/// ログ出力機能付きの MultiTask を管理するクラス
	/// </summary>
	public sealed class MultiTaskWithLog : ISequenceTask
	{
		//==============================================================================
		// デリゲート
		//==============================================================================
		public delegate void OnParentCallback( string parentName );

		public delegate void OnChildCallback( string parentName, string childName );

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
		public static event OnParentCallback OnStartParent;
		public static event OnParentCallback OnFinishParent;
		public static event OnChildCallback  OnStartChild;
		public static event OnChildCallback  OnFinishChild;

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