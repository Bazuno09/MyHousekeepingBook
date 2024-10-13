using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyHousekeepingBook
{
    public partial class Shinki : Form
    {
        static Shinki shinki;

        #region コンストラクタS　リージョン（区切り）

        public Shinki()
        {
            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Shinki createShinki()
        {
            if(shinki == null)
            {
                shinki = new Shinki();
                return shinki;
            }
            else
            {
                return shinki;
            }
        }

        /// <summary>
        /// 登録ボタン押下処理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (textPass2.Text == textPass3.Text)
            {
                SaveData();
            }
            else
            {
                MessageBox.Show("入力したパスワードが異なっております", "お知らせ",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CSVへの保存メソッド
        /// </summary>
        private void SaveData()
        {
            string path = "LoginData.csv";

            List<int> ids = new List<int>();

            // Csv保存用の二次元配列
            List<List<string>> csvDatas = new List<List<string>>();

            // 既に登録してあるUserIDと新規作成したUserIDで重複がないかを確かめる
            bool existFlag = false;

            // 読み込みたいCSVファイルのパスを指定して開く
            StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("shift_jis"));
            {
                // ID、UserID、Passwordのヘッダー文字列
                bool headerFlag = false;

                // 末尾まで繰り返す
                while (!sr.EndOfStream)
                {
                    // CSVファイルの一行を読み込む
                    string line = sr.ReadLine();

                    // 初回はヘッダーなのでスルー(一行読み込んだ後にContinueしないと、次の行を読み取ってくれないので、ここに書く)
                    if (!headerFlag)
                    {
                        headerFlag = true;
                        continue;
                    }

                    // 読み込んだ一行をカンマ毎に分けて配列に格納する
                    string[] values = line.Split(',');

                    // 重複がないか確認する
                    if (textId1.Text == values[1].Trim())
                    {
                        existFlag = true;
                        break;
                    }

                    csvDatas.Add(values.ToList());

                    ids.Add(int.Parse(values[0]));
                }

                sr.Close();
            }

            // 既にUserIDが存在していた
            if (existFlag)
            {
                MessageBox.Show("入力したUserIDは既に存在しております", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var maxId = 0;

            // IDの最大値を取得する
            if (ids.Count() > 0)
            {
                maxId = ids.Max() + 1;
            }

            // 書き込み処理
            StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.Default);
            {
                string strData = "ID, UserID, Password";

                // 先にheaderを記載する
                sw.WriteLine(strData);

                // Csv保存用の二次元配列に保存したデータに対してループを回す(ここには既に保存されているデータが格納されている)
                foreach (var csvData in csvDatas)
                {
                    strData = csvData[0] + "," + csvData[1] + "," + csvData[2];

                    sw.WriteLine(strData);
                }

                // 新規作成したデータ
                strData = maxId + ", " + textId1.Text + ", " + textPass2.Text;

                sw.WriteLine(strData);

                sw.Close();
            }


            MessageBox.Show("登録完了しました", "お知らせ", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
