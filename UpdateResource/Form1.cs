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
        Dictionary<string, ExcelDataComm> dicDistinctName = new Dictionary<string, ExcelDataComm>();


        public Form1()
        {
            InitializeComponent();
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

            if (cbLang.SelectedItem == null)
            {
                MessageBox.Show("语言代码不能为空！");
                return false;
            }
            languageCode = cbLang.SelectedItem.ToString();

            return true;
        }

        private bool CheckExcel()
        {
            dicDistinctName.Clear();
            DataTable dt = ExcelToDataTable(filePath, true);

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("excel中没有数据！");
                return false;
            }
            //if (dt.Columns.Contains("LanguageCode"))
            //{
            //    languageCode = dt.Rows[0]["LanguageCode"].ToString();
            //    if (string.IsNullOrWhiteSpace(languageCode))
            //    {
            //        MessageBox.Show("列LanguageCode不能为空！");
            //        return false;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show("excel中没有列：LanguageCode！");
            //    return false;
            //}


            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string name_zh_cn = dt.Rows[i]["name_zh_cn"].ToString();
                string name_zh_cn2 = dt.Columns.Contains("name_zh_cn2") ? dt.Rows[i]["name_zh_cn2"].ToString() : string.Empty;
                //  string name2 = string.Empty;
                string name_update2 = string.Empty;
                string key = name_zh_cn;

                if (!string.IsNullOrWhiteSpace(name_zh_cn2))
                {
                    key = name_zh_cn + "ψ" + name_zh_cn2;
                    //   name2 = dt.Rows[i]["name2"].ToString();
                    name_update2 = dt.Columns.Contains("name_update2") ? dt.Rows[i]["name_update2"].ToString() : string.Empty;
                }

                if (!dicDistinctName.ContainsKey(key))
                {
                    dicDistinctName.Add(key,
                        new ExcelDataComm()
                        {
                            LanguageCode = languageCode,
                            Name_zh_cn = name_zh_cn,
                            //    Name = dt.Rows[i]["name"].ToString(),
                            Name_update = dt.Rows[i]["name_update"].ToString(),
                            Name_zh_cn2 = name_zh_cn2,
                            //   Name2 = name2,
                            Name_update2 = name_update2
                        }
                    );
                }
            }

            if (dicDistinctName.Count() == 0)
            {
                MessageBox.Show("请选择正确的excel！");
                return false;
            }
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
                                                case CellType.Formula:
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

        private void WriteData<T>(string updateSql, string insertSql, List<T> updateResult, List<T> insertResult) where T : class
        {
            using (IDbConnection conn = new SqlConnection(dbWriteConfig))
            {
                conn.Open();
                if (updateResult.Any())
                {
                    var resultList = new List<T>();
                    if (updateResult.Count > 500)
                    {
                        foreach (var item in updateResult)
                        {
                            resultList.Add(item);
                            if (resultList.Count == 500)
                            {
                                conn.Execute(updateSql, resultList);
                                resultList.Clear();
                            }
                        }
                        if (resultList.Any())
                        {
                            conn.Execute(updateSql, resultList);
                            resultList.Clear();
                        }
                    }
                    else
                    {
                        conn.Execute(updateSql, updateResult);
                    }
                }
                if (insertResult.Any())
                {
                    var resultList = new List<T>();
                    if (insertResult.Count > 500)
                    {
                        foreach (var item in insertResult)
                        {
                            resultList.Add(item);
                            if (resultList.Count == 500)
                            {
                                conn.Execute(insertSql, resultList);
                                resultList.Clear();
                            }
                        }
                        if (resultList.Any())
                        {
                            conn.Execute(insertSql, resultList);
                            resultList.Clear();
                        }
                    }
                    else
                    {
                        conn.Execute(insertSql, insertResult);
                    }
                }
            }
            MessageBox.Show("成功");
        }

        #endregion


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

            string querySql = "SELECT [SysNo] AS SystemCategorySysNo ,[CategoryName] FROM [YZ_Operation].[dbo].[SystemCategory] WITH(NOLOCK) WHERE CommonStatus<>-999";

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

            if (!CheckExcel())
            {
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
                    result.ForEach(str =>
                    {
                        if (str.CategoryName.Equals(item.Key))
                        {
                            str.CategoryName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
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
            WriteData<SystemCategory_Resource>(updateSql, insertSql, updateResult, insertResult);
        }


        private void btnSupplierCategory_ResourceByName_Click(object sender, EventArgs e)
        {

            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS SupplierCategorySysNo ,[CategoryName] FROM [YZ_Supplier].[dbo].[SupplierCategory] WITH(NOLOCK) WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE    [YZ_Supplier].[dbo].[SupplierCategory_Resource]
                                      SET 
                                           [CategoryName] = @CategoryName
                                          ,[EditUserSysNo] =0
                                          ,[EditUserName] = '王爱民'
                                          ,[EditDate] = GETDATE()
                                     WHERE SupplierCategorySysNo=@SupplierCategorySysNo AND LanguageCode=@LanguageCode";


            string insertSql = @"INSERT INTO [YZ_Supplier].[dbo].[SupplierCategory_Resource]
                                   ([SupplierCategorySysNo]
                                   ,[LanguageCode]
                                   ,[CategoryName]
                                   ,[InUserSysNo]
                                   ,[InUserName]
                                   ,[InDate]
                                   ,[EditUserSysNo]
                                   ,[EditUserName]
                                   ,[EditDate])
                             VALUES
                                   (@SupplierCategorySysNo
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
                                               [SupplierCategorySysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Supplier].[dbo].[SupplierCategory_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND SupplierCategorySysNo IN({1})";
            #endregion

            List<SupplierCategory_Resource> result = new List<SupplierCategory_Resource>();//excel映射到ResourceEntity后数据
            List<SupplierCategory_Resource> updateResult = new List<SupplierCategory_Resource>();//需要update的数据
            List<SupplierCategory_Resource> insertResult = new List<SupplierCategory_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SupplierCategory_Resource>(querySql);
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
                    result.ForEach(str =>
                    {
                        if (str.CategoryName.Equals(item.Key))
                        {
                            str.CategoryName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.SupplierCategorySysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SupplierCategory_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SupplierCategorySysNo == r.SupplierCategorySysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SupplierCategorySysNo == r.SupplierCategorySysNo);
                }).ToList();
            }

            //写
            WriteData<SupplierCategory_Resource>(updateSql, insertSql, updateResult, insertResult);

        }


        private void btnArea_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS SystemAreaSysNo ,[AreaName] FROM [YZ_Operation].[dbo].[SystemArea] WITH(NOLOCK)  WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE    [YZ_Operation].[dbo].[SystemArea_Resource]
                                      SET 
                                           [AreaName] = @AreaName
                                          ,[EditUserSysNo] =0
                                          ,[EditUserName] = '王爱民'
                                          ,[EditDate] = GETDATE()
                                     WHERE SystemAreaSysNo=@SystemAreaSysNo AND LanguageCode=@LanguageCode";


            string insertSql = @"INSERT INTO YZ_Operation.[dbo].[SystemArea_Resource]
                                   ([SystemAreaSysNo]
                                   ,[LanguageCode]
                                   ,[AreaName]
                                   ,[InUserSysNo]
                                   ,[InUserName]
                                   ,[InDate]
                                   ,[EditUserSysNo]
                                   ,[EditUserName]
                                   ,[EditDate])
                             VALUES
                                   (@SystemAreaSysNo
                                   ,@LanguageCode
                                   , @AreaName
                                   ,0
                                   ,'王爱民'
                                   ,GetDate()
                                   , 0
                                   ,'王爱民'
                                   ,GetDate())";

            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [SystemAreaSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Operation].[dbo].[SystemArea_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND SystemAreaSysNo IN({1})";
            #endregion

            List<SystemArea_Resource> result = new List<SystemArea_Resource>();//excel映射到ResourceEntity后数据
            List<SystemArea_Resource> updateResult = new List<SystemArea_Resource>();//需要update的数据
            List<SystemArea_Resource> insertResult = new List<SystemArea_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemArea_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.AreaName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.AreaName.Equals(item.Key))
                        {
                            str.AreaName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.SystemAreaSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemArea_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemAreaSysNo == r.SystemAreaSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemAreaSysNo == r.SystemAreaSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemArea_Resource>(updateSql, insertSql, updateResult, insertResult);

        }

        private void btnBidTool_TenderBidStatusItem_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS BidTool_TenderBidStatusItemSysNo ,[NoticeContent] FROM [YZ_Tender].[dbo].[BidTool_TenderBidStatusItem] WITH(NOLOCK) WHERE NoticeContent NOT IN ('','bid_notice','bid_tender','adjustment_notice','result','resulted')";


            string updateSql = @"UPDATE    [YZ_Tender].[dbo].[BidTool_TenderBidStatusItem_Resource]
                                      SET 
                                           [NoticeContent] = @NoticeContent
                                     WHERE BidTool_TenderBidStatusItemSysNo=@BidTool_TenderBidStatusItemSysNo AND LanguageCode=@LanguageCode";


            string insertSql = @"INSERT INTO [YZ_Tender].[dbo].[BidTool_TenderBidStatusItem_Resource]
                                   ([BidTool_TenderBidStatusItemSysNo]
                                   ,[LanguageCode]
                                   ,[NoticeContent]
                                  )
                             VALUES
                                   (@BidTool_TenderBidStatusItemSysNo
                                   ,@LanguageCode
                                   ,@NoticeContent
                                   )";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [BidTool_TenderBidStatusItemSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Tender].[dbo].[BidTool_TenderBidStatusItem_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND BidTool_TenderBidStatusItemSysNo IN({1})";
            #endregion

            List<BidTool_TenderBidStatusItem_Resource> result = new List<BidTool_TenderBidStatusItem_Resource>();//excel映射到ResourceEntity后数据
            List<BidTool_TenderBidStatusItem_Resource> updateResult = new List<BidTool_TenderBidStatusItem_Resource>();//需要update的数据
            List<BidTool_TenderBidStatusItem_Resource> insertResult = new List<BidTool_TenderBidStatusItem_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<BidTool_TenderBidStatusItem_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.NoticeContent)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.NoticeContent.Equals(item.Key))
                        {
                            str.NoticeContent = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.BidTool_TenderBidStatusItemSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<BidTool_TenderBidStatusItem_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.BidTool_TenderBidStatusItemSysNo == r.BidTool_TenderBidStatusItemSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.BidTool_TenderBidStatusItemSysNo == r.BidTool_TenderBidStatusItemSysNo);
                }).ToList();
            }
            //写
            WriteData<BidTool_TenderBidStatusItem_Resource>(updateSql, insertSql, updateResult, insertResult);


        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS MenuSysNo ,[MenuName] FROM [YZ_AuthCenter].[dbo].[SystemMenu] WITH(NOLOCK)  WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE   [YZ_AuthCenter].[dbo].[SystemMenu_Resource]
                                      SET 
                                           [MenuName] = @MenuName
                                     WHERE MenuSysNo=@MenuSysNo AND LanguageCode=@LanguageCode";


            string insertSql = @"INSERT INTO  [YZ_AuthCenter].[dbo].[SystemMenu_Resource]
                                   ([MenuSysNo]
                                   ,[LanguageCode]
                                   ,[MenuName]
                                    )
                             VALUES
                                   (@MenuSysNo
                                   ,@LanguageCode
                                   ,@MenuName
                                    )";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [MenuSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_AuthCenter].[dbo].[SystemMenu_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND MenuSysNo IN({1})";
            #endregion

            List<SystemMenu_Resource> result = new List<SystemMenu_Resource>();//excel映射到ResourceEntity后数据
            List<SystemMenu_Resource> updateResult = new List<SystemMenu_Resource>();//需要update的数据
            List<SystemMenu_Resource> insertResult = new List<SystemMenu_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemMenu_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.MenuName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.MenuName.Equals(item.Key))
                        {
                            str.MenuName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.MenuSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemMenu_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.MenuSysNo == r.MenuSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.MenuSysNo == r.MenuSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemMenu_Resource>(updateSql, insertSql, updateResult, insertResult);
            
        }

        private void btnSystemFunction_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS FunctionSysNo ,[FunctionName] FROM [YZ_AuthCenter].[dbo].[SystemFunction] WITH(NOLOCK)   WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE   [YZ_AuthCenter].[dbo].[SystemFunction_Resource]
                                      SET 
                                           [FunctionName] = @FunctionName
                                     WHERE FunctionSysNo=@FunctionSysNo AND LanguageCode=@LanguageCode";


            string insertSql = @"INSERT INTO  [YZ_AuthCenter].[dbo].[SystemFunction_Resource]
                                   ([FunctionSysNo]
                                   ,[LanguageCode]
                                   ,[FunctionName]
                                    )
                             VALUES
                                   (@FunctionSysNo
                                   ,@LanguageCode
                                   ,@FunctionName
                                    )";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [FunctionSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_AuthCenter].[dbo].[SystemFunction_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND FunctionSysNo IN({1})";
            #endregion

            List<SystemFunction_Resource> result = new List<SystemFunction_Resource>();//excel映射到ResourceEntity后数据
            List<SystemFunction_Resource> updateResult = new List<SystemFunction_Resource>();//需要update的数据
            List<SystemFunction_Resource> insertResult = new List<SystemFunction_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemFunction_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.FunctionName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.FunctionName.Equals(item.Key))
                        {
                            str.FunctionName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.FunctionSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemFunction_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.FunctionSysNo == r.FunctionSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.FunctionSysNo == r.FunctionSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemFunction_Resource>(updateSql, insertSql, updateResult, insertResult);
        }

        private void btnSystemTagRole_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS RoleSysNo ,[RoleName] FROM [YZ_AuthCenter].[dbo].[SystemTagRole] WITH(NOLOCK)  WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE   [YZ_AuthCenter].[dbo].[SystemTagRole_Resource]
                                      SET 
                                           [RoleName] = @RoleName
                                     WHERE RoleSysNo=@RoleSysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_AuthCenter].[dbo].[SystemTagRole_Resource]
                                               ([RoleSysNo]
                                               ,[LanguageCode]
                                               ,[RoleName])
                                         VALUES
                                               (@RoleSysNo
                                               ,@LanguageCode
                                               ,@RoleName)";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [RoleSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_AuthCenter].[dbo].[SystemTagRole_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND RoleSysNo IN({1})";
            #endregion

            List<SystemTagRole_Resource> result = new List<SystemTagRole_Resource>();//excel映射到ResourceEntity后数据
            List<SystemTagRole_Resource> updateResult = new List<SystemTagRole_Resource>();//需要update的数据
            List<SystemTagRole_Resource> insertResult = new List<SystemTagRole_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemTagRole_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.RoleName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.RoleName.Equals(item.Key))
                        {
                            str.RoleName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.RoleSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemTagRole_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.RoleSysNo == r.RoleSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.RoleSysNo == r.RoleSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemTagRole_Resource>(updateSql, insertSql, updateResult, insertResult);

        }

        private void btnApplication_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS ApplicationSysNo ,[ApplicationName] FROM [YZ_AuthCenter].[dbo].[SystemApplication] WITH(NOLOCK) ";


            string updateSql = @"UPDATE   [YZ_AuthCenter].[dbo].[SystemApplication_Resource]
                                      SET 
                                           [ApplicationName] = @ApplicationName
                                     WHERE ApplicationSysNo=@ApplicationSysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_AuthCenter].[dbo].[SystemApplication_Resource]
                                               ([ApplicationSysNo]
                                               ,[LanguageCode]
                                               ,[ApplicationName])
                                         VALUES
                                               (@ApplicationSysNo
                                               ,@LanguageCode
                                               ,@ApplicationName)";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [ApplicationSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_AuthCenter].[dbo].[SystemApplication_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND ApplicationSysNo IN({1})";
            #endregion

            List<SystemApplication_Resource> result = new List<SystemApplication_Resource>();//excel映射到ResourceEntity后数据
            List<SystemApplication_Resource> updateResult = new List<SystemApplication_Resource>();//需要update的数据
            List<SystemApplication_Resource> insertResult = new List<SystemApplication_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemApplication_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.ApplicationName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.ApplicationName.Equals(item.Key))
                        {
                            str.ApplicationName = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.ApplicationSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemApplication_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.ApplicationSysNo == r.ApplicationSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.ApplicationSysNo == r.ApplicationSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemApplication_Resource>(updateSql, insertSql, updateResult, insertResult);

        }


        private void btnAuditNode_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS NodeSysNo ,[NodeName],ApplicationName FROM [YZ_Audit].[dbo].[AuditNode] WITH(NOLOCK)  WHERE CommonStatus<>-999";


            string updateSql = @"UPDATE  [YZ_Audit].[dbo].[AuditNode_Resource]
                                      SET 
                                           [NodeName] = @NodeName,
                                           ApplicationName=@ApplicationName
                                     WHERE NodeSysNo=@NodeSysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_Audit].[dbo].[AuditNode_Resource]
                                               ([NodeSysNo]
                                               ,[LanguageCode]
                                               ,[NodeName]
                                               ,[ApplicationName])
                                         VALUES
                                               (@NodeSysNo
                                               ,@LanguageCode
                                               ,@NodeName
                                               ,@ApplicationName)";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [NodeSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Audit].[dbo].[AuditNode_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND NodeSysNo IN({1})";
            #endregion

            List<AuditNode_Resource> result = new List<AuditNode_Resource>();//excel映射到ResourceEntity后数据
            List<AuditNode_Resource> updateResult = new List<AuditNode_Resource>();//需要update的数据
            List<AuditNode_Resource> insertResult = new List<AuditNode_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<AuditNode_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.ApplicationName + "ψ" + m.NodeName)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if ((str.ApplicationName + "ψ" + str.NodeName).Equals(item.Key))
                        {
                            str.ApplicationName = item.Value.Name_update;
                            str.NodeName = item.Value.Name_update2;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.NodeSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<AuditNode_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.NodeSysNo == r.NodeSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.NodeSysNo == r.NodeSysNo);
                }).ToList();
            }

            //写
            WriteData<AuditNode_Resource>(updateSql, insertSql, updateResult, insertResult);

        }

        private void btnOrg_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            //  string querySql = "SELECT [SysNo] AS SystemOrganizationSysNo ,[OrganizationName],OrganizationFullName FROM [YZ_AuthCenter].[dbo].[SystemOrganization] WITH(NOLOCK) ";

            //todo:只翻译中建股份公司
            //string querySql = "SELECT [SysNo] AS SystemOrganizationSysNo ,[OrganizationName],OrganizationFullName FROM [YZ_AuthCenter].[dbo].[SystemOrganization] WITH(NOLOCK) WHERE OrganizationCode LIKE '01000001%' AND CommonStatus<>-999";
            string querySql = "SELECT [SysNo] AS SystemOrganizationSysNo ,[OrganizationName],OrganizationFullName FROM [YZ_AuthCenter].[dbo].[SystemOrganization] WITH(NOLOCK) WHERE CommonStatus<>-999";



            string updateSql = @"UPDATE  [YZ_AuthCenter].[dbo].[SystemOrganization_Resource]
                                      SET 
                                           [OrganizationName] = @OrganizationName,
                                           OrganizationFullName=@OrganizationFullName,
                                            EditUserSysNo=0,
                                            EditUserName='王爱民',
                                            EditDate=GetDate()
                                     WHERE SystemOrganizationSysNo=@SystemOrganizationSysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_AuthCenter].[dbo].[SystemOrganization_Resource]
                                   ([SystemOrganizationSysNo]
                                   ,[LanguageCode]
                                   ,[OrganizationName]
                                   ,[OrganizationFullName]
                                   ,[InUserSysNo]
                                   ,[InUserName]
                                   ,[InDate]
                                   ,[EditUserSysNo]
                                   ,[EditUserName]
                                   ,[EditDate])
                             VALUES
                                   (@SystemOrganizationSysNo
                                   , @LanguageCode
                                   , @OrganizationName
                                   , @OrganizationFullName
                                   , 0
                                   , '王爱民'
                                   , GetDate()
                                   , 0
                                   , '王爱民'
                                   , GetDate())";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [SystemOrganizationSysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_AuthCenter].[dbo].[SystemOrganization_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND SystemOrganizationSysNo IN({1})";
            #endregion

            List<SystemOrganization_Resource> result = new List<SystemOrganization_Resource>();//excel映射到ResourceEntity后数据
            List<SystemOrganization_Resource> updateResult = new List<SystemOrganization_Resource>();//需要update的数据
            List<SystemOrganization_Resource> insertResult = new List<SystemOrganization_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<SystemOrganization_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m =>
                {
                    string key = m.OrganizationName + "ψ" + m.OrganizationFullName;
                    return dicDistinctName.ContainsKey(key);

                }).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if ((str.OrganizationName + "ψ" + str.OrganizationFullName).Equals(item.Key))
                        {
                            str.OrganizationName = item.Value.Name_update;
                            str.OrganizationFullName =item.Value.Name_update2;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.SystemOrganizationSysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<SystemOrganization_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemOrganizationSysNo == r.SystemOrganizationSysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.SystemOrganizationSysNo == r.SystemOrganizationSysNo);
                }).ToList();
            }

            //写
            WriteData<SystemOrganization_Resource>(updateSql, insertSql, updateResult, insertResult);
        }

        private void btnCurrence_Click(object sender, EventArgs e)
        {
            #region check and sql

            if (!CheckData())
            {
                return;
            }

            string querySql = "SELECT [SysNo] AS CurrencySysNo ,[Name] FROM [YZ_Operation].[dbo].[Currency] WITH(NOLOCK) ";


            string updateSql = @"UPDATE   [YZ_Operation].[dbo].[Currency_Resource]
                                      SET 
                                           [Name] = @Name
                                     WHERE CurrencySysNo=@CurrencySysNo AND LanguageCode=@LanguageCode";

            string insertSql = @"INSERT INTO [YZ_Operation].[dbo].[Currency_Resource]
                                               ([CurrencySysNo]
                                               ,[LanguageCode]
                                               ,[Name]
                                               ,[InUserSysNo]
                                               ,[InUserName]
                                               ,[InDate])
                                         VALUES
                                               (@CurrencySysNo
                                               ,@LanguageCode
                                               ,@Name
                                               , '王爱民'
                                               , GetDate()
                                               , 0)";
            //需要update的数据
            string queryResourceSql = @"SELECT 
                                               [CurrencySysNo]
                                              ,[LanguageCode]
                                       FROM  [YZ_Operation].[dbo].[Currency_Resource] WITH(NOLOCK)
                                       WHERE [LanguageCode]='{0}' AND CurrencySysNo IN({1})";
            #endregion

            List<Currency_Resource> result = new List<Currency_Resource>();//excel映射到ResourceEntity后数据
            List<Currency_Resource> updateResult = new List<Currency_Resource>();//需要update的数据
            List<Currency_Resource> insertResult = new List<Currency_Resource>();//需要insert的数据

            if (!CheckExcel())
            {
                return;
            }

            //读
            using (IDbConnection conn = new SqlConnection(dbReadConfig))
            {
                conn.Open();
                var queryResult = conn.Query<Currency_Resource>(querySql);
                if (queryResult.Count() == 0)
                {
                    MessageBox.Show("待翻译表中无数据可修改！");
                    return;
                }
                result = queryResult.Where(m => dicDistinctName.ContainsKey(m.Name)).ToList();//excel中和待翻译数据表对应的数据
                if (!result.Any())
                {
                    MessageBox.Show("待翻译表中无对应数据可翻译！(通过中文名称匹配)");
                    return;
                }
                foreach (var item in dicDistinctName)
                {
                    result.ForEach(str =>
                    {
                        if (str.Name.Equals(item.Key))
                        {
                            str.Name = item.Value.Name_update;
                            str.LanguageCode = languageCode;
                        }
                    });
                }

                //resource表中需要update的数据
                string sysNos = string.Join(",", result.Select(m => m.CurrencySysNo));
                queryResourceSql = string.Format(queryResourceSql, languageCode, sysNos);
                var queryResoureceResult = conn.Query<Currency_Resource>(queryResourceSql).ToList();
                updateResult = result.Where(r =>
                {
                    return queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.CurrencySysNo == r.CurrencySysNo);
                }).ToList();
                //resource表中需要insert的数据
                insertResult = result.Where(r =>
                {
                    return !queryResoureceResult.Exists(tqrr => tqrr.LanguageCode == r.LanguageCode && tqrr.CurrencySysNo == r.CurrencySysNo);
                }).ToList();
            }

            //写
            WriteData<Currency_Resource>(updateSql, insertSql, updateResult, insertResult);

        }
    }
}
