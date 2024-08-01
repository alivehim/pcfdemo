using Prism.Mvvm;
using Prism.Navigation;
using System.ComponentModel;

namespace UtilityTools.Core.Mvvm
{
    public abstract class ViewModelBase : BindableBase, IDestructible
    {
        protected ViewModelBase()
        {

        }

        //public event PropertyChangedEventHandler PropertyChanged;

        //protected void RaisePropertyChangedEvent(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChangedEventArgs e = new PropertyChangedEventArgs(propertyName);
        //        PropertyChanged(this, e);
        //    }
        //}

        public virtual void Destroy()
        {

        }
    }
}
