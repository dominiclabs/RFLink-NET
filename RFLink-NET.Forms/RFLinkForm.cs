using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace RFLinkNet.Forms
{
    public partial class RFLinkForm : Form
    {
        RFLinkClient rfLinkClient = null;
        object lockme = new object(); 

        public RFLinkForm()
        {
            InitializeComponent();

            // Add default columns
            dataGridView1.Columns.Add("Key", "Key");
            dataGridView1.Columns.Add("Protocol", "Protocol");
        }

        /// <summary>
        /// Add data to Log TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReceivedStdOut(object sender, EventArgs e)
        {
            string data = ((RFEventArgs)e).Data;

            textbox_out_stdout.AppendText(data + Environment.NewLine);
        }

        /// <summary>
        /// Add RF Data to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ReceivedRFOut(object sender, EventArgs e)
        {
            RFData rf = ((RFData)e);
            string output = rf.ToString();

            lock (lockme)
            {
                // Have we seen this one before?
                DataGridViewRow row = FindRowHash(rf.HashKey);

                if (row == null)
                {
                    DataGridViewRow newrow = new DataGridViewRow();
                    int rowID = dataGridView1.Rows.Add(newrow);
                    row = dataGridView1.Rows[rowID];
                    row.Cells["Key"].Value = rf.HashKey;
                }

                foreach (var field in rf.Fields)
                {
                    if (false == dataGridView1.Columns.Contains(field.Key))
                    {
                        dataGridView1.Columns.Add(field.Key, field.Key);
                    }

                    row.Cells[field.Key].Value = field.Value;
                }
            }
        }

        /// <summary>
        /// Find an existing row based on the hash
        /// </summary>
        /// <param name="hash"></param>
        /// <returns>Grid row if exists otherwise null</returns>
        private DataGridViewRow FindRowHash(string hash)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                try
                {
                    string rowhash = row.Cells["Key"].Value.ToString();

                    if (rowhash == hash)
                    {
                        return row;
                    }
                }
                catch
                {
                    // Not a problem, value probably doesn't exist
                }

            }

            return null;
        }

        /// <summary>
        /// Connect to RFLink device using values 
        /// defined in from data
        /// </summary>
        private string RFLinkConnect(string port, List<string> stateFields)
        {
            rfLinkClient = new RFLinkClient(port);
            rfLinkClient.EventLogOut += ReceivedStdOut;
            rfLinkClient.EventRFOut += ReceivedRFOut;
            rfLinkClient.StateFields.AddRange(stateFields);

            try
            {
                if (rfLinkClient.Connect() == true)
                {
                    return String.Format("Connected with RFLink Version {0}, Rev {1}, build {2}", rfLinkClient.Settings.Version, rfLinkClient.Settings.Rev, rfLinkClient.Settings.Build);
                }
                else
                {
                    return "Connection Failed";
                }
            }
            catch (Exception ex)
            {
                return String.Format("Connection Failed with exception {0}", ex.Message);
            }
        }

        /// <summary>
        /// Get list state fields from user input
        /// </summary>
        /// <returns></returns>
        private List<string> GetStateFields()
        {
            List<string> statefieldList = new List<string>();
            foreach (var item in listBox_statefields.Items)
            {
                statefieldList.Add(item.ToString());
            }
            return statefieldList;
        }

        /// <summary>
        /// Kill the RFLink connection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisconnectFromRFLink(object sender, EventArgs e)
        {
            rfLinkClient.Close();
        }

        /// <summary>
        /// Send data to RFFlink defined in textbox_input_command
        /// and repeat based on textbox_input_commandretry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendData(object sender, EventArgs e)
        {
            int retry = 1;

            try
            {
                string retryText = textbox_input_commandretry.Text;
                if (!String.IsNullOrEmpty(retryText))
                {
                    Convert.ToInt32(retryText);
                }

                rfLinkClient?.SendRawData(textbox_input_command.Text, retry);
            }
            catch(Exception ex)
            {
                textbox_out_stdout.AppendText(ex.Message + Environment.NewLine);
            }
        }
        
        /// <summary>
        /// Add a new state field to the user list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewStateField(object sender, EventArgs e)
        {
            listBox_statefields.Items.Add(textbox_input_ignoreadd.Text);
            rfLinkClient?.StateFields.Clear();
            rfLinkClient?.StateFields.AddRange(GetStateFields());
            textbox_input_ignoreadd.Text = string.Empty;
        }

        private void RemoveStateField(object sender, EventArgs e)
        {
            listBox_statefields.Items.Remove(listBox_statefields.SelectedItem);

            rfLinkClient?.StateFields.Clear();
            rfLinkClient?.StateFields.AddRange(GetStateFields());
        }

        private void ResetDataGrid(object sender, EventArgs e)
        {
            lock (lockme)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    dataGridView1.Rows.Remove(row);
                }
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            textbox_out_status.Text = RFLinkConnect(textbox_input_port.Text, GetStateFields());
        }

        private void textbox_input_port_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textbox_out_status.Text = RFLinkConnect(textbox_input_port.Text, GetStateFields());
            }
        }

        private void textbox_input_command_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendData(null, null);
            }
        }
    }
}
