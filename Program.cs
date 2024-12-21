/****************************************************************************
**                    SAKARYA ÜNİVERSİTESİ
**                BİLGİSAYAR VE BİLİŞİM BİLİMLERİ FAKÜLTESİ
**                    BİLGİSAYAR MÜHENDİSLİĞİ BÖLÜMÜ
**                   NESNEYE DAYALI PROGRAMLAMA DERSİ
**                    2024-2025 BAHAR DÖNEMİ
**    
**                ÖDEV NUMARASI..........:1. Ödev
**                ÖĞRENCİ ADI............:Fatih Bayram
**                ÖĞRENCİ NUMARASI.......:E245013038
**                DERSİN ALINDIĞI GRUP...:Uzaktan Eğitim A
****************************************************************************/

using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace KelimeIslemProgrami
{
    public class KelimeIslemUygulamasi : Form
    {
        // Menü çubuğu ve RichTextBox tanımları
        private MenuStrip menuStrip;
        private RichTextBox richTextBox;
        private string currentFilePath; // Şu an üzerinde çalışılan dosyanın yolu
        private bool isModified; // Dosyada değişiklik yapılıp yapılmadığını izler

        public KelimeIslemUygulamasi()
        {
            // Bileşenlerin başlatılması
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            // MenuStrip oluşturma ve menü öğelerini ekleme
            menuStrip = new MenuStrip();

            var dosyaMenu = CreateMenuItem("Dosya", new ToolStripMenuItem[]
            {
                CreateMenuItem("Yeni", YeniDosya_Click), // Yeni bir dosya oluşturur
                CreateMenuItem("Dosya Aç", DosyaAc_Click), // Mevcut bir dosyayı açar
                CreateMenuItem("Dosya Kaydet", DosyaKaydet_Click), // Dosyayı kaydeder
                CreateMenuItem("Çıkış", Cikis_Click) // Programdan çıkar
            });

            var bicimMenu = CreateMenuItem("Biçim", new ToolStripMenuItem[]
            {
                CreateMenuItem("Kes", Kes_Click), // Seçili metni keser
                CreateMenuItem("Kopyala", Kopyala_Click), // Seçili metni kopyalar
                CreateMenuItem("Yapıştır", Yapistir_Click) // Panodaki metni yapıştırır
            });

            var ayarlarMenu = CreateMenuItem("Ayarlar", new ToolStripMenuItem[]
            {
                CreateMenuItem("Renk", RenkDegistir_Click), // Yazı rengini değiştirir
                CreateMenuItem("Yazı Tipi", YaziTipiDegistir_Click), // Yazı tipini değiştirir
                CreateMenuItem("C# Şablonu", CSharpSablonu_Click), // C# şablon dosyası oluşturur
                CreateMenuItem("C Şablonu", CPlainSablonu_Click), // C şablon dosyası oluşturur
                CreateMenuItem("C++ Şablonu", CppSablonu_Click) // C++ şablon dosyası oluşturur
            });

            menuStrip.Items.AddRange(new ToolStripItem[] { dosyaMenu, bicimMenu, ayarlarMenu });

            // RichTextBox oluşturma
            richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill // RichTextBox, formu tamamen kaplar
            };

            // Context Menü oluşturma (Sağ tık menüsü)
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.AddRange(new ToolStripItem[]
            {
                CreateMenuItem("Kes", Kes_Click),
                CreateMenuItem("Kopyala", Kopyala_Click),
                CreateMenuItem("Yapıştır", Yapistir_Click),
                CreateMenuItem("Renk", RenkDegistir_Click),
                CreateMenuItem("Yazı Tipi", YaziTipiDegistir_Click)
            });

            richTextBox.ContextMenuStrip = contextMenu;

            // Form ayarları
            Controls.Add(richTextBox);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;

            Text = "Kelime İşlem Programı"; // Form başlığı
            WindowState = FormWindowState.Maximized; // Form başlatıldığında tam ekran açılır
        }

        private ToolStripMenuItem CreateMenuItem(string text, EventHandler onClick)
        {
            // Menü öğesi oluşturur ve olay işleyiciyi bağlar
            return new ToolStripMenuItem(text, null, onClick);
        }

        private ToolStripMenuItem CreateMenuItem(string text, ToolStripMenuItem[] subItems)
        {
            // Alt menü öğeleri olan bir menü öğesi oluşturur
            var menuItem = new ToolStripMenuItem(text);
            menuItem.DropDownItems.AddRange(subItems);
            return menuItem;
        }

        private void YeniDosya_Click(object sender, EventArgs e)
        {
            // Yeni bir dosya oluşturulurken kaydedilmemiş değişiklikler kontrol edilir
            if (CheckUnsavedChanges())
            {
                richTextBox.Clear();
                currentFilePath = null;
                isModified = false;
            }
        }

        private void DosyaAc_Click(object sender, EventArgs e)
        {
            // Mevcut bir dosya açılır
            if (CheckUnsavedChanges())
            {
                using (var openFileDialog = new OpenFileDialog { Filter = "Metin Dosyaları|*.txt|Tüm Dosyalar|*.*" })
                {
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = openFileDialog.FileName;
                        richTextBox.Text = File.ReadAllText(currentFilePath);
                        isModified = false;
                    }
                }
            }
        }

        private void DosyaKaydet_Click(object sender, EventArgs e)
        {
            // Dosya kaydedilir, yeni dosya ise bir yol seçilir
            if (string.IsNullOrEmpty(currentFilePath))
            {
                using (var saveFileDialog = new SaveFileDialog { Filter = "Metin Dosyaları|*.txt|Tüm Dosyalar|*.*" })
                {
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = saveFileDialog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            File.WriteAllText(currentFilePath, richTextBox.Text);
            isModified = false;
        }

        private void Cikis_Click(object sender, EventArgs e)
        {
            // Programdan çıkmadan önce kaydedilmemiş değişiklikler kontrol edilir
            if (CheckUnsavedChanges())
            {
                Application.Exit();
            }
        }

        private void Kes_Click(object sender, EventArgs e)
        {
            // Seçili metni keser
            richTextBox.Cut();
        }

        private void Kopyala_Click(object sender, EventArgs e)
        {
            // Seçili metni kopyalar
            richTextBox.Copy();
        }

        private void Yapistir_Click(object sender, EventArgs e)
        {
            // Panodaki metni yapıştırır
            richTextBox.Paste();
        }

        private void RenkDegistir_Click(object sender, EventArgs e)
        {
            // Yazı rengini değiştirir
            using (var colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.ForeColor = colorDialog.Color;
                }
            }
        }

        private void YaziTipiDegistir_Click(object sender, EventArgs e)
        {
            // Yazı tipini değiştirir
            using (var fontDialog = new FontDialog())
            {
                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox.Font = fontDialog.Font;
                }
            }
        }

        private void CSharpSablonu_Click(object sender, EventArgs e)
        {
            // C# şablon dosyası oluşturur
            richTextBox.Text = "using System;\n\nnamespace basitornek\n{\n    class Program\n    {\n        static void Main(string[] args)\n        {\n            Console.WriteLine(\"Merhaba Millet\");\n            Console.ReadKey();\n        }\n    }\n}";
        }

        private void CPlainSablonu_Click(object sender, EventArgs e)
        {
            // C şablon dosyası oluşturur
            richTextBox.Text = "#include <stdio.h>\n\nint main() {\n    printf(\"Hello, World!\\n\");\n    return 0;\n}";
        }

        private void CppSablonu_Click(object sender, EventArgs e)
        {
            // C++ şablon dosyası oluşturur
            richTextBox.Text = "#include <iostream>\n\nint main() {\n    std::cout << \"Hello, World!\\n\";\n    return 0;\n}";
        }

        private bool CheckUnsavedChanges()
        {
            // Kaydedilmemiş değişiklikleri kontrol eder
            if (isModified)
            {
                var result = MessageBox.Show("Değişiklikler kaydedilsin mi?", "Uyarı", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                {
                    DosyaKaydet_Click(null, null);
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            return true;
        }

        [STAThread]
        public static void Main()
        {
            // Program giriş noktası
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new KelimeIslemUygulamasi());
        }
    }
}
