using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualLotteryConfigTool
{
    class FileUtility
    {
        static ExcelWorksheets excelWorksheets = null;

        static public void LoadXlsFile(string filename)
        {
            try
            {
                FileInfo exisitingFile = new FileInfo(filename);
                ExcelPackage package = new ExcelPackage(exisitingFile);
                excelWorksheets = package.Workbook.Worksheets;
            }
            catch(Exception)
            {
                throw;
            }
        }

        static public DataTable GetDataTable(string name)
        {
            DataTable dataTable = new DataTable();
            //查找是否具有该页签
            int num = excelWorksheets.Count;
            for (int i = 1; i <= num; i++)
            {
                if (excelWorksheets[i].Name == name)
                {
                    dataTable.TableName = name;
                    //读取这个页签
                    //获取worksheet的行数
                    int rows = excelWorksheets[i].Dimension.End.Row;
                    //获取worksheet的列数
                    int cols = excelWorksheets[i].Dimension.End.Column;

                    DataRow datarow = null;
                    for (int j = 1; j <= rows; j++)
                    {
                        if (j > 1)
                            datarow = dataTable.Rows.Add();

                        for (int k = 1; k <= cols; k++)
                        {
                            //默认将第一行设置为datatable的标题
                            if (j == 1)
                                dataTable.Columns.Add(GetString(excelWorksheets[i].Cells[j, k].Value));
                            //剩下的写入datatable
                            else
                                datarow[k - 1] = GetString(excelWorksheets[i].Cells[j, k].Value);
                        }
                    }
                    return dataTable;
                }
            }

            return dataTable;
        }

        private static string GetString(object obj)
        {
            try
            {
                return obj.ToString();
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public static bool IsRightFile(string path , string type)
        {
            if(File.Exists(path))
            {
                string extension = GetExtensionName(path);
                if(type == "xls"||type == "xlsx")
                {
                    if (extension.Equals("xls") || extension.Equals("xlsx"))
                    {
                        return true;
                    }
                }
                else
                {
                    if(extension.Equals(type))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string GetExtensionName(string srcString)
        {
            string extension = "";
            int index = srcString.IndexOf(".");
            if (srcString.Length > 0 && index >= 0)
            {
                extension = srcString.Substring(index + 1, srcString.Length - index - 1);
                extension.TrimEnd(' ');
            }
            return extension;
        }


        //统计这一列从point开始，到出现第一个空单元格
        public static int CountKey(ref DataTable dataTable, Point point)
        {
            int count = 0;
            //因为DataTable的表格行数从1开始计数，Point的值从0开始计数，所以需要+1
            string temp = dataTable.Rows[point.Y + count + 1][point.X].ToString();
            while ((dataTable.Rows.Count > point.Y + count + 1) && (temp != ""))
            {
                
                count++;
                temp = dataTable.Rows[point.Y + count + 1][point.X].ToString();
            }
            return count;
        }
        //搜索当前列直到table最后一行，统计字符串出现次数
        public static int CountKeyGlobal(ref DataTable dataTable , Point point,string str)
        {
            int count = 0;
            for(int i = point.Y;i< dataTable.Rows.Count;i++)
            {
                if(dataTable.Rows[i][point.X].ToString() == str)
                {
                    count++;
                }
            }
            return count;
        }

        //搜索传入位置的那一列，找到相对应的str
        public static Point SearchColumn( ref DataTable dataTable,Point point , string str)
        {
            for(int i=0; i<dataTable.Rows.Count;i++)
            {
                if(dataTable.Rows[i][point.X].ToString() == str)
                {
                    point.Y = i;
                    return point;
                }
            }


            return point;
        }
        //从当前位置，向下搜索这一列，找到对应的str
        public static Point SearchColumnNext(ref DataTable dataTable, Point point, string str)
        {
            for (int i = point.Y+1; i < dataTable.Rows.Count; i++)
            {
                if (dataTable.Rows[i][point.X].ToString() == str)
                {
                    point.Y = i;
                    return point;
                }
            }
            return point;
        }

        //搜索传入位置的那一行，找到相对应的str
        public static Point SearchRow(ref DataTable dataTable,Point point , string str)
        {
            for (int i = 0; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Rows[point.Y][i].ToString() == str)
                {
                    point.X = i;
                    return point;
                }
            }

            return point;
        }
        //从当前位置，向右搜索这一列，找到对应的str
        public static Point SearchRowNext(ref DataTable dataTable, Point point, string str)
        {
            for (int i = point.X+1; i < dataTable.Columns.Count; i++)
            {
                if (dataTable.Rows[point.Y][i].ToString() == str)
                {
                    point.X = i;
                    return point;
                }
            }
            return point;
        }

    }
}
