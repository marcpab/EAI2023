
namespace EAI.SAPNco.IDOC.Model.Structure
{
    using System;
	using System.Collections.Generic;

	/// <summary>
	/// Rfc structure : EDI_DC40
	/// </summary>
	public partial class EDI_DC40
	{

		/// <summary>
		/// Documentation: Name der Tabellenstruktur
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String TABNAM { get; set; }

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
		/// Documentation: SAP-Release des IDocs
		/// Type         : CHAR ( C), length 8, decimals 0
		/// </summary>
		public String DOCREL { get; set; }

		/// <summary>
		/// Documentation: Status des IDocs
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String STATUS { get; set; }

		/// <summary>
		/// Documentation: Richtung
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String DIRECT { get; set; }

		/// <summary>
		/// Documentation: Ausgabemodus
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String OUTMOD { get; set; }

		/// <summary>
		/// Documentation: Übersteuerung im Eingang
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String EXPRSS { get; set; }

		/// <summary>
		/// Documentation: Testkennzeichen
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String TEST { get; set; }

		/// <summary>
		/// Documentation: Name des Basistyps
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String IDOCTYP { get; set; }

		/// <summary>
		/// Documentation: Erweiterung (vom Kunden definiert)
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String CIMTYP { get; set; }

		/// <summary>
		/// Documentation: Nachrichtentyp
		/// Type         : CHAR ( C), length 60, decimals 0
		/// </summary>
		public String MESTYP { get; set; }

		/// <summary>
		/// Documentation: Nachrichtenvariante
		/// Type         : CHAR ( C), length 6, decimals 0
		/// </summary>
		public String MESCOD { get; set; }

		/// <summary>
		/// Documentation: Nachrichtenfunktion
		/// Type         : CHAR ( C), length 6, decimals 0
		/// </summary>
		public String MESFCT { get; set; }

		/// <summary>
		/// Documentation: EDI-Standard, Kennzeichen
		/// Type         : CHAR ( C), length 2, decimals 0
		/// </summary>
		public String STD { get; set; }

		/// <summary>
		/// Documentation: EDI-Standard, Version und Release
		/// Type         : CHAR ( C), length 12, decimals 0
		/// </summary>
		public String STDVRS { get; set; }

		/// <summary>
		/// Documentation: EDI-Nachrichtentyp
		/// Type         : CHAR ( C), length 12, decimals 0
		/// </summary>
		public String STDMES { get; set; }

		/// <summary>
		/// Documentation: Absenderport (SAP-System, externes Subsystem)
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String SNDPOR { get; set; }

		/// <summary>
		/// Documentation: Partnerart des Absenders
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String SNDPRT { get; set; }

		/// <summary>
		/// Documentation: Partnerrolle des Absenders
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String SNDPFC { get; set; }

		/// <summary>
		/// Documentation: Partnernummer des Absenders
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String SNDPRN { get; set; }

		/// <summary>
		/// Documentation: Absenderadresse (SADR)
		/// Type         : CHAR ( C), length 42, decimals 0
		/// </summary>
		public String SNDSAD { get; set; }

		/// <summary>
		/// Documentation: Logische Adresse des Absenders
		/// Type         : CHAR ( C), length 140, decimals 0
		/// </summary>
		public String SNDLAD { get; set; }

		/// <summary>
		/// Documentation: Empfängerport
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String RCVPOR { get; set; }

		/// <summary>
		/// Documentation: Partnerart des Empfängers
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String RCVPRT { get; set; }

		/// <summary>
		/// Documentation: Partnerrolle des Empfängers
		/// Type         : CHAR ( C), length 4, decimals 0
		/// </summary>
		public String RCVPFC { get; set; }

		/// <summary>
		/// Documentation: Partnernummer des Empfängers
		/// Type         : CHAR ( C), length 20, decimals 0
		/// </summary>
		public String RCVPRN { get; set; }

		/// <summary>
		/// Documentation: Empfängeradresse (SADR)
		/// Type         : CHAR ( C), length 42, decimals 0
		/// </summary>
		public String RCVSAD { get; set; }

		/// <summary>
		/// Documentation: Logische Adresse des Empfängers
		/// Type         : CHAR ( C), length 140, decimals 0
		/// </summary>
		public String RCVLAD { get; set; }

		/// <summary>
		/// Documentation: Erstellungsdatum
		/// Type         : DATE ( D), length 16, decimals 0
		/// </summary>
		public DateTimeOffset CREDAT { get; set; }

		/// <summary>
		/// Documentation: Erstellungsuhrzeit
		/// Type         : TIME ( T), length 12, decimals 0
		/// </summary>
		public DateTimeOffset CRETIM { get; set; }

		/// <summary>
		/// Documentation: Übertragungsdatei (EDI Interchange)
		/// Type         : CHAR ( C), length 28, decimals 0
		/// </summary>
		public String REFINT { get; set; }

		/// <summary>
		/// Documentation: Nachrichtengruppe (EDI Message Group)
		/// Type         : CHAR ( C), length 28, decimals 0
		/// </summary>
		public String REFGRP { get; set; }

		/// <summary>
		/// Documentation: Nachricht (EDI Message)
		/// Type         : CHAR ( C), length 28, decimals 0
		/// </summary>
		public String REFMES { get; set; }

		/// <summary>
		/// Documentation: Schlüssel des externen Nachrichtenarchivs
		/// Type         : CHAR ( C), length 140, decimals 0
		/// </summary>
		public String ARCKEY { get; set; }

		/// <summary>
		/// Documentation: Serialisierung
		/// Type         : CHAR ( C), length 40, decimals 0
		/// </summary>
		public String SERIAL { get; set; }
	}
}

