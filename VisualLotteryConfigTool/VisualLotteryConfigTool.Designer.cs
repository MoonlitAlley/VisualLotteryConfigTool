namespace VisualLotteryConfigTool
{
    partial class VisualLotteryConfigTool
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGetXlsxFile = new System.Windows.Forms.Button();
            this.btnGetGeneralConfig = new System.Windows.Forms.Button();
            this.btnGetLotteryConfig = new System.Windows.Forms.Button();
            this.btnGetFitmentsConfig = new System.Windows.Forms.Button();
            this.btnGetMobileConfig = new System.Windows.Forms.Button();
            this.btnGetTimelinessConfig = new System.Windows.Forms.Button();
            this.GeneralConfigText = new System.Windows.Forms.TextBox();
            this.XlsxConfigFileText = new System.Windows.Forms.TextBox();
            this.LotteryConfigText = new System.Windows.Forms.TextBox();
            this.FitmentsConfigText = new System.Windows.Forms.TextBox();
            this.MobileConfigText = new System.Windows.Forms.TextBox();
            this.TimelinessText = new System.Windows.Forms.TextBox();
            this.LogBox = new System.Windows.Forms.TextBox();
            this.btnGenerateAll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnGetXlsxFile
            // 
            this.btnGetXlsxFile.Location = new System.Drawing.Point(148, 35);
            this.btnGetXlsxFile.Name = "btnGetXlsxFile";
            this.btnGetXlsxFile.Size = new System.Drawing.Size(158, 23);
            this.btnGetXlsxFile.TabIndex = 0;
            this.btnGetXlsxFile.Text = "选择配置文件";
            this.btnGetXlsxFile.UseVisualStyleBackColor = true;
            this.btnGetXlsxFile.Click += new System.EventHandler(this.btnGetXlsxFile_Click);
            // 
            // btnGetGeneralConfig
            // 
            this.btnGetGeneralConfig.Location = new System.Drawing.Point(117, 106);
            this.btnGetGeneralConfig.Name = "btnGetGeneralConfig";
            this.btnGetGeneralConfig.Size = new System.Drawing.Size(240, 23);
            this.btnGetGeneralConfig.TabIndex = 1;
            this.btnGetGeneralConfig.Text = "选择 general_lottery_config.xml";
            this.btnGetGeneralConfig.UseVisualStyleBackColor = true;
            this.btnGetGeneralConfig.Click += new System.EventHandler(this.btnGetGeneralConfig_Click);
            // 
            // btnGetLotteryConfig
            // 
            this.btnGetLotteryConfig.Location = new System.Drawing.Point(117, 156);
            this.btnGetLotteryConfig.Name = "btnGetLotteryConfig";
            this.btnGetLotteryConfig.Size = new System.Drawing.Size(240, 23);
            this.btnGetLotteryConfig.TabIndex = 2;
            this.btnGetLotteryConfig.Text = "选择 visible_lottery_config.xml";
            this.btnGetLotteryConfig.UseVisualStyleBackColor = true;
            this.btnGetLotteryConfig.Click += new System.EventHandler(this.btnGetLotteryConfig_Click);
            // 
            // btnGetFitmentsConfig
            // 
            this.btnGetFitmentsConfig.Location = new System.Drawing.Point(117, 210);
            this.btnGetFitmentsConfig.Name = "btnGetFitmentsConfig";
            this.btnGetFitmentsConfig.Size = new System.Drawing.Size(240, 23);
            this.btnGetFitmentsConfig.TabIndex = 3;
            this.btnGetFitmentsConfig.Text = "visible_lottery_fitments_config.xml";
            this.btnGetFitmentsConfig.UseVisualStyleBackColor = true;
            this.btnGetFitmentsConfig.Click += new System.EventHandler(this.btnGetFitmentsConfig_Click);
            // 
            // btnGetMobileConfig
            // 
            this.btnGetMobileConfig.Location = new System.Drawing.Point(117, 265);
            this.btnGetMobileConfig.Name = "btnGetMobileConfig";
            this.btnGetMobileConfig.Size = new System.Drawing.Size(240, 23);
            this.btnGetMobileConfig.TabIndex = 4;
            this.btnGetMobileConfig.Text = "选择 mobile_config.xml";
            this.btnGetMobileConfig.UseVisualStyleBackColor = true;
            this.btnGetMobileConfig.Click += new System.EventHandler(this.btnGetMobileConfig_Click);
            // 
            // btnGetTimelinessConfig
            // 
            this.btnGetTimelinessConfig.Location = new System.Drawing.Point(117, 315);
            this.btnGetTimelinessConfig.Name = "btnGetTimelinessConfig";
            this.btnGetTimelinessConfig.Size = new System.Drawing.Size(240, 23);
            this.btnGetTimelinessConfig.TabIndex = 5;
            this.btnGetTimelinessConfig.Text = "选择 item_timeliness_config.xml";
            this.btnGetTimelinessConfig.UseVisualStyleBackColor = true;
            this.btnGetTimelinessConfig.Click += new System.EventHandler(this.btnGetTimelinessConfig_Click);
            // 
            // GeneralConfigText
            // 
            this.GeneralConfigText.AllowDrop = true;
            this.GeneralConfigText.Location = new System.Drawing.Point(399, 106);
            this.GeneralConfigText.Name = "GeneralConfigText";
            this.GeneralConfigText.Size = new System.Drawing.Size(273, 21);
            this.GeneralConfigText.TabIndex = 6;
            this.GeneralConfigText.DragDrop += new System.Windows.Forms.DragEventHandler(this.GeneralConfigTextDragDrop);
            this.GeneralConfigText.DragEnter += new System.Windows.Forms.DragEventHandler(this.GeneralConfigTextDragEnter);
            // 
            // XlsxConfigFileText
            // 
            this.XlsxConfigFileText.AllowDrop = true;
            this.XlsxConfigFileText.Location = new System.Drawing.Point(399, 35);
            this.XlsxConfigFileText.Name = "XlsxConfigFileText";
            this.XlsxConfigFileText.Size = new System.Drawing.Size(273, 21);
            this.XlsxConfigFileText.TabIndex = 7;
            this.XlsxConfigFileText.DragDrop += new System.Windows.Forms.DragEventHandler(this.XlsxConfigFileTextDragDrop);
            this.XlsxConfigFileText.DragEnter += new System.Windows.Forms.DragEventHandler(this.XlsxConfigFileTextDragEnter);
            // 
            // LotteryConfigText
            // 
            this.LotteryConfigText.AllowDrop = true;
            this.LotteryConfigText.Location = new System.Drawing.Point(399, 156);
            this.LotteryConfigText.Name = "LotteryConfigText";
            this.LotteryConfigText.Size = new System.Drawing.Size(273, 21);
            this.LotteryConfigText.TabIndex = 8;
            this.LotteryConfigText.DragDrop += new System.Windows.Forms.DragEventHandler(this.LotteryConfigTextDragDrop);
            this.LotteryConfigText.DragEnter += new System.Windows.Forms.DragEventHandler(this.LotteryConfigTextDragEnter);
            // 
            // FitmentsConfigText
            // 
            this.FitmentsConfigText.AllowDrop = true;
            this.FitmentsConfigText.Location = new System.Drawing.Point(399, 210);
            this.FitmentsConfigText.Name = "FitmentsConfigText";
            this.FitmentsConfigText.Size = new System.Drawing.Size(273, 21);
            this.FitmentsConfigText.TabIndex = 9;
            this.FitmentsConfigText.DragDrop += new System.Windows.Forms.DragEventHandler(this.FitmentsConfigTextDragDrop);
            this.FitmentsConfigText.DragEnter += new System.Windows.Forms.DragEventHandler(this.FitmentsConfigTextDragEnter);
            // 
            // MobileConfigText
            // 
            this.MobileConfigText.AllowDrop = true;
            this.MobileConfigText.Location = new System.Drawing.Point(399, 265);
            this.MobileConfigText.Name = "MobileConfigText";
            this.MobileConfigText.Size = new System.Drawing.Size(273, 21);
            this.MobileConfigText.TabIndex = 10;
            this.MobileConfigText.DragDrop += new System.Windows.Forms.DragEventHandler(this.MobileConfigTextDragDrop);
            this.MobileConfigText.DragEnter += new System.Windows.Forms.DragEventHandler(this.MobileConfigTextDragEnter);
            // 
            // TimelinessText
            // 
            this.TimelinessText.AllowDrop = true;
            this.TimelinessText.Location = new System.Drawing.Point(399, 315);
            this.TimelinessText.Name = "TimelinessText";
            this.TimelinessText.Size = new System.Drawing.Size(273, 21);
            this.TimelinessText.TabIndex = 11;
            this.TimelinessText.DragDrop += new System.Windows.Forms.DragEventHandler(this.TimelinessTextDragDrop);
            this.TimelinessText.DragEnter += new System.Windows.Forms.DragEventHandler(this.TimelinessTextDragEnter);
            // 
            // LogBox
            // 
            this.LogBox.Location = new System.Drawing.Point(39, 417);
            this.LogBox.Name = "LogBox";
            this.LogBox.Size = new System.Drawing.Size(267, 21);
            this.LogBox.TabIndex = 12;
            this.LogBox.Text = "进度显示...";
            // 
            // btnGenerateAll
            // 
            this.btnGenerateAll.Location = new System.Drawing.Point(313, 372);
            this.btnGenerateAll.Name = "btnGenerateAll";
            this.btnGenerateAll.Size = new System.Drawing.Size(135, 23);
            this.btnGenerateAll.TabIndex = 13;
            this.btnGenerateAll.Text = "一键导出";
            this.btnGenerateAll.UseVisualStyleBackColor = true;
            this.btnGenerateAll.Click += new System.EventHandler(this.btnGenerateAll_Click);
            // 
            // VisualLotteryConfigTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnGenerateAll);
            this.Controls.Add(this.LogBox);
            this.Controls.Add(this.TimelinessText);
            this.Controls.Add(this.MobileConfigText);
            this.Controls.Add(this.FitmentsConfigText);
            this.Controls.Add(this.LotteryConfigText);
            this.Controls.Add(this.XlsxConfigFileText);
            this.Controls.Add(this.GeneralConfigText);
            this.Controls.Add(this.btnGetTimelinessConfig);
            this.Controls.Add(this.btnGetMobileConfig);
            this.Controls.Add(this.btnGetFitmentsConfig);
            this.Controls.Add(this.btnGetLotteryConfig);
            this.Controls.Add(this.btnGetGeneralConfig);
            this.Controls.Add(this.btnGetXlsxFile);
            this.Name = "VisualLotteryConfigTool";
            this.Text = "可视化抽奖配置工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetXlsxFile;
        private System.Windows.Forms.Button btnGetGeneralConfig;
        private System.Windows.Forms.Button btnGetLotteryConfig;
        private System.Windows.Forms.Button btnGetFitmentsConfig;
        private System.Windows.Forms.Button btnGetMobileConfig;
        private System.Windows.Forms.Button btnGetTimelinessConfig;
        private System.Windows.Forms.TextBox GeneralConfigText;
        private System.Windows.Forms.TextBox XlsxConfigFileText;
        private System.Windows.Forms.TextBox LotteryConfigText;
        private System.Windows.Forms.TextBox FitmentsConfigText;
        private System.Windows.Forms.TextBox MobileConfigText;
        private System.Windows.Forms.TextBox TimelinessText;
        private System.Windows.Forms.TextBox LogBox;
        private System.Windows.Forms.Button btnGenerateAll;
    }
}

