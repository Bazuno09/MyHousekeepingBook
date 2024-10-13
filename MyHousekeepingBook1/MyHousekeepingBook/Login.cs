using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace MyHousekeepingBook
{
    public partial class Login : Form


    {
        //初期値
        Form1 form1 = null;
        //  Shinki shinki = new Shinki();
       
        /// <summary>
        /// 
        /// </summary>
        public Login()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        //Loginボタンのイベントハンドラ(password)
        //private void Login_Load(object sender, EventArgs e)
        //private void btnLogin_Click(object sender, EventArgs e)
        //textBox1のイベントハンドラ
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

           // SaveData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShinkiAdd_Click(object sender, EventArgs e)
        {
            Shinki shinki = new Shinki();
            shinki.ShowDialog();
        }

        //ファイルの読み込み
        private void LoadData()
        {
            string path = "LoginData.csv";
            bool fileExists = File.Exists(path);

            // Csvに保存されたデータに存在するか確認用フラグ
            bool existFlag = false;

            // 紐づけるためのユニークなID
            string loginID = string.Empty;

            if (fileExists)
            {
                // 読み込みたいCSVファイルのパスを指定して開く
                StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("shift_jis"));
                {
                    bool headerFlag = false;

                    // 末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // CSVファイルの一行を読み込む
                        string line = sr.ReadLine();

                        // 初回はヘッダーなのでスルー
                        if (!headerFlag)
                        {
                            headerFlag = true;
                            continue;
                        }

                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        string[] values = line.Split(',');

                        if (textBox1.Text == values[1].Trim() && textBox2.Text == values[2].Trim())
                        {
                            loginID = values[0].Trim();
                            existFlag = true;
                            break;
                        }
                    }

                    sr.Close();
                }
            }

            if (existFlag)
            {
                Form1 form1 = new Form1();
                form1.LoginID = loginID;
                form1.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("入力されたデータは見つかりませんでした", "入力エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Login_Load(object sender, EventArgs e)
        {
        }
    }
}
    




