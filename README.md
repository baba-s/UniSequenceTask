# Uni Sequence Task

## Example

```cs
using System;
using System.Collections;
using UniSequenceTask;
using UnityEngine;

public class Example : MonoBehaviour
{
    private void Start()
    {
        SingleTaskWithLog.OnStartParent  += parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithLog.OnFinishParent += parentName => Debug.Log( $"[SingleTask]「{parentName}」終了" );
        SingleTaskWithLog.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithLog.OnFinishChild  += ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了" );

        SingleTaskWithTimeLog.OnStartParent  += parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithTimeLog.OnFinishParent += ( parentName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒" );
        SingleTaskWithTimeLog.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithTimeLog.OnFinishChild  += ( parentName, childName, elapsedTime ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒" );

        SingleTaskWithProfiler.OnStartParent  += parentName => Debug.Log( $"[SingleTask]「{parentName}」開始" );
        SingleTaskWithProfiler.OnFinishParent += ( parentName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
        SingleTaskWithProfiler.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」開始" );
        SingleTaskWithProfiler.OnFinishChild  += ( parentName, childName, elapsedTime, gcCount ) => Debug.Log( $"[SingleTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );

        MultiTaskWithLog.OnStartParent  += parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithLog.OnFinishParent += parentName => Debug.Log( $"[MultiTask]「{parentName}」終了" );
        MultiTaskWithLog.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithLog.OnFinishChild  += ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了" );

        MultiTaskWithTimeLog.OnStartParent  += parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithTimeLog.OnFinishParent += ( parentName, elapsedTime ) => Debug.Log( $"[MultiTask]「{parentName}」終了    {elapsedTime:0.00} 秒" );
        MultiTaskWithTimeLog.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithTimeLog.OnFinishChild  += ( parentName, childName, elapsedTime ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒" );

        MultiTaskWithProfiler.OnStartParent  += parentName => Debug.Log( $"[MultiTask]「{parentName}」開始" );
        MultiTaskWithProfiler.OnFinishParent += ( parentName, elapsedTime, gcCount ) => Debug.Log( $"[MultiTask]「{parentName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
        MultiTaskWithProfiler.OnStartChild   += ( parentName, childName ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」開始" );
        MultiTaskWithProfiler.OnFinishChild  += ( parentName, childName, elapsedTime, gcCount ) => Debug.Log( $"[MultiTask]「{parentName}」「{childName}」終了    {elapsedTime:0.00} 秒    GC {gcCount} 回" );
    }

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Alpha1 ) )
        {
            var task1 = new SingleTask
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task1.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha2 ) )
        {
            var task2 = new SingleTaskWithLog
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task2.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha3 ) )
        {
            var task3 = new SingleTaskWithTimeLog
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task3.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha4 ) )
        {
            var task4 = new SingleTaskWithProfiler
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task4.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha5 ) )
        {
            var task5 = new MultiTask
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task5.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha6 ) )
        {
            var task6 = new MultiTaskWithLog
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task6.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha7 ) )
        {
            var task7 = new MultiTaskWithTimeLog
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task7.Play( "ピカチュウ" );
        }

        if ( Input.GetKeyDown( KeyCode.Alpha8 ) )
        {
            var task8 = new MultiTaskWithProfiler
            {
                { "Call 1", onEnded => StartCoroutine( Call( 0.3f, onEnded ) ) },
                { "Call 2", onEnded => StartCoroutine( Call( 0.1f, onEnded ) ) },
                { "Call 3", onEnded => StartCoroutine( Call( 0.0f, onEnded ) ) },
                { "Call 4", onEnded => StartCoroutine( Call( 0.2f, onEnded ) ) },
            };
            task8.Play( "ピカチュウ" );
        }
    }

    private IEnumerator Call( float delay, Action callback )
    {
        yield return new WaitForSeconds( delay );
        callback?.Invoke();
    }
}
```