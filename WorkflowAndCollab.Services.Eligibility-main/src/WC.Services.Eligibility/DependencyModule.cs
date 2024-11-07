using CareSource.WC.Core.DependencyInjection.Interfaces;
using CareSource.WC.Entities.Members;

namespace CareSource.WC.Services.Eligibility
{
    using CareSource.WC.Core.DependencyInjection;
    using CareSource.WC.Services.Eligibility.Managers;
    using CareSource.WC.Services.Eligibility.Managers.Interfaces;
    using CareSource.WC.Services.Eligibility.Adapters;
    using CareSource.WC.Services.Eligibility.Adapters.Interfaces;
    using CareSource.WC.Entities.Eligibility;

    public class DependencyModule : IDependencyModule
	{
		public ushort LoadOrder => 500;

		public void Load(DependencyCollection dependencyCollection, IEnvironmentContext environmentContext)
		{
            dependencyCollection.AddSingletonDependency<IMemberManager<Member>, MemberManager>();
            dependencyCollection.AddSingletonDependency<IMemberAdapter<Member>, FacetsFXIServiceMemberAdapter>();
            dependencyCollection.AddSingletonDependency<IEligibilityManager<Eligibility>, EligibilityManager>();
            dependencyCollection.AddSingletonDependency<IEligibilityAdapter<Eligibility>, FacetsFXIServiceEligibilityAdapter>();
        }
	}
}
