using AsyncFileCopy.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncFileCopy
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            long totalCopied = 0;

            using ( FileStream sourceFileStream = new FileStream( this.tbxSource.Text, FileMode.Open ) )
            {
                using ( FileStream destFileStream = new FileStream( this.tbxDest.Text, FileMode.Create ) )
                {
                    byte[] buffer = new byte[1024 * 1024];
                    int nRead = 0;

                    while ( ( nRead = await sourceFileStream.ReadAsync( buffer, 0, buffer.Length) ) != 0 )
                    {
                        await destFileStream.WriteAsync( buffer, 0, nRead );
                        totalCopied += nRead;

                        this.Progressbar.Value = (int)( ( (double)totalCopied / (double)sourceFileStream.Length ) * 100 );
                    }
                }
            }
        }
    }
}
