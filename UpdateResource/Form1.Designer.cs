namespace UpdateResource
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbReadDB = new System.Windows.Forms.TextBox();
            this.btnSupplierCategory_ResourceByName = new System.Windows.Forms.Button();
            this.btnSystemCategoryByName = new System.Windows.Forms.Button();
            this.t = new System.Windows.Forms.Label();
            this.tbWriteDB = new System.Windows.Forms.TextBox();
            this.tbResource = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnOrg = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(281, 108);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 30);
            this.label1.TabIndex = 51;
            this.label1.Text = "ReadDB：";
            // 
            // tbReadDB
            // 
            this.tbReadDB.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbReadDB.Location = new System.Drawing.Point(528, 96);
            this.tbReadDB.Name = "tbReadDB";
            this.tbReadDB.Size = new System.Drawing.Size(858, 42);
            this.tbReadDB.TabIndex = 50;
            this.tbReadDB.Text = " data source=.;database=YZ_AuthCenter\r\n;user id=sa;password=yzw@123;Timeout=30;";
            // 
            // btnSupplierCategory_ResourceByName
            // 
            this.btnSupplierCategory_ResourceByName.Location = new System.Drawing.Point(513, 533);
            this.btnSupplierCategory_ResourceByName.Name = "btnSupplierCategory_ResourceByName";
            this.btnSupplierCategory_ResourceByName.Size = new System.Drawing.Size(191, 72);
            this.btnSupplierCategory_ResourceByName.TabIndex = 48;
            this.btnSupplierCategory_ResourceByName.Text = "更新分供方分类资源";
            this.btnSupplierCategory_ResourceByName.UseVisualStyleBackColor = true;
            this.btnSupplierCategory_ResourceByName.Click += new System.EventHandler(this.btnSupplierCategory_ResourceByName_Click);
            // 
            // btnSystemCategoryByName
            // 
            this.btnSystemCategoryByName.Location = new System.Drawing.Point(286, 533);
            this.btnSystemCategoryByName.Name = "btnSystemCategoryByName";
            this.btnSystemCategoryByName.Size = new System.Drawing.Size(191, 72);
            this.btnSystemCategoryByName.TabIndex = 47;
            this.btnSystemCategoryByName.Text = "更新品类资源";
            this.btnSystemCategoryByName.UseVisualStyleBackColor = true;
            this.btnSystemCategoryByName.Click += new System.EventHandler(this.btnSystemCategoryByName_Click);
            // 
            // t
            // 
            this.t.AutoSize = true;
            this.t.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.t.Location = new System.Drawing.Point(281, 177);
            this.t.Name = "t";
            this.t.Size = new System.Drawing.Size(148, 30);
            this.t.TabIndex = 46;
            this.t.Text = "WriteDB：";
            // 
            // tbWriteDB
            // 
            this.tbWriteDB.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbWriteDB.Location = new System.Drawing.Point(528, 165);
            this.tbWriteDB.Name = "tbWriteDB";
            this.tbWriteDB.Size = new System.Drawing.Size(858, 42);
            this.tbWriteDB.TabIndex = 45;
            this.tbWriteDB.Text = " data source=.;database=YZ_AuthCenter\r\n;user id=sa;password=yzw@123;Timeout=30;";
            // 
            // tbResource
            // 
            this.tbResource.Location = new System.Drawing.Point(528, 275);
            this.tbResource.Multiline = true;
            this.tbResource.Name = "tbResource";
            this.tbResource.Size = new System.Drawing.Size(858, 72);
            this.tbResource.TabIndex = 40;
            this.tbResource.Text = "C:\\Users\\admin\\Desktop\\法语\\品类 - 法语.xlsx";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(285, 275);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(191, 72);
            this.btnSelect.TabIndex = 39;
            this.btnSelect.Text = "选择数据";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnOrg
            // 
            this.btnOrg.Location = new System.Drawing.Point(729, 533);
            this.btnOrg.Name = "btnOrg";
            this.btnOrg.Size = new System.Drawing.Size(191, 72);
            this.btnOrg.TabIndex = 52;
            this.btnOrg.Text = "更新组织资源";
            this.btnOrg.UseVisualStyleBackColor = true;
            this.btnOrg.Click += new System.EventHandler(this.btnOrg_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1666, 700);
            this.Controls.Add(this.btnOrg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbReadDB);
            this.Controls.Add(this.btnSupplierCategory_ResourceByName);
            this.Controls.Add(this.btnSystemCategoryByName);
            this.Controls.Add(this.t);
            this.Controls.Add(this.tbWriteDB);
            this.Controls.Add(this.tbResource);
            this.Controls.Add(this.btnSelect);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbReadDB;
        private System.Windows.Forms.Button btnSupplierCategory_ResourceByName;
        private System.Windows.Forms.Button btnSystemCategoryByName;
        private System.Windows.Forms.Label t;
        private System.Windows.Forms.TextBox tbWriteDB;
        private System.Windows.Forms.TextBox tbResource;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnOrg;
    }
}

