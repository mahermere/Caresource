﻿using System;

namespace WC.Services.ImportProcessor.Dotnet8.Connection.Interfaces
{
    public interface IApplicationConnectionAdapter<out TApplicationModel> : IDisposable
    {
        TApplicationModel Application { get; }

        bool IsConnected { get; }

        TApplicationModel Connect();

        TApplicationModel Connect(string username, string password);

        void Disconnect();
    }
}
