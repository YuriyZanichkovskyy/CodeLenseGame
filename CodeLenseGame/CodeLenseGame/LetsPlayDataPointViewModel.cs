using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CodeLenseGame
{
    public class LetsPlayDataPointViewModel : GlyphDataPointViewModel
    {
        public LetsPlayDataPointViewModel(ICodeLensDataPoint dataPoint)
            : base(dataPoint)
        {
            Descriptor = "Play";
            HasDetails = true;
            GameModel = new Game2048();
        }

        public Game2048 GameModel { get; private set; }

        public override ImageSource GlyphSource
        {
            get { return Icons.Game2048Icon; }
        }
    }
}
