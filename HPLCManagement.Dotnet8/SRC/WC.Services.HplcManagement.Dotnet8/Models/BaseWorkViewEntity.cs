// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2023. All rights reserved.
// 
//   WC.Services.HplcManagement
//   BaseWorkViewEntity.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

namespace WC.Services.HplcManagement.Dotnet8.Models
{
    using System;
    using System.Text.Json.Serialization;

    public class BaseWorkViewEntity
    {
        [JsonIgnore]
        public long? ClassId { get; internal set; }

        /// <summary>
        /// Gets or internal sets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the base work view entity created by
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public string CreatedBy { get; internal set; }

        /// <summary>
        /// Gets or internal sets the base work view entity created date
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public DateTime CreatedDate { get; internal set; }

        /// <summary>
        /// Gets or internal sets the base work view entity identifier
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public long Id { get; internal set; }

        /// <summary>
        /// Gets or internal sets the base work view entity name
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets or internal sets the base work view entity revision by
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public string RevisionBy { get; internal set; }

        /// <summary>
        /// Gets or internal sets the base work view entity revision date
        /// </summary>
        /// <remarks>
        ///  This attribute is not submitted to the creation but will be returned when doing a
        ///  search or any get
        /// </remarks>
        public DateTime? RevisionDate { get; internal set; }
    }
}