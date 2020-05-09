using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTool.Common
{
    public class LanguageIcon
    {
        public static Dictionary<string, PackIconKind> IconPack = new Dictionary<string, PackIconKind>
        {
            { "cs", PackIconKind.LanguageCsharp },
            { "xaml", PackIconKind.LanguageXaml },
            { "xml", PackIconKind.LanguageXaml },
            { "h", PackIconKind.LanguageC },
            { "c", PackIconKind.LanguageC },
            { "cpp", PackIconKind.LanguageCpp },
            { "jsp", PackIconKind.LanguageJavascript },
            { "json", PackIconKind.CodeJson },
            { "aspx", PackIconKind.LanguageCsharp },
            { "asp", PackIconKind.LanguageCsharp },
            { "php", PackIconKind.LanguagePhp },
            { "css", PackIconKind.LanguageCss3 },
            { "py", PackIconKind.LanguagePython },
            { "go", PackIconKind.LanguageGo },
            { "java", PackIconKind.LanguageJava },
            { "lua", PackIconKind.LanguageLua }
        };
    }
}
