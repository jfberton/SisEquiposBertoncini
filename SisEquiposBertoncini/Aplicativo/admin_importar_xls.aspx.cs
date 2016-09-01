using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.IO;


namespace SisEquiposBertoncini.Aplicativo
{
    public partial class admin_importar_xls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_leer_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                if (Path.GetExtension(FileUpload1.FileName) == ".xlsx")
                {
                    ExcelPackage package = new ExcelPackage(FileUpload1.FileContent);
                    GridView1.DataSource = package.ToDataTable();
                    GridView1.DataBind();
                }
            }
        }


    }
}