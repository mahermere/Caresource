using Hyland.Unity;

namespace WC.Services.OnBase.Dotnet8.Repository
{
    public class OnBaseRepository : IRepository, IDisposable
    {
        private Application _onBaseApplication = null;
        public Application Application { get => _onBaseApplication; set => _onBaseApplication = value; }

        public void Dispose()
        {
            if (_onBaseApplication != null && _onBaseApplication.IsConnected)
            {
                _onBaseApplication.Disconnect();
                _onBaseApplication.Dispose();
            }

        }
    }
}
