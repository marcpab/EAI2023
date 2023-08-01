
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_IAPI17
	/// </summary>
	public partial class EDI_IAPI17
	{

		/// <summary>
		/// Documentation: Nachrichtentyp
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String MESTYP { get; set; }

		/// <summary>
		/// Documentation: Kurzbeschreibung zum Objekt
		/// Type         : CHAR ( C), length 120, decimals 0
		/// </summary>
		public String DESCRP { get; set; }

		/// <summary>
		/// Documentation: Basistyp
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String IDOCTYP { get; set; }

		/// <summary>
		/// Documentation: Erweiterung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String CIMTYP { get; set; }

		/// <summary>
		/// Documentation: Release, zu der die Nachr.artzuordnung gültig ist
		/// Type         : CHAR ( C), length 8, decimals 0
		/// </summary>
		public String RELEASED { get; set; }
	}
}

