# Uni Sequence Task

コールバックの直列実行・並列実行が可能なクラス

## Example

```cs
using Kogane;
using System;
using System.Collections;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        // ログ出力のイベント登録
        SingleTaskWithLog.OnStartParent  = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithLog.OnFinishParent = parentName => Debug.Log( $"[SingleTask]「{parentName}」終了" );
        SingleTaskWithLog.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithLog.OnFinishChild  = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了" );

        SingleTaskWithTimeLog.OnStartParent  = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithTimeLog.OnFinishParent = ( parentName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒" );
        SingleTaskWithTimeLog.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithTimeLog.OnFinishChild  = ( parentName, childName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒" );

        SingleTaskWithProfiler.OnStartParent  = parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithProfiler.OnFinishParent = ( parentName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
        SingleTaskWithProfiler.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithProfiler.OnFinishChild  = ( parentName, childName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );

        MultiTaskWithLog.OnStartParent  = parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithLog.OnFinishParent = parentName => Debug.Log( $"[MultiTask]「{parentName}」終了" );
        MultiTaskWithLog.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithLog.OnFinishChild  = ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了" );

        MultiTaskWithTimeLog.OnStartParent  = parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithTimeLog.OnFinishParent = ( parentName, elapsedTime ) => Debug.Log( $"[MultiTask]「{parentName}」終了    {elapsedTime:0.00} 秒" );
        MultiTaskWithTimeLog.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithTimeLog.OnFinishChild  = ( parentName, childName, elapsedTime ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒" );

        MultiTaskWithProfiler.OnStartParent  = parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithProfiler.OnFinishParent = ( parentName, elapsedTime, gcCount ) => Debug.Log( $"[MultiTask]「{parentName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
        MultiTaskWithProfiler.OnStartChild   = ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithProfiler.OnFinishChild  = ( parentName, childName, elapsedTime, gcCount ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
        {
            // 直列実行
            var task = new SingleTask
            {
                onNext => StartCoroutine( Call( 0.3f, onNext ) ),
                onNext => StartCoroutine( Call( 0.1f, onNext ) ),
                onNext => StartCoroutine( Call( 0.0f, onNext ) ),
                onNext => StartCoroutine( Call( 0.2f, onNext ) ),
            };
            task.Play( () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
        {
            // 直列実行（開始終了のログ出力付き）
            var task = new SingleTaskWithLog
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
        {
            // 直列実行（経過時間のログ出力付き）
            var task = new SingleTaskWithTimeLog
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
        {
            // 直列実行（GC 発生回数のログ出力付き）
            var task = new SingleTaskWithProfiler
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha5 ) )
        {
            // 並列実行
            var task = new MultiTask
            {
                onNext => StartCoroutine( Call( 0.3f, onNext ) ),
                onNext => StartCoroutine( Call( 0.1f, onNext ) ),
                onNext => StartCoroutine( Call( 0.0f, onNext ) ),
                onNext => StartCoroutine( Call( 0.2f, onNext ) ),
            };
            task.Play( () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha6 ) )
        {
            // 並列実行（開始終了のログ出力付き）
            var task = new MultiTaskWithLog
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha7 ) )
        {
            // 並列実行（経過時間のログ出力付き）
            var task = new MultiTaskWithTimeLog
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha8 ) )
        {
            // 並列実行（GC 発生回数のログ出力付き）
            var task = new MultiTaskWithProfiler
            {
                { "Call 1", onNext => StartCoroutine( Call( 0.3f, onNext ) ) },
                { "Call 2", onNext => StartCoroutine( Call( 0.1f, onNext ) ) },
                { "Call 3", onNext => StartCoroutine( Call( 0.0f, onNext ) ) },
                { "Call 4", onNext => StartCoroutine( Call( 0.2f, onNext ) ) },
            };
            task.Play( "ピカチュウ", () => Debug.Log( "完了" ) );
        }
    }

    private IEnumerator Call( float delay, Action callback )
    {
        yield return new WaitForSeconds( delay );
        callback?.Invoke();
    }
}
```
