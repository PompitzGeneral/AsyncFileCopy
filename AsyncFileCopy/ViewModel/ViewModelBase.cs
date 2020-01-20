using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncFileCopy.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void RaisePropertyChanged( params string[] propertyNames )
		{
			foreach ( string name in propertyNames )
			{
				OnPropertyChanged( new PropertyChangedEventArgs( name ) );
			}
		}

		protected virtual void OnPropertyChanged( PropertyChangedEventArgs e )
		{
			PropertyChanged?.Invoke( this, e );
		}

		public virtual bool CanClose( )
		{
			return true;
		}
    }
}
