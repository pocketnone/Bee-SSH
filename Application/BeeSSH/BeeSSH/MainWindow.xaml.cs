using System.Windows;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;
using BeeSSH.DiscordRPC;

namespace BeeSSH
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
            
            InitializeComponent();
            DiscordRPC_Manager.RunDiscordRPC();

        }
    }
}
