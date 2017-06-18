using System;

namespace App.Infra.Data.DbFactory
{
	public class Disposable : IDisposable
	{
		private bool _isDisposed;

		public Disposable()
		{
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this._isDisposed & disposing)
			{
				this.DisposeCore();
			}
			this._isDisposed = true;
		}

		protected virtual void DisposeCore()
		{
		}

		~Disposable()
		{
			this.Dispose(false);
		}
	}
}