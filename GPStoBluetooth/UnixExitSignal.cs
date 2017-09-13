using Mono.Unix;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPStoBluetooth
{
    public class UnixExitSignal
    {
        public event EventHandler Exit;
        private event EventHandler DoNothing;

        UnixSignal[] signals = new UnixSignal[] 
        {
        new UnixSignal(Mono.Unix.Native.Signum.SIGTERM),
        new UnixSignal(Mono.Unix.Native.Signum.SIGINT),
        new UnixSignal(Mono.Unix.Native.Signum.SIGUSR1)
        };

        UnixSignal[] memorySignal = new UnixSignal[]
        {
        new UnixSignal(Mono.Unix.Native.Signum.SIGSEGV)
        };

        public UnixExitSignal()
        {
            Task.Factory.StartNew(() =>
            {
                // blocking call to wait for any kill signal
                UnixSignal.WaitAny(signals);
                Exit?.Invoke(null, EventArgs.Empty);
            });

            Task.Factory.StartNew(() =>
            {
                // blocking call to wait for any SIGSEGV signals
                UnixSignal.WaitAny(memorySignal);
                DoNothing?.Invoke(null, EventArgs.Empty);
            });
        }

    }
}
