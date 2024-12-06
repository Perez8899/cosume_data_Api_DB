using System.Globalization;

namespace MVCClaseSeis
{
    internal class RequestLocalizationOptions
    {
        public object DefaultRequestCulture { get; set; }
        public CultureInfo[] SupportedCultures { get; set; }
        public CultureInfo[] SupportedUICultures { get; set; }
    }
}