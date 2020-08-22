using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptographyHelperUtil.Commands
{
    public interface INotifyCanExecuteChanged
    {
        event EventHandler CanExecuteChanged;

        bool CanExecute();
    }

    public abstract class NotifyCanExecuteChangedBase : INotifyCanExecuteChanged
    {
        public event EventHandler CanExecuteChanged;
        protected virtual void OnCanExecuteChanged()
        {
            var evt = CanExecuteChanged;
            if (evt != null)
            {
                evt(this, EventArgs.Empty);
            }
        }

        public abstract bool CanExecute();
    }
}
