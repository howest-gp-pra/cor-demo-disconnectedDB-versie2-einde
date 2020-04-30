using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using DisconnectedVersie2.LIB.Entities;

namespace DisconnectedVersie2.LIB.Services
{
    public class XMLHelper
    {
        public static List<Address> addresses;
        public static List<AddressType> addressTypes;
        public static void GetData()
        {
            DataSet ds = ReadXML();
            addressTypes = new List<AddressType>();
            AddressType addressType;
            foreach(DataRow rw in ds.Tables["Soorten"].Rows)
            {
                addressType = new AddressType();
                addressType.ID = rw["id"].ToString();
                addressType.Soort = rw["soort"].ToString();
                addressTypes.Add(addressType);
            }
            addresses = new List<Address>();
            Address address;
            foreach (DataRow rw in ds.Tables["Adressen"].Rows)
            {
                address = new Address();
                address.ID = rw["id"].ToString();
                address.Naam = rw["naam"].ToString();
                address.Adres = rw["adres"].ToString();
                address.Post = rw["post"].ToString();
                address.Gemeente = rw["gemeente"].ToString();
                address.Land = rw["land"].ToString();
                address.Soort_ID = rw["soort_id"].ToString();
                addresses.Add(address);
            }
        }
        private static DataSet ReadXML()
        {
            DataSet ds;
            string xmlMap = Directory.GetCurrentDirectory() + "/XMLBestanden";
            string xmlBestand = Directory.GetCurrentDirectory() + "/XMLBestanden/adressen.xml";
            if (!Directory.Exists(xmlMap))
                Directory.CreateDirectory(xmlMap);
            if (!File.Exists(xmlBestand))
            {
                ds = CreateTables();
                CreateSeedings(ds);
            }
            else
            {
                ds = new DataSet();
                ds.ReadXml(xmlBestand, XmlReadMode.ReadSchema);
            }
            return ds;
        }
 
        private static DataSet CreateTables()
        {
            // datatable Soorten aanmaken
            DataSet ds = new DataSet();
            DataTable dtSoorten = new DataTable();
            dtSoorten.TableName = "Soorten";

            // datacolumn's aanmaken
            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "ID";
            dc.DataType = typeof(string);
            dtSoorten.Columns.Add(dc);
            dtSoorten.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "soort";
            dc.DataType = typeof(string);
            dc.Unique = true;
            dc.AllowDBNull = false;
            dtSoorten.Columns.Add(dc);

            ds.Tables.Add(dtSoorten);


            // datatable Adressen aanmaken
            DataTable dtAdressen = new DataTable();
            dtAdressen.TableName = "Adressen";

            // datacolumn's aanmaken
            dc = new DataColumn();
            dc.ColumnName = "ID";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);
            dtAdressen.PrimaryKey = new DataColumn[] { dc };

            dc = new DataColumn();
            dc.ColumnName = "naam";
            dc.DataType = typeof(string);
            dc.AllowDBNull = false;
            dtAdressen.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "adres";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "post";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "gemeente";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "land";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);

            dc = new DataColumn();
            dc.ColumnName = "soort_id";
            dc.DataType = typeof(string);
            dtAdressen.Columns.Add(dc);

            ds.Tables.Add(dtAdressen);

            ds.Relations.Add(ds.Tables["Soorten"].Columns["ID"], ds.Tables["Adressen"].Columns["soort_id"]);
            return ds;
        }
        private static void CreateSeedings(DataSet ds)
        {
            DataRow dr = ds.Tables["soorten"].NewRow();
            dr["ID"] = Guid.NewGuid().ToString();
            dr["soort"] = "Familie";
            ds.Tables["soorten"].Rows.Add(dr);

            dr = ds.Tables["soorten"].NewRow();
            dr["ID"] = Guid.NewGuid().ToString();
            dr["soort"] = "Vrienden";
            ds.Tables["soorten"].Rows.Add(dr);

            dr = ds.Tables["soorten"].NewRow();
            dr["ID"] = Guid.NewGuid().ToString();
            dr["soort"] = "Klanten";
            ds.Tables["soorten"].Rows.Add(dr);

        }
        public static void SaveData()
        {
            WriteXML();
        }
        private static void WriteXML()
        {
            string XMLMap = Directory.GetCurrentDirectory() + "/XMLBestanden";
            string XMLBestand = Directory.GetCurrentDirectory() + "/XMLBestanden/adressen.xml";
            if (!Directory.Exists(XMLMap))
                Directory.CreateDirectory(XMLMap);
            if (File.Exists(XMLBestand))
            {
                File.Delete(XMLBestand);
            }

            DataSet ds = CreateTables();

            foreach(AddressType addressType in addressTypes)
            {
                DataRow dr = ds.Tables["soorten"].NewRow();
                dr["ID"] = addressType.ID;
                dr["soort"] = addressType.Soort;
                ds.Tables["soorten"].Rows.Add(dr);
            }
            foreach(Address address in addresses)
            {
                DataRow dr = ds.Tables["Adressen"].NewRow();
                dr["ID"] = address.ID;
                dr["naam"] = address.Naam;
                dr["adres"] = address.Adres;
                dr["post"] = address.Post;
                dr["gemeente"] = address.Gemeente;
                dr["land"] = address.Land;
                dr["soort_id"] = address.Soort_ID;
                ds.Tables["Adressen"].Rows.Add(dr);
            }
            ds.WriteXml(XMLBestand, XmlWriteMode.WriteSchema);
        }

    }
}
