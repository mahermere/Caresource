using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CareSource.WC.Services.CreateGrievanceAppeals
{
	using CareSource.WC.OnBase.Core.Configuration;
	using CareSource.WC.OnBase.Core.DependencyInjection;
	using CareSource.WC.OnBase.Core.Http;
	using CareSource.WC.OnBase.Core.Http.Interfaces;
	using CareSource.WC.Services.CreateGrievanceAppeals.Manager;
	using Unity;

	public class DependencyModule : IDependencyModule
	{
		public void Load(
			IUnityContainer container)
		{
			log4net.Config.XmlConfigurator.Configure();

			container.RegisterType(
				typeof(GrievanceAppealManager),
				typeof(GrievanceAppealManager));
		}

		public ushort LoadOrder => 500;
	}
}