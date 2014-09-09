using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.CodeSense.Roslyn;
using Microsoft.VisualStudio.Language.Intellisense;
using Microsoft.VisualStudio.Utilities;

namespace CodeLenseGame
{
    [Name("Play2048")]
    [Export(typeof(ICodeLensDataPointProvider))]
    public class LetsPlayDataPointProvider : ICodeLensDataPointProvider
    {
        public bool CanCreateDataPoint(ICodeLensDescriptor descriptor)
        {
            var elementDescriptor = descriptor as ICodeElementDescriptor;
            if (elementDescriptor != null && elementDescriptor.Kind == SyntaxNodeKind.Method)
            {
                var methodDeclaration = elementDescriptor.SyntaxNode as MethodDeclarationSyntax;
                return methodDeclaration != null && methodDeclaration.Identifier.Value != null
                       && methodDeclaration.Identifier.ValueText.Contains("2048");
            }

            return false;
        }

        public ICodeLensDataPoint CreateDataPoint(ICodeLensDescriptor descriptor)
        {
            return new LetsPlayDataPoint();
        }
    }
}
