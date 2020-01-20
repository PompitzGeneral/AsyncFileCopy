using AsyncFileCopy.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AsyncFileCopy.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            WireCommand();
        }

        public RelayCommand AsyncCopyCommand { get; private set; }
        public RelayCommand CopyCommand  { get; private set; }
        public RelayCommand ClearCommand    { get; private set; }
        public RelayCommand SourceFileOpenCommand { get; private set; }
        public RelayCommand DestinationFileSaveCommand { get; private set; }

        private void WireCommand()
        {
            AsyncCopyCommand = new RelayCommand( AsyncCopyCommand_Executed );
            CopyCommand      = new RelayCommand( CopyCommand_Executed );
            ClearCommand    = new RelayCommand( ClearCommand_Executed );
            SourceFileOpenCommand = new RelayCommand( SourceFileOpenCommand_Executed );
            DestinationFileSaveCommand = new RelayCommand( DestinationFileSaveCommand_Executed );
        }

        private string sourceFilePath;
        public string SourceFilePath
        {
            get
            {
                return sourceFilePath;
            }
            set
            {
                if ( sourceFilePath != value )
                {
                    sourceFilePath = value;
                    RaisePropertyChanged( "SourceFilePath" );
                }
            }
        }

        private string destinationFilePath;
        public string DestinationFilePath
        {
            get
            {
                return destinationFilePath;
            }
            set
            {
                if ( destinationFilePath != value )
                {
                    destinationFilePath = value;
                    RaisePropertyChanged( "DestinationFilePath" );
                }
            }
        }

        private int progressPercentage = 0;
        public int ProgressPercentage
        {
            get
            {
                return progressPercentage;
            }
            set
            {
                if ( progressPercentage != value )
                {
                    progressPercentage = value;
                    RaisePropertyChanged( "ProgressPercentage" );
                }
            }
        }
        
        private async void AsyncCopyCommand_Executed( object parameter )
        {
            long totalCopied = 0;

            using ( FileStream sourceFileStream = new FileStream( this.sourceFilePath, FileMode.Open ) )
            {
                using ( FileStream destFileStream = new FileStream( this.destinationFilePath, FileMode.Create ) )
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int nRead = 0;

                    while ( ( nRead = await sourceFileStream.ReadAsync( buffer, 0, buffer.Length) ) != 0 )
                    {
                        await destFileStream.WriteAsync( buffer, 0, nRead );
                        totalCopied += nRead;

                        ProgressPercentage = (int)( ( (double)totalCopied / (double)destFileStream.Length ) * 100 );
                    }
                }
            }
        }

        private void CopyCommand_Executed( object parameter )
        {
            long totalCopied = 0;

            using ( FileStream sourceFileStream = new FileStream( this.sourceFilePath, FileMode.Open ) )
            {
                using ( FileStream destFileStream = new FileStream( this.destinationFilePath, FileMode.Create ) )
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int nRead = 0;

                    while ( ( nRead = sourceFileStream.Read( buffer, 0, buffer.Length ) ) != 0 )
                    {
                        destFileStream.Write( buffer, 0, nRead );
                        totalCopied += nRead;

                        ProgressPercentage = (int)( ( (double)totalCopied / (double)destFileStream.Length ) * 100 );
                    }
                }
            }
        }

        private void ClearCommand_Executed( object parameter )
        {
            ProgressPercentage = 0;
        }

        private void SourceFileOpenCommand_Executed( object parameter )
        {
            var dlg = new OpenFileDialog();

            if ( dlg.ShowDialog() == true )
            {
                this.SourceFilePath = dlg.FileName;
            }
        }

        private void DestinationFileSaveCommand_Executed( object parameter )
        {
            var dlg = new SaveFileDialog();

            if ( dlg.ShowDialog() == true )
            {
                this.DestinationFilePath = dlg.FileName;
            }
        }
    }
}
