using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//WPF is lacking some of the WinForms features - such as a Folder Browser - so we need to reference the WinForms.dll
//This little assignment allows us to access the WinForms Forms quite easily.
using WinForms = System.Windows.Forms;
//To more easily perform some file operations.
using IO = System.IO;

//F1Graphics Swapper Version 2.0
//Allows the user to quickly and easily swap between user-defined F1Graphics.cfg files.
//Copyright(C) 2016 Douglas Spangenberg

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, version 3 of the License.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.

//In relation specifically to use for the Grand Prix series of games by Microprose,
//this program (both source code and compiled .exe) are subject to the GrandPrixGames.org permission policy.
//You should have received a copy of this policy with the program. If not, see <https://www.grandprixgames.org/read.php?4,1010656>

namespace F1GraphicsRedux
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Current location of the Program; Used for the settings file
        string appPath = IO.Directory.GetCurrentDirectory();

        //Dialog for getting the GP4 Path from the user.
        WinForms.FolderBrowserDialog folderBrowserDialog1 = new WinForms.FolderBrowserDialog();

        public MainWindow()
        {
            InitializeComponent();
            //Run the .ini loader
            iniLoader();
        }

        //Method to check for - and read from - the settings.ini file
        //See INIFILE.CS for details.
        private void iniLoader()
        {
            if (IO.File.Exists(appPath + "/Settings.ini"))
            {
                this.textBox1.Text = INIFile.ReadValue("Settings", "Path", appPath + "/Settings.ini");
            }
        }

        //'GP4 Path' Button
        private void button1_Click(object sender, RoutedEventArgs e)
        {          
            WinForms.DialogResult dialogResult = this.folderBrowserDialog1.ShowDialog();
			if (dialogResult == WinForms.DialogResult.OK)
			{
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;            
                Console.WriteLine("Selected Folder Is " + this.textBox1.Text);
			}
            
        }

        //'Swap' Button
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //Make sure a Path is selected
            if (this.textBox1.Text == null)
            {
                MessageBox.Show("No path selected");
                return;
            }
            //Make sure a .cfg is selected
            if (this.comboBox1.SelectedItem == null)
            {
                MessageBox.Show("No .cfg selected");
                return;
            }
            //Check if the file being swapped is actually the current f1graphics file.
            if (!this.comboBox1.SelectedItem.ToString().Contains("f1graphics.cfg"))
            {
                //Get the Path as a String, to make it easier to refer to.
                string text = this.textBox1.Text;
                //Get the name of the 'new' file
                string fileName = IO.Path.GetFileName(this.comboBox1.SelectedItem.ToString());
                //Copy the 'new' file to the current F1Graphics.cfg
                IO.File.Copy(this.comboBox1.SelectedItem.ToString(), text + "\\f1graphics.cfg", true);
                //Report back to the user.
                MessageBox.Show("'f1graphics.cfg' replaced with '" + fileName + "'");
                return;
            }
            else
                //If they try and replace the file with itself, let them know that they 'succeeded'.
                MessageBox.Show("'f1graphics.cfg' replaced with 'f1graphics.cfg'");
        }

        //When the 'GP4 Path' Textbox is updated, either via the Folder Browser or by the user typing.
        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Check folder for *.cfg files, and store them in an Array
            string[] array = IO.Directory.GetFiles(this.textBox1.Text, "*.cfg");
            //Populate the ComboBox with the Array items
            comboBox1.ItemsSource = array;
            //Auto-select the first one
            comboBox1.SelectedIndex = 0;
            //Update the .ini file
            INIFile.WriteValue("Settings", "Path", this.textBox1.Text, appPath + "/Settings.ini");
        }

    }
}
