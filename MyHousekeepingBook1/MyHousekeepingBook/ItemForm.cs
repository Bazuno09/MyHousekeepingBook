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
    public partial class ItemForm : Form
    {
        //cp7-10:フォームの処理
        public ItemForm(CategoryDataSet dsCategory)
        {
            InitializeComponent();         //初期化処理
            categoryDataSet.Merge(dsCategory);
        }
        //-----------------------------------
        //cp8-8:データの変更機能の実装
        //-----------------------------------
        //ItemFormの処理
        public ItemForm(CategoryDataSet dsCategory, //第１引数、分類一覧（型付きでーたセット型）
                        DateTime nowDate,           //第２引数、日付（日付型）
                        string category,            //第３引数、分類（文字列型）
                        string item,                //第４引数、品名（文字列型）
                        int money,                  //第５引数、金額（整数型）
                        int tax,                    //第6引数、消費税（整数型）
                        string remarks,          //第7引数、備考（文字列型）
                         string id)
        {
            InitializeComponent();         //初期化処理
            categoryDataSet.Merge(dsCategory);
            monCalendar.SetDate(nowDate);
            cmbCategory.Text = category;
            txtItem.Text = item;
            mtxtMoney.Text = money.ToString();
            mtxtTax.Text = tax.ToString();
            txtRemarks.Text = remarks;
        }


        /* private void mtxtTax_Click(object sender, EventArgs e)
         {
             if (String.IsNullOrEmpty(mtxtTax.Text))
             {
                 MessageBox.Show("入力してください");
             }

         }*/
    }
}
