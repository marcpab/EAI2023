
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_IAPI11
	/// </summary>
	public partial class EDI_IAPI11
	{

		/// <summary>
		/// Documentation: Laufende Nummer des Segments in IDoc-Typ
		/// Type         : NUM ( N), length 8, decimals 0
		/// </summary>
		public Decimal NR { get; set; }

		/// <summary>
		/// Documentation: Segmenttyp in 30stelliger Darstellung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String SEGMENTTYP { get; set; }

		/// <summary>
		/// Documentation: IDoc Entwicklung : Segmentdefinition
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String SEGMENTDEF { get; set; }

		/// <summary>
		/// Documentation: Kennzeichen: qualifiziertes Segment im IDoc?
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String QUALIFIER { get; set; }

		/// <summary>
		/// Documentation: Länge eines Feldes (Zahl der Stellen)
		/// Type         : NUM ( N), length 8, decimals 0
		/// </summary>
		public Decimal SEGLEN { get; set; }

		/// <summary>
		/// Documentation: Segmenttyp in 30stelliger Darstellung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String PARSEG { get; set; }

		/// <summary>
		/// Documentation: Laufende Nummer des Elternsegments
		/// Type         : NUM ( N), length 8, decimals 0
		/// </summary>
		public Decimal PARPNO { get; set; }

		/// <summary>
		/// Documentation: Kennz. Elternsegment: Segment ist Beginn einer Seg.Gruppe
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String PARFLG { get; set; }

		/// <summary>
		/// Documentation: Kennzeichen: muß vorkommen!
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String MUSTFL { get; set; }

		/// <summary>
		/// Documentation: Minimale Anzahl von Segmenten in Folge
		/// Type         : NUM ( N), length 20, decimals 0
		/// </summary>
		public Decimal OCCMIN { get; set; }

		/// <summary>
		/// Documentation: Maximale Anzahl von Segmenten in Folge
		/// Type         : NUM ( N), length 20, decimals 0
		/// </summary>
		public Decimal OCCMAX { get; set; }

		/// <summary>
		/// Documentation: Hierarchieebene des Segments in Zwischenstruktur
		/// Type         : NUM ( N), length 4, decimals 0
		/// </summary>
		public Decimal HLEVEL { get; set; }

		/// <summary>
		/// Documentation: Kurzbeschreibung zum Objekt
		/// Type         : CHAR ( C), length 120, decimals 0
		/// </summary>
		public String DESCRP { get; set; }

		/// <summary>
		/// Documentation: Kennzeichen für Gruppen: Muß vorkommen
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String GRP_MUSTFL { get; set; }

		/// <summary>
		/// Documentation: Minimale Anzahl von Gruppen in Folge
		/// Type         : NUM ( N), length 20, decimals 0
		/// </summary>
		public Decimal GRP_OCCMIN { get; set; }

		/// <summary>
		/// Documentation: Maximale Anzahl von Gruppen in Folge
		/// Type         : NUM ( N), length 20, decimals 0
		/// </summary>
		public Decimal GRP_OCCMAX { get; set; }

		/// <summary>
		/// Documentation: Segmenttyp in 30stelliger Darstellung
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String REFSEGTYP { get; set; }
	}
}

