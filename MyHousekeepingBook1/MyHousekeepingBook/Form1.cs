using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHousekeepingBook
{

    public partial class Form1 : Form
    {
        string ID;
        string path; //出力ファイル名

        public Form1()
        {
            InitializeComponent();

        }

        /// <summary>
        /// ログイン情報のID
        /// </summary>
        private string m_LoginID;

        /// <summary>
        /// ログイン情報のID
        /// </summary>
        public string LoginID
        {
            set { m_LoginID = value; }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {

            AddData();
        }

        private void 追加AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddData();
        }

        //cp7-11:フォームの処理（データの格納）
        private void AddData()
        {
            ItemForm frmItem = new ItemForm(categoryDataSet1); //登録フォームの作成  

            DialogResult drRet = frmItem.ShowDialog();   //登録画面のフォームをモーダル画面で表示
            if (drRet == DialogResult.OK)
            {
                moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
                    frmItem.monCalendar.SelectionRange.Start,
                    frmItem.cmbCategory.Text,
                    frmItem.txtItem.Text,
                    int.Parse(frmItem.mtxtMoney.Text),
                    int.Parse(frmItem.mtxtTax.Text),
                    frmItem.txtRemarks.Text);
            }
        }

        //cp7-10:フォームの処理（データの扱い方）
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();

            //型付きデータセット（categoryDataSet1）
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("給料", "入金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("食費", "出金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("雑費", "出金");
            categoryDataSet1.CategoryDataTable.AddCategoryDataTableRow("住居", "出金");

           /* this.Visible = true;
            this.Activate();*/
        }

        

        //---------------------------------
        //7-12:サブルーチンの具体的な使い方
        //---------------------------------
        
        //[終了]ボタンのイベントハンドラ
        private void buttonEnd_Click(object sender, EventArgs e)
        {
            this.Close();
           
        }
        //[ファイル]メニューの[終了]イベントハンドラ
        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //------------------------------------------------
        //CP8-6：ファイルの入出力（データの保存機能の実装）
        //------------------------------------------------
        // ファイルへ出力する処理の記述例
        private void SaveData()
        {
            string path = "MoneyData.csv";    //出力ファイル名
            string strData = "";              //1行分のデータ

            //ファイルを作成するクラスのインスタンスを作成
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                path,
                false,
                System.Text.Encoding.Default);

            //レコードの数だけ、ループさせる
            foreach (MoneyDataSet.moneyDataTableRow drMoney
                in moneyDataSet.moneyDataTable)

            {
                //１行分の値を保持する変数に現在行のデータを代入します
                strData = m_LoginID + ","
                        + drMoney.日付.ToShortDateString() + ","
                        + drMoney.分類 + ","
                        + drMoney.品名 + ","
                        + drMoney.金額.ToString() + ","
                        + drMoney.消費税.ToString() + ","
                        + drMoney.備考;

                //1行分のデータを出力
                sw.WriteLine(strData);

            }
            sw.Close();  //ファイルを閉じる
        }

        //----------------------------------------
        //フォーム終了時のイベントハンドラのコード
        //----------------------------------------

        //生成されたフォームを閉じる処理のイベントハンドラ
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveData();
        }
        //生成された保存処理のイベントハンドラ
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveData();
    
            
        }

        //---------------------------------------------------
        //cp8-7:ファイルの入出力（データの読み込み機能の実装）
        //---------------------------------------------------
        //LOadData()サブルーチンのコード
        //CSVファイルがなくなるまで読み込む処理を繰り返す 
        private void LoadData()
        {
           string path = "MoneyData.csv";             //入力ファイル名
            string delimStr = ",";                      //区切り文字
            char[] delimiter = delimStr.ToCharArray();   //区切り文字をまとめる
            string[] strData;
            string strLine;                             //1行分のデータ
            bool fileExists = System.IO.File.Exists(path); //ファイルが存在するかどうかを確認
            if (fileExists)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(
                                                path,
                                                System.Text.Encoding.Default);
                while (sr.Peek() >= 0)
                {
                    strLine = sr.ReadLine();
                    strData = strLine.Split(delimiter);

                    if (m_LoginID == strData[0])
                    {
                        moneyDataSet.moneyDataTable.AddmoneyDataTableRow(
                            DateTime.Parse(strData[1]),
                            strData[2],
                            strData[3],
                            int.Parse(strData[4]),
                            int.Parse(strData[5]),
                            strData[6]);
                    }
                }
                sr.Close();
            }
        
        }
        //-----------------------------------------
        //cp8-8:データの変更機能の実装
        //-----------------------------------------
        //データを追加・変更する処理
        private void UpdateData()
        {
            int nowRow = dgv.CurrentRow.Index;
            DateTime oldDate
                    = DateTime.Parse(dgv.Rows[nowRow].Cells[1].Value.ToString());
            string oldCategory = dgv.Rows[nowRow].Cells[2].Value.ToString();
            string oldItem = dgv.Rows[nowRow].Cells[3].Value.ToString();
            int oldMoney
                = int.Parse(dgv.Rows[nowRow].Cells[4].Value.ToString());
            int oldTax
                = int.Parse(dgv.Rows[nowRow].Cells[5].Value.ToString());
            string oldRemarks = dgv.Rows[nowRow].Cells[6].Value.ToString();
            string oldID = dgv.Rows[nowRow].Cells[0].Value.ToString();
            ItemForm frmItem = new ItemForm(categoryDataSet1,
                                            oldDate,
                                            oldCategory,
                                            oldItem,
                                            oldMoney,
                                            oldTax,
                                            oldRemarks,
                                            oldID);
            DialogResult drRet = frmItem.ShowDialog();
            if (drRet == DialogResult.OK)
            {
                dgv.Rows[nowRow].Cells[1].Value
                                        = frmItem.monCalendar.SelectionRange.Start;
                dgv.Rows[nowRow].Cells[2].Value = frmItem.cmbCategory.Text;
                dgv.Rows[nowRow].Cells[3].Value = frmItem.txtItem.Text;
                dgv.Rows[nowRow].Cells[4].Value = int.Parse(frmItem.mtxtMoney.Text);
                dgv.Rows[nowRow].Cells[5].Value = int.Parse(frmItem.mtxtTax.Text);
                dgv.Rows[nowRow].Cells[6].Value = frmItem.txtRemarks.Text;
              //  dgv.Rows[nowRow].Cells[5].Value = ID;
            }
        }

           private void buttonChange_Click(object sender, EventArgs e)
           {
               UpdateData();
           }

        private void 変更CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateData();
        }



        
        /* private void buttonChange_Click1(object sender, EventArgs e)
        {
            UpdateData();
        }
        */

        /*        private void buttonChange_Click(object sender, EventArgs e)
                {
                    UpdateData();
                }
        */


        //-------------------------------------
        //cp8-9:データの削除機能の実装
        //-------------------------------------
        //削除処理
        private void DeleteData()
        {
            int nowRow = dgv.CurrentRow.Index;
            dgv.Rows.RemoveAt(nowRow);    //現在行を削除
        }

        private void buttonDalete_Click(object sender,EventArgs e)
        {
            DeleteData();
        }

        private void 削除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteData();
        }

        //--------------------------------
        //cp9-3:集計機能の実装
        //--------------------------------
        //データを集計する処理
        private void CalcSummary()
        {
            string expression;
            summaryDataSet.SumDataTable.Clear();
            foreach (MoneyDataSet.moneyDataTableRow drMoney
                     in moneyDataSet.moneyDataTable)
            {
                expression = "日付= '" + drMoney.日付.ToShortDateString() + "'";
                SummaryDataSet.SumDataTableRow[] curDR
                = (SummaryDataSet.SumDataTableRow[])
                   summaryDataSet.SumDataTable.Select(expression);
                if (curDR.Length == 0)
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow = (CategoryDataSet.CategoryDataTableRow[])
                                       categoryDataSet1.CategoryDataTable.Select(
                                           "分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類 == "入金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(drMoney.日付,
                                                                       drMoney.金額,
                                                                       0);
                    }
                    else if (selectedDataRow[0].入出金分類　== "出金")
                    {
                        summaryDataSet.SumDataTable.AddSumDataTableRow(drMoney.日付,
                                                                       0,
                                                                        drMoney.金額);
                    }

                }
                else
                {
                    CategoryDataSet.CategoryDataTableRow[] selectedDataRow;
                    selectedDataRow = (CategoryDataSet.CategoryDataTableRow[])
                                       categoryDataSet1.CategoryDataTable.Select(
                                           "分類='" + drMoney.分類 + "'");
                    if (selectedDataRow[0].入出金分類　== "入金")
                    {
                        curDR[0].入金合計 += drMoney.金額;
                    }
                    else if (selectedDataRow[0].入出金分類 == "出金")
                    {
                        curDR[0].出金合計 += drMoney.金額;
                    }
                }
            }
        }

        //CalcSummary()サブルーチンを呼び出す
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSummary();
        }

        //一覧表示タブと集計表示タブの画面を切り替える
        private void 一覧表示LToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabList);
        }

        private void 集計表示SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabControl1.SelectTab(tabSummary);
        }

        private void moneyDataTableBindingSource_CurrentChanged(object sender, EventArgs e)
        {
           // LoadData();
        }
    }
    }
