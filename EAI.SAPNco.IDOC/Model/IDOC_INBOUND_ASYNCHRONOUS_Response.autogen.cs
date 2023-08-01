
namespace EAI.SAPNco.IDOC.Model
{
    using EAI.SAPNco.IDOC.Model.Structure;
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// </summary>
	public partial class IDOC_INBOUND_ASYNCHRONOUS_Response
	{

		/// <summary>
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : False
		/// DefaultValue : 
		/// </summary>
		public List<EDI_DC40> IDOC_CONTROL_REC_40 { get; set; }

		/// <summary>
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : False
		/// DefaultValue : 
		/// </summary>
		public List<EDI_DD40> IDOC_DATA_REC_40 { get; set; }
	}
}

