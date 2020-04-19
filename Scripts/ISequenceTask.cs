using System;
using System.Collections;

namespace UniSequenceTask
{
	public interface ISequenceTask : IEnumerable
	{
		void Add( string text, Action<Action> task );

		void Play( string text, Action onCompleted );
	}

	public static class ISequenceTaskExt
	{
		public static void Add( this ISequenceTask self, Action<Action> task )
		{
			self.Add( string.Empty, task );
		}

		public static void Play( this ISequenceTask self )
		{
			self.Play( onCompleted: null );
		}

		public static void Play( this ISequenceTask self, Action onCompleted )
		{
			self.Play( string.Empty, onCompleted );
		}

		public static void Play( this ISequenceTask self, string text )
		{
			self.Play( text, null );
		}
	}
}