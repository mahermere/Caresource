using Hyland.Unity;

namespace WC.Services.Document.Dotnet8.Authorization
{
    public abstract class OnBaseApplicationAbstractFactory
    {
        public abstract Application GetApplication(string key);
        public abstract Application GetApplication();
    }
}
