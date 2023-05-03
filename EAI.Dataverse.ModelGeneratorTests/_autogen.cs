using System;
using EAI.OData;
namespace EAI.Dataverse.ModelGeneratorTests.Model
{
	/// <summary>
	/// Display name: Account
	/// Description : Business that represents a customer or potential customer. The company that is billed in business transactions.
	/// </summary>
	public partial class account
	{
		public const string EntitySet = "accounts";

		public ODataType ODataType { get => new ODataType() { Name = "Microsoft.Dynamics.CRM.Account" }; }

		public static ODataBind ToLookup(Guid? guid) => guid == null ? null : new ODataBind() {EntityName = EntitySet, EntityId = guid.Value};

		public ODataBind ToLookup() => ToLookup(accountid);


		public enum accountcategorycodeEnum {
			PreferredCustomer = 1,
			Standard = 2,
		}

		/// <summary>
		/// Display name: Category
		/// Description : Select a category to indicate whether the customer account is standard or preferred.
		/// Picklist, length -1
		/// </summary>
		public accountcategorycodeEnum? accountcategorycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: accountcategorycode
		/// Virtual, length -1
		/// </summary>
		public string accountcategorycodename { get; set; }  // Virtual, length -1

		public enum accountclassificationcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Classification
		/// Description : Select a classification code to indicate the potential value of the customer account based on the projected return on investment, cooperation level, sales cycle length or other criteria.
		/// Picklist, length -1
		/// </summary>
		public accountclassificationcodeEnum? accountclassificationcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: accountclassificationcode
		/// Virtual, length -1
		/// </summary>
		public string accountclassificationcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Account
		/// Description : Unique identifier of the account.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? accountid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Account Number
		/// Description : Type an ID number or code for the account to quickly search and identify the account in system views.
		/// String, length 20
		/// </summary>
		public string accountnumber { get; set; }  // String, length 20

		public enum accountratingcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Account Rating
		/// Description : Select a rating to indicate the value of the customer account.
		/// Picklist, length -1
		/// </summary>
		public accountratingcodeEnum? accountratingcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: accountratingcode
		/// Virtual, length -1
		/// </summary>
		public string accountratingcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: ID
		/// Description : Unique identifier for address 1.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? address1_addressid { get; set; }  // Uniqueidentifier, length -1

		public enum address1_addresstypecodeEnum {
			BillTo = 1,
			ShipTo = 2,
			Primary = 3,
			Other = 4,
		}

