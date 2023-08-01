namespace EAI.SAPNco.IDOC.Metadata
{
    public enum DataTypeEnum
    {
        ACCP = 1 , // Posting period YYYYMM
        CHAR = 2 , // Character String
        CLNT = 3 , // Client
        CUKY = 4 , // Currency key, referenced by CURR fields
        CURR = 5 , // Currency field, stored as DEC
        D16D = 6 , // Decimal Floating Point, 16 Digits, DEC on Database
        D16R = 7 , // Decimal Floating Point, 16 Digits, RAW on Database
        D16S = 8 , // Decimal Floating Point. 16 Digits, with Scale Field
        D34D = 9 , // Decimal Floating Point, 34 Digits, DEC on Database
        D34R = 10, // Decimal Floating Point, 34 Digits, RAW on Database
        D34S = 11, // Decimal Floating Point, 34 Digits, with Scale Field
        DATS = 12, // Date field (YYYYMMDD) stored as char(8)
        DEC  = 13, // Counter or amount field with comma and sign
        FLTP = 14, // Floating point number, accurate to 8 bytes
        INT1 = 15, //  	 	1-byte integer, integer number <= 255
        INT2 = 16, //  	 	2-byte integer, only for length field before LCHR or LRAW
        INT4 = 17, //  	 	4-byte integer, integer number with sign
        LANG = 18, // Language key
        LCHR = 19, // Long character string, requires preceding INT2 field
        LRAW = 20, // Long byte string, requires preceding INT2 field
        NUMC = 21, // Character string with only digits
        PREC = 22, // Obsolete data type, do not use
        QUAN = 23, // Quantity field, points to a unit field with format UNIT
        RAW  = 24, // Uninterpreted sequence of bytes
        RSTR = 25, // Byte String of Variable Length
        SSTR = 26, // Short Character String of Variable Length
        STRG = 27, // Character String of Variable Length
        TIMS = 28, // Time field(hhmmss), stored as char (6)
        UNIT = 29, // Unit key for QUAN fields
        VARC = 30, // Long character string, no longer supported from Rel. 3.0

        Unknown = 99
    }
}
