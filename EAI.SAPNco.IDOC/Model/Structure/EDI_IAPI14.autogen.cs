
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_IAPI14
	/// </summary>
	public partial class EDI_IAPI14
	{

		/// <summary>
		/// Documentation: Name interner Struktur
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String STRNAME { get; set; }

		/// <summary>
		/// Documentation: Feldname
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String FIELDNAME { get; set; }

		/// <summary>
		/// Documentation: Unterer Wert / Einzelwert
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String FLDVALUE_L { get; set; }

		/// <summary>
		/// Documentation: Oberer Wert
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String FLDVALUE_H { get; set; }

		/// <summary>
		/// Documentation: Erläuternder Kurztext
		/// Type         : CHAR ( C), length 120, decimals 0
		/// </summary>
		public String DESCRP { get; set; }
	}
}

