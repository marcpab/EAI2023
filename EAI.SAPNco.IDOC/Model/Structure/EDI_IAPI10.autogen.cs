
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_IAPI10
	/// </summary>
	public partial class EDI_IAPI10
	{

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
		/// Documentation: Art des Entwicklungsobjekts
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String STRUCTTYPE { get; set; }

		/// <summary>
		/// Documentation: Kurzbeschreibung zum Objekt
		/// Type         : CHAR ( C), length 120, decimals 0
		/// </summary>
		public String DESCRP { get; set; }

		/// <summary>
		/// Documentation: Vorgänger eines Basistyps oder einer Erweiterung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String PRETYP { get; set; }

		/// <summary>
		/// Documentation: Freigaberelease
		/// Type         : CHAR ( C), length 8, decimals 0
		/// </summary>
		public String RELEASED { get; set; }

		/// <summary>
		/// Documentation: Verantwortliche Person
		/// Type         : CHAR ( C), length 24, decimals 0
		/// </summary>
		public String PRESP { get; set; }

		/// <summary>
		/// Documentation: Bearbeitende Person
		/// Type         : CHAR ( C), length 24, decimals 0
		/// </summary>
		public String PWORK { get; set; }

		/// <summary>
		/// Documentation: Letzte Änderung: Person
		/// Type         : CHAR ( C), length 24, decimals 0
		/// </summary>
		public String PLAST { get; set; }

		/// <summary>
		/// Documentation: Anwendungsfreigaberelease des Basistyps
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String APPLREL { get; set; }
	}
}

