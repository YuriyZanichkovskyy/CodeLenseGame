using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using CodeLenseGame.Enums;

namespace CodeLenseGame
{
    /// <summary>
    /// Interaction logic for GameView.xaml
    /// </summary>
    public partial class GameView : UserControl
    {
        public GameView()
        {
            InitializeComponent();
            Loaded += GameView_Loaded;
        }

        void GameView_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkElement element = FindVisualParent(this, el => el.GetType().Name == "NonLogicalAdornerDecorator");
            if (element != null)
            {
                element.MaxHeight = 500;
                element.MaxWidth = 500;
            }
        }

        public LetsPlayDataPointViewModel ViewModel
        {
            get { return DataContext as LetsPlayDataPointViewModel; }
        }

        public static FrameworkElement FindVisualParent(FrameworkElement child, Func<FrameworkElement, bool> found)
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            Debug.WriteLine(parentObject);

            if (parentObject == null) return null;

            if (found((FrameworkElement) parentObject))
            {
                return (FrameworkElement) parentObject;
            }
            else
            {
                return FindVisualParent((FrameworkElement) parentObject, found);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.Key)
            {
                    case Key.Left:
                    ViewModel.GameModel.PerformMove(MoveDirection.Left);
                    e.Handled = true;
                    break;
                    case Key.Right:
                    ViewModel.GameModel.PerformMove(MoveDirection.Right);
                    e.Handled = true;
                    break;
                    case Key.Up:
                    ViewModel.GameModel.PerformMove(MoveDirection.Up);
                    e.Handled = true;
                    break;
                    case Key.Down:
                    ViewModel.GameModel.PerformMove(MoveDirection.Down);
                    e.Handled = true;
                    break;
            }
        }
    }
}
