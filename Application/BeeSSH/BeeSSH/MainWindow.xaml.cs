using System.Windows;
using Syncfusion.SfSkinManager;
using Syncfusion.Windows.Shared;

namespace BeeSSH
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ChromelessWindow
    {
        public MainWindow()
        {
            SfSkinManager.SetTheme(this, new Theme("MaterialDark", new string[] { "ButtonAdv", "ChromelessWindow" }));
            InitializeComponent();
        }
    }
}
