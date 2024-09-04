// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2021. All rights reserved.
// 
//   WC.Services.Hplc
//   WorkViewStateClassState.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using WC.Services.Hplc.Dotnet8.Models;

namespace WC.Services.Hplc.Dotnet8.Mappers
{

    public class WorkViewStateClassState
        : BaseWorkViewObjectModelMapper<State>
    {
        public override State GetMappedModel(WorkViewObject original)
        {
            State state = base.GetMappedModel(original);

            state.Name = GetStringValue(Constants.State.Name);
            state.Abbreviation = GetStringValue(Constants.State.Abbreviation);

            return state;
        }
    }
}