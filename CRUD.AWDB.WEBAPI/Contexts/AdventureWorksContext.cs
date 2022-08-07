using CRUD.AWDB.WEBAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD.AWDB.WEBAPI.Contexts
{
    public class AdventureWorksContext : DbContext
    {
        public AdventureWorksContext(DbContextOptions<AdventureWorksContext> options) : base(options)
        {

        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<BusinessEntityAddress> BusinessEntityAddresses { get; set; } = null!;
        public virtual DbSet<CountryRegion> CountryRegions { get; set; } = null!;
        public virtual DbSet<EmailAddress> EmailAddresses { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Person> People { get; set; } = null!;
        public virtual DbSet<PersonPhone> PersonPhones { get; set; } = null!;
        public virtual DbSet<PhoneNumberType> PhoneNumberTypes { get; set; } = null!;
        public virtual DbSet<StateProvince> StateProvinces { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address", "Person");

                entity.HasComment("Street address information for customers, employees, and vendors.");

                entity.HasIndex(e => e.Rowguid, "AK_Address_rowguid")
                    .IsUnique();

                entity.HasIndex(e => new { e.AddressLine1, e.AddressLine2, e.City, e.StateProvinceId, e.PostalCode }, "IX_Address_AddressLine1_AddressLine2_City_StateProvinceID_PostalCode")
                    .IsUnique();

                entity.HasIndex(e => e.StateProvinceId, "IX_Address_StateProvinceID");

                entity.Property(e => e.AddressId)
                    .HasColumnName("AddressID")
                    .HasComment("Primary key for Address records.");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(60)
                    .HasComment("First street address line.");

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(60)
                    .HasComment("Second street address line.");

                entity.Property(e => e.City)
                    .HasMaxLength(30)
                    .HasComment("Name of the city.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(15)
                    .HasComment("Postal code for the street address.");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.Property(e => e.StateProvinceId)
                    .HasColumnName("StateProvinceID")
                    .HasComment("Unique identification number for the state or province. Foreign key to StateProvince table.");

                entity.HasOne(d => d.StateProvince)
                    .WithMany(p => p.Addresses)
                    .HasForeignKey(d => d.StateProvinceId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });



            modelBuilder.Entity<BusinessEntityAddress>(entity =>
            {
                entity.HasKey(e => new { e.BusinessEntityId, e.AddressId, e.AddressTypeId })
                    .HasName("PK_BusinessEntityAddress_BusinessEntityID_AddressID_AddressTypeID");

                entity.ToTable("BusinessEntityAddress", "Person");

                entity.HasComment("Cross-reference table mapping customers, vendors, and employees to their addresses.");

                entity.HasIndex(e => e.Rowguid, "AK_BusinessEntityAddress_rowguid")
                    .IsUnique();

                entity.HasIndex(e => e.AddressId, "IX_BusinessEntityAddress_AddressID");

                entity.HasIndex(e => e.AddressTypeId, "IX_BusinessEntityAddress_AddressTypeID");

                entity.Property(e => e.BusinessEntityId)
                    .HasColumnName("BusinessEntityID")
                    .HasComment("Primary key. Foreign key to BusinessEntity.BusinessEntityID.");

                entity.Property(e => e.AddressId)
                    .HasColumnName("AddressID")
                    .HasComment("Primary key. Foreign key to Address.AddressID.");

                entity.Property(e => e.AddressTypeId)
                    .HasColumnName("AddressTypeID")
                    .HasComment("Primary key. Foreign key to AddressType.AddressTypeID.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.HasOne(d => d.Address)
                    .WithMany(p => p.BusinessEntityAddresses)
                    .HasForeignKey(d => d.AddressId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });



            modelBuilder.Entity<CountryRegion>(entity =>
            {
                entity.HasKey(e => e.CountryRegionCode)
                    .HasName("PK_CountryRegion_CountryRegionCode");

                entity.ToTable("CountryRegion", "Person");

                entity.HasComment("Lookup table containing the ISO standard codes for countries and regions.");

                entity.HasIndex(e => e.Name, "AK_CountryRegion_Name")
                    .IsUnique();

                entity.Property(e => e.CountryRegionCode)
                    .HasMaxLength(3)
                    .HasComment("ISO standard code for countries and regions.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Country or region name.");
            });



            modelBuilder.Entity<EmailAddress>(entity =>
            {
                entity.HasKey(e => new { e.BusinessEntityId, e.EmailAddressId })
                    .HasName("PK_EmailAddress_BusinessEntityID_EmailAddressID");

                entity.ToTable("EmailAddress", "Person");

                entity.HasComment("Where to send a person email.");

                entity.HasIndex(e => e.EmailAddress1, "IX_EmailAddress_EmailAddress");

                entity.Property(e => e.BusinessEntityId)
                    .HasColumnName("BusinessEntityID")
                    .HasComment("Primary key. Person associated with this email address.  Foreign key to Person.BusinessEntityID");

                entity.Property(e => e.EmailAddressId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EmailAddressID")
                    .HasComment("Primary key. ID of this email address.");

                entity.Property(e => e.EmailAddress1)
                    .HasMaxLength(50)
                    .HasColumnName("EmailAddress")
                    .HasComment("E-mail address for the person.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.EmailAddresses)
                    .HasForeignKey(d => d.BusinessEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.BusinessEntityId)
                    .HasName("PK_Employee_BusinessEntityID");

                entity.ToTable("Employee", "HumanResources");

                entity.HasComment("Employee information such as salary, department, and title.");

                entity.HasIndex(e => e.LoginId, "AK_Employee_LoginID")
                    .IsUnique();

                entity.HasIndex(e => e.NationalIdnumber, "AK_Employee_NationalIDNumber")
                    .IsUnique();

                entity.HasIndex(e => e.Rowguid, "AK_Employee_rowguid")
                    .IsUnique();

                entity.Property(e => e.BusinessEntityId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessEntityID")
                    .HasComment("Primary key for Employee records.  Foreign key to BusinessEntity.BusinessEntityID.");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasComment("Date of birth.");

                entity.Property(e => e.CurrentFlag)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("0 = Inactive, 1 = Active");

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("M = Male, F = Female");

                entity.Property(e => e.HireDate)
                    .HasColumnType("date")
                    .HasComment("Employee hired on this date.");

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(50)
                    .HasComment("Work title such as Buyer or Sales Representative.");

                entity.Property(e => e.LoginId)
                    .HasMaxLength(256)
                    .HasColumnName("LoginID")
                    .HasComment("Network login.");

                entity.Property(e => e.MaritalStatus)
                    .HasMaxLength(1)
                    .IsFixedLength()
                    .HasComment("M = Married, S = Single");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.NationalIdnumber)
                    .HasMaxLength(15)
                    .HasColumnName("NationalIDNumber")
                    .HasComment("Unique national identification number such as a social security number.");

                entity.Property(e => e.OrganizationLevel)
                    .HasComputedColumnSql("([OrganizationNode].[GetLevel]())", false)
                    .HasComment("The depth of the employee in the corporate hierarchy.");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.Property(e => e.SalariedFlag)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("Job classification. 0 = Hourly, not exempt from collective bargaining. 1 = Salaried, exempt from collective bargaining.");

                entity.Property(e => e.SickLeaveHours).HasComment("Number of available sick leave hours.");

                entity.Property(e => e.VacationHours).HasComment("Number of available vacation hours.");

                entity.HasOne(d => d.BusinessEntity)
                    .WithOne(p => p.Employee)
                    .HasForeignKey<Employee>(d => d.BusinessEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });



            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.BusinessEntityId)
                    .HasName("PK_Person_BusinessEntityID");

                entity.ToTable("Person", "Person");

                entity.HasComment("Human beings involved with AdventureWorks: employees, customer contacts, and vendor contacts.");

                entity.HasIndex(e => e.Rowguid, "AK_Person_rowguid")
                    .IsUnique();

                entity.HasIndex(e => new { e.LastName, e.FirstName, e.MiddleName }, "IX_Person_LastName_FirstName_MiddleName");

                entity.HasIndex(e => e.AdditionalContactInfo, "PXML_Person_AddContact");

                entity.HasIndex(e => e.Demographics, "PXML_Person_Demographics");

                entity.HasIndex(e => e.Demographics, "XMLPATH_Person_Demographics");

                entity.HasIndex(e => e.Demographics, "XMLPROPERTY_Person_Demographics");

                entity.HasIndex(e => e.Demographics, "XMLVALUE_Person_Demographics");

                entity.Property(e => e.BusinessEntityId)
                    .ValueGeneratedNever()
                    .HasColumnName("BusinessEntityID")
                    .HasComment("Primary key for Person records.");

                entity.Property(e => e.AdditionalContactInfo)
                    .HasColumnType("xml")
                    .HasComment("Additional contact information about the person stored in xml format. ");

                entity.Property(e => e.Demographics)
                    .HasColumnType("xml")
                    .HasComment("Personal information such as hobbies, and income collected from online shoppers. Used for sales analysis.");

                entity.Property(e => e.EmailPromotion).HasComment("0 = Contact does not wish to receive e-mail promotions, 1 = Contact does wish to receive e-mail promotions from AdventureWorks, 2 = Contact does wish to receive e-mail promotions from AdventureWorks and selected partners. ");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .HasComment("First name of the person.");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .HasComment("Last name of the person.");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(50)
                    .HasComment("Middle name or middle initial of the person.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.NameStyle).HasComment("0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.");

                entity.Property(e => e.PersonType)
                    .HasMaxLength(2)
                    .IsFixedLength()
                    .HasComment("Primary type of person: SC = Store Contact, IN = Individual (retail) customer, SP = Sales person, EM = Employee (non-sales), VC = Vendor contact, GC = General contact");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.Property(e => e.Suffix)
                    .HasMaxLength(10)
                    .HasComment("Surname suffix. For example, Sr. or Jr.");

                entity.Property(e => e.Title)
                    .HasMaxLength(8)
                    .HasComment("A courtesy title. For example, Mr. or Ms.");

                
            });



            modelBuilder.Entity<PersonPhone>(entity =>
            {
                entity.HasKey(e => new { e.BusinessEntityId, e.PhoneNumber, e.PhoneNumberTypeId })
                    .HasName("PK_PersonPhone_BusinessEntityID_PhoneNumber_PhoneNumberTypeID");

                entity.ToTable("PersonPhone", "Person");

                entity.HasComment("Telephone number and type of a person.");

                entity.HasIndex(e => e.PhoneNumber, "IX_PersonPhone_PhoneNumber");

                entity.Property(e => e.BusinessEntityId)
                    .HasColumnName("BusinessEntityID")
                    .HasComment("Business entity identification number. Foreign key to Person.BusinessEntityID.");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(25)
                    .HasComment("Telephone number identification number.");

                entity.Property(e => e.PhoneNumberTypeId)
                    .HasColumnName("PhoneNumberTypeID")
                    .HasComment("Kind of phone number. Foreign key to PhoneNumberType.PhoneNumberTypeID.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.HasOne(d => d.BusinessEntity)
                    .WithMany(p => p.PersonPhones)
                    .HasForeignKey(d => d.BusinessEntityId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.PhoneNumberType)
                    .WithMany(p => p.PersonPhones)
                    .HasForeignKey(d => d.PhoneNumberTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<PhoneNumberType>(entity =>
            {
                entity.ToTable("PhoneNumberType", "Person");

                entity.HasComment("Type of phone number of a person.");

                entity.Property(e => e.PhoneNumberTypeId)
                    .HasColumnName("PhoneNumberTypeID")
                    .HasComment("Primary key for telephone number type records.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("Name of the telephone number type");
            });



            modelBuilder.Entity<StateProvince>(entity =>
            {
                entity.ToTable("StateProvince", "Person");

                entity.HasComment("State and province lookup table.");

                entity.HasIndex(e => e.Name, "AK_StateProvince_Name")
                    .IsUnique();

                entity.HasIndex(e => new { e.StateProvinceCode, e.CountryRegionCode }, "AK_StateProvince_StateProvinceCode_CountryRegionCode")
                    .IsUnique();

                entity.HasIndex(e => e.Rowguid, "AK_StateProvince_rowguid")
                    .IsUnique();

                entity.Property(e => e.StateProvinceId)
                    .HasColumnName("StateProvinceID")
                    .HasComment("Primary key for StateProvince records.");

                entity.Property(e => e.CountryRegionCode)
                    .HasMaxLength(3)
                    .HasComment("ISO standard country or region code. Foreign key to CountryRegion.CountryRegionCode. ");

                entity.Property(e => e.IsOnlyStateProvinceFlag)
                    .IsRequired()
                    .HasDefaultValueSql("((1))")
                    .HasComment("0 = StateProvinceCode exists. 1 = StateProvinceCode unavailable, using CountryRegionCode.");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("Date and time the record was last updated.");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .HasComment("State or province description.");

                entity.Property(e => e.Rowguid)
                    .HasColumnName("rowguid")
                    .HasDefaultValueSql("(newid())")
                    .HasComment("ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.");

                entity.Property(e => e.StateProvinceCode)
                    .HasMaxLength(3)
                    .IsFixedLength()
                    .HasComment("ISO standard state or province code.");

                entity.Property(e => e.TerritoryId)
                    .HasColumnName("TerritoryID")
                    .HasComment("ID of the territory in which the state or province is located. Foreign key to SalesTerritory.SalesTerritoryID.");

                entity.HasOne(d => d.CountryRegionCodeNavigation)
                    .WithMany(p => p.StateProvinces)
                    .HasForeignKey(d => d.CountryRegionCode)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                
            });


        }




    }
}
