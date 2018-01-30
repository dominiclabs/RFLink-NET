using System.Windows.Forms;

namespace RFLinkNet.Forms
{
    partial class RFLinkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if(components != null)
                {
                    components.Dispose();
                }

                if (rfLinkClient != null)
                {
                    rfLinkClient.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button_connect = new System.Windows.Forms.Button();
            this.textbox_input_port = new System.Windows.Forms.TextBox();
            this.textbox_out_stdout = new System.Windows.Forms.TextBox();
            this.button_disconnect = new System.Windows.Forms.Button();
            this.textbox_out_status = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.textbox_input_command = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.listBox_statefields = new System.Windows.Forms.ListBox();
            this.textbox_input_ignoreadd = new System.Windows.Forms.TextBox();
            this.button_addIgnore = new System.Windows.Forms.Button();
            this.button_reset = new System.Windows.Forms.Button();
            this.label_state_description = new System.Windows.Forms.Label();
            this.textbox_input_commandretry = new System.Windows.Forms.TextBox();
            this.label_command_description = new System.Windows.Forms.Label();
            this.label_repeat_description = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button_connect
            // 
            this.button_connect.Location = new System.Drawing.Point(108, 12);
            this.button_connect.Name = "button_connect";
            this.button_connect.Size = new System.Drawing.Size(75, 23);
            this.button_connect.TabIndex = 1;
            this.button_connect.Text = "Connect";
            this.button_connect.UseVisualStyleBackColor = true;
            this.button_connect.Click += new System.EventHandler(this.button_connect_Click);
            // 
            // textbox_input_port
            // 
            this.textbox_input_port.AcceptsReturn = true;
            this.textbox_input_port.Location = new System.Drawing.Point(12, 12);
            this.textbox_input_port.Name = "textbox_input_port";
            this.textbox_input_port.Size = new System.Drawing.Size(90, 20);
            this.textbox_input_port.TabIndex = 0;
            this.textbox_input_port.Text = "COM1";
            this.textbox_input_port.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_input_port_KeyDown);
            // 
            // textbox_out_stdout
            // 
            this.textbox_out_stdout.Location = new System.Drawing.Point(189, 64);
            this.textbox_out_stdout.Multiline = true;
            this.textbox_out_stdout.Name = "textbox_out_stdout";
            this.textbox_out_stdout.ReadOnly = true;
            this.textbox_out_stdout.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textbox_out_stdout.Size = new System.Drawing.Size(652, 95);
            this.textbox_out_stdout.TabIndex = 2;
            // 
            // button_disconnect
            // 
            this.button_disconnect.Location = new System.Drawing.Point(189, 12);
            this.button_disconnect.Name = "button_disconnect";
            this.button_disconnect.Size = new System.Drawing.Size(75, 23);
            this.button_disconnect.TabIndex = 3;
            this.button_disconnect.Text = "Disconnect";
            this.button_disconnect.UseVisualStyleBackColor = true;
            this.button_disconnect.Click += new System.EventHandler(this.DisconnectFromRFLink);
            // 
            // textbox_out_status
            // 
            this.textbox_out_status.BackColor = System.Drawing.SystemColors.InfoText;
            this.textbox_out_status.ForeColor = System.Drawing.SystemColors.Window;
            this.textbox_out_status.Location = new System.Drawing.Point(12, 39);
            this.textbox_out_status.Name = "textbox_out_status";
            this.textbox_out_status.ReadOnly = true;
            this.textbox_out_status.Size = new System.Drawing.Size(333, 20);
            this.textbox_out_status.TabIndex = 4;
            this.textbox_out_status.Text = "Waiting for status";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 165);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(829, 258);
            this.dataGridView1.TabIndex = 99;
            // 
            // textbox_input_command
            // 
            this.textbox_input_command.Location = new System.Drawing.Point(376, 39);
            this.textbox_input_command.Name = "textbox_input_command";
            this.textbox_input_command.Size = new System.Drawing.Size(249, 20);
            this.textbox_input_command.TabIndex = 2;
            this.textbox_input_command.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textbox_input_command_KeyDown);
            // 
            // button_send
            // 
            this.button_send.Location = new System.Drawing.Point(675, 37);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(75, 23);
            this.button_send.TabIndex = 7;
            this.button_send.Text = "Send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.SendData);
            // 
            // listBox_ignore
            // 
            this.listBox_statefields.FormattingEnabled = true;
            this.listBox_statefields.Items.AddRange(new object[] {
            "BAT",
            "CHIME",
            "CMD",
            "TEMP"});
            this.listBox_statefields.Location = new System.Drawing.Point(12, 103);
            this.listBox_statefields.Name = "listBox_ignore";
            this.listBox_statefields.Size = new System.Drawing.Size(171, 56);
            this.listBox_statefields.Sorted = true;
            this.listBox_statefields.TabIndex = 8;
            this.listBox_statefields.DoubleClick += new System.EventHandler(this.RemoveStateField);
            // 
            // textbox_input_ignoreadd
            // 
            this.textbox_input_ignoreadd.Location = new System.Drawing.Point(12, 64);
            this.textbox_input_ignoreadd.Name = "textbox_input_ignoreadd";
            this.textbox_input_ignoreadd.Size = new System.Drawing.Size(118, 20);
            this.textbox_input_ignoreadd.TabIndex = 9;
            this.textbox_input_ignoreadd.Text = "Add new state field...";
            // 
            // button_addIgnore
            // 
            this.button_addIgnore.Location = new System.Drawing.Point(137, 64);
            this.button_addIgnore.Name = "button_addIgnore";
            this.button_addIgnore.Size = new System.Drawing.Size(46, 20);
            this.button_addIgnore.TabIndex = 10;
            this.button_addIgnore.Text = "Add";
            this.button_addIgnore.UseVisualStyleBackColor = true;
            this.button_addIgnore.Click += new System.EventHandler(this.AddNewStateField);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(270, 12);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 4;
            this.button_reset.Text = "Reset Table";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.ResetDataGrid);
            // 
            // label_state_description
            // 
            this.label_state_description.AutoSize = true;
            this.label_state_description.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_state_description.Location = new System.Drawing.Point(12, 89);
            this.label_state_description.Name = "label_state_description";
            this.label_state_description.Size = new System.Drawing.Size(174, 7);
            this.label_state_description.TabIndex = 100;
            this.label_state_description.Text = "State fields are ignored when calculating device key";
            // 
            // textbox_input_commandretry
            // 
            this.textbox_input_commandretry.Location = new System.Drawing.Point(632, 39);
            this.textbox_input_commandretry.Name = "textbox_input_commandretry";
            this.textbox_input_commandretry.Size = new System.Drawing.Size(37, 20);
            this.textbox_input_commandretry.TabIndex = 101;
            this.textbox_input_commandretry.Text = "1";
            // 
            // label_command_description
            // 
            this.label_command_description.AutoSize = true;
            this.label_command_description.Location = new System.Drawing.Point(373, 22);
            this.label_command_description.Name = "label_command_description";
            this.label_command_description.Size = new System.Drawing.Size(54, 13);
            this.label_command_description.TabIndex = 102;
            this.label_command_description.Text = "Command";
            // 
            // label_repeat_description
            // 
            this.label_repeat_description.AutoSize = true;
            this.label_repeat_description.Location = new System.Drawing.Point(629, 22);
            this.label_repeat_description.Name = "label_repeat_description";
            this.label_repeat_description.Size = new System.Drawing.Size(42, 13);
            this.label_repeat_description.TabIndex = 102;
            this.label_repeat_description.Text = "Repeat";
            // 
            // RFLinkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 435);
            this.Controls.Add(this.label_repeat_description);
            this.Controls.Add(this.label_command_description);
            this.Controls.Add(this.textbox_input_commandretry);
            this.Controls.Add(this.label_state_description);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.button_addIgnore);
            this.Controls.Add(this.textbox_input_ignoreadd);
            this.Controls.Add(this.listBox_statefields);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textbox_input_command);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textbox_out_status);
            this.Controls.Add(this.button_disconnect);
            this.Controls.Add(this.textbox_out_stdout);
            this.Controls.Add(this.textbox_input_port);
            this.Controls.Add(this.button_connect);
            this.Name = "RFLinkForm";
            this.Text = "RFLink Controller";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_connect;
        private System.Windows.Forms.TextBox textbox_input_port;
        private System.Windows.Forms.TextBox textbox_out_stdout;
        private System.Windows.Forms.Button button_disconnect;
        private System.Windows.Forms.TextBox textbox_out_status;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textbox_input_command;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.ListBox listBox_statefields;
        private System.Windows.Forms.TextBox textbox_input_ignoreadd;
        private System.Windows.Forms.Button button_addIgnore;
        private System.Windows.Forms.Button button_reset;
        private Label label_state_description;
        private TextBox textbox_input_commandretry;
        private Label label_command_description;
        private Label label_repeat_description;
    }
}

