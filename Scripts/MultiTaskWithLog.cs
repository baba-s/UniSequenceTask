﻿using System;
using System.Collections;
using System.Diagnostics;

namespace UniSequenceTask
{
	/// <summary>
	/// ログ出力機能付きの MultiTask を管理するクラス
	/// </summary>
	public sealed class MultiTaskWithLog : ISequenceTask
	{
		//==============================================================================
		// 変数(readonly)
		//==============================================================================
		private readonly MultiTask m_task = new MultiTask();

		//==============================================================================
		// 変数
		//==============================================================================
		private string m_name = string.Empty;

		//==============================================================================
		// プロパティ(static)
		//==============================================================================
		public static bool IsLogEnabled { get; set; } = true; // ログ出力が有効の場合 true

		//==============================================================================
		// 関数
		//==============================================================================
		/// <summary>
		/// タスクを追加します
		/// </summary>
		public void Add( string text, Action<Action> task )
		{
#if ENABLE_DEBUG_LOG

			m_task.Add( onEnded =>
			{
				Log( $"【MultiTask】「{m_name}」「{text}」開始" );
				task( () =>
				{
					Log( $"【MultiTask】「{m_name}」「{text}」終了" );
					onEnded();
				} );
			} );
#else
			m_task.Add( text, task );
#endif
		}

		/// <summary>
		/// タスクを実行します
		/// </summary>
		public void Play( string text, Action onCompleted )
		{
#if ENABLE_DEBUG_LOG

			m_name = text;

			Log( $"【MultiTask】「{m_name}」開始" );
			m_task.Play( () =>
			{
				Log( $"【MultiTask】「{m_name}」終了" );
				onCompleted?.Invoke();
			} );
#else
			m_task.Play( text, onCompleted );
#endif
		}

		/// <summary>
		/// ログ出力します
		/// </summary>
		[Conditional( "ENABLE_DEBUG_LOG" )]
		private static void Log( string message )
		{
			if ( !IsLogEnabled ) return;
			UnityEngine.Debug.Log( message );
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