		/// <summary>
		/// Display name: Address 1: Address Type
		/// Description : Select the primary address type.
		/// Picklist, length -1
		/// </summary>
		public address1_addresstypecodeEnum? address1_addresstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_addresstypecode
		/// Virtual, length -1
		/// </summary>
		public string address1_addresstypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: City
		/// Description : Type the city for the primary address.
		/// String, length 80
		/// </summary>
		public string address1_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 1
		/// Description : Shows the complete primary address.
		/// Memo, length 1000
		/// </summary>
		public string address1_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Address 1: Country/Region
		/// Description : Type the country or region for the primary address.
		/// String, length 80
		/// </summary>
		public string address1_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 1: County
		/// Description : Type the county for the primary address.
		/// String, length 50
		/// </summary>
		public string address1_county { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Fax
		/// Description : Type the fax number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_fax { get; set; }  // String, length 50

		public enum address1_freighttermscodeEnum {
			FOB = 1,
			NoCharge = 2,
		}

		/// <summary>
		/// Display name: Address 1: Freight Terms
		/// Description : Select the freight terms for the primary address to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public address1_freighttermscodeEnum? address1_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string address1_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: Latitude
		/// Description : Type the latitude value for the primary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address1_latitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 1: Street 1
		/// Description : Type the first line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Street 2
		/// Description : Type the second line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Street 3
		/// Description : Type the third line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Longitude
		/// Description : Type the longitude value for the primary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address1_longitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 1: Name
		/// Description : Type a descriptive name for the primary address, such as Corporate Headquarters.
		/// String, length 200
		/// </summary>
		public string address1_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Address 1: ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the primary address.
		/// String, length 20
		/// </summary>
		public string address1_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 1: Post Office Box
		/// Description : Type the post office box number of the primary address.
		/// String, length 20
		/// </summary>
		public string address1_postofficebox { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 1: Primary Contact Name
		/// Description : Type the name of the main contact at the account's primary address.
		/// String, length 100
		/// </summary>
		public string address1_primarycontactname { get; set; }  // String, length 100

		public enum address1_shippingmethodcodeEnum {
			Airborne = 1,
			DHL = 2,
			FedEx = 3,
			UPS = 4,
			PostalMail = 5,
			FullLoad = 6,
			WillCall = 7,
		}

		/// <summary>
		/// Display name: Address 1: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public address1_shippingmethodcodeEnum? address1_shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string address1_shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: State/Province
		/// Description : Type the state or province of the primary address.
		/// String, length 50
		/// </summary>
		public string address1_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address Phone
		/// Description : Type the main phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Telephone 2
		/// Description : Type a second phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Telephone 3
		/// Description : Type a third phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone3 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: UPS Zone
		/// Description : Type the UPS zone of the primary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
		/// String, length 4
		/// </summary>
		public string address1_upszone { get; set; }  // String, length 4
		/// <summary>
		/// Display name: Address 1: UTC Offset
		/// Description : Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
		/// Integer, length -1
		/// </summary>
		public int? address1_utcoffset { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Address 2: ID
		/// Description : Unique identifier for address 2.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? address2_addressid { get; set; }  // Uniqueidentifier, length -1

		public enum address2_addresstypecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Address Type
		/// Description : Select the secondary address type.
		/// Picklist, length -1
		/// </summary>
		public address2_addresstypecodeEnum? address2_addresstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_addresstypecode
		/// Virtual, length -1
		/// </summary>
		public string address2_addresstypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: City
		/// Description : Type the city for the secondary address.
		/// String, length 80
		/// </summary>
		public string address2_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 2
		/// Description : Shows the complete secondary address.
		/// Memo, length 1000
		/// </summary>
		public string address2_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Address 2: Country/Region
		/// Description : Type the country or region for the secondary address.
		/// String, length 80
		/// </summary>
		public string address2_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 2: County
		/// Description : Type the county for the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_county { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Fax
		/// Description : Type the fax number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_fax { get; set; }  // String, length 50

		public enum address2_freighttermscodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Freight Terms
		/// Description : Select the freight terms for the secondary address to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public address2_freighttermscodeEnum? address2_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string address2_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: Latitude
		/// Description : Type the latitude value for the secondary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address2_latitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 2: Street 1
		/// Description : Type the first line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Street 2
		/// Description : Type the second line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Street 3
		/// Description : Type the third line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Longitude
		/// Description : Type the longitude value for the secondary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address2_longitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 2: Name
		/// Description : Type a descriptive name for the secondary address, such as Corporate Headquarters.
		/// String, length 200
		/// </summary>
		public string address2_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Address 2: ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the secondary address.
		/// String, length 20
		/// </summary>
		public string address2_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 2: Post Office Box
		/// Description : Type the post office box number of the secondary address.
		/// String, length 20
		/// </summary>
		public string address2_postofficebox { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 2: Primary Contact Name
		/// Description : Type the name of the main contact at the account's secondary address.
		/// String, length 100
		/// </summary>
		public string address2_primarycontactname { get; set; }  // String, length 100

		public enum address2_shippingmethodcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public address2_shippingmethodcodeEnum? address2_shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string address2_shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: State/Province
		/// Description : Type the state or province of the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 1
		/// Description : Type the main phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 2
		/// Description : Type a second phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 3
		/// Description : Type a third phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone3 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: UPS Zone
		/// Description : Type the UPS zone of the secondary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
		/// String, length 4
		/// </summary>
		public string address2_upszone { get; set; }  // String, length 4
		/// <summary>
		/// Display name: Address 2: UTC Offset
		/// Description : Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
		/// Integer, length -1
		/// </summary>
		public int? address2_utcoffset { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Created By (IP Address)
		/// String, length 100
		/// </summary>
		public string adx_createdbyipaddress { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created By (User Name)
		/// String, length 100
		/// </summary>
		public string adx_createdbyusername { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified By (IP Address)
		/// String, length 100
		/// </summary>
		public string adx_modifiedbyipaddress { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified By (User Name)
		/// String, length 100
		/// </summary>
		public string adx_modifiedbyusername { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Aging 30
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging30 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 30 (Base)
		/// Description : The base currency equivalent of the aging 30 field.
		/// Money, length -1
		/// </summary>
		public decimal? aging30_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 60
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging60 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 60 (Base)
		/// Description : The base currency equivalent of the aging 60 field.
		/// Money, length -1
		/// </summary>
		public decimal? aging60_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 90
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging90 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 90 (Base)
		/// Description : The base currency equivalent of the aging 90 field.
		/// Money, length -1
		/// </summary>
		public decimal? aging90_base { get; set; }  // Money, length -1

		public enum businesstypecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Business Type
		/// Description : Select the legal designation or other business type of the account for contracts or reporting purposes.
		/// Picklist, length -1
		/// </summary>
		public businesstypecodeEnum? businesstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: businesstypecode
		/// Virtual, length -1
		/// </summary>
		public string businesstypecodename { get; set; }  // Virtual, length -1

		public enum cos_aquisestatusEnum {
			Erfolgreich = 585670000,
			Adresse = 585670001,
			Aktiv = 585670002,
			Nichtinteressiert = 585670003,
		}

		/// <summary>
		/// Display name: Aquise Status
		/// Picklist, length -1
		/// </summary>
		public cos_aquisestatusEnum? cos_aquisestatus { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: cos_aquisestatus
		/// Virtual, length -1
		/// </summary>
		public string cos_aquisestatusname { get; set; }  // Virtual, length -1

		public enum cos_colouroptionEnum {
			Blue = 585670000,
			Green = 585670001,
			Red = 585670002,
		}

		/// <summary>
		/// Display name: ColourOption
		/// Picklist, length -1
		/// </summary>
		public cos_colouroptionEnum? cos_colouroption { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: cos_colouroption
		/// Virtual, length -1
		/// </summary>
		public string cos_colouroptionname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Data Quality Proofed
		/// Boolean, length -1
		/// </summary>
		public bool? cos_dataqualityproofed { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: cos_dataqualityproofed
		/// Virtual, length -1
		/// </summary>
		public string cos_dataqualityproofedname { get; set; }  // Virtual, length -1

		public enum cos_mitgliedschaftEnum {
			Gast = 585670000,
			eG = 585670001,
			Still = 585670002,
			Kombipartner = 585670003,
			Premiummitglied = 585670004,
			MDEVerbund = 585670005,
			gelistet = 585670006,
		}

		/// <summary>
		/// Display name: Mitgliedschaft
		/// Picklist, length -1
		/// </summary>
		public cos_mitgliedschaftEnum? cos_mitgliedschaft { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: cos_mitgliedschaft
		/// Virtual, length -1
		/// </summary>
		public string cos_mitgliedschaftname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_accountbase_createdby
		/// </summary>
		public ODataBind createdbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_accountbase_createdby
		/// Reference   : createdby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdby { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdby_value { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Schema name : lk_externalparty_account_createdby
		/// </summary>
		public ODataBind CreatedByExternalPartyLookup { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Schema name : lk_externalparty_account_createdby
		/// Reference   : createdbyexternalparty -> externalparty(externalpartyid)
		/// </summary>
		public dynamic CreatedByExternalParty { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Lookup, targets: externalparty
		/// </summary>
		public Guid? _createdbyexternalparty_value { get; set; }
		/// <summary>
		/// Attribute of: createdbyexternalparty
		/// String, length 100
		/// </summary>
		public string createdbyexternalpartyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdbyexternalparty
		/// String, length 100
		/// </summary>
		public string createdbyexternalpartyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created On
		/// Description : Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? createdon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_accountbase_createdonbehalfby
		/// </summary>
		public ODataBind createdonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_accountbase_createdonbehalfby
		/// Reference   : createdonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdonbehalfby { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Credit Limit
		/// Description : Type the credit limit of the account. This is a useful reference when you address invoice and accounting issues with the customer.
		/// Money, length -1
		/// </summary>
		public decimal? creditlimit { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Credit Limit (Base)
		/// Description : Shows the credit limit converted to the system's default base currency for reporting purposes.
		/// Money, length -1
		/// </summary>
		public decimal? creditlimit_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Credit Hold
		/// Description : Select whether the credit for the account is on hold. This is a useful reference while addressing the invoice and accounting issues with the customer.
		/// Boolean, length -1
		/// </summary>
		public bool? creditonhold { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: creditonhold
		/// Virtual, length -1
		/// </summary>
		public string creditonholdname { get; set; }  // Virtual, length -1

		public enum customersizecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Customer Size
		/// Description : Select the size category or range of the account for segmentation and reporting purposes.
		/// Picklist, length -1
		/// </summary>
		public customersizecodeEnum? customersizecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: customersizecode
		/// Virtual, length -1
		/// </summary>
		public string customersizecodename { get; set; }  // Virtual, length -1

		public enum customertypecodeEnum {
			Competitor = 1,
			Consultant = 2,
			Customer = 3,
			Influencer = 4,
			Insurancecarrier = 5,
			Investor = 6,
			Partner = 7,
			Press = 8,
			Prospect = 9,
			Reseller = 10,
			Supplier = 11,
			Vendor = 12,
			Other = 13,
		}

		/// <summary>
		/// Display name: Relationship Type
		/// Description : Select the category that best describes the relationship between the account and your organization.
		/// Picklist, length -1
		/// </summary>
		public customertypecodeEnum? customertypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: customertypecode
		/// Virtual, length -1
		/// </summary>
		public string customertypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Product Price List
		/// Description : Choose the default price list associated with the account to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Schema name : price_level_accounts
		/// </summary>
		public ODataBind defaultpricelevelidLookup { get; set; }
		/// <summary>
		/// Display name: Product Price List
		/// Description : Choose the default price list associated with the account to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Schema name : price_level_accounts
		/// Reference   : defaultpricelevelid -> pricelevel(pricelevelid)
		/// </summary>
		public dynamic defaultpricelevelid { get; set; }
		/// <summary>
		/// Display name: Product Price List
		/// Description : Choose the default price list associated with the account to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Lookup, targets: pricelevel
		/// </summary>
		public Guid? _defaultpricelevelid_value { get; set; }
		/// <summary>
		/// Attribute of: defaultpricelevelid
		/// String, length 100
		/// </summary>
		public string defaultpricelevelidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Description
		/// Description : Type additional information to describe the account, such as an excerpt from the company's website.
		/// Memo, length 2000
		/// </summary>
		public string description { get; set; }  // Memo, length 2000
		/// <summary>
		/// Display name: Do not allow Bulk Emails
		/// Description : Select whether the account allows bulk email sent through campaigns. If Do Not Allow is selected, the account can be added to marketing lists, but is excluded from email.
		/// Boolean, length -1
		/// </summary>
		public bool? donotbulkemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotbulkemail
		/// Virtual, length -1
		/// </summary>
		public string donotbulkemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Bulk Mails
		/// Description : Select whether the account allows bulk postal mail sent through marketing campaigns or quick campaigns. If Do Not Allow is selected, the account can be added to marketing lists, but will be excluded from the postal mail.
		/// Boolean, length -1
		/// </summary>
		public bool? donotbulkpostalmail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotbulkpostalmail
		/// Virtual, length -1
		/// </summary>
		public string donotbulkpostalmailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Emails
		/// Description : Select whether the account allows direct email sent from Microsoft Dynamics 365.
		/// Boolean, length -1
		/// </summary>
		public bool? donotemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotemail
		/// Virtual, length -1
		/// </summary>
		public string donotemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Faxes
		/// Description : Select whether the account allows faxes. If Do Not Allow is selected, the account will be excluded from fax activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotfax { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotfax
		/// Virtual, length -1
		/// </summary>
		public string donotfaxname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Phone Calls
		/// Description : Select whether the account allows phone calls. If Do Not Allow is selected, the account will be excluded from phone call activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotphone { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotphone
		/// Virtual, length -1
		/// </summary>
		public string donotphonename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Mails
		/// Description : Select whether the account allows direct mail. If Do Not Allow is selected, the account will be excluded from letter activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotpostalmail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotpostalmail
		/// Virtual, length -1
		/// </summary>
		public string donotpostalmailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: donotsendmm
		/// Virtual, length -1
		/// </summary>
		public string donotsendmarketingmaterialname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Send Marketing Materials
		/// Description : Select whether the account accepts marketing materials, such as brochures or catalogs.
		/// Boolean, length -1
		/// </summary>
		public bool? donotsendmm { get; set; }  // Boolean, length -1
		/// <summary>
		/// Display name: Email
		/// Description : Type the primary email address for the account.
		/// String, length 100
		/// </summary>
		public string emailaddress1 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Email Address 2
		/// Description : Type the secondary email address for the account.
		/// String, length 100
		/// </summary>
		public string emailaddress2 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Email Address 3
		/// Description : Type an alternate email address for the account.
		/// String, length 100
		/// </summary>
		public string emailaddress3 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Default Image
		/// Description : Shows the default image for the record.
		/// Attribute of: entityimageid
		/// Virtual, length -1
		/// </summary>
		public string entityimage { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: entityimageid
		/// BigInt, length -1
		/// </summary>
		public long? entityimage_timestamp { get; set; }  // BigInt, length -1
		/// <summary>
		/// Attribute of: entityimageid
		/// String, length 200
		/// </summary>
		public string entityimage_url { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Entity Image Id
		/// Description : For internal use only.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? entityimageid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Exchange Rate
		/// Description : Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// Decimal, length -1
		/// </summary>
		public decimal? exchangerate { get; set; }  // Decimal, length -1
		/// <summary>
		/// Display name: Fax
		/// Description : Type the fax number for the account.
		/// String, length 50
		/// </summary>
		public string fax { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Follow Email Activity
		/// Description : Information about whether to allow following email activity like opens, attachment views and link clicks for emails sent to the account.
		/// Boolean, length -1
		/// </summary>
		public bool? followemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: followemail
		/// Virtual, length -1
		/// </summary>
		public string followemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: FTP Site
		/// Description : Type the URL for the account's FTP site to enable users to access data and share documents.
		/// String, length 200
		/// </summary>
		public string ftpsiteurl { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Import Sequence Number
		/// Description : Unique identifier of the data import or data migration that created this record.
		/// Integer, length -1
		/// </summary>
		public int? importsequencenumber { get; set; }  // Integer, length -1

		public enum industrycodeEnum {
			Accounting = 1,
			AgricultureandNonpetrolNaturalResourceExtraction = 2,
			BroadcastingPrintingandPublishing = 3,
			Brokers = 4,
			BuildingSupplyRetail = 5,
			BusinessServices = 6,
			Consulting = 7,
			ConsumerServices = 8,
			DesignDirectionandCreativeManagement = 9,
			DistributorsDispatchersandProcessors = 10,
			DoctorsOfficesandClinics = 11,
			DurableManufacturing = 12,
			EatingandDrinkingPlaces = 13,
			EntertainmentRetail = 14,
			EquipmentRentalandLeasing = 15,
			Financial = 16,
			FoodandTobaccoProcessing = 17,
			InboundCapitalIntensiveProcessing = 18,
			InboundRepairandServices = 19,
			Insurance = 20,
			LegalServices = 21,
			NonDurableMerchandiseRetail = 22,
			OutboundConsumerService = 23,
			PetrochemicalExtractionandDistribution = 24,
			ServiceRetail = 25,
			SIGAffiliations = 26,
			SocialServices = 27,
			SpecialOutboundTradeContractors = 28,
			SpecialtyRealty = 29,
			Transportation = 30,
			UtilityCreationandDistribution = 31,
			VehicleRetail = 32,
			Wholesale = 33,
		}

		/// <summary>
		/// Display name: Industry
		/// Description : Select the account's primary industry for use in marketing segmentation and demographic analysis.
		/// Picklist, length -1
		/// </summary>
		public industrycodeEnum? industrycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: industrycode
		/// Virtual, length -1
		/// </summary>
		public string industrycodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Boolean, length -1
		/// </summary>
		public bool? isprivate { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: isprivate
		/// Virtual, length -1
		/// </summary>
		public string isprivatename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Last On Hold Time
		/// Description : Contains the date and time stamp of the last on hold time.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? lastonholdtime { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Last Date Included in Campaign
		/// Description : Shows the date when the account was last included in a marketing campaign or quick campaign.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? lastusedincampaign { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Market Capitalization
		/// Description : Type the market capitalization of the account to identify the company's equity, used as an indicator in financial performance analysis.
		/// Money, length -1
		/// </summary>
		public decimal? marketcap { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Market Capitalization (Base)
		/// Description : Shows the market capitalization converted to the system's default base currency.
		/// Money, length -1
		/// </summary>
		public decimal? marketcap_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Marketing Only
		/// Description : Whether is only for marketing
		/// Boolean, length -1
		/// </summary>
		public bool? marketingonly { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: marketingonly
		/// Virtual, length -1
		/// </summary>
		public string marketingonlyname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: masterid
		/// String, length 100
		/// </summary>
		public string masteraccountidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: masterid
		/// String, length 100
		/// </summary>
		public string masteraccountidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Master ID
		/// Description : Shows the master account that the account was merged with.
		/// Schema name : account_master_account
		/// </summary>
		public ODataBind masteridLookup { get; set; }
		/// <summary>
		/// Display name: Master ID
		/// Description : Shows the master account that the account was merged with.
		/// Schema name : account_master_account
		/// Reference   : masterid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account masterid { get; set; }
		/// <summary>
		/// Display name: Master ID
		/// Description : Shows the master account that the account was merged with.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _masterid_value { get; set; }
		/// <summary>
		/// Display name: Merged
		/// Description : Shows whether the account has been merged with another account.
		/// Boolean, length -1
		/// </summary>
		public bool? merged { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: merged
		/// Virtual, length -1
		/// </summary>
		public string mergedname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_accountbase_modifiedby
		/// </summary>
		public ODataBind modifiedbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_accountbase_modifiedby
		/// Reference   : modifiedby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedby { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedby_value { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Schema name : lk_externalparty_account_modifiedby
		/// </summary>
		public ODataBind ModifiedByExternalPartyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Schema name : lk_externalparty_account_modifiedby
		/// Reference   : modifiedbyexternalparty -> externalparty(externalpartyid)
		/// </summary>
		public dynamic ModifiedByExternalParty { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Lookup, targets: externalparty
		/// </summary>
		public Guid? _modifiedbyexternalparty_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedbyexternalparty
		/// String, length 100
		/// </summary>
		public string modifiedbyexternalpartyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedbyexternalparty
		/// String, length 100
		/// </summary>
		public string modifiedbyexternalpartyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified On
		/// Description : Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? modifiedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_accountbase_modifiedonbehalfby
		/// </summary>
		public ODataBind modifiedonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_accountbase_modifiedonbehalfby
		/// Reference   : modifiedonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedonbehalfby { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Account.
		/// Schema name : msa_account_managingpartner
		/// </summary>
		public ODataBind msa_managingpartneridLookup { get; set; }
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Account.
		/// Schema name : msa_account_managingpartner
		/// Reference   : msa_managingpartnerid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account msa_managingpartnerid { get; set; }
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Account.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _msa_managingpartnerid_value { get; set; }
		/// <summary>
		/// Attribute of: msa_managingpartnerid
		/// String, length 160
		/// </summary>
		public string msa_managingpartneridname { get; set; }  // String, length 160
		/// <summary>
		/// Attribute of: msa_managingpartnerid
		/// String, length 160
		/// </summary>
		public string msa_managingpartneridyominame { get; set; }  // String, length 160
		/// <summary>
		/// Display name: KPI
		/// Schema name : msdyn_msdyn_accountkpiitem_account_accountkpiid
		/// </summary>
		public ODataBind msdyn_accountkpiidLookup { get; set; }
		/// <summary>
		/// Display name: KPI
		/// Schema name : msdyn_msdyn_accountkpiitem_account_accountkpiid
		/// Reference   : msdyn_accountkpiid -> msdyn_accountkpiitem(msdyn_accountkpiitemid)
		/// </summary>
		public dynamic msdyn_accountkpiid { get; set; }
		/// <summary>
		/// Display name: KPI
		/// Lookup, targets: msdyn_accountkpiitem
		/// </summary>
		public Guid? _msdyn_accountkpiid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_accountkpiid
		/// String, length 100
		/// </summary>
		public string msdyn_accountkpiidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Billing Account
		/// Description : Reference to an other account to be used for billing (only to be used if billing account differs)
		/// Schema name : msdyn_account_account_BillingAccount
		/// </summary>
		public ODataBind msdyn_billingaccount_accountLookup { get; set; }
		/// <summary>
		/// Display name: Billing Account
		/// Description : Reference to an other account to be used for billing (only to be used if billing account differs)
		/// Schema name : msdyn_account_account_BillingAccount
		/// Reference   : msdyn_billingaccount -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account msdyn_billingaccount_account { get; set; }
		/// <summary>
		/// Display name: Billing Account
		/// Description : Reference to an other account to be used for billing (only to be used if billing account differs)
		/// Lookup, targets: account
		/// </summary>
		public Guid? _msdyn_billingaccount_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_billingaccount
		/// String, length 160
		/// </summary>
		public string msdyn_billingaccountname { get; set; }  // String, length 160
		/// <summary>
		/// Attribute of: msdyn_billingaccount
		/// String, length 160
		/// </summary>
		public string msdyn_billingaccountyominame { get; set; }  // String, length 160
		/// <summary>
		/// Display name: External ID
		/// Description : External Account ID from other systems.
		/// String, length 100
		/// </summary>
		public string msdyn_externalaccountid { get; set; }  // String, length 100
		/// <summary>
		/// Display name: GDPR Optout
		/// Description : Describes whether account is opted out or not
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_gdproptout { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_gdproptout
		/// Virtual, length -1
		/// </summary>
		public string msdyn_gdproptoutname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Preferred Resource (Deprecated)
		/// Schema name : msdyn_bookableresource_account_PreferredResource
		/// </summary>
		public ODataBind msdyn_PreferredResourceLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Resource (Deprecated)
		/// Schema name : msdyn_bookableresource_account_PreferredResource
		/// Reference   : msdyn_preferredresource -> bookableresource(bookableresourceid)
		/// </summary>
		public dynamic msdyn_PreferredResource { get; set; }
		/// <summary>
		/// Display name: Preferred Resource (Deprecated)
		/// Lookup, targets: bookableresource
		/// </summary>
		public Guid? _msdyn_preferredresource_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_preferredresource
		/// String, length 100
		/// </summary>
		public string msdyn_preferredresourcename { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Sales Acceleration Insights ID
		/// Description : Sales Acceleration Insights ID
		/// Schema name : msdyn_insightsid_salesaccelerationinsights
		/// </summary>
		public ODataBind msdyn_salesaccelerationinsightidLookup { get; set; }
		/// <summary>
		/// Display name: Sales Acceleration Insights ID
		/// Description : Sales Acceleration Insights ID
		/// Schema name : msdyn_insightsid_salesaccelerationinsights
		/// Reference   : msdyn_salesaccelerationinsightid -> msdyn_salesaccelerationinsight(msdyn_salesaccelerationinsightid)
		/// </summary>
		public dynamic msdyn_salesaccelerationinsightid { get; set; }
		/// <summary>
		/// Display name: Sales Acceleration Insights ID
		/// Description : Sales Acceleration Insights ID
		/// Lookup, targets: msdyn_salesaccelerationinsight
		/// </summary>
		public Guid? _msdyn_salesaccelerationinsightid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_salesaccelerationinsightid
		/// String, length 100
		/// </summary>
		public string msdyn_salesaccelerationinsightidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Sales Tax Code
		/// Description : Default Sales Tax Code
		/// Schema name : msdyn_msdyn_taxcode_account_SalesTaxCode
		/// </summary>
		public ODataBind msdyn_salestaxcodeLookup { get; set; }
		/// <summary>
		/// Display name: Sales Tax Code
		/// Description : Default Sales Tax Code
		/// Schema name : msdyn_msdyn_taxcode_account_SalesTaxCode
		/// Reference   : msdyn_salestaxcode -> msdyn_taxcode(msdyn_taxcodeid)
		/// </summary>
		public dynamic msdyn_salestaxcode { get; set; }
		/// <summary>
		/// Display name: Sales Tax Code
		/// Description : Default Sales Tax Code
		/// Lookup, targets: msdyn_taxcode
		/// </summary>
		public Guid? _msdyn_salestaxcode_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_salestaxcode
		/// String, length 100
		/// </summary>
		public string msdyn_salestaxcodename { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with account.
		/// Schema name : msdyn_msdyn_segment_account
		/// </summary>
		public ODataBind msdyn_segmentidLookup { get; set; }
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with account.
		/// Schema name : msdyn_msdyn_segment_account
		/// Reference   : msdyn_segmentid -> msdyn_segment(msdyn_segmentid)
		/// </summary>
		public dynamic msdyn_segmentid { get; set; }
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with account.
		/// Lookup, targets: msdyn_segment
		/// </summary>
		public Guid? _msdyn_segmentid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_segmentid
		/// String, length 100
		/// </summary>
		public string msdyn_segmentidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Service Territory
		/// Description : The Service Territory this account belongs to. This is used to optimize scheduling and routing
		/// Schema name : msdyn_territory_account_ServiceTerritory
		/// </summary>
		public ODataBind msdyn_serviceterritoryLookup { get; set; }
		/// <summary>
		/// Display name: Service Territory
		/// Description : The Service Territory this account belongs to. This is used to optimize scheduling and routing
		/// Schema name : msdyn_territory_account_ServiceTerritory
		/// Reference   : msdyn_serviceterritory -> territory(territoryid)
		/// </summary>
		public dynamic msdyn_serviceterritory { get; set; }
		/// <summary>
		/// Display name: Service Territory
		/// Description : The Service Territory this account belongs to. This is used to optimize scheduling and routing
		/// Lookup, targets: territory
		/// </summary>
		public Guid? _msdyn_serviceterritory_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_serviceterritory
		/// String, length 200
		/// </summary>
		public string msdyn_serviceterritoryname { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Tax Exempt
		/// Description : Select whether the account is tax exempt.
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_taxexempt { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_taxexempt
		/// Virtual, length -1
		/// </summary>
		public string msdyn_taxexemptname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Tax Exempt Number
		/// Description : Shows the government tax exempt number.
		/// String, length 20
		/// </summary>
		public string msdyn_taxexemptnumber { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Travel Charge
		/// Description : Enter the travel charge to include on work orders. This value will be multiplied by the travel charge type.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_travelcharge { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Travel Charge (Base)
		/// Description : Value of the Travel Charge in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_travelcharge_base { get; set; }  // Money, length -1

		public enum msdyn_travelchargetypeEnum {
			Hourly = 690970000,
			Mileage = 690970001,
			Fixed = 690970002,
			None = 690970003,
		}

		/// <summary>
		/// Display name: Travel Charge Type
		/// Description : Specify how travel is charged for this account.
		/// Picklist, length -1
		/// </summary>
		public msdyn_travelchargetypeEnum? msdyn_travelchargetype { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_travelchargetype
		/// Virtual, length -1
		/// </summary>
		public string msdyn_travelchargetypename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Work Hour Template
		/// Schema name : msdyn_msdyn_workhourtemplate_account_workhourtemplate
		/// </summary>
		public ODataBind msdyn_workhourtemplateLookup { get; set; }
		/// <summary>
		/// Display name: Work Hour Template
		/// Schema name : msdyn_msdyn_workhourtemplate_account_workhourtemplate
		/// Reference   : msdyn_workhourtemplate -> msdyn_workhourtemplate(msdyn_workhourtemplateid)
		/// </summary>
		public dynamic msdyn_workhourtemplate { get; set; }
		/// <summary>
		/// Display name: Work Hour Template
		/// Lookup, targets: msdyn_workhourtemplate
		/// </summary>
		public Guid? _msdyn_workhourtemplate_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_workhourtemplate
		/// String, length 100
		/// </summary>
		public string msdyn_workhourtemplatename { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Work Order Instructions
		/// Description : Shows the default instructions to show on new work orders.
		/// Memo, length 4000
		/// </summary>
		public string msdyn_workorderinstructions { get; set; }  // Memo, length 4000
		/// <summary>
		/// Display name: Account Name
		/// Description : Type the company or business name.
		/// String, length 160
		/// </summary>
		public string name { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Number of Employees
		/// Description : Type the number of employees that work at the account for use in marketing segmentation and demographic analysis.
		/// Integer, length -1
		/// </summary>
		public int? numberofemployees { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: On Hold Time (Minutes)
		/// Description : Shows how long, in minutes, that the record was on hold.
		/// Integer, length -1
		/// </summary>
		public int? onholdtime { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Open Deals
		/// Description : Number of open opportunities against an account and its child accounts.
		/// Integer, length -1
		/// </summary>
		public int? opendeals { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Open Deals (Last Updated On)
		/// Description : Last Updated time of rollup field Open Deals.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? opendeals_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Open Deals (State)
		/// Description : State of rollup field Open Deals.
		/// Integer, length -1
		/// </summary>
		public int? opendeals_state { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Open Revenue
		/// Description : Sum of open revenue against an account and its child accounts.
		/// Money, length -1
		/// </summary>
		public decimal? openrevenue { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Open Revenue (Base)
		/// Description : Value of the Open Revenue in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? openrevenue_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Open Revenue (Last Updated On)
		/// Description : Last Updated time of rollup field Open Revenue.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? openrevenue_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Open Revenue (State)
		/// Description : State of rollup field Open Revenue.
		/// Integer, length -1
		/// </summary>
		public int? openrevenue_state { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the account was created from if the account was created by converting a lead in Microsoft Dynamics 365. This is used to relate the account to data on the originating lead for use in reporting and analytics.
		/// Schema name : account_originating_lead
		/// </summary>
		public ODataBind originatingleadidLookup { get; set; }
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the account was created from if the account was created by converting a lead in Microsoft Dynamics 365. This is used to relate the account to data on the originating lead for use in reporting and analytics.
		/// Schema name : account_originating_lead
		/// Reference   : originatingleadid -> lead(leadid)
		/// </summary>
		public dynamic originatingleadid { get; set; }
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the account was created from if the account was created by converting a lead in Microsoft Dynamics 365. This is used to relate the account to data on the originating lead for use in reporting and analytics.
		/// Lookup, targets: lead
		/// </summary>
		public Guid? _originatingleadid_value { get; set; }
		/// <summary>
		/// Attribute of: originatingleadid
		/// String, length 100
		/// </summary>
		public string originatingleadidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: originatingleadid
		/// String, length 100
		/// </summary>
		public string originatingleadidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Record Created On
		/// Description : Date and time that the record was migrated.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? overriddencreatedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Schema name : owner_accounts
		/// </summary>
		public ODataBind owneridLookup { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Schema name : owner_accounts
		/// Reference   : ownerid -> owner(ownerid)
		/// </summary>
		public dynamic ownerid { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Owner, targets: systemuser, team
		/// </summary>
		public Guid? _ownerid_value { get; set; }
		/// <summary>
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: ownerid
		/// EntityName, length -1
		/// </summary>
		public string owneridtype { get; set; }  // EntityName, length -1
		/// <summary>
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridyominame { get; set; }  // String, length 100

		public enum ownershipcodeEnum {
			Public = 1,
			Private = 2,
			Subsidiary = 3,
			Other = 4,
		}

		/// <summary>
		/// Display name: Ownership
		/// Description : Select the account's ownership structure, such as public or private.
		/// Picklist, length -1
		/// </summary>
		public ownershipcodeEnum? ownershipcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: ownershipcode
		/// Virtual, length -1
		/// </summary>
		public string ownershipcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Shows the business unit that the record owner belongs to.
		/// Schema name : business_unit_accounts
		/// </summary>
		public ODataBind owningbusinessunitLookup { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Shows the business unit that the record owner belongs to.
		/// Schema name : business_unit_accounts
		/// Reference   : owningbusinessunit -> businessunit(businessunitid)
		/// </summary>
		public dynamic owningbusinessunit { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Shows the business unit that the record owner belongs to.
		/// Lookup, targets: businessunit
		/// </summary>
		public Guid? _owningbusinessunit_value { get; set; }
		/// <summary>
		/// Attribute of: owningbusinessunit
		/// String, length 160
		/// </summary>
		public string owningbusinessunitname { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the account.
		/// Schema name : team_accounts
		/// </summary>
		public ODataBind owningteamLookup { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the account.
		/// Schema name : team_accounts
		/// Reference   : owningteam -> team(teamid)
		/// </summary>
		public dynamic owningteam { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the account.
		/// Lookup, targets: team
		/// </summary>
		public Guid? _owningteam_value { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the account.
		/// Schema name : user_accounts
		/// </summary>
		public ODataBind owninguserLookup { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the account.
		/// Schema name : user_accounts
		/// Reference   : owninguser -> systemuser(systemuserid)
		/// </summary>
		public dynamic owninguser { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the account.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _owninguser_value { get; set; }
		/// <summary>
		/// Display name: Parent Account
		/// Description : Choose the parent account associated with this account to show parent and child businesses in reporting and analytics.
		/// Schema name : account_parent_account
		/// </summary>
		public ODataBind parentaccountidLookup { get; set; }
		/// <summary>
		/// Display name: Parent Account
		/// Description : Choose the parent account associated with this account to show parent and child businesses in reporting and analytics.
		/// Schema name : account_parent_account
		/// Reference   : parentaccountid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account parentaccountid { get; set; }
		/// <summary>
		/// Display name: Parent Account
		/// Description : Choose the parent account associated with this account to show parent and child businesses in reporting and analytics.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _parentaccountid_value { get; set; }
		/// <summary>
		/// Attribute of: parentaccountid
		/// String, length 100
		/// </summary>
		public string parentaccountidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: parentaccountid
		/// String, length 100
		/// </summary>
		public string parentaccountidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Participates in Workflow
		/// Description : For system use only. Legacy Microsoft Dynamics CRM 3.0 workflow data.
		/// Boolean, length -1
		/// </summary>
		public bool? participatesinworkflow { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: participatesinworkflow
		/// Virtual, length -1
		/// </summary>
		public string participatesinworkflowname { get; set; }  // Virtual, length -1

		public enum paymenttermscodeEnum {
			Net30 = 1,
			_210Net30 = 2,
			Net45 = 3,
			Net60 = 4,
		}

		/// <summary>
		/// Display name: Payment Terms
		/// Description : Select the payment terms to indicate when the customer needs to pay the total amount.
		/// Picklist, length -1
		/// </summary>
		public paymenttermscodeEnum? paymenttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: paymenttermscode
		/// Virtual, length -1
		/// </summary>
		public string paymenttermscodename { get; set; }  // Virtual, length -1

		public enum preferredappointmentdaycodeEnum {
			Sunday = 0,
			Monday = 1,
			Tuesday = 2,
			Wednesday = 3,
			Thursday = 4,
			Friday = 5,
			Saturday = 6,
		}

		/// <summary>
		/// Display name: Preferred Day
		/// Description : Select the preferred day of the week for service appointments.
		/// Picklist, length -1
		/// </summary>
		public preferredappointmentdaycodeEnum? preferredappointmentdaycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredappointmentdaycode
		/// Virtual, length -1
		/// </summary>
		public string preferredappointmentdaycodename { get; set; }  // Virtual, length -1

		public enum preferredappointmenttimecodeEnum {
			Morning = 1,
			Afternoon = 2,
			Evening = 3,
		}

		/// <summary>
		/// Display name: Preferred Time
		/// Description : Select the preferred time of day for service appointments.
		/// Picklist, length -1
		/// </summary>
		public preferredappointmenttimecodeEnum? preferredappointmenttimecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredappointmenttimecode
		/// Virtual, length -1
		/// </summary>
		public string preferredappointmenttimecodename { get; set; }  // Virtual, length -1

		public enum preferredcontactmethodcodeEnum {
			Any = 1,
			Email = 2,
			Phone = 3,
			Fax = 4,
			Mail = 5,
		}

		/// <summary>
		/// Display name: Preferred Method of Contact
		/// Description : Select the preferred method of contact.
		/// Picklist, length -1
		/// </summary>
		public preferredcontactmethodcodeEnum? preferredcontactmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredcontactmethodcode
		/// Virtual, length -1
		/// </summary>
		public string preferredcontactmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the account's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Schema name : equipment_accounts
		/// </summary>
		public ODataBind preferredequipmentidLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the account's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Schema name : equipment_accounts
		/// Reference   : preferredequipmentid -> equipment(equipmentid)
		/// </summary>
		public dynamic preferredequipmentid { get; set; }
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the account's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Lookup, targets: equipment
		/// </summary>
		public Guid? _preferredequipmentid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredequipmentid
		/// String, length 100
		/// </summary>
		public string preferredequipmentidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the account's preferred service for reference when you schedule service activities.
		/// Schema name : service_accounts
		/// </summary>
		public ODataBind preferredserviceidLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the account's preferred service for reference when you schedule service activities.
		/// Schema name : service_accounts
		/// Reference   : preferredserviceid -> service(serviceid)
		/// </summary>
		public dynamic preferredserviceid { get; set; }
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the account's preferred service for reference when you schedule service activities.
		/// Lookup, targets: service
		/// </summary>
		public Guid? _preferredserviceid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredserviceid
		/// String, length 100
		/// </summary>
		public string preferredserviceidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the preferred service representative for reference when you schedule service activities for the account.
		/// Schema name : system_user_accounts
		/// </summary>
		public ODataBind preferredsystemuseridLookup { get; set; }
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the preferred service representative for reference when you schedule service activities for the account.
		/// Schema name : system_user_accounts
		/// Reference   : preferredsystemuserid -> systemuser(systemuserid)
		/// </summary>
		public dynamic preferredsystemuserid { get; set; }
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the preferred service representative for reference when you schedule service activities for the account.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _preferredsystemuserid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredsystemuserid
		/// String, length 100
		/// </summary>
		public string preferredsystemuseridname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: preferredsystemuserid
		/// String, length 100
		/// </summary>
		public string preferredsystemuseridyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Primary Contact
		/// Description : Choose the primary contact for the account to provide quick access to contact details.
		/// Schema name : account_primary_contact
		/// </summary>
		public ODataBind primarycontactidLookup { get; set; }
		/// <summary>
		/// Display name: Primary Contact
		/// Description : Choose the primary contact for the account to provide quick access to contact details.
		/// Schema name : account_primary_contact
		/// Reference   : primarycontactid -> contact(contactid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.contact primarycontactid { get; set; }
		/// <summary>
		/// Display name: Primary Contact
		/// Description : Choose the primary contact for the account to provide quick access to contact details.
		/// Lookup, targets: contact
		/// </summary>
		public Guid? _primarycontactid_value { get; set; }
		/// <summary>
		/// Attribute of: primarycontactid
		/// String, length 100
		/// </summary>
		public string primarycontactidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: primarycontactid
		/// String, length 100
		/// </summary>
		public string primarycontactidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Primary Satori ID
		/// Description : Primary Satori ID for Account
		/// String, length 200
		/// </summary>
		public string primarysatoriid { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Primary Twitter ID
		/// Description : Primary Twitter ID for Account
		/// String, length 128
		/// </summary>
		public string primarytwitterid { get; set; }  // String, length 128
		/// <summary>
		/// Display name: Process
		/// Description : Shows the ID of the process.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? processid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Annual Revenue
		/// Description : Type the annual revenue for the account, used as an indicator in financial performance analysis.
		/// Money, length -1
		/// </summary>
		public decimal? revenue { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Annual Revenue (Base)
		/// Description : Shows the annual revenue converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? revenue_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Shares Outstanding
		/// Description : Type the number of shares available to the public for the account. This number is used as an indicator in financial performance analysis.
		/// Integer, length -1
		/// </summary>
		public int? sharesoutstanding { get; set; }  // Integer, length -1

		public enum shippingmethodcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Shipping Method
		/// Description : Select a shipping method for deliveries sent to the account's address to designate the preferred carrier or other delivery option.
		/// Picklist, length -1
		/// </summary>
		public shippingmethodcodeEnum? shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: SIC Code
		/// Description : Type the Standard Industrial Classification (SIC) code that indicates the account's primary industry of business, for use in marketing segmentation and demographic analysis.
		/// String, length 20
		/// </summary>
		public string sic { get; set; }  // String, length 20
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Account record.
		/// Schema name : manualsla_account
		/// </summary>
		public ODataBind sla_account_slaLookup { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Account record.
		/// Schema name : manualsla_account
		/// Reference   : slaid -> sla(slaid)
		/// </summary>
		public dynamic sla_account_sla { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Account record.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slaid_value { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Schema name : sla_account
		/// </summary>
		public ODataBind slainvokedid_account_slaLookup { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Schema name : sla_account
		/// Reference   : slainvokedid -> sla(slaid)
		/// </summary>
		public dynamic slainvokedid_account_sla { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slainvokedid_value { get; set; }
		/// <summary>
		/// Attribute of: slainvokedid
		/// String, length 100
		/// </summary>
		public string slainvokedidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: slaid
		/// String, length 100
		/// </summary>
		public string slaname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Process Stage
		/// Description : Shows the ID of the stage.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? stageid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Stock Exchange
		/// Description : Type the stock exchange at which the account is listed to track their stock and financial performance of the company.
		/// String, length 20
		/// </summary>
		public string stockexchange { get; set; }  // String, length 20
		/// <summary>
		/// Display name: TeamsFollowed
		/// Description : Number of users or conversations followed the record
		/// Integer, length -1
		/// </summary>
		public int? teamsfollowed { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Main Phone
		/// Description : Type the main phone number for this account.
		/// String, length 50
		/// </summary>
		public string telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Other Phone
		/// Description : Type a second phone number for this account.
		/// String, length 50
		/// </summary>
		public string telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Telephone 3
		/// Description : Type a third phone number for this account.
		/// String, length 50
		/// </summary>
		public string telephone3 { get; set; }  // String, length 50

		public enum territorycodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Territory Code
		/// Description : Select a region or territory for the account for use in segmentation and analysis.
		/// Picklist, length -1
		/// </summary>
		public territorycodeEnum? territorycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: territorycode
		/// Virtual, length -1
		/// </summary>
		public string territorycodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Territory
		/// Description : Choose the sales region or territory for the account to make sure the account is assigned to the correct representative and for use in segmentation and analysis.
		/// Schema name : territory_accounts
		/// </summary>
		public ODataBind territoryidLookup { get; set; }
		/// <summary>
		/// Display name: Territory
		/// Description : Choose the sales region or territory for the account to make sure the account is assigned to the correct representative and for use in segmentation and analysis.
		/// Schema name : territory_accounts
		/// Reference   : territoryid -> territory(territoryid)
		/// </summary>
		public dynamic territoryid { get; set; }
		/// <summary>
		/// Display name: Territory
		/// Description : Choose the sales region or territory for the account to make sure the account is assigned to the correct representative and for use in segmentation and analysis.
		/// Lookup, targets: territory
		/// </summary>
		public Guid? _territoryid_value { get; set; }
		/// <summary>
		/// Attribute of: territoryid
		/// String, length 100
		/// </summary>
		public string territoryidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Ticker Symbol
		/// Description : Type the stock exchange symbol for the account to track financial performance of the company. You can click the code entered in this field to access the latest trading information from MSN Money.
		/// String, length 10
		/// </summary>
		public string tickersymbol { get; set; }  // String, length 10
		/// <summary>
		/// Display name: Time Spent by me
		/// Description : Total time spent for emails (read and write) and meetings by me in relation to account record.
		/// String, length 1250
		/// </summary>
		public string timespentbymeonemailandmeetings { get; set; }  // String, length 1250
		/// <summary>
		/// Display name: Time Zone Rule Version Number
		/// Description : For internal use only.
		/// Integer, length -1
		/// </summary>
		public int? timezoneruleversionnumber { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_account
		/// </summary>
		public ODataBind transactioncurrencyidLookup { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_account
		/// Reference   : transactioncurrencyid -> transactioncurrency(transactioncurrencyid)
		/// </summary>
		public dynamic transactioncurrencyid { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Lookup, targets: transactioncurrency
		/// </summary>
		public Guid? _transactioncurrencyid_value { get; set; }
		/// <summary>
		/// Attribute of: transactioncurrencyid
		/// String, length 100
		/// </summary>
		public string transactioncurrencyidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Traversed Path
		/// Description : For internal use only.
		/// String, length 1250
		/// </summary>
		public string traversedpath { get; set; }  // String, length 1250
		/// <summary>
		/// Display name: UTC Conversion Time Zone Code
		/// Description : Time zone code that was in use when the record was created.
		/// Integer, length -1
		/// </summary>
		public int? utcconversiontimezonecode { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Version Number
		/// Description : Version number of the account.
		/// BigInt, length -1
		/// </summary>
		public long? versionnumber { get; set; }  // BigInt, length -1
		/// <summary>
		/// Display name: Website
		/// Description : Type the account's website URL to get quick details about the company profile.
		/// String, length 200
		/// </summary>
		public string websiteurl { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Yomi Account Name
		/// Description : Type the phonetic spelling of the company name, if specified in Japanese, to make sure the name is pronounced correctly in phone calls and other communications.
		/// String, length 160
		/// </summary>
		public string yominame { get; set; }  // String, length 160
	}
}

namespace EAI.Dataverse.ModelGeneratorTests.Model
{
	/// <summary>
	/// Display name: Contact
	/// Description : Person with whom a business unit has a relationship, such as customer, supplier, and colleague.
	/// </summary>
	public partial class contact
	{
		public const string EntitySet = "contacts";

		public ODataType ODataType { get => new ODataType() { Name = "Microsoft.Dynamics.CRM.Contact" }; }

		public static ODataBind ToLookup(Guid? guid) => guid == null ? null : new ODataBind() {EntityName = EntitySet, EntityId = guid.Value};

		public ODataBind ToLookup() => ToLookup(contactid);

		/// <summary>
		/// Display name: Account
		/// Description : Unique identifier of the account with which the contact is associated.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _accountid_value { get; set; }
		/// <summary>
		/// Attribute of: accountid
		/// String, length 100
		/// </summary>
		public string accountidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: accountid
		/// String, length 100
		/// </summary>
		public string accountidyominame { get; set; }  // String, length 100

		public enum accountrolecodeEnum {
			DecisionMaker = 1,
			Employee = 2,
			Influencer = 3,
		}

		/// <summary>
		/// Display name: Role
		/// Description : Select the contact's role within the company or sales process, such as decision maker, employee, or influencer.
		/// Picklist, length -1
		/// </summary>
		public accountrolecodeEnum? accountrolecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: accountrolecode
		/// Virtual, length -1
		/// </summary>
		public string accountrolecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: ID
		/// Description : Unique identifier for address 1.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? address1_addressid { get; set; }  // Uniqueidentifier, length -1

		public enum address1_addresstypecodeEnum {
			BillTo = 1,
			ShipTo = 2,
			Primary = 3,
			Other = 4,
		}

		/// <summary>
		/// Display name: Address 1: Address Type
		/// Description : Select the primary address type.
		/// Picklist, length -1
		/// </summary>
		public address1_addresstypecodeEnum? address1_addresstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_addresstypecode
		/// Virtual, length -1
		/// </summary>
		public string address1_addresstypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: City
		/// Description : Type the city for the primary address.
		/// String, length 80
		/// </summary>
		public string address1_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 1
		/// Description : Shows the complete primary address.
		/// Memo, length 1000
		/// </summary>
		public string address1_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Address 1: Country/Region
		/// Description : Type the country or region for the primary address.
		/// String, length 80
		/// </summary>
		public string address1_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 1: County
		/// Description : Type the county for the primary address.
		/// String, length 50
		/// </summary>
		public string address1_county { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Fax
		/// Description : Type the fax number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_fax { get; set; }  // String, length 50

		public enum address1_freighttermscodeEnum {
			FOB = 1,
			NoCharge = 2,
		}

		/// <summary>
		/// Display name: Address 1: Freight Terms
		/// Description : Select the freight terms for the primary address to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public address1_freighttermscodeEnum? address1_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string address1_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: Latitude
		/// Description : Type the latitude value for the primary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address1_latitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 1: Street 1
		/// Description : Type the first line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Street 2
		/// Description : Type the second line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Street 3
		/// Description : Type the third line of the primary address.
		/// String, length 250
		/// </summary>
		public string address1_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 1: Longitude
		/// Description : Type the longitude value for the primary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address1_longitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 1: Name
		/// Description : Type a descriptive name for the primary address, such as Corporate Headquarters.
		/// String, length 200
		/// </summary>
		public string address1_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Address 1: ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the primary address.
		/// String, length 20
		/// </summary>
		public string address1_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 1: Post Office Box
		/// Description : Type the post office box number of the primary address.
		/// String, length 20
		/// </summary>
		public string address1_postofficebox { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 1: Primary Contact Name
		/// Description : Type the name of the main contact at the account's primary address.
		/// String, length 100
		/// </summary>
		public string address1_primarycontactname { get; set; }  // String, length 100

		public enum address1_shippingmethodcodeEnum {
			Airborne = 1,
			DHL = 2,
			FedEx = 3,
			UPS = 4,
			PostalMail = 5,
			FullLoad = 6,
			WillCall = 7,
		}

		/// <summary>
		/// Display name: Address 1: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public address1_shippingmethodcodeEnum? address1_shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address1_shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string address1_shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 1: State/Province
		/// Description : Type the state or province of the primary address.
		/// String, length 50
		/// </summary>
		public string address1_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Phone
		/// Description : Type the main phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Telephone 2
		/// Description : Type a second phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: Telephone 3
		/// Description : Type a third phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address1_telephone3 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 1: UPS Zone
		/// Description : Type the UPS zone of the primary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
		/// String, length 4
		/// </summary>
		public string address1_upszone { get; set; }  // String, length 4
		/// <summary>
		/// Display name: Address 1: UTC Offset
		/// Description : Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
		/// Integer, length -1
		/// </summary>
		public int? address1_utcoffset { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Address 2: ID
		/// Description : Unique identifier for address 2.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? address2_addressid { get; set; }  // Uniqueidentifier, length -1

		public enum address2_addresstypecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Address Type
		/// Description : Select the secondary address type.
		/// Picklist, length -1
		/// </summary>
		public address2_addresstypecodeEnum? address2_addresstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_addresstypecode
		/// Virtual, length -1
		/// </summary>
		public string address2_addresstypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: City
		/// Description : Type the city for the secondary address.
		/// String, length 80
		/// </summary>
		public string address2_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 2
		/// Description : Shows the complete secondary address.
		/// Memo, length 1000
		/// </summary>
		public string address2_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Address 2: Country/Region
		/// Description : Type the country or region for the secondary address.
		/// String, length 80
		/// </summary>
		public string address2_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 2: County
		/// Description : Type the county for the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_county { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Fax
		/// Description : Type the fax number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_fax { get; set; }  // String, length 50

		public enum address2_freighttermscodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Freight Terms
		/// Description : Select the freight terms for the secondary address to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public address2_freighttermscodeEnum? address2_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string address2_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: Latitude
		/// Description : Type the latitude value for the secondary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address2_latitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 2: Street 1
		/// Description : Type the first line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Street 2
		/// Description : Type the second line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Street 3
		/// Description : Type the third line of the secondary address.
		/// String, length 250
		/// </summary>
		public string address2_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 2: Longitude
		/// Description : Type the longitude value for the secondary address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address2_longitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 2: Name
		/// Description : Type a descriptive name for the secondary address, such as Corporate Headquarters.
		/// String, length 100
		/// </summary>
		public string address2_name { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Address 2: ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the secondary address.
		/// String, length 20
		/// </summary>
		public string address2_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 2: Post Office Box
		/// Description : Type the post office box number of the secondary address.
		/// String, length 20
		/// </summary>
		public string address2_postofficebox { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 2: Primary Contact Name
		/// Description : Type the name of the main contact at the account's secondary address.
		/// String, length 100
		/// </summary>
		public string address2_primarycontactname { get; set; }  // String, length 100

		public enum address2_shippingmethodcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 2: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public address2_shippingmethodcodeEnum? address2_shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address2_shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string address2_shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 2: State/Province
		/// Description : Type the state or province of the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 1
		/// Description : Type the main phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 2
		/// Description : Type a second phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: Telephone 3
		/// Description : Type a third phone number associated with the secondary address.
		/// String, length 50
		/// </summary>
		public string address2_telephone3 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 2: UPS Zone
		/// Description : Type the UPS zone of the secondary address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
		/// String, length 4
		/// </summary>
		public string address2_upszone { get; set; }  // String, length 4
		/// <summary>
		/// Display name: Address 2: UTC Offset
		/// Description : Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
		/// Integer, length -1
		/// </summary>
		public int? address2_utcoffset { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Address 3: ID
		/// Description : Unique identifier for address 3.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? address3_addressid { get; set; }  // Uniqueidentifier, length -1

		public enum address3_addresstypecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 3: Address Type
		/// Description : Select the third address type.
		/// Picklist, length -1
		/// </summary>
		public address3_addresstypecodeEnum? address3_addresstypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address3_addresstypecode
		/// Virtual, length -1
		/// </summary>
		public string address3_addresstypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 3: City
		/// Description : Type the city for the 3rd address.
		/// String, length 80
		/// </summary>
		public string address3_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 3
		/// Description : Shows the complete third address.
		/// Memo, length 1000
		/// </summary>
		public string address3_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Address3: Country/Region
		/// Description : the country or region for the 3rd address.
		/// String, length 80
		/// </summary>
		public string address3_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Address 3: County
		/// Description : Type the county for the third address.
		/// String, length 50
		/// </summary>
		public string address3_county { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 3: Fax
		/// Description : Type the fax number associated with the third address.
		/// String, length 50
		/// </summary>
		public string address3_fax { get; set; }  // String, length 50

		public enum address3_freighttermscodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 3: Freight Terms
		/// Description : Select the freight terms for the third address to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public address3_freighttermscodeEnum? address3_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address3_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string address3_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address 3: Latitude
		/// Description : Type the latitude value for the third address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address3_latitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address3: Street 1
		/// Description : the first line of the 3rd address.
		/// String, length 250
		/// </summary>
		public string address3_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address3: Street 2
		/// Description : the second line of the 3rd address.
		/// String, length 250
		/// </summary>
		public string address3_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address3: Street 3
		/// Description : the third line of the 3rd address.
		/// String, length 250
		/// </summary>
		public string address3_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Address 3: Longitude
		/// Description : Type the longitude value for the third address for use in mapping and other applications.
		/// Double, length -1
		/// </summary>
		public double? address3_longitude { get; set; }  // Double, length -1
		/// <summary>
		/// Display name: Address 3: Name
		/// Description : Type a descriptive name for the third address, such as Corporate Headquarters.
		/// String, length 200
		/// </summary>
		public string address3_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Address3: ZIP/Postal Code
		/// Description : the ZIP Code or postal code for the 3rd address.
		/// String, length 20
		/// </summary>
		public string address3_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 3: Post Office Box
		/// Description : the post office box number of the 3rd address.
		/// String, length 20
		/// </summary>
		public string address3_postofficebox { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Address 3: Primary Contact Name
		/// Description : Type the name of the main contact at the account's third address.
		/// String, length 100
		/// </summary>
		public string address3_primarycontactname { get; set; }  // String, length 100

		public enum address3_shippingmethodcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Address 3: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public address3_shippingmethodcodeEnum? address3_shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: address3_shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string address3_shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Address3: State/Province
		/// Description : the state or province of the third address.
		/// String, length 50
		/// </summary>
		public string address3_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 3: Telephone1
		/// Description : Type the main phone number associated with the third address.
		/// String, length 50
		/// </summary>
		public string address3_telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 3: Telephone2
		/// Description : Type a second phone number associated with the third address.
		/// String, length 50
		/// </summary>
		public string address3_telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 3: Telephone3
		/// Description : Type a third phone number associated with the primary address.
		/// String, length 50
		/// </summary>
		public string address3_telephone3 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Address 3: UPS Zone
		/// Description : Type the UPS zone of the third address to make sure shipping charges are calculated correctly and deliveries are made promptly, if shipped by UPS.
		/// String, length 4
		/// </summary>
		public string address3_upszone { get; set; }  // String, length 4
		/// <summary>
		/// Display name: Address 3: UTC Offset
		/// Description : Select the time zone, or UTC offset, for this address so that other people can reference it when they contact someone at this address.
		/// Integer, length -1
		/// </summary>
		public int? address3_utcoffset { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Confirm Remove Password
		/// Boolean, length -1
		/// </summary>
		public bool? adx_confirmremovepassword { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_confirmremovepassword
		/// Virtual, length -1
		/// </summary>
		public string adx_confirmremovepasswordname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Created By IP Address
		/// String, length 100
		/// </summary>
		public string adx_createdbyipaddress { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created By Username
		/// String, length 100
		/// </summary>
		public string adx_createdbyusername { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Access Failed Count
		/// Description : Shows the current count of failed password attempts for the contact.
		/// Integer, length -1
		/// </summary>
		public int? adx_identity_accessfailedcount { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Email Confirmed
		/// Description : Determines if the email is confirmed by the contact.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_emailaddress1confirmed { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_emailaddress1confirmed
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_emailaddress1confirmedname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Last Successful Login
		/// Description : Indicates the last date and time the user successfully signed in to a portal.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? adx_identity_lastsuccessfullogin { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Local Login Disabled
		/// Description : Indicates that the contact can no longer sign in to the portal using the local account.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_locallogindisabled { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_locallogindisabled
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_locallogindisabledname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Lockout Enabled
		/// Description : Determines if this contact will track failed access attempts and become locked after too many failed attempts. To prevent the contact from becoming locked, you can disable this setting.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_lockoutenabled { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_lockoutenabled
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_lockoutenabledname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Lockout End Date
		/// Description : Shows the moment in time when the locked contact becomes unlocked again.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? adx_identity_lockoutenddate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Login Enabled
		/// Description : Determines if web authentication is enabled for the contact.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_logonenabled { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_logonenabled
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_logonenabledname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Mobile Phone Confirmed
		/// Description : Determines if the phone number is confirmed by the contact.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_mobilephoneconfirmed { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_mobilephoneconfirmed
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_mobilephoneconfirmedname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: New Password Input
		/// String, length 100
		/// </summary>
		public string adx_identity_newpassword { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Password Hash
		/// String, length 128
		/// </summary>
		public string adx_identity_passwordhash { get; set; }  // String, length 128
		/// <summary>
		/// Display name: Security Stamp
		/// Description : A token used to manage the web authentication session.
		/// String, length 100
		/// </summary>
		public string adx_identity_securitystamp { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Two Factor Enabled
		/// Description : Determines if two-factor authentication is enabled for the contact.
		/// Boolean, length -1
		/// </summary>
		public bool? adx_identity_twofactorenabled { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_identity_twofactorenabled
		/// Virtual, length -1
		/// </summary>
		public string adx_identity_twofactorenabledname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: User Name
		/// Description : Shows the user identity for local web authentication.
		/// String, length 100
		/// </summary>
		public string adx_identity_username { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified By IP Address
		/// String, length 100
		/// </summary>
		public string adx_modifiedbyipaddress { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified By Username
		/// String, length 100
		/// </summary>
		public string adx_modifiedbyusername { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Organization Name
		/// String, length 250
		/// </summary>
		public string adx_organizationname { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Preferred Language
		/// Description : User’s preferred portal language
		/// Schema name : adx_portallanguage_contact
		/// </summary>
		public ODataBind adx_preferredlanguageidLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Language
		/// Description : User’s preferred portal language
		/// Schema name : adx_portallanguage_contact
		/// Reference   : adx_preferredlanguageid -> adx_portallanguage(adx_portallanguageid)
		/// </summary>
		public dynamic adx_preferredlanguageid { get; set; }
		/// <summary>
		/// Display name: Preferred Language
		/// Description : User’s preferred portal language
		/// Lookup, targets: adx_portallanguage
		/// </summary>
		public Guid? _adx_preferredlanguageid_value { get; set; }
		/// <summary>
		/// Attribute of: adx_preferredlanguageid
		/// String, length 100
		/// </summary>
		public string adx_preferredlanguageidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Profile Alert
		/// Boolean, length -1
		/// </summary>
		public bool? adx_profilealert { get; set; }  // Boolean, length -1
		/// <summary>
		/// Display name: Profile Alert Date
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? adx_profilealertdate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Profile Alert Instructions
		/// Memo, length 4096
		/// </summary>
		public string adx_profilealertinstructions { get; set; }  // Memo, length 4096
		/// <summary>
		/// Attribute of: adx_profilealert
		/// Virtual, length -1
		/// </summary>
		public string adx_profilealertname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Profile Is Anonymous
		/// Boolean, length -1
		/// </summary>
		public bool? adx_profileisanonymous { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: adx_profileisanonymous
		/// Virtual, length -1
		/// </summary>
		public string adx_profileisanonymousname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Profile Last Activity
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? adx_profilelastactivity { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Profile Modified On
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? adx_profilemodifiedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Public Profile Copy
		/// Memo, length 64000
		/// </summary>
		public string adx_publicprofilecopy { get; set; }  // Memo, length 64000
		/// <summary>
		/// Display name: Time Zone
		/// Integer, length -1
		/// </summary>
		public int? adx_timezone { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Aging 30
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging30 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 30 (Base)
		/// Description : Shows the Aging 30 field converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? aging30_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 60
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging60 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 60 (Base)
		/// Description : Shows the Aging 60 field converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? aging60_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 90
		/// Description : For system use only.
		/// Money, length -1
		/// </summary>
		public decimal? aging90 { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Aging 90 (Base)
		/// Description : Shows the Aging 90 field converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? aging90_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Anniversary
		/// Description : Enter the date of the contact's wedding or service anniversary for use in customer gift programs or other communications.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? anniversary { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Annual Income
		/// Description : Type the contact's annual income for use in profiling and financial analysis.
		/// Money, length -1
		/// </summary>
		public decimal? annualincome { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Annual Income (Base)
		/// Description : Shows the Annual Income field converted to the system's default base currency. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? annualincome_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Assistant
		/// Description : Type the name of the contact's assistant.
		/// String, length 100
		/// </summary>
		public string assistantname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Assistant Phone
		/// Description : Type the phone number for the contact's assistant.
		/// String, length 50
		/// </summary>
		public string assistantphone { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Birthday
		/// Description : Enter the contact's birthday for use in customer gift programs or other communications.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? birthdate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Business Phone 2
		/// Description : Type a second business phone number for this contact.
		/// String, length 50
		/// </summary>
		public string business2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Business Card
		/// Description : Stores Image of the Business Card
		/// Memo, length 1073741823
		/// </summary>
		public string businesscard { get; set; }  // Memo, length 1073741823
		/// <summary>
		/// Display name: BusinessCardAttributes
		/// Description : Stores Business Card Control Properties.
		/// String, length 4000
		/// </summary>
		public string businesscardattributes { get; set; }  // String, length 4000
		/// <summary>
		/// Display name: Callback Number
		/// Description : Type a callback phone number for this contact.
		/// String, length 50
		/// </summary>
		public string callback { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Children's Names
		/// Description : Type the names of the contact's children for reference in communications and client programs.
		/// String, length 255
		/// </summary>
		public string childrensnames { get; set; }  // String, length 255
		/// <summary>
		/// Display name: Company Phone
		/// Description : Type the company phone of the contact.
		/// String, length 50
		/// </summary>
		public string company { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Contact
		/// Description : Unique identifier of the contact.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? contactid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_contactbase_createdby
		/// </summary>
		public ODataBind createdbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_contactbase_createdby
		/// Reference   : createdby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdby { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdby_value { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Schema name : lk_externalparty_contact_createdby
		/// </summary>
		public ODataBind CreatedByExternalPartyLookup { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Schema name : lk_externalparty_contact_createdby
		/// Reference   : createdbyexternalparty -> externalparty(externalpartyid)
		/// </summary>
		public dynamic CreatedByExternalParty { get; set; }
		/// <summary>
		/// Display name: Created By (External Party)
		/// Description : Shows the external party who created the record.
		/// Lookup, targets: externalparty
		/// </summary>
		public Guid? _createdbyexternalparty_value { get; set; }
		/// <summary>
		/// Attribute of: createdbyexternalparty
		/// String, length 100
		/// </summary>
		public string createdbyexternalpartyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdbyexternalparty
		/// String, length 100
		/// </summary>
		public string createdbyexternalpartyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created On
		/// Description : Shows the date and time when the record was created. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? createdon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_contact_createdonbehalfby
		/// </summary>
		public ODataBind createdonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_contact_createdonbehalfby
		/// Reference   : createdonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdonbehalfby { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Credit Limit
		/// Description : Type the credit limit of the contact for reference when you address invoice and accounting issues with the customer.
		/// Money, length -1
		/// </summary>
		public decimal? creditlimit { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Credit Limit (Base)
		/// Description : Shows the Credit Limit field converted to the system's default base currency for reporting purposes. The calculations use the exchange rate specified in the Currencies area.
		/// Money, length -1
		/// </summary>
		public decimal? creditlimit_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Credit Hold
		/// Description : Select whether the contact is on a credit hold, for reference when addressing invoice and accounting issues.
		/// Boolean, length -1
		/// </summary>
		public bool? creditonhold { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: creditonhold
		/// Virtual, length -1
		/// </summary>
		public string creditonholdname { get; set; }  // Virtual, length -1

		public enum customersizecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Customer Size
		/// Description : Select the size of the contact's company for segmentation and reporting purposes.
		/// Picklist, length -1
		/// </summary>
		public customersizecodeEnum? customersizecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: customersizecode
		/// Virtual, length -1
		/// </summary>
		public string customersizecodename { get; set; }  // Virtual, length -1

		public enum customertypecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Relationship Type
		/// Description : Select the category that best describes the relationship between the contact and your organization.
		/// Picklist, length -1
		/// </summary>
		public customertypecodeEnum? customertypecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: customertypecode
		/// Virtual, length -1
		/// </summary>
		public string customertypecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the default price list associated with the contact to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Schema name : price_level_contacts
		/// </summary>
		public ODataBind defaultpricelevelidLookup { get; set; }
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the default price list associated with the contact to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Schema name : price_level_contacts
		/// Reference   : defaultpricelevelid -> pricelevel(pricelevelid)
		/// </summary>
		public dynamic defaultpricelevelid { get; set; }
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the default price list associated with the contact to make sure the correct product prices for this customer are applied in sales opportunities, quotes, and orders.
		/// Lookup, targets: pricelevel
		/// </summary>
		public Guid? _defaultpricelevelid_value { get; set; }
		/// <summary>
		/// Attribute of: defaultpricelevelid
		/// String, length 100
		/// </summary>
		public string defaultpricelevelidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Department
		/// Description : Type the department or business unit where the contact works in the parent company or business.
		/// String, length 100
		/// </summary>
		public string department { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Description
		/// Description : Type additional information to describe the contact, such as an excerpt from the company's website.
		/// Memo, length 2000
		/// </summary>
		public string description { get; set; }  // Memo, length 2000
		/// <summary>
		/// Display name: Do not allow Bulk Emails
		/// Description : Select whether the contact accepts bulk email sent through marketing campaigns or quick campaigns. If Do Not Allow is selected, the contact can be added to marketing lists, but will be excluded from the email.
		/// Boolean, length -1
		/// </summary>
		public bool? donotbulkemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotbulkemail
		/// Virtual, length -1
		/// </summary>
		public string donotbulkemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Bulk Mails
		/// Description : Select whether the contact accepts bulk postal mail sent through marketing campaigns or quick campaigns. If Do Not Allow is selected, the contact can be added to marketing lists, but will be excluded from the letters.
		/// Boolean, length -1
		/// </summary>
		public bool? donotbulkpostalmail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotbulkpostalmail
		/// Virtual, length -1
		/// </summary>
		public string donotbulkpostalmailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Emails
		/// Description : Select whether the contact allows direct email sent from Microsoft Dynamics 365. If Do Not Allow is selected, Microsoft Dynamics 365 will not send the email.
		/// Boolean, length -1
		/// </summary>
		public bool? donotemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotemail
		/// Virtual, length -1
		/// </summary>
		public string donotemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Faxes
		/// Description : Select whether the contact allows faxes. If Do Not Allow is selected, the contact will be excluded from any fax activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotfax { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotfax
		/// Virtual, length -1
		/// </summary>
		public string donotfaxname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Phone Calls
		/// Description : Select whether the contact accepts phone calls. If Do Not Allow is selected, the contact will be excluded from any phone call activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotphone { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotphone
		/// Virtual, length -1
		/// </summary>
		public string donotphonename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Do not allow Mails
		/// Description : Select whether the contact allows direct mail. If Do Not Allow is selected, the contact will be excluded from letter activities distributed in marketing campaigns.
		/// Boolean, length -1
		/// </summary>
		public bool? donotpostalmail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: donotpostalmail
		/// Virtual, length -1
		/// </summary>
		public string donotpostalmailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: donotsendmm
		/// Virtual, length -1
		/// </summary>
		public string donotsendmarketingmaterialname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Send Marketing Materials
		/// Description : Select whether the contact accepts marketing materials, such as brochures or catalogs. Contacts that opt out can be excluded from marketing initiatives.
		/// Boolean, length -1
		/// </summary>
		public bool? donotsendmm { get; set; }  // Boolean, length -1

		public enum educationcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Education
		/// Description : Select the contact's highest level of education for use in segmentation and analysis.
		/// Picklist, length -1
		/// </summary>
		public educationcodeEnum? educationcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: educationcode
		/// Virtual, length -1
		/// </summary>
		public string educationcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Email
		/// Description : Type the primary email address for the contact.
		/// String, length 100
		/// </summary>
		public string emailaddress1 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Email Address 2
		/// Description : Type the secondary email address for the contact.
		/// String, length 100
		/// </summary>
		public string emailaddress2 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Email Address 3
		/// Description : Type an alternate email address for the contact.
		/// String, length 100
		/// </summary>
		public string emailaddress3 { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Employee
		/// Description : Type the employee ID or number for the contact for reference in orders, service cases, or other communications with the contact's organization.
		/// String, length 50
		/// </summary>
		public string employeeid { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Entity Image
		/// Description : Shows the default image for the record.
		/// Attribute of: entityimageid
		/// Virtual, length -1
		/// </summary>
		public string entityimage { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: entityimageid
		/// BigInt, length -1
		/// </summary>
		public long? entityimage_timestamp { get; set; }  // BigInt, length -1
		/// <summary>
		/// Attribute of: entityimageid
		/// String, length 200
		/// </summary>
		public string entityimage_url { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Entity Image Id
		/// Description : For internal use only.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? entityimageid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Exchange Rate
		/// Description : Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// Decimal, length -1
		/// </summary>
		public decimal? exchangerate { get; set; }  // Decimal, length -1
		/// <summary>
		/// Display name: External User Identifier
		/// Description : Identifier for an external user.
		/// String, length 50
		/// </summary>
		public string externaluseridentifier { get; set; }  // String, length 50

		public enum familystatuscodeEnum {
			Single = 1,
			Married = 2,
			Divorced = 3,
			Widowed = 4,
		}

		/// <summary>
		/// Display name: Marital Status
		/// Description : Select the marital status of the contact for reference in follow-up phone calls and other communications.
		/// Picklist, length -1
		/// </summary>
		public familystatuscodeEnum? familystatuscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: familystatuscode
		/// Virtual, length -1
		/// </summary>
		public string familystatuscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Fax
		/// Description : Type the fax number for the contact.
		/// String, length 50
		/// </summary>
		public string fax { get; set; }  // String, length 50
		/// <summary>
		/// Display name: First Name
		/// Description : Type the contact's first name to make sure the contact is addressed correctly in sales calls, email, and marketing campaigns.
		/// String, length 50
		/// </summary>
		public string firstname { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Follow Email Activity
		/// Description : Information about whether to allow following email activity like opens, attachment views and link clicks for emails sent to the contact.
		/// Boolean, length -1
		/// </summary>
		public bool? followemail { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: followemail
		/// Virtual, length -1
		/// </summary>
		public string followemailname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: FTP Site
		/// Description : Type the URL for the contact's FTP site to enable users to access data and share documents.
		/// String, length 200
		/// </summary>
		public string ftpsiteurl { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Full Name
		/// Description : Combines and shows the contact's first and last names so that the full name can be displayed in views and reports.
		/// String, length 160
		/// </summary>
		public string fullname { get; set; }  // String, length 160

		public enum gendercodeEnum {
			Male = 1,
			Female = 2,
		}

		/// <summary>
		/// Display name: Gender
		/// Description : Select the contact's gender to make sure the contact is addressed correctly in sales calls, email, and marketing campaigns.
		/// Picklist, length -1
		/// </summary>
		public gendercodeEnum? gendercode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: gendercode
		/// Virtual, length -1
		/// </summary>
		public string gendercodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Government
		/// Description : Type the passport number or other government ID for the contact for use in documents or reports.
		/// String, length 50
		/// </summary>
		public string governmentid { get; set; }  // String, length 50

		public enum haschildrencodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Has Children
		/// Description : Select whether the contact has any children for reference in follow-up phone calls and other communications.
		/// Picklist, length -1
		/// </summary>
		public haschildrencodeEnum? haschildrencode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: haschildrencode
		/// Virtual, length -1
		/// </summary>
		public string haschildrencodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Home Phone 2
		/// Description : Type a second home phone number for this contact.
		/// String, length 50
		/// </summary>
		public string home2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Import Sequence Number
		/// Description : Unique identifier of the data import or data migration that created this record.
		/// Integer, length -1
		/// </summary>
		public int? importsequencenumber { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Auto-created
		/// Description : Information about whether the contact was auto-created when promoting an email or an appointment.
		/// Boolean, length -1
		/// </summary>
		public bool? isautocreate { get; set; }  // Boolean, length -1
		/// <summary>
		/// Display name: Back Office Customer
		/// Description : Select whether the contact exists in a separate accounting or other system, such as Microsoft Dynamics GP or another ERP database, for use in integration processes.
		/// Boolean, length -1
		/// </summary>
		public bool? isbackofficecustomer { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: isbackofficecustomer
		/// Virtual, length -1
		/// </summary>
		public string isbackofficecustomername { get; set; }  // Virtual, length -1
		/// <summary>
		/// Boolean, length -1
		/// </summary>
		public bool? isprivate { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: isprivate
		/// Virtual, length -1
		/// </summary>
		public string isprivatename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Job Title
		/// Description : Type the job title of the contact to make sure the contact is addressed correctly in sales calls, email, and marketing campaigns.
		/// String, length 100
		/// </summary>
		public string jobtitle { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Last Name
		/// Description : Type the contact's last name to make sure the contact is addressed correctly in sales calls, email, and marketing campaigns.
		/// String, length 50
		/// </summary>
		public string lastname { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Last On Hold Time
		/// Description : Contains the date and time stamp of the last on hold time.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? lastonholdtime { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Last Date Included in Campaign
		/// Description : Shows the date when the contact was last included in a marketing campaign or quick campaign.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? lastusedincampaign { get; set; }  // DateTime, length -1

		public enum leadsourcecodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Lead Source
		/// Description : Select the primary marketing source that directed the contact to your organization.
		/// Picklist, length -1
		/// </summary>
		public leadsourcecodeEnum? leadsourcecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: leadsourcecode
		/// Virtual, length -1
		/// </summary>
		public string leadsourcecodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Manager
		/// Description : Type the name of the contact's manager for use in escalating issues or other follow-up communications with the contact.
		/// String, length 100
		/// </summary>
		public string managername { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Manager Phone
		/// Description : Type the phone number for the contact's manager.
		/// String, length 50
		/// </summary>
		public string managerphone { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Marketing Only
		/// Description : Whether is only for marketing
		/// Boolean, length -1
		/// </summary>
		public bool? marketingonly { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: marketingonly
		/// Virtual, length -1
		/// </summary>
		public string marketingonlyname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Attribute of: masterid
		/// String, length 100
		/// </summary>
		public string mastercontactidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: masterid
		/// String, length 100
		/// </summary>
		public string mastercontactidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Master ID
		/// Description : Unique identifier of the master contact for merge.
		/// Schema name : contact_master_contact
		/// </summary>
		public ODataBind masteridLookup { get; set; }
		/// <summary>
		/// Display name: Master ID
		/// Description : Unique identifier of the master contact for merge.
		/// Schema name : contact_master_contact
		/// Reference   : masterid -> contact(contactid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.contact masterid { get; set; }
		/// <summary>
		/// Display name: Master ID
		/// Description : Unique identifier of the master contact for merge.
		/// Lookup, targets: contact
		/// </summary>
		public Guid? _masterid_value { get; set; }
		/// <summary>
		/// Display name: Merged
		/// Description : Shows whether the account has been merged with a master contact.
		/// Boolean, length -1
		/// </summary>
		public bool? merged { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: merged
		/// Virtual, length -1
		/// </summary>
		public string mergedname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Middle Name
		/// Description : Type the contact's middle name or initial to make sure the contact is addressed correctly.
		/// String, length 50
		/// </summary>
		public string middlename { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Mobile Phone
		/// Description : Type the mobile phone number for the contact.
		/// String, length 50
		/// </summary>
		public string mobilephone { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_contactbase_modifiedby
		/// </summary>
		public ODataBind modifiedbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_contactbase_modifiedby
		/// Reference   : modifiedby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedby { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedby_value { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Schema name : lk_externalparty_contact_modifiedby
		/// </summary>
		public ODataBind ModifiedByExternalPartyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Schema name : lk_externalparty_contact_modifiedby
		/// Reference   : modifiedbyexternalparty -> externalparty(externalpartyid)
		/// </summary>
		public dynamic ModifiedByExternalParty { get; set; }
		/// <summary>
		/// Display name: Modified By (External Party)
		/// Description : Shows the external party who modified the record.
		/// Lookup, targets: externalparty
		/// </summary>
		public Guid? _modifiedbyexternalparty_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedbyexternalparty
		/// String, length 100
		/// </summary>
		public string modifiedbyexternalpartyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedbyexternalparty
		/// String, length 100
		/// </summary>
		public string modifiedbyexternalpartyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified On
		/// Description : Shows the date and time when the record was last updated. The date and time are displayed in the time zone selected in Microsoft Dynamics 365 options.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? modifiedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Schema name : lk_contact_modifiedonbehalfby
		/// </summary>
		public ODataBind modifiedonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Schema name : lk_contact_modifiedonbehalfby
		/// Reference   : modifiedonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedonbehalfby { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Contact.
		/// Schema name : msa_contact_managingpartner
		/// </summary>
		public ODataBind msa_managingpartneridLookup { get; set; }
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Contact.
		/// Schema name : msa_contact_managingpartner
		/// Reference   : msa_managingpartnerid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account msa_managingpartnerid { get; set; }
		/// <summary>
		/// Display name: Managing Partner
		/// Description : Unique identifier for Account associated with Contact.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _msa_managingpartnerid_value { get; set; }
		/// <summary>
		/// Attribute of: msa_managingpartnerid
		/// String, length 160
		/// </summary>
		public string msa_managingpartneridname { get; set; }  // String, length 160
		/// <summary>
		/// Attribute of: msa_managingpartnerid
		/// String, length 160
		/// </summary>
		public string msa_managingpartneridyominame { get; set; }  // String, length 160
		/// <summary>
		/// Display name: KPI
		/// Description : Maps to contact KPI records
		/// Schema name : msdyn_msdyn_contactkpiitem_contact_contactkpiid
		/// </summary>
		public ODataBind msdyn_contactkpiidLookup { get; set; }
		/// <summary>
		/// Display name: KPI
		/// Description : Maps to contact KPI records
		/// Schema name : msdyn_msdyn_contactkpiitem_contact_contactkpiid
		/// Reference   : msdyn_contactkpiid -> msdyn_contactkpiitem(msdyn_contactkpiitemid)
		/// </summary>
		public dynamic msdyn_contactkpiid { get; set; }
		/// <summary>
		/// Display name: KPI
		/// Description : Maps to contact KPI records
		/// Lookup, targets: msdyn_contactkpiitem
		/// </summary>
		public Guid? _msdyn_contactkpiid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_contactkpiid
		/// String, length 100
		/// </summary>
		public string msdyn_contactkpiidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Disable Web Tracking
		/// Description : Indicates that the contact has opted out of web tracking.
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_disablewebtracking { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_disablewebtracking
		/// Virtual, length -1
		/// </summary>
		public string msdyn_disablewebtrackingname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: GDPR Optout
		/// Description : Describes whether contact is opted out or not
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_gdproptout { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_gdproptout
		/// Virtual, length -1
		/// </summary>
		public string msdyn_gdproptoutname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Is Minor
		/// Description : Indicates that the contact is considered a minor in their jurisdiction.
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_isminor { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_isminor
		/// Virtual, length -1
		/// </summary>
		public string msdyn_isminorname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Is Minor with Parental Consent
		/// Description : Indicates that the contact is considered a minor in their jurisdiction and has parental consent.
		/// Boolean, length -1
		/// </summary>
		public bool? msdyn_isminorwithparentalconsent { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: msdyn_isminorwithparentalconsent
		/// Virtual, length -1
		/// </summary>
		public string msdyn_isminorwithparentalconsentname { get; set; }  // Virtual, length -1

		public enum msdyn_orgchangestatusEnum {
			NoFeedback = 0,
			NotatCompany = 1,
			Ignore = 2,
		}

		/// <summary>
		/// Display name: Not at Company Flag
		/// Description : Whether or not the contact belongs to the associated account
		/// Picklist, length -1
		/// </summary>
		public msdyn_orgchangestatusEnum? msdyn_orgchangestatus { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_orgchangestatus
		/// Virtual, length -1
		/// </summary>
		public string msdyn_orgchangestatusname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Portal Terms Agreement Date
		/// Description : Indicates the date and time that the person agreed to the portal terms and conditions.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_portaltermsagreementdate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with contact.
		/// Schema name : msdyn_msdyn_segment_contact
		/// </summary>
		public ODataBind msdyn_segmentidLookup { get; set; }
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with contact.
		/// Schema name : msdyn_msdyn_segment_contact
		/// Reference   : msdyn_segmentid -> msdyn_segment(msdyn_segmentid)
		/// </summary>
		public dynamic msdyn_segmentid { get; set; }
		/// <summary>
		/// Display name: Segment Id
		/// Description : Unique identifier for Segment associated with contact.
		/// Lookup, targets: msdyn_segment
		/// </summary>
		public Guid? _msdyn_segmentid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_segmentid
		/// String, length 100
		/// </summary>
		public string msdyn_segmentidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Nickname
		/// Description : Type the contact's nickname.
		/// String, length 100
		/// </summary>
		public string nickname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: No. of Children
		/// Description : Type the number of children the contact has for reference in follow-up phone calls and other communications.
		/// Integer, length -1
		/// </summary>
		public int? numberofchildren { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: On Hold Time (Minutes)
		/// Description : Shows how long, in minutes, that the record was on hold.
		/// Integer, length -1
		/// </summary>
		public int? onholdtime { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the contact was created if the contact was created by converting a lead in Microsoft Dynamics 365. This is used to relate the contact to the data on the originating lead for use in reporting and analytics.
		/// Schema name : contact_originating_lead
		/// </summary>
		public ODataBind originatingleadidLookup { get; set; }
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the contact was created if the contact was created by converting a lead in Microsoft Dynamics 365. This is used to relate the contact to the data on the originating lead for use in reporting and analytics.
		/// Schema name : contact_originating_lead
		/// Reference   : originatingleadid -> lead(leadid)
		/// </summary>
		public dynamic originatingleadid { get; set; }
		/// <summary>
		/// Display name: Originating Lead
		/// Description : Shows the lead that the contact was created if the contact was created by converting a lead in Microsoft Dynamics 365. This is used to relate the contact to the data on the originating lead for use in reporting and analytics.
		/// Lookup, targets: lead
		/// </summary>
		public Guid? _originatingleadid_value { get; set; }
		/// <summary>
		/// Attribute of: originatingleadid
		/// String, length 100
		/// </summary>
		public string originatingleadidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: originatingleadid
		/// String, length 100
		/// </summary>
		public string originatingleadidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Record Created On
		/// Description : Date and time that the record was migrated.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? overriddencreatedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Schema name : owner_contacts
		/// </summary>
		public ODataBind owneridLookup { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Schema name : owner_contacts
		/// Reference   : ownerid -> owner(ownerid)
		/// </summary>
		public dynamic ownerid { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Enter the user or team who is assigned to manage the record. This field is updated every time the record is assigned to a different user.
		/// Owner, targets: systemuser, team
		/// </summary>
		public Guid? _ownerid_value { get; set; }
		/// <summary>
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: ownerid
		/// EntityName, length -1
		/// </summary>
		public string owneridtype { get; set; }  // EntityName, length -1
		/// <summary>
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier of the business unit that owns the contact.
		/// Schema name : business_unit_contacts
		/// </summary>
		public ODataBind owningbusinessunitLookup { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier of the business unit that owns the contact.
		/// Schema name : business_unit_contacts
		/// Reference   : owningbusinessunit -> businessunit(businessunitid)
		/// </summary>
		public dynamic owningbusinessunit { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier of the business unit that owns the contact.
		/// Lookup, targets: businessunit
		/// </summary>
		public Guid? _owningbusinessunit_value { get; set; }
		/// <summary>
		/// Attribute of: owningbusinessunit
		/// String, length 160
		/// </summary>
		public string owningbusinessunitname { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the contact.
		/// Schema name : team_contacts
		/// </summary>
		public ODataBind owningteamLookup { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the contact.
		/// Schema name : team_contacts
		/// Reference   : owningteam -> team(teamid)
		/// </summary>
		public dynamic owningteam { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier of the team who owns the contact.
		/// Lookup, targets: team
		/// </summary>
		public Guid? _owningteam_value { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the contact.
		/// Schema name : contact_owning_user
		/// </summary>
		public ODataBind owninguserLookup { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the contact.
		/// Schema name : contact_owning_user
		/// Reference   : owninguser -> systemuser(systemuserid)
		/// </summary>
		public dynamic owninguser { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier of the user who owns the contact.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _owninguser_value { get; set; }
		/// <summary>
		/// Display name: Pager
		/// Description : Type the pager number for the contact.
		/// String, length 50
		/// </summary>
		public string pager { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Parent Contact
		/// Description : Unique identifier of the parent contact.
		/// Lookup, targets: contact
		/// </summary>
		public Guid? _parentcontactid_value { get; set; }
		/// <summary>
		/// Attribute of: parentcontactid
		/// String, length 100
		/// </summary>
		public string parentcontactidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: parentcontactid
		/// String, length 100
		/// </summary>
		public string parentcontactidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Company Name
		/// Description : Select the parent account or parent contact for the contact to provide a quick link to additional details, such as financial information, activities, and opportunities.
		/// Schema name : contact_customer_accounts
		/// </summary>
		public ODataBind parentcustomerid_accountLookup { get; set; }
		/// <summary>
		/// Display name: Company Name
		/// Description : Select the parent account or parent contact for the contact to provide a quick link to additional details, such as financial information, activities, and opportunities.
		/// Schema name : contact_customer_contacts
		/// </summary>
		public ODataBind parentcustomerid_contactLookup { get; set; }
		/// <summary>
		/// Display name: Company Name
		/// Description : Select the parent account or parent contact for the contact to provide a quick link to additional details, such as financial information, activities, and opportunities.
		/// Schema name : contact_customer_accounts
		/// Reference   : parentcustomerid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account parentcustomerid_account { get; set; }
		/// <summary>
		/// Display name: Company Name
		/// Description : Select the parent account or parent contact for the contact to provide a quick link to additional details, such as financial information, activities, and opportunities.
		/// Schema name : contact_customer_contacts
		/// Reference   : parentcustomerid -> contact(contactid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.contact parentcustomerid_contact { get; set; }
		/// <summary>
		/// Display name: Company Name
		/// Description : Select the parent account or parent contact for the contact to provide a quick link to additional details, such as financial information, activities, and opportunities.
		/// Customer, targets: account, contact
		/// </summary>
		public Guid? _parentcustomerid_value { get; set; }
		/// <summary>
		/// Attribute of: parentcustomerid
		/// String, length 160
		/// </summary>
		public string parentcustomeridname { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Parent Customer Type
		/// Attribute of: parentcustomerid
		/// EntityName, length -1
		/// </summary>
		public string parentcustomeridtype { get; set; }  // EntityName, length -1
		/// <summary>
		/// Attribute of: parentcustomerid
		/// String, length 450
		/// </summary>
		public string parentcustomeridyominame { get; set; }  // String, length 450
		/// <summary>
		/// Display name: Participates in Workflow
		/// Description : Shows whether the contact participates in workflow rules.
		/// Boolean, length -1
		/// </summary>
		public bool? participatesinworkflow { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: participatesinworkflow
		/// Virtual, length -1
		/// </summary>
		public string participatesinworkflowname { get; set; }  // Virtual, length -1

		public enum paymenttermscodeEnum {
			Net30 = 1,
			_210Net30 = 2,
			Net45 = 3,
			Net60 = 4,
		}

		/// <summary>
		/// Display name: Payment Terms
		/// Description : Select the payment terms to indicate when the customer needs to pay the total amount.
		/// Picklist, length -1
		/// </summary>
		public paymenttermscodeEnum? paymenttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: paymenttermscode
		/// Virtual, length -1
		/// </summary>
		public string paymenttermscodename { get; set; }  // Virtual, length -1

		public enum preferredappointmentdaycodeEnum {
			Sunday = 0,
			Monday = 1,
			Tuesday = 2,
			Wednesday = 3,
			Thursday = 4,
			Friday = 5,
			Saturday = 6,
		}

		/// <summary>
		/// Display name: Preferred Day
		/// Description : Select the preferred day of the week for service appointments.
		/// Picklist, length -1
		/// </summary>
		public preferredappointmentdaycodeEnum? preferredappointmentdaycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredappointmentdaycode
		/// Virtual, length -1
		/// </summary>
		public string preferredappointmentdaycodename { get; set; }  // Virtual, length -1

		public enum preferredappointmenttimecodeEnum {
			Morning = 1,
			Afternoon = 2,
			Evening = 3,
		}

		/// <summary>
		/// Display name: Preferred Time
		/// Description : Select the preferred time of day for service appointments.
		/// Picklist, length -1
		/// </summary>
		public preferredappointmenttimecodeEnum? preferredappointmenttimecode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredappointmenttimecode
		/// Virtual, length -1
		/// </summary>
		public string preferredappointmenttimecodename { get; set; }  // Virtual, length -1

		public enum preferredcontactmethodcodeEnum {
			Any = 1,
			Email = 2,
			Phone = 3,
			Fax = 4,
			Mail = 5,
		}

		/// <summary>
		/// Display name: Preferred Method of Contact
		/// Description : Select the preferred method of contact.
		/// Picklist, length -1
		/// </summary>
		public preferredcontactmethodcodeEnum? preferredcontactmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: preferredcontactmethodcode
		/// Virtual, length -1
		/// </summary>
		public string preferredcontactmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the contact's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Schema name : equipment_contacts
		/// </summary>
		public ODataBind preferredequipmentidLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the contact's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Schema name : equipment_contacts
		/// Reference   : preferredequipmentid -> equipment(equipmentid)
		/// </summary>
		public dynamic preferredequipmentid { get; set; }
		/// <summary>
		/// Display name: Preferred Facility/Equipment
		/// Description : Choose the contact's preferred service facility or equipment to make sure services are scheduled correctly for the customer.
		/// Lookup, targets: equipment
		/// </summary>
		public Guid? _preferredequipmentid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredequipmentid
		/// String, length 100
		/// </summary>
		public string preferredequipmentidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the contact's preferred service to make sure services are scheduled correctly for the customer.
		/// Schema name : service_contacts
		/// </summary>
		public ODataBind preferredserviceidLookup { get; set; }
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the contact's preferred service to make sure services are scheduled correctly for the customer.
		/// Schema name : service_contacts
		/// Reference   : preferredserviceid -> service(serviceid)
		/// </summary>
		public dynamic preferredserviceid { get; set; }
		/// <summary>
		/// Display name: Preferred Service
		/// Description : Choose the contact's preferred service to make sure services are scheduled correctly for the customer.
		/// Lookup, targets: service
		/// </summary>
		public Guid? _preferredserviceid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredserviceid
		/// String, length 100
		/// </summary>
		public string preferredserviceidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the regular or preferred customer service representative for reference when scheduling service activities for the contact.
		/// Schema name : system_user_contacts
		/// </summary>
		public ODataBind preferredsystemuseridLookup { get; set; }
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the regular or preferred customer service representative for reference when scheduling service activities for the contact.
		/// Schema name : system_user_contacts
		/// Reference   : preferredsystemuserid -> systemuser(systemuserid)
		/// </summary>
		public dynamic preferredsystemuserid { get; set; }
		/// <summary>
		/// Display name: Preferred User
		/// Description : Choose the regular or preferred customer service representative for reference when scheduling service activities for the contact.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _preferredsystemuserid_value { get; set; }
		/// <summary>
		/// Attribute of: preferredsystemuserid
		/// String, length 100
		/// </summary>
		public string preferredsystemuseridname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: preferredsystemuserid
		/// String, length 100
		/// </summary>
		public string preferredsystemuseridyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Process
		/// Description : Shows the ID of the process.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? processid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Salutation
		/// Description : Type the salutation of the contact to make sure the contact is addressed correctly in sales calls, email messages, and marketing campaigns.
		/// String, length 100
		/// </summary>
		public string salutation { get; set; }  // String, length 100

		public enum shippingmethodcodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public shippingmethodcodeEnum? shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Contact record.
		/// Schema name : manualsla_contact
		/// </summary>
		public ODataBind sla_contact_slaLookup { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Contact record.
		/// Schema name : manualsla_contact
		/// Reference   : slaid -> sla(slaid)
		/// </summary>
		public dynamic sla_contact_sla { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the Contact record.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slaid_value { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Schema name : sla_contact
		/// </summary>
		public ODataBind slainvokedid_contact_slaLookup { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Schema name : sla_contact
		/// Reference   : slainvokedid -> sla(slaid)
		/// </summary>
		public dynamic slainvokedid_contact_sla { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this case. This field is for internal use only.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slainvokedid_value { get; set; }
		/// <summary>
		/// Attribute of: slainvokedid
		/// String, length 100
		/// </summary>
		public string slainvokedidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: slaid
		/// String, length 100
		/// </summary>
		public string slaname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Spouse/Partner Name
		/// Description : Type the name of the contact's spouse or partner for reference during calls, events, or other communications with the contact.
		/// String, length 100
		/// </summary>
		public string spousesname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Process Stage
		/// Description : Shows the ID of the stage.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? stageid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Subscription
		/// Description : For internal use only.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? subscriptionid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Suffix
		/// Description : Type the suffix used in the contact's name, such as Jr. or Sr. to make sure the contact is addressed correctly in sales calls, email, and marketing campaigns.
		/// String, length 10
		/// </summary>
		public string suffix { get; set; }  // String, length 10
		/// <summary>
		/// Display name: TeamsFollowed
		/// Description : Number of users or conversations followed the record
		/// Integer, length -1
		/// </summary>
		public int? teamsfollowed { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Business Phone
		/// Description : Type the main phone number for this contact.
		/// String, length 50
		/// </summary>
		public string telephone1 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Home Phone
		/// Description : Type a second phone number for this contact.
		/// String, length 50
		/// </summary>
		public string telephone2 { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Telephone 3
		/// Description : Type a third phone number for this contact.
		/// String, length 50
		/// </summary>
		public string telephone3 { get; set; }  // String, length 50

		public enum territorycodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Territory
		/// Description : Select a region or territory for the contact for use in segmentation and analysis.
		/// Picklist, length -1
		/// </summary>
		public territorycodeEnum? territorycode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: territorycode
		/// Virtual, length -1
		/// </summary>
		public string territorycodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Time Spent by me
		/// Description : Total time spent for emails (read and write) and meetings by me in relation to the contact record.
		/// String, length 1250
		/// </summary>
		public string timespentbymeonemailandmeetings { get; set; }  // String, length 1250
		/// <summary>
		/// Display name: Time Zone Rule Version Number
		/// Description : For internal use only.
		/// Integer, length -1
		/// </summary>
		public int? timezoneruleversionnumber { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_contact
		/// </summary>
		public ODataBind transactioncurrencyidLookup { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_contact
		/// Reference   : transactioncurrencyid -> transactioncurrency(transactioncurrencyid)
		/// </summary>
		public dynamic transactioncurrencyid { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Lookup, targets: transactioncurrency
		/// </summary>
		public Guid? _transactioncurrencyid_value { get; set; }
		/// <summary>
		/// Attribute of: transactioncurrencyid
		/// String, length 100
		/// </summary>
		public string transactioncurrencyidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Traversed Path
		/// Description : For internal use only.
		/// String, length 1250
		/// </summary>
		public string traversedpath { get; set; }  // String, length 1250
		/// <summary>
		/// Display name: UTC Conversion Time Zone Code
		/// Description : Time zone code that was in use when the record was created.
		/// Integer, length -1
		/// </summary>
		public int? utcconversiontimezonecode { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Version Number
		/// Description : Version number of the contact.
		/// BigInt, length -1
		/// </summary>
		public long? versionnumber { get; set; }  // BigInt, length -1
		/// <summary>
		/// Display name: Website
		/// Description : Type the contact's professional or personal website or blog URL.
		/// String, length 200
		/// </summary>
		public string websiteurl { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Yomi First Name
		/// Description : Type the phonetic spelling of the contact's first name, if the name is specified in Japanese, to make sure the name is pronounced correctly in phone calls with the contact.
		/// String, length 150
		/// </summary>
		public string yomifirstname { get; set; }  // String, length 150
		/// <summary>
		/// Display name: Yomi Full Name
		/// Description : Shows the combined Yomi first and last names of the contact so that the full phonetic name can be displayed in views and reports.
		/// String, length 450
		/// </summary>
		public string yomifullname { get; set; }  // String, length 450
		/// <summary>
		/// Display name: Yomi Last Name
		/// Description : Type the phonetic spelling of the contact's last name, if the name is specified in Japanese, to make sure the name is pronounced correctly in phone calls with the contact.
		/// String, length 150
		/// </summary>
		public string yomilastname { get; set; }  // String, length 150
		/// <summary>
		/// Display name: Yomi Middle Name
		/// Description : Type the phonetic spelling of the contact's middle name, if the name is specified in Japanese, to make sure the name is pronounced correctly in phone calls with the contact.
		/// String, length 150
		/// </summary>
		public string yomimiddlename { get; set; }  // String, length 150
	}
}

namespace EAI.Dataverse.ModelGeneratorTests.Model
{
	/// <summary>
	/// Display name: Quote
	/// Description : Formal offer for products and/or services, proposed at specific prices and related payment terms, which is sent to a prospective customer.
	/// </summary>
	public partial class quote
	{
		public const string EntitySet = "quotes";

		public ODataType ODataType { get => new ODataType() { Name = "Microsoft.Dynamics.CRM.Quote" }; }

		public static ODataBind ToLookup(Guid? guid) => guid == null ? null : new ODataBind() {EntityName = EntitySet, EntityId = guid.Value};

		public ODataBind ToLookup() => ToLookup(quoteid);

		/// <summary>
		/// Display name: Account
		/// Description : Unique identifier of the account with which the quote is associated.
		/// Lookup, targets: account
		/// </summary>
		public Guid? _accountid_value { get; set; }
		/// <summary>
		/// Attribute of: accountid
		/// String, length 100
		/// </summary>
		public string accountidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: accountid
		/// String, length 100
		/// </summary>
		public string accountidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Bill To Address ID
		/// Description : Unique identifier of the billing address.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? billto_addressid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Bill To City
		/// Description : Type the city for the customer's billing address.
		/// String, length 80
		/// </summary>
		public string billto_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Bill To Address
		/// Description : Shows the complete Bill To address.
		/// Memo, length 1000
		/// </summary>
		public string billto_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Bill To Contact Name
		/// Description : Type the primary contact name at the customer's billing address.
		/// String, length 150
		/// </summary>
		public string billto_contactname { get; set; }  // String, length 150
		/// <summary>
		/// Display name: Bill To Country/Region
		/// Description : Type the country or region for the customer's billing address.
		/// String, length 80
		/// </summary>
		public string billto_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Bill To Fax
		/// Description : Type the fax number for the customer's billing address.
		/// String, length 50
		/// </summary>
		public string billto_fax { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Bill To Street 1
		/// Description : Type the first line of the customer's billing address.
		/// String, length 250
		/// </summary>
		public string billto_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Bill To Street 2
		/// Description : Type the second line of the customer's billing address.
		/// String, length 250
		/// </summary>
		public string billto_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Bill To Street 3
		/// Description : Type the third line of the billing address.
		/// String, length 250
		/// </summary>
		public string billto_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Bill To Name
		/// Description : Type a name for the customer's billing address, such as "Headquarters" or "Field office", to identify the address.
		/// String, length 200
		/// </summary>
		public string billto_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Bill To ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the billing address.
		/// String, length 20
		/// </summary>
		public string billto_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Bill To State/Province
		/// Description : Type the state or province for the billing address.
		/// String, length 50
		/// </summary>
		public string billto_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Bill To Phone
		/// Description : Type the phone number for the customer's billing address.
		/// String, length 50
		/// </summary>
		public string billto_telephone { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Source Campaign
		/// Description : Shows the campaign that the order was created from.
		/// Schema name : campaign_quotes
		/// </summary>
		public ODataBind campaignidLookup { get; set; }
		/// <summary>
		/// Display name: Source Campaign
		/// Description : Shows the campaign that the order was created from.
		/// Schema name : campaign_quotes
		/// Reference   : campaignid -> campaign(campaignid)
		/// </summary>
		public dynamic campaignid { get; set; }
		/// <summary>
		/// Display name: Source Campaign
		/// Description : Shows the campaign that the order was created from.
		/// Lookup, targets: campaign
		/// </summary>
		public Guid? _campaignid_value { get; set; }
		/// <summary>
		/// Attribute of: campaignid
		/// String, length 100
		/// </summary>
		public string campaignidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Closed On
		/// Description : Enter the date when the quote was closed to indicate the expiration, revision, or cancellation date.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? closedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Contact
		/// Description : Unique identifier of the contact associated with the quote.
		/// Lookup, targets: contact
		/// </summary>
		public Guid? _contactid_value { get; set; }
		/// <summary>
		/// Attribute of: contactid
		/// String, length 100
		/// </summary>
		public string contactidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: contactid
		/// String, length 100
		/// </summary>
		public string contactidyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_quotebase_createdby
		/// </summary>
		public ODataBind createdbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Schema name : lk_quotebase_createdby
		/// Reference   : createdby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdby { get; set; }
		/// <summary>
		/// Display name: Created By
		/// Description : Shows who created the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdby_value { get; set; }
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdby
		/// String, length 100
		/// </summary>
		public string createdbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Created On
		/// Description : Date and time when the record was created.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? createdon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_quote_createdonbehalfby
		/// </summary>
		public ODataBind createdonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Schema name : lk_quote_createdonbehalfby
		/// Reference   : createdonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic createdonbehalfby { get; set; }
		/// <summary>
		/// Display name: Created By (Delegate)
		/// Description : Shows who created the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _createdonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: createdonbehalfby
		/// String, length 100
		/// </summary>
		public string createdonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
		/// Schema name : quote_customer_accounts
		/// </summary>
		public ODataBind customerid_accountLookup { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
		/// Schema name : quote_customer_contacts
		/// </summary>
		public ODataBind customerid_contactLookup { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
		/// Schema name : quote_customer_accounts
		/// Reference   : customerid -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account customerid_account { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
		/// Schema name : quote_customer_contacts
		/// Reference   : customerid -> contact(contactid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.contact customerid_contact { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Select the customer account or contact to provide a quick link to additional customer details, such as account information, activities, and opportunities.
		/// Customer, targets: account, contact
		/// </summary>
		public Guid? _customerid_value { get; set; }
		/// <summary>
		/// Attribute of: customerid
		/// String, length 160
		/// </summary>
		public string customeridname { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Potential Customer Type
		/// Attribute of: customerid
		/// EntityName, length -1
		/// </summary>
		public string customeridtype { get; set; }  // EntityName, length -1
		/// <summary>
		/// Attribute of: customerid
		/// String, length 450
		/// </summary>
		public string customeridyominame { get; set; }  // String, length 450
		/// <summary>
		/// Display name: Description
		/// Description : Type additional information to describe the quote, such as the products or services offered or details about the customer's product preferences.
		/// Memo, length 2000
		/// </summary>
		public string description { get; set; }  // Memo, length 2000
		/// <summary>
		/// Display name: Quote Discount Amount
		/// Description : Type the discount amount for the quote if the customer is eligible for special savings.
		/// Money, length -1
		/// </summary>
		public decimal? discountamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Quote Discount Amount (Base)
		/// Description : Value of the Quote Discount Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? discountamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Quote Discount (%)
		/// Description : Type the discount rate that should be applied to the Detail Amount field to include additional savings for the customer in the quote.
		/// Decimal, length -1
		/// </summary>
		public decimal? discountpercentage { get; set; }  // Decimal, length -1
		/// <summary>
		/// Display name: Effective from
		/// Description : Enter the date when the quote pricing is effective or was first communicated to the customer.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? effectivefrom { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Effective to
		/// Description : Enter the expiration date or last day the quote pricing is effective for the customer.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? effectiveto { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Email Address
		/// Description : The primary email address for the entity.
		/// String, length 100
		/// </summary>
		public string emailaddress { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Exchange Rate
		/// Description : Shows the conversion rate of the record's currency. The exchange rate is used to convert all money fields in the record from the local currency to the system's default currency.
		/// Decimal, length -1
		/// </summary>
		public decimal? exchangerate { get; set; }  // Decimal, length -1
		/// <summary>
		/// Display name: Due By
		/// Description : Enter the date a decision or order is due from the customer to indicate the expiration date of the quote.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? expireson { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Freight Amount
		/// Description : Type the cost of freight or shipping for the products included in the quote for use in calculating the Total Amount field.
		/// Money, length -1
		/// </summary>
		public decimal? freightamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Freight Amount (Base)
		/// Description : Value of the Freight Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? freightamount_base { get; set; }  // Money, length -1

		public enum freighttermscodeEnum {
			FOB = 1,
			NoCharge = 2,
		}

		/// <summary>
		/// Display name: Freight Terms
		/// Description : Select the freight terms to make sure shipping charges are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public freighttermscodeEnum? freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Import Sequence Number
		/// Description : Sequence number of the import that created this record.
		/// Integer, length -1
		/// </summary>
		public int? importsequencenumber { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Last On Hold Time
		/// Description : Contains the date time stamp of the last on hold time.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? lastonholdtime { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_quotebase_modifiedby
		/// </summary>
		public ODataBind modifiedbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Schema name : lk_quotebase_modifiedby
		/// Reference   : modifiedby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedby { get; set; }
		/// <summary>
		/// Display name: Modified By
		/// Description : Shows who last updated the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedby_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedby
		/// String, length 100
		/// </summary>
		public string modifiedbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Modified On
		/// Description : Date and time when the record was modified.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? modifiedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Schema name : lk_quote_modifiedonbehalfby
		/// </summary>
		public ODataBind modifiedonbehalfbyLookup { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Schema name : lk_quote_modifiedonbehalfby
		/// Reference   : modifiedonbehalfby -> systemuser(systemuserid)
		/// </summary>
		public dynamic modifiedonbehalfby { get; set; }
		/// <summary>
		/// Display name: Modified By (Delegate)
		/// Description : Shows who last updated the record on behalf of another user.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _modifiedonbehalfby_value { get; set; }
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: modifiedonbehalfby
		/// String, length 100
		/// </summary>
		public string modifiedonbehalfbyyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Customer Account associated with this Quote
		/// Schema name : msdyn_account_quote_Account
		/// </summary>
		public ODataBind msdyn_accountLookup { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Customer Account associated with this Quote
		/// Schema name : msdyn_account_quote_Account
		/// Reference   : msdyn_account -> account(accountid)
		/// </summary>
		public EAI.Dataverse.ModelGeneratorTests.Model.account msdyn_account { get; set; }
		/// <summary>
		/// Display name: Potential Customer
		/// Description : Customer Account associated with this Quote
		/// Lookup, targets: account
		/// </summary>
		public Guid? _msdyn_account_value { get; set; }
		/// <summary>
		/// Display name: Account Manager
		/// Description : Account manager responsible for the quote.
		/// Schema name : msdyn_accountmanager_quote
		/// </summary>
		public ODataBind msdyn_AccountManagerIdLookup { get; set; }
		/// <summary>
		/// Display name: Account Manager
		/// Description : Account manager responsible for the quote.
		/// Schema name : msdyn_accountmanager_quote
		/// Reference   : msdyn_accountmanagerid -> systemuser(systemuserid)
		/// </summary>
		public dynamic msdyn_AccountManagerId { get; set; }
		/// <summary>
		/// Display name: Account Manager
		/// Description : Account manager responsible for the quote.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _msdyn_accountmanagerid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_accountmanagerid
		/// String, length 200
		/// </summary>
		public string msdyn_accountmanageridname { get; set; }  // String, length 200
		/// <summary>
		/// Attribute of: msdyn_accountmanagerid
		/// String, length 200
		/// </summary>
		public string msdyn_accountmanageridyominame { get; set; }  // String, length 200
		/// <summary>
		/// Attribute of: msdyn_account
		/// String, length 160
		/// </summary>
		public string msdyn_accountname { get; set; }  // String, length 160
		/// <summary>
		/// Attribute of: msdyn_account
		/// String, length 160
		/// </summary>
		public string msdyn_accountyominame { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Adjusted Gross Margin (%)
		/// Description : Shows the estimated gross margin after accounting for non-chargeable components.
		/// Decimal, length -1
		/// </summary>
		public decimal? msdyn_adjustedgrossmargin { get; set; }  // Decimal, length -1

		public enum msdyn_competitiveEnum {
			CustomerBudgetNotAvailable = 192350000,
			WithinCustomerExpectations = 192350001,
			OutsideCustomerExpectations = 192350002,
		}

		/// <summary>
		/// Display name: Competitive
		/// Description : Shows how the quote estimation of sales value and schedule compare to customer expectations on those parameters. Possible values are 1: Within Customer expectations, 2: Not Within Customer expectations, and 0: Customer expectations Not Available.
		/// Picklist, length -1
		/// </summary>
		public msdyn_competitiveEnum? msdyn_competitive { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_competitive
		/// Virtual, length -1
		/// </summary>
		public string msdyn_competitivename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Contracting Unit
		/// Description : The organizational unit in charge of the contract.
		/// Schema name : msdyn_organizationalunit_quote
		/// </summary>
		public ODataBind msdyn_ContractOrganizationalUnitIdLookup { get; set; }
		/// <summary>
		/// Display name: Contracting Unit
		/// Description : The organizational unit in charge of the contract.
		/// Schema name : msdyn_organizationalunit_quote
		/// Reference   : msdyn_contractorganizationalunitid -> msdyn_organizationalunit(msdyn_organizationalunitid)
		/// </summary>
		public dynamic msdyn_ContractOrganizationalUnitId { get; set; }
		/// <summary>
		/// Display name: Contracting Unit
		/// Description : The organizational unit in charge of the contract.
		/// Lookup, targets: msdyn_organizationalunit
		/// </summary>
		public Guid? _msdyn_contractorganizationalunitid_value { get; set; }
		/// <summary>
		/// Attribute of: msdyn_contractorganizationalunitid
		/// String, length 100
		/// </summary>
		public string msdyn_contractorganizationalunitidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Customer Budget
		/// Description : Shows the total customer budget for the quote, computed from all the quote lines.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_customerbudgetrollup { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Customer Budget (Base)
		/// Description : Shows the value of the customer budget in the base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_customerbudgetrollup_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Customer Budget (Last Updated On)
		/// Description : Last Updated time of rollup field Customer Budget.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_customerbudgetrollup_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Customer Budget (State)
		/// Description : State of rollup field Customer Budget.
		/// Integer, length -1
		/// </summary>
		public int? msdyn_customerbudgetrollup_state { get; set; }  // Integer, length -1

		public enum msdyn_estimatedbudgetEnum {
			BudgetEstimateNotAvailable = 192350000,
			WithinCustomerBudget = 192350001,
			ExceedsCustomerBudget = 192350002,
		}

		/// <summary>
		/// Display name: Estimated Budget
		/// Description : Shows how the estimated sales value on the quote compares to customer budgets. Possible values are 1: Within Customer Budget, 2: Exceeds Customer Budget, 0: Budget Estimate Not Available
		/// Picklist, length -1
		/// </summary>
		public msdyn_estimatedbudgetEnum? msdyn_estimatedbudget { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_estimatedbudget
		/// Virtual, length -1
		/// </summary>
		public string msdyn_estimatedbudgetname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Estimated Completion
		/// Description : Estimated completion date, computed from the details of each quote line.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_estimatedcompletionrollup { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Estimated Completion (Last Updated On)
		/// Description : Last Updated time of rollup field Estimated Completion.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_estimatedcompletionrollup_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Estimated Completion (State)
		/// Description : State of rollup field Estimated Completion.
		/// Integer, length -1
		/// </summary>
		public int? msdyn_estimatedcompletionrollup_state { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Estimated Cost
		/// Description : The estimated cost of this quote
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_estimatedcost { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Estimated Cost (Base)
		/// Description : Value of the Estimated Cost in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_estimatedcost_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Estimated Margin
		/// Description : Estimated Margin of this quote
		/// Decimal, length -1
		/// </summary>
		public decimal? msdyn_estimatedquotemargin { get; set; }  // Decimal, length -1

		public enum msdyn_estimatedscheduleEnum {
			ScheduleNotAvailable = 192350000,
			EstimatedToFinishEarly = 192350001,
			EstimatedToFinishLate = 192350002,
			EstimatedToFinishOnSchedule = 192350003,
		}

		/// <summary>
		/// Display name: Estimated Schedule
		/// Description : Shows how the estimated schedule on the quote compares to customer expectations. Possible values are 1: Estimated To Finish Early, 2: Estimated To Finish Late, 3: Estimated To Finish On Schedule, and 0: Schedule Not Available.
		/// Picklist, length -1
		/// </summary>
		public msdyn_estimatedscheduleEnum? msdyn_estimatedschedule { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_estimatedschedule
		/// Virtual, length -1
		/// </summary>
		public string msdyn_estimatedschedulename { get; set; }  // Virtual, length -1

		public enum msdyn_feasibleEnum {
			FeasibilityNotAvailable = 192350000,
			Feasible = 192350001,
			NotFeasible = 192350002,
		}

		/// <summary>
		/// Display name: Feasibility
		/// Description : Shows how the quote estimation compares to project estimation. Possible values are 0: Feasibility Not Available, 1: Feasible, and 2: Not Feasible.
		/// Picklist, length -1
		/// </summary>
		public msdyn_feasibleEnum? msdyn_feasible { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_feasible
		/// Virtual, length -1
		/// </summary>
		public string msdyn_feasiblename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Gross Margin (%)
		/// Description : Shows the estimated gross margin without accounting for non-chargeable components.
		/// Decimal, length -1
		/// </summary>
		public decimal? msdyn_grossmargin { get; set; }  // Decimal, length -1
		/// <summary>
		/// Display name: Invoice Setup Totals
		/// Description : The totals of all assigned Invoice Setups
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_invoicesetuptotals { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Invoice Setup Totals (Base)
		/// Description : Value of the Invoice Setup Totals in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_invoicesetuptotals_base { get; set; }  // Money, length -1

		public enum msdyn_ordertypeEnum {
			Itembased = 192350000,
			ServiceMaintenanceBased = 690970002,
			Workbased = 192350001,
		}

		/// <summary>
		/// Display name: Order Type
		/// Description : Internal use only.
		/// Picklist, length -1
		/// </summary>
		public msdyn_ordertypeEnum? msdyn_ordertype { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_ordertype
		/// Virtual, length -1
		/// </summary>
		public string msdyn_ordertypename { get; set; }  // Virtual, length -1

		public enum msdyn_profitabilityEnum {
			ProfitabilityNotAvailable = 192350000,
			Profitable = 192350001,
			NotProfitable = 192350002,
		}

		/// <summary>
		/// Display name: Profitability
		/// Description : Shows the estimated profitability of the quote. Possible values are Profitable, Not Profitable, and Profitability not available.
		/// Picklist, length -1
		/// </summary>
		public msdyn_profitabilityEnum? msdyn_profitability { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: msdyn_profitability
		/// Virtual, length -1
		/// </summary>
		public string msdyn_profitabilityname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Latest Quote Line End Date
		/// Description : The latest end date of all associated quote lines
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_quotelineenddate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Earliest Quote Line Start Date
		/// Description : The earliest Start Date of all Quote Lines that are associated to this quote
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_quotelinestartdate { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: TotalAmount
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: TotalAmount (Base)
		/// Description : Value of the TotalAmount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Chargeable Cost
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalchargeablecostrollup { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Chargeable Cost (Base)
		/// Description : Value of the Total Chargeable Cost in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalchargeablecostrollup_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Chargeable Cost (Last Updated On)
		/// Description : Last Updated time of rollup field Total Chargeable Cost.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_totalchargeablecostrollup_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Total Chargeable Cost (State)
		/// Description : State of rollup field Total Chargeable Cost.
		/// Integer, length -1
		/// </summary>
		public int? msdyn_totalchargeablecostrollup_state { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Total Non-chargeable Cost
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalnonchargeablecostrollup { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Non-chargeable Cost (Base)
		/// Description : Value of the Total Non-chargeable Cost in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? msdyn_totalnonchargeablecostrollup_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Non-chargeable Cost (Last Updated On)
		/// Description : Last Updated time of rollup field Total Non-chargeable Cost.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? msdyn_totalnonchargeablecostrollup_date { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Total Non-chargeable Cost (State)
		/// Description : State of rollup field Total Non-chargeable Cost.
		/// Integer, length -1
		/// </summary>
		public int? msdyn_totalnonchargeablecostrollup_state { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Name
		/// Description : Type a descriptive name for the quote.
		/// String, length 300
		/// </summary>
		public string name { get; set; }  // String, length 300
		/// <summary>
		/// Display name: On Hold Time (Minutes)
		/// Description : Shows the duration in minutes for which the quote was on hold.
		/// Integer, length -1
		/// </summary>
		public int? onholdtime { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Opportunity
		/// Description : Choose the opportunity that the quote is related to for reporting and analytics.
		/// Schema name : opportunity_quotes
		/// </summary>
		public ODataBind opportunityidLookup { get; set; }
		/// <summary>
		/// Display name: Opportunity
		/// Description : Choose the opportunity that the quote is related to for reporting and analytics.
		/// Schema name : opportunity_quotes
		/// Reference   : opportunityid -> opportunity(opportunityid)
		/// </summary>
		public dynamic opportunityid { get; set; }
		/// <summary>
		/// Display name: Opportunity
		/// Description : Choose the opportunity that the quote is related to for reporting and analytics.
		/// Lookup, targets: opportunity
		/// </summary>
		public Guid? _opportunityid_value { get; set; }
		/// <summary>
		/// Attribute of: opportunityid
		/// String, length 100
		/// </summary>
		public string opportunityidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Record Created On
		/// Description : Date and time that the record was migrated.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? overriddencreatedon { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Owner
		/// Description : Owner Id
		/// Schema name : owner_quotes
		/// </summary>
		public ODataBind owneridLookup { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Owner Id
		/// Schema name : owner_quotes
		/// Reference   : ownerid -> owner(ownerid)
		/// </summary>
		public dynamic ownerid { get; set; }
		/// <summary>
		/// Display name: Owner
		/// Description : Owner Id
		/// Owner, targets: systemuser, team
		/// </summary>
		public Guid? _ownerid_value { get; set; }
		/// <summary>
		/// Description : Name of the owner
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridname { get; set; }  // String, length 100
		/// <summary>
		/// Description : Owner Id Type
		/// Attribute of: ownerid
		/// EntityName, length -1
		/// </summary>
		public string owneridtype { get; set; }  // EntityName, length -1
		/// <summary>
		/// Description : Yomi name of the owner
		/// Attribute of: ownerid
		/// String, length 100
		/// </summary>
		public string owneridyominame { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier for the business unit that owns the record
		/// Schema name : business_unit_quotes
		/// </summary>
		public ODataBind owningbusinessunitLookup { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier for the business unit that owns the record
		/// Schema name : business_unit_quotes
		/// Reference   : owningbusinessunit -> businessunit(businessunitid)
		/// </summary>
		public dynamic owningbusinessunit { get; set; }
		/// <summary>
		/// Display name: Owning Business Unit
		/// Description : Unique identifier for the business unit that owns the record
		/// Lookup, targets: businessunit
		/// </summary>
		public Guid? _owningbusinessunit_value { get; set; }
		/// <summary>
		/// Attribute of: owningbusinessunit
		/// String, length 160
		/// </summary>
		public string owningbusinessunitname { get; set; }  // String, length 160
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier for the team that owns the record.
		/// Schema name : team_quotes
		/// </summary>
		public ODataBind owningteamLookup { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier for the team that owns the record.
		/// Schema name : team_quotes
		/// Reference   : owningteam -> team(teamid)
		/// </summary>
		public dynamic owningteam { get; set; }
		/// <summary>
		/// Display name: Owning Team
		/// Description : Unique identifier for the team that owns the record.
		/// Lookup, targets: team
		/// </summary>
		public Guid? _owningteam_value { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier for the user that owns the record.
		/// Schema name : system_user_quotes
		/// </summary>
		public ODataBind owninguserLookup { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier for the user that owns the record.
		/// Schema name : system_user_quotes
		/// Reference   : owninguser -> systemuser(systemuserid)
		/// </summary>
		public dynamic owninguser { get; set; }
		/// <summary>
		/// Display name: Owning User
		/// Description : Unique identifier for the user that owns the record.
		/// Lookup, targets: systemuser
		/// </summary>
		public Guid? _owninguser_value { get; set; }

		public enum paymenttermscodeEnum {
			Net30 = 1,
			_210Net30 = 2,
			Net45 = 3,
			Net60 = 4,
		}

		/// <summary>
		/// Display name: Payment Terms
		/// Description : Select the payment terms to indicate when the customer needs to pay the total amount.
		/// Picklist, length -1
		/// </summary>
		public paymenttermscodeEnum? paymenttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: paymenttermscode
		/// Virtual, length -1
		/// </summary>
		public string paymenttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the price list associated with this record to make sure the products associated with the campaign are offered at the correct prices.
		/// Schema name : price_level_quotes
		/// </summary>
		public ODataBind pricelevelidLookup { get; set; }
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the price list associated with this record to make sure the products associated with the campaign are offered at the correct prices.
		/// Schema name : price_level_quotes
		/// Reference   : pricelevelid -> pricelevel(pricelevelid)
		/// </summary>
		public dynamic pricelevelid { get; set; }
		/// <summary>
		/// Display name: Price List
		/// Description : Choose the price list associated with this record to make sure the products associated with the campaign are offered at the correct prices.
		/// Lookup, targets: pricelevel
		/// </summary>
		public Guid? _pricelevelid_value { get; set; }
		/// <summary>
		/// Attribute of: pricelevelid
		/// String, length 100
		/// </summary>
		public string pricelevelidname { get; set; }  // String, length 100

		public enum pricingerrorcodeEnum {
			None = 0,
			DetailError = 1,
			MissingPriceLevel = 2,
			InactivePriceLevel = 3,
			MissingQuantity = 4,
			MissingUnitPrice = 5,
			MissingProduct = 6,
			InvalidProduct = 7,
			MissingPricingCode = 8,
			InvalidPricingCode = 9,
			MissingUOM = 10,
			ProductNotInPriceLevel = 11,
			MissingPriceLevelAmount = 12,
			MissingPriceLevelPercentage = 13,
			MissingPrice = 14,
			MissingCurrentCost = 15,
			MissingStandardCost = 16,
			InvalidPriceLevelAmount = 17,
			InvalidPriceLevelPercentage = 18,
			InvalidPrice = 19,
			InvalidCurrentCost = 20,
			InvalidStandardCost = 21,
			InvalidRoundingPolicy = 22,
			InvalidRoundingOption = 23,
			InvalidRoundingAmount = 24,
			PriceCalculationError = 25,
			InvalidDiscountType = 26,
			DiscountTypeInvalidState = 27,
			InvalidDiscount = 28,
			InvalidQuantity = 29,
			InvalidPricingPrecision = 30,
			MissingProductDefaultUOM = 31,
			MissingProductUOMSchedule = 32,
			InactiveDiscountType = 33,
			InvalidPriceLevelCurrency = 34,
			PriceAttributeOutOfRange = 35,
			BaseCurrencyAttributeOverflow = 36,
			BaseCurrencyAttributeUnderflow = 37,
			Transactioncurrencyisnotsetfortheproductpricelistitem = 38,
		}

		/// <summary>
		/// Display name: Pricing Error 
		/// Description : Pricing error for the quote.
		/// Picklist, length -1
		/// </summary>
		public pricingerrorcodeEnum? pricingerrorcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: pricingerrorcode
		/// Virtual, length -1
		/// </summary>
		public string pricingerrorcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Process Id
		/// Description : Contains the id of the process associated with the entity.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? processid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Quote
		/// Description : Unique identifier of the quote.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? quoteid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Quote ID
		/// Description : Shows the quote number for customer reference and searching capabilities. The number cannot be modified.
		/// String, length 100
		/// </summary>
		public string quotenumber { get; set; }  // String, length 100
		/// <summary>
		/// Display name: Requested Delivery Date
		/// Description : Enter the delivery date requested by the customer for all products in the quote.
		/// DateTime, length -1
		/// </summary>
		public DateTimeOffset? requestdeliveryby { get; set; }  // DateTime, length -1
		/// <summary>
		/// Display name: Revision ID
		/// Description : Shows the version number of the quote for revision history tracking.
		/// Integer, length -1
		/// </summary>
		public int? revisionnumber { get; set; }  // Integer, length -1

		public enum shippingmethodcodeEnum {
			Airborne = 1,
			DHL = 2,
			FedEx = 3,
			UPS = 4,
			PostalMail = 5,
			FullLoad = 6,
			WillCall = 7,
		}

		/// <summary>
		/// Display name: Shipping Method
		/// Description : Select a shipping method for deliveries sent to this address.
		/// Picklist, length -1
		/// </summary>
		public shippingmethodcodeEnum? shippingmethodcode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: shippingmethodcode
		/// Virtual, length -1
		/// </summary>
		public string shippingmethodcodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Ship To Address ID
		/// Description : Unique identifier of the shipping address.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? shipto_addressid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Ship To City
		/// Description : Type the city for the customer's shipping address.
		/// String, length 80
		/// </summary>
		public string shipto_city { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Ship To Address
		/// Description : Shows the complete Ship To address.
		/// Memo, length 1000
		/// </summary>
		public string shipto_composite { get; set; }  // Memo, length 1000
		/// <summary>
		/// Display name: Ship To Contact Name
		/// Description : Type the primary contact name at the customer's shipping address.
		/// String, length 150
		/// </summary>
		public string shipto_contactname { get; set; }  // String, length 150
		/// <summary>
		/// Display name: Ship To Country/Region
		/// Description : Type the country or region for the customer's shipping address.
		/// String, length 80
		/// </summary>
		public string shipto_country { get; set; }  // String, length 80
		/// <summary>
		/// Display name: Ship To Fax
		/// Description : Type the fax number for the customer's shipping address.
		/// String, length 50
		/// </summary>
		public string shipto_fax { get; set; }  // String, length 50

		public enum shipto_freighttermscodeEnum {
			DefaultValue = 1,
		}

		/// <summary>
		/// Display name: Ship To Freight Terms
		/// Description : Select the freight terms to make sure shipping orders are processed correctly.
		/// Picklist, length -1
		/// </summary>
		public shipto_freighttermscodeEnum? shipto_freighttermscode { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: shipto_freighttermscode
		/// Virtual, length -1
		/// </summary>
		public string shipto_freighttermscodename { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: Ship To Street 1
		/// Description : Type the first line of the customer's shipping address.
		/// String, length 250
		/// </summary>
		public string shipto_line1 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Ship To Street 2
		/// Description : Type the second line of the customer's shipping address.
		/// String, length 250
		/// </summary>
		public string shipto_line2 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Ship To Street 3
		/// Description : Type the third line of the shipping address.
		/// String, length 250
		/// </summary>
		public string shipto_line3 { get; set; }  // String, length 250
		/// <summary>
		/// Display name: Ship To Name
		/// Description : Type a name for the customer's shipping address, such as "Headquarters" or "Field office",  to identify the address.
		/// String, length 200
		/// </summary>
		public string shipto_name { get; set; }  // String, length 200
		/// <summary>
		/// Display name: Ship To ZIP/Postal Code
		/// Description : Type the ZIP Code or postal code for the shipping address.
		/// String, length 20
		/// </summary>
		public string shipto_postalcode { get; set; }  // String, length 20
		/// <summary>
		/// Display name: Ship To State/Province
		/// Description : Type the state or province for the shipping address.
		/// String, length 50
		/// </summary>
		public string shipto_stateorprovince { get; set; }  // String, length 50
		/// <summary>
		/// Display name: Ship To Phone
		/// Description : Type the phone number for the customer's shipping address.
		/// String, length 50
		/// </summary>
		public string shipto_telephone { get; set; }  // String, length 50

		public enum skippricecalculationEnum {
			DoPriceCalcAlways = 0,
			SkipPriceCalcOnRetrieve = 1,
		}

		/// <summary>
		/// Display name: Skip Price Calculation
		/// Description : Skip Price Calculation (For Internal use)
		/// Picklist, length -1
		/// </summary>
		public skippricecalculationEnum? skippricecalculation { get; set; }  // Picklist, length -1
		/// <summary>
		/// Attribute of: skippricecalculation
		/// Virtual, length -1
		/// </summary>
		public string skippricecalculationname { get; set; }  // Virtual, length -1
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the quote record.
		/// Schema name : manualsla_quote
		/// </summary>
		public ODataBind sla_quote_slaLookup { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the quote record.
		/// Schema name : manualsla_quote
		/// Reference   : slaid -> sla(slaid)
		/// </summary>
		public dynamic sla_quote_sla { get; set; }
		/// <summary>
		/// Display name: SLA
		/// Description : Choose the service level agreement (SLA) that you want to apply to the quote record.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slaid_value { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this quote. This field is for internal use only.
		/// Schema name : sla_quote
		/// </summary>
		public ODataBind slainvokedid_quote_slaLookup { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this quote. This field is for internal use only.
		/// Schema name : sla_quote
		/// Reference   : slainvokedid -> sla(slaid)
		/// </summary>
		public dynamic slainvokedid_quote_sla { get; set; }
		/// <summary>
		/// Display name: Last SLA applied
		/// Description : Last SLA that was applied to this quote. This field is for internal use only.
		/// Lookup, targets: sla
		/// </summary>
		public Guid? _slainvokedid_value { get; set; }
		/// <summary>
		/// Attribute of: slainvokedid
		/// String, length 100
		/// </summary>
		public string slainvokedidname { get; set; }  // String, length 100
		/// <summary>
		/// Attribute of: slaid
		/// String, length 100
		/// </summary>
		public string slaname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Stage Id
		/// Description : Contains the id of the stage where the entity is located.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? stageid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: Time Zone Rule Version Number
		/// Description : For internal use only.
		/// Integer, length -1
		/// </summary>
		public int? timezoneruleversionnumber { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Total amount
		/// Description : Shows the total amount due, calculated as the sum of the products, discounts, freight, and taxes for the quote.
		/// Money, length -1
		/// </summary>
		public decimal? totalamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Amount (Base)
		/// Description : Value of the Total Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totalamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Pre-Freight Amount
		/// Description : Shows the total product amount for the quote, minus any discounts. This value is added to freight and tax amounts in the calculation for the total amount due for the quote.
		/// Money, length -1
		/// </summary>
		public decimal? totalamountlessfreight { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Pre-Freight Amount (Base)
		/// Description : Value of the Total Pre-Freight Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totalamountlessfreight_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Discount Amount
		/// Description : Shows the total discount amount, based on the discount price and rate entered on the quote.
		/// Money, length -1
		/// </summary>
		public decimal? totaldiscountamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Discount Amount (Base)
		/// Description : Value of the Total Discount Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totaldiscountamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Detail Amount
		/// Description : Shows the sum of all existing and write-in products included on the quote, based on the specified price list and quantities.
		/// Money, length -1
		/// </summary>
		public decimal? totallineitemamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Detail Amount (Base)
		/// Description : Value of the Total Detail Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totallineitemamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Line Item Discount Amount
		/// Description : Shows the total of the Manual Discount amounts specified on all products included in the quote. This value is reflected in the Detail Amount field on the quote and is added to any discount amount or rate specified on the quote
		/// Money, length -1
		/// </summary>
		public decimal? totallineitemdiscountamount { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Line Item Discount Amount (Base)
		/// Description : Value of the Total Line Item Discount Amount in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totallineitemdiscountamount_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Tax
		/// Description : Shows the total of the Tax amounts specified on all products included in the quote, included in the Total Amount due calculation for the quote.
		/// Money, length -1
		/// </summary>
		public decimal? totaltax { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Total Tax (Base)
		/// Description : Value of the Total Tax in base currency.
		/// Money, length -1
		/// </summary>
		public decimal? totaltax_base { get; set; }  // Money, length -1
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_quote
		/// </summary>
		public ODataBind transactioncurrencyidLookup { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Schema name : transactioncurrency_quote
		/// Reference   : transactioncurrencyid -> transactioncurrency(transactioncurrencyid)
		/// </summary>
		public dynamic transactioncurrencyid { get; set; }
		/// <summary>
		/// Display name: Currency
		/// Description : Choose the local currency for the record to make sure budgets are reported in the correct currency.
		/// Lookup, targets: transactioncurrency
		/// </summary>
		public Guid? _transactioncurrencyid_value { get; set; }
		/// <summary>
		/// Attribute of: transactioncurrencyid
		/// String, length 100
		/// </summary>
		public string transactioncurrencyidname { get; set; }  // String, length 100
		/// <summary>
		/// Display name: (Deprecated) Traversed Path
		/// Description : A comma separated list of string values representing the unique identifiers of stages in a Business Process Flow Instance in the order that they occur.
		/// String, length 1250
		/// </summary>
		public string traversedpath { get; set; }  // String, length 1250
		/// <summary>
		/// Display name: Unique Dsc ID
		/// Description : For internal use only.
		/// Uniqueidentifier, length -1
		/// </summary>
		public Guid? uniquedscid { get; set; }  // Uniqueidentifier, length -1
		/// <summary>
		/// Display name: UTC Conversion Time Zone Code
		/// Description : Time zone code that was in use when the record was created.
		/// Integer, length -1
		/// </summary>
		public int? utcconversiontimezonecode { get; set; }  // Integer, length -1
		/// <summary>
		/// Display name: Version Number
		/// Description : Version Number
		/// BigInt, length -1
		/// </summary>
		public long? versionnumber { get; set; }  // BigInt, length -1
		/// <summary>
		/// Display name: Ship To
		/// Description : Select whether the products included in the quote should be shipped to the specified address or held until the customer calls with further pick up or delivery instructions.
		/// Boolean, length -1
		/// </summary>
		public bool? willcall { get; set; }  // Boolean, length -1
		/// <summary>
		/// Attribute of: willcall
		/// Virtual, length -1
		/// </summary>
		public string willcallname { get; set; }  // Virtual, length -1
	}
}

