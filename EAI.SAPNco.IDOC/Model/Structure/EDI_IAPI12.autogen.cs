
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_IAPI12
	/// </summary>
	public partial class EDI_IAPI12
	{

		/// <summary>
		/// Documentation: Segmenttyp in 30stelliger Darstellung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String SEGMENTTYP { get; set; }

		/// <summary>
		/// Documentation: Feldname
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String FIELDNAME { get; set; }

		/// <summary>
		/// Documentation: Interne Länge in Bytes
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public Decimal INTLEN { get; set; }

		/// <summary>
		/// Documentation: Ausgabelänge
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public Decimal EXTLEN { get; set; }

		/// <summary>
		/// Documentation: Positionsnummer des Felds
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public Decimal FIELD_POS { get; set; }

		/// <summary>
		/// Documentation: Position des ersten Bytes
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public Decimal BYTE_FIRST { get; set; }

		/// <summary>
		/// Documentation: Position des letzten Bytes
		/// Type         : NUM ( N), length 12, decimals 0
		/// </summary>
		public Decimal BYTE_LAST { get; set; }

		/// <summary>
		/// Documentation: Datenelement (semantische Domäne)
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String ROLLNAME { get; set; }

		/// <summary>
		/// Documentation: Bezeichnung einer Domäne
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String DOMNAME { get; set; }

		/// <summary>
		/// Documentation: R/3-DD: Dynprodatentyp fuer Screen-Painter
		/// Type         : CHAR ( C), length 8, decimals 0
		/// </summary>
		public String DATATYPE { get; set; }

		/// <summary>
		/// Documentation: Kurzbeschreibung zum Objekt
		/// Type         : CHAR ( C), length 120, decimals 0
		/// </summary>
		public String DESCRP { get; set; }

		/// <summary>
		/// Documentation: IDoc Entwicklung : Kennzeichen ISO-Code in Feld
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String ISOCODE { get; set; }

		/// <summary>
		/// Documentation: Wertetabelle für IDoc-Segmentfeld
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String VALUETAB { get; set; }
	}
}

