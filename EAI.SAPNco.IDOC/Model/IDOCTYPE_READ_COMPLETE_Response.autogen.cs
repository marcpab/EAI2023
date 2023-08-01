
namespace EAI.SAPNco.IDOC.Model
{
    using EAI.SAPNco.IDOC.Model.Structure;
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// </summary>
	public partial class IDOCTYPE_READ_COMPLETE_Response
	{

		/// <summary>
		/// Documentation: Kopfdaten des IDoctypen
		/// Direction    : EXPORT
		/// Type         : STRUCTURE ( u or v), length 201, decimals 0
		/// Optional     : False
		/// DefaultValue : 
		/// </summary>
		public EDI_IAPI10 PE_HEADER { get; set; }

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

