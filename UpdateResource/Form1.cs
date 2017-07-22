using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace UpdateResource
{
    public partial class Form1 : Form
    {
        private string filePath;
        private string dbWriteConfig;
        private string dbReadConfig;
        private string languageCode;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.xlsx)|*.xlsx";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                tbResource.Text = openFileDialog.FileName;
            }
        }
        private void btnSystemCategoryByName_Click(object sender, EventArgs e)
        {


            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS SystemCategorySysNo ,[CategoryName] FROM [YZ_Operation].[dbo].[SystemCategory] WITH(NOLOCK)";

            string updateSql = @"UPDATE    [YZ_Operation].[dbo].[SystemCategory_Resource]
                                      SET 
                                           [CategoryName] = @CategoryName
                                          ,[EditUserSysNo] =0
                                          ,[EditUserName] = '王爱民'
                                          ,[EditDate] = GETDATE()
                                     WHERE SystemCategorySysNo=@SystemCategorySysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_Operation].[dbo].[SystemCategory_Resource]
                                   ([SystemCategorySysNo]
                                   ,[LanguageCode]
                                   ,[CategoryName]
                                   ,[InUserSysNo]
                                   ,[InUserName]
                                   ,[InDate]
                                   ,[EditUserSysNo]
                                   ,[EditUserName]
                                   ,[EditDate])
                             VALUES
                                   (@SystemCategorySysNo
                                   ,@LanguageCode
                                   ,@CategoryName
                                   ,0
                                   ,'王爱民'
                                   ,GetDate()
                                   ,0
                                   ,'王爱民'
                                   ,GetDate())";

            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [SystemCategorySysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Operation].[dbo].[SystemCategory_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND SystemCategorySysNo IN({1})";
            #endregion

            List<SystemCategory_Resource> result = new List<SystemCategory_Resource>();//excel映射到ResourceEntity后数据
            List<SystemCategory_Resource> updateResult = new List<SystemCategory_Resource>();//需要update的数据
            List<SystemCategory_Resource> insertResult = new List<SystemCategory_Resource>();//需要insert的数据
            string languageCode = string.Empty;

            DataTable dt = ExcelToDataTable(filePath, true);
            Dictionary<string, DataComm> dicDistinctName = new Dictionary<string, DataComm>();

            if (dt.Rows.Count > 0)
                languageCode = dt.Rows[0]["LanguageCode"].ToString();
            else
                throw new Exception("excel中没有数据！");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name_zh_cn = dt.Rows[i]["name_zh_cn"].ToString();
                if (!dicDistinctName.ContainsKey(name_zh_cn))
                {
                    dicDistinctName.Add(name_zh_cn,
                        new DataComm()
                        {
                            Name_zh_cn = name_zh_cn,
                            LanguageCode = languageCode,
                            Name = dt.Rows[i]["name"].ToString(),
                            Name_update = dt.Rows[i]["name_update"].ToString()
                        }
                    );
                }
            }


            if (dicDistinctName.Count() == 0)
            {
                MessageBox.Show("请选择正确的excel！");
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemCategory_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.CategoryName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.Where(str => str.CategoryName.Equals(item.Key)).ToList().ForEach(str =>
                    {
                        str.CategoryName = string.IsNullOrWhiteSpace(item.Value.Name_update) ? item.Value.Name : item.Value.Name_update;
                        str.LanguageCode = item.Value.LanguageCode;
                    }
                    );
                }

                //resource表中需要update的数据
                string systemCategorySysNos = string.Join(",", result.Select(m => m.SystemCategorySysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, systemCategorySysNos);
                var queryResoureceResult = conn.Query<SystemCategory_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemCategorySysNo == r.SystemCategorySysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemCategorySysNo == r.SystemCategorySysNo);
                }).ToList();

            }

            //写
            using (IDbConnection conn = new SqlConnection(dbWriteConfig))
            {
                conn.Open();
                if (updateResult.Any())
                {
                    conn.Execute(updateSql, updateResult);
                }
                if (insertResult.Any())
                {
                    conn.Execute(insertSql, insertResult);
                }
            }
            MessageBox.Show("成功");
        }

        private void btnSupplierCategory_ResourceByName_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            string updateSql = @"UPDATE    [YZ_Supplier].[dbo].[SupplierCategory_Resource]
                                      SET 
                                           [CategoryName] = @CategoryName
                                          ,[EditUserSysNo] =0
                                          ,[EditUserName] = '王爱民'
                                          ,[EditDate] = GETDATE()
                                     WHERE SupplierCategorySysNo=@SupplierCategorySysNo AND LanguageCode=@LanguageCode";

            string querySql = "SELECT [SysNo] AS SupplierCategorySysNo ,[CategoryName] FROM [YZ_Supplier].[dbo].[SupplierCategory] WITH(NOLOCK)";

            IEnumerable<SupplierCategory_Resource> result;

            Dictionary<string, DataComm> dicDistinctName = new Dictionary<string, DataComm>();
            DataTable dt = ExcelToDataTable(filePath, true);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name_zh_cn = dt.Rows[i]["name_zh_cn"].ToString();
                if (!dicDistinctName.ContainsKey(name_zh_cn))
                {
                    dicDistinctName.Add(name_zh_cn,
                        new DataComm()
                        {
                            Name_zh_cn = name_zh_cn,
                            LanguageCode = dt.Rows[i]["LanguageCode"].ToString(),
                            Name = dt.Rows[i]["name"].ToString(),
                            Name_update = dt.Rows[i]["name_update"].ToString()
                        }
                    );
                }
            }


            if (dicDistinctName.Count() == 0)
            {
                MessageBox.Show("请选择正确的excel！");
                return;
            }

            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SupplierCategory_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("资源表中无数据可修改！");
                    return;
                }

                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.CategoryName)).ToList();

                foreach (var item in dicDistinctName)
                {
                    result.Where(str => str.CategoryName.Equals(item.Key)).ToList().ForEach(str =>
                    {
                        str.CategoryName = string.IsNullOrWhiteSpace(item.Value.Name_update) ? item.Value.Name : item.Value.Name_update;
                        str.LanguageCode = item.Value.LanguageCode;
                    }
                    );
                }
            }

            //写
            using (IDbConnection conn = new SqlConnection(dbWriteConfig))
            {
                conn.Open();
                conn.Execute(updateSql, result);
            }
            MessageBox.Show("成功");
        }

        #region comm


        private bool CheckData()
        {

            dbReadConfig = tbReadDB.Text.Trim();
            if (dbReadConfig == "")
            {
                MessageBox.Show("ReadDB不能为空！");
                return false;
            }
            dbWriteConfig = tbWriteDB.Text.Trim();
            if (dbWriteConfig == "")
            {
                MessageBox.Show("WriteDB不能为空！");
                return false;
            }

            filePath = tbResource.Text.Trim();
            if (filePath.IndexOf(".xlsx") <= 0)
            {
                MessageBox.Show("请选择正确的excel");
                return false;
            }

            //languageCode = tbLanguageCode.Text.Trim();
            //if (languageCode == "")
            //{
            //    MessageBox.Show("语言代码不能为空！");
            //    return false;
            //}
            return true;
        }

        /// <summary>
        /// 将excel导入到datatable
        /// </summary>
        /// <param name="filePath">excel路径</param>
        /// <param name="isColumnName">第一行是否是列名</param>
        /// <returns>返回datatable</returns>
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            int startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx") > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls") > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0);//读取第一个sheet，当然也可以循环读取每个sheet
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            int rowCount = sheet.LastRowNum;//总行数
                            if (rowCount > 0)
                            {
                                IRow firstRow = sheet.GetRow(0);//第一行
                                int cellCount = firstRow.LastCellNum;//列数

                                //构建datatable的列
                                if (isColumnName)
                                {
                                    startRow = 1;//如果第一行是列名，则从第二行开始读取
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell != null)
                                        {
                                            if (cell.StringCellValue != null)
                                            {
                                                column = new DataColumn(cell.StringCellValue);
                                                dataTable.Columns.Add(column);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行
                                for (int i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                        {
                                            dataRow[j] = "";
                                        }
                                        else
                                        {
                                            //CellType(Unknown = -1,Numeric = 0,String = 1,Formula = 2,Blank = 3,Boolean = 4,Error = 5,)
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;
                                                case CellType.Numeric:
                                                    short format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;
                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                        }
                                    }
                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }
                return dataTable;
            }
            catch (Exception ex)
            {
                if (fs != null)
                {
                    fs.Close();
                }
                return null;
            }
        }

        #endregion

        private void btnOrg_Click(object sender, EventArgs e)
        {

        }
    }
}
