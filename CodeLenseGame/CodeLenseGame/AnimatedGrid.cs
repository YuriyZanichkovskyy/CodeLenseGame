using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CodeLenseGame
{
    public class AnimatedGrid : Grid
    {
        protected override Size ArrangeOverride(Size arrangeSize)
        {
            foreach (var element in Children)
            {
                //get previous positions, and after arrange apply transform
            }
            return base.ArrangeOverride(arrangeSize);
        }
    }
}
