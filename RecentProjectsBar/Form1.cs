using RecentProjectsBar.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecentProjectsBar
{
    public partial class Form1 : Form
    {
        IEnumerable<string> RecentlyUsedProjects
        {
            get
            {
                return Properties.Settings.Default[Constants.Constants.RecentlyUsedProjects].ToString().Split(';').Where(s => !string.IsNullOrEmpty(s));
            }
            set
            {
                Properties.Settings.Default[Constants.Constants.RecentlyUsedProjects] = string.Join(";", value);
                Properties.Settings.Default.Save();
            }
        }
        public string CurrentProject
        {
            get => Properties.Settings.Default[Constants.Constants.CurrentProject].ToString();
            set
            {
                Properties.Settings.Default[Constants.Constants.CurrentProject] = value;
                Properties.Settings.Default.Save();
            }
        }

        public Form1()
        {
            InitializeComponent();

            SetRecentlyUsedFileNamesMenuStrip();
        }

        private void SetRecentlyUsedFileNamesMenuStrip()
        {
            letzteProjekteToolStripMenuItem.DropDownItems.Clear();
            foreach (var item in RecentlyUsedProjects)
            {
                letzteProjekteToolStripMenuItem.DropDownItems.Add(item, null, OnMenuItemClick);
            }
            letzteProjekteToolStripMenuItem.Visible = letzteProjekteToolStripMenuItem.DropDownItems.Count != 0;
        }

        private void OnMenuItemClick(object sender, EventArgs e)
        {
            if (sender is ToolStripDropDownItem dropDownItem)
            {
                MessageBox.Show("Should open project " + dropDownItem.Text);
            }
        }

        private void AddRecenltyUsedProjectsItem(string item)
        {
            RecentlyUsedProjects = new HashSet<string>((new[] { item })
                .Union(RecentlyUsedProjects))
                .Take(3);

            SetRecentlyUsedFileNamesMenuStrip();
        }

        private void neuesProjektToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                CurrentProject = dialog.FileName;
                AddRecenltyUsedProjectsItem(dialog.FileName);
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
        }
    }
}
