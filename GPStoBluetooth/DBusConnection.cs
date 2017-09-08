using DBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    public class DBusConnection : IDisposable
    {
        private Bus _system;
        public Exception _startupException { get; private set; }
        private ManualResetEvent _started = new ManualResetEvent(false);
        private Thread _dbusLoop;
        private bool _run = false;
        private bool _isStarted = false;
        private string _connectionName;

        public DBusConnection(string connectionName)
        {
            _connectionName = connectionName;
            Startup();
        }

        public Bus System
        {
            get
            {
                if (_isStarted)
                {
                    return _system;
                }
                else
                {
                    throw new InvalidOperationException("Not connected to DBus");
                }
            }
        }

        private void Startup()
        {
            // Run a message loop for DBus on a new thread.
            _run = true;
            _dbusLoop = new Thread(DBusLoop);
            _dbusLoop.IsBackground = true;
            _dbusLoop.Start();
            _started.WaitOne(60 * 1000);
            _started.Close();
            if (_startupException != null)
            {
                throw _startupException;
            }
            else
            {
                _isStarted = true;
            }
        }

        private void DBusLoop()
        {
            try
            {
                _system = Bus.System;
                _system.RequestName(_connectionName);
            }
            catch (Exception ex)
            {
                _startupException = ex;
                return;
            }
            finally
            {
                _started.Set();
            }

            while (_run)
            {
                _system.Iterate();
            }
        }

        private void Shutdown()
        {
            _run = false;
            try
            {
                _dbusLoop.Join(1000);
            }
            catch
            {
                try
                {
                    _dbusLoop.Abort();
                }
                catch
                {
                }
            }
        }

        public void Dispose()
        {
            if (_isStarted)
            {
                Shutdown();
            }
        }
    }
}
