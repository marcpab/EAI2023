
namespace EAI.SAPNco.IDOC.Model
{
    using EAI.SAPNco.IDOC.Model.Structure;
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// </summary>
	public partial class IDOCTYPE_READ_COMPLETE
	{

		/// <summary>
		/// Documentation: IDoc-Entwicklung: Anwendungsrelease der Segmentdefinition
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 10, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public String PI_APPLREL { get; set; }

		/// <summary>
		/// Documentation: Erweiterung
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 30, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public String PI_CIMTYP { get; set; }

		/// <summary>
		/// Documentation: Basistyp
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 30, decimals 0
		/// Optional     : False
		/// DefaultValue : 
		/// </summary>
		public String PI_IDOCTYP { get; set; }

		/// <summary>
		/// Documentation: nicht freigegebene Segmentdefinition verwenden
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 1, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public String PI_READ_UNREL { get; set; }

		/// <summary>
		/// Documentation: Release
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 4, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public String PI_RELEASE { get; set; }

		/// <summary>
		/// Documentation: Version der IDoc-Satzarten
		/// Direction    : IMPORT
		/// Type         : CHAR ( C), length 1, decimals 0
		/// Optional     : True
		/// DefaultValue : '3'
		/// </summary>
		public String PI_VERSION { get; set; }

		/// <summary>
		/// Documentation: Struktur der Segemte des IDoctypen (die Felder)
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public List<EDI_IAPI12> PT_FIELDS { get; set; }

		/// <summary>
		/// Documentation: zulässige Werte für die Segmentfelder
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public List<EDI_IAPI14> PT_FVALUES { get; set; }

		/// <summary>
		/// Documentation: verknüpfte logische Nachrichten
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public List<EDI_IAPI17> PT_MESSAGES { get; set; }

		/// <summary>
		/// Documentation: Struktur des IDoctypen (die Segmente)
		/// Direction    : TABLES
		/// Type         : TABLE ( h), length 8, decimals 0
		/// Optional     : True
		/// DefaultValue : 
		/// </summary>
		public List<EDI_IAPI11> PT_SEGMENTS { get; set; }
	}
}

