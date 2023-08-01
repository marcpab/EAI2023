
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_DD40
	/// </summary>
	public partial class EDI_DD40
	{

		/// <summary>
		/// Documentation: Segment (externer Name)
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String SEGNAM { get; set; }

		/// <summary>
		/// Documentation: Mandant
		/// Type         : CHAR ( C), length 6, decimals 0
		/// </summary>
		public String MANDT { get; set; }

		/// <summary>
		/// Documentation: Nummer des IDocs
		/// Type         : CHAR ( C), length 32, decimals 0
		/// </summary>
		public String DOCNUM { get; set; }

		/// <summary>
		/// Documentation: Segmentnummer
		/// Type         : CHAR ( C), length 12, decimals 0
		/// </summary>
		public String SEGNUM { get; set; }

		/// <summary>
		/// Documentation: Nummer des übergeordneten Elternsegments
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public String PSGNUM { get; set; }

		/// <summary>
		/// Documentation: Hierarchieebene des SAP-Segmentes
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String HLEVEL { get; set; }

		/// <summary>
		/// Documentation: Anwendungsdaten
		/// Type         : CHAR ( C), length 2000, decimals 0
		/// </summary>
		public String SDATA { get; set; }
	}
}

