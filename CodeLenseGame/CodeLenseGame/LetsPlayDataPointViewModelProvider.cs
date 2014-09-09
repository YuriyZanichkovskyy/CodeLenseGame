using Microsoft.VisualStudio.CodeSense.Editor;
using Microsoft.VisualStudio.Language.Intellisense;

namespace CodeLenseGame
{
    [DataPointViewModelProvider(typeof(LetsPlayDataPoint))]
    public class LetsPlayDataPointViewModelProvider : GlyphDataPointViewModelProvider<LetsPlayDataPointViewModel>
    {
        protected override LetsPlayDataPointViewModel GetViewModel(ICodeLensDataPoint dataPoint)
        {
            return new LetsPlayDataPointViewModel(dataPoint);
        }
    }
}