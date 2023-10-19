namespace EAI.SAPNco.Utils.Model
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Description : FUNCTION RFC_READ_TABLE
    /// </summary>
    public partial class RFC_READ_TABLE
    {

        /// <summary>
        /// Documentation: Zeichen für Markierung von Feldgrenzen in DATA
        /// Direction    : IMPORT
        /// Type         : CHAR ( C), length 1, decimals 0
        /// Optional     : True
        /// DefaultValue : SPACE
        /// </summary>
        public string DELIMITER { get; set; }

        /// <summary>
        /// Documentation: Lesezugriff mit ORDER BY PRIMARY KEY
        /// Direction    : IMPORT
        /// Type         : CHAR ( C), length 1, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public string GET_SORTED { get; set; }

        /// <summary>
        /// Documentation: falls <> SPACE, wird nur FIELDS gefüllt
        /// Direction    : IMPORT
        /// Type         : CHAR ( C), length 1, decimals 0
        /// Optional     : True
        /// DefaultValue : SPACE
        /// </summary>
        public string NO_DATA { get; set; }

        /// <summary>
        /// Documentation: Tabelle, aus der gelesen wird
        /// Direction    : IMPORT
        /// Type         : CHAR ( C), length 30, decimals 0
        /// Optional     : False
        /// DefaultValue : 
        /// </summary>
        public string QUERY_TABLE { get; set; }

        /// <summary>
        /// Documentation: Maximale Trefferanzahl
        /// Direction    : IMPORT
        /// Type         : INT4 ( I), length 4, decimals 0
        /// Optional     : True
        /// DefaultValue : 0
        /// </summary>
        public int ROWCOUNT { get; set; }

        /// <summary>
        /// Documentation: Erste <rowskips> Treffer überspringen
        /// Direction    : IMPORT
        /// Type         : INT4 ( I), length 4, decimals 0
        /// Optional     : True
        /// DefaultValue : 0
        /// </summary>
        public int ROWSKIPS { get; set; }

        /// <summary>
        /// Documentation: ET_DATA anstatt DATA für Rückgabe nutzen
        /// Direction    : IMPORT
        /// Type         : CHAR ( C), length 1, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public string USE_ET_DATA_4_RETURN { get; set; }

        /// <summary>
        /// Documentation: Gelesene Daten (512 Zeichen je Zeile)
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<TAB512> DATA { get; set; }

        /// <summary>
        /// Documentation: Namen (in) und Struktur (out) gelesener Felder
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<RFC_DB_FLD> FIELDS { get; set; }

        /// <summary>
        /// Documentation: Selektionsangaben, "WHERE-Klauseln" (in)
        /// Direction    : TABLES
        /// Type         : TABLE ( h), length 8, decimals 0
        /// Optional     : True
        /// DefaultValue : 
        /// </summary>
        public List<RFC_DB_OPT> OPTIONS { get; set; }
    }
}

