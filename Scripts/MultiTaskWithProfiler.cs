﻿using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace UniSequenceTask
{
	/// <summary>
	/// 処理時間や GC 発生回数のログ出力機能付きの MultiTask を管理するクラス
	/// </summary>
	public sealed class MultiTaskWithProfiler : ISequenceTask
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
				var startTime = Time.realtimeSinceStartup;
				var gcWatcher = new GCWatcher();
				gcWatcher.Start();
				task( () =>
				{
					gcWatcher.Stop();
					Log( $"【MultiTask】「{m_name}」「{text}」終了    {( Time.realtimeSinceStartup - startTime ).ToString( "0.###" ) } 秒    GC {gcWatcher.Count.ToString()} 回" );
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
			var startTime = Time.realtimeSinceStartup;
			var gcWatcher = new GCWatcher();
			gcWatcher.Start();
			m_task.Play( () =>
			{
				gcWatcher.Stop();
				Log( $"【MultiTask】「{m_name}」終了    {( Time.realtimeSinceStartup - startTime ).ToString( "0.###" ) } 秒    GC {gcWatcher.Count.ToString()} 回" );
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