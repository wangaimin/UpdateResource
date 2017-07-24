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
            this.btnArea = new System.Windows.Forms.Button();
            this.btnBidTool_TenderBidStatusItem = new System.Windows.Forms.Button();
            this.btnMenu = new System.Windows.Forms.Button();
            this.btnSystemTagRole = new System.Windows.Forms.Button();
            this.btnSystemFunction = new System.Windows.Forms.Button();
            this.btnAuditNode = new System.Windows.Forms.Button();
            this.btnApplication = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
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
            this.btnSupplierCategory_ResourceByName.Text = "分供方分类资源";
            this.btnSupplierCategory_ResourceByName.UseVisualStyleBackColor = true;
            this.btnSupplierCategory_ResourceByName.Click += new System.EventHandler(this.btnSupplierCategory_ResourceByName_Click);
            // 
            // btnSystemCategoryByName
            // 
            this.btnSystemCategoryByName.Location = new System.Drawing.Point(286, 533);
            this.btnSystemCategoryByName.Name = "btnSystemCategoryByName";
            this.btnSystemCategoryByName.Size = new System.Drawing.Size(191, 72);
            this.btnSystemCategoryByName.TabIndex = 47;
            this.btnSystemCategoryByName.Text = "品类资源";
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
            this.btnOrg.Text = "组织资源";
            this.btnOrg.UseVisualStyleBackColor = true;
            this.btnOrg.Click += new System.EventHandler(this.btnOrg_Click);
            // 
            // btnArea
            // 
            this.btnArea.Location = new System.Drawing.Point(937, 533);
            this.btnArea.Name = "btnArea";
            this.btnArea.Size = new System.Drawing.Size(191, 72);
            this.btnArea.TabIndex = 53;
            this.btnArea.Text = "区域资源";
            this.btnArea.UseVisualStyleBackColor = true;
            this.btnArea.Click += new System.EventHandler(this.btnArea_Click);
            // 
            // btnBidTool_TenderBidStatusItem
            // 
            this.btnBidTool_TenderBidStatusItem.Location = new System.Drawing.Point(1154, 533);
            this.btnBidTool_TenderBidStatusItem.Name = "btnBidTool_TenderBidStatusItem";
            this.btnBidTool_TenderBidStatusItem.Size = new System.Drawing.Size(191, 72);
            this.btnBidTool_TenderBidStatusItem.TabIndex = 54;
            this.btnBidTool_TenderBidStatusItem.Text = "投标状态资源";
            this.btnBidTool_TenderBidStatusItem.UseVisualStyleBackColor = true;
            this.btnBidTool_TenderBidStatusItem.Click += new System.EventHandler(this.btnBidTool_TenderBidStatusItem_Click);
            // 
            // btnMenu
            // 
            this.btnMenu.Location = new System.Drawing.Point(286, 646);
            this.btnMenu.Name = "btnMenu";
            this.btnMenu.Size = new System.Drawing.Size(191, 72);
            this.btnMenu.TabIndex = 55;
            this.btnMenu.Text = "菜单资源";
            this.btnMenu.UseVisualStyleBackColor = true;
            this.btnMenu.Click += new System.EventHandler(this.btnMenu_Click);
            // 
            // btnSystemTagRole
            // 
            this.btnSystemTagRole.Location = new System.Drawing.Point(729, 646);
            this.btnSystemTagRole.Name = "btnSystemTagRole";
            this.btnSystemTagRole.Size = new System.Drawing.Size(191, 72);
            this.btnSystemTagRole.TabIndex = 56;
            this.btnSystemTagRole.Text = "角色类型资源";
            this.btnSystemTagRole.UseVisualStyleBackColor = true;
            this.btnSystemTagRole.Click += new System.EventHandler(this.btnSystemTagRole_Click);
            // 
            // btnSystemFunction
            // 
            this.btnSystemFunction.Location = new System.Drawing.Point(513, 646);
            this.btnSystemFunction.Name = "btnSystemFunction";
            this.btnSystemFunction.Size = new System.Drawing.Size(191, 72);
            this.btnSystemFunction.TabIndex = 57;
            this.btnSystemFunction.Text = "功能权限资源";
            this.btnSystemFunction.UseVisualStyleBackColor = true;
            this.btnSystemFunction.Click += new System.EventHandler(this.btnSystemFunction_Click);
            // 
            // btnAuditNode
            // 
            this.btnAuditNode.Location = new System.Drawing.Point(937, 646);
            this.btnAuditNode.Name = "btnAuditNode";
            this.btnAuditNode.Size = new System.Drawing.Size(191, 72);
            this.btnAuditNode.TabIndex = 58;
            this.btnAuditNode.Text = "审批节点资源";
            this.btnAuditNode.UseVisualStyleBackColor = true;
            this.btnAuditNode.Click += new System.EventHandler(this.btnAuditNode_Click);
            // 
            // btnApplication
            // 
            this.btnApplication.Location = new System.Drawing.Point(1154, 646);
            this.btnApplication.Name = "btnApplication";
            this.btnApplication.Size = new System.Drawing.Size(191, 72);
            this.btnApplication.TabIndex = 59;
            this.btnApplication.Text = "系统资源";
            this.btnApplication.UseVisualStyleBackColor = true;
            this.btnApplication.Click += new System.EventHandler(this.btnApplication_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(90, 468);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 30);
            this.label2.TabIndex = 60;
            this.label2.Text = "新增、更新：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1666, 804);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnApplication);
            this.Controls.Add(this.btnAuditNode);
            this.Controls.Add(this.btnSystemFunction);
            this.Controls.Add(this.btnSystemTagRole);
            this.Controls.Add(this.btnMenu);
            this.Controls.Add(this.btnBidTool_TenderBidStatusItem);
            this.Controls.Add(this.btnArea);
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
            this.Text = "新增、更新多语言翻译";
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
        private System.Windows.Forms.Button btnArea;
        private System.Windows.Forms.Button btnBidTool_TenderBidStatusItem;
        private System.Windows.Forms.Button btnMenu;
        private System.Windows.Forms.Button btnSystemTagRole;
        private System.Windows.Forms.Button btnSystemFunction;
        private System.Windows.Forms.Button btnAuditNode;
        private System.Windows.Forms.Button btnApplication;
        private System.Windows.Forms.Label label2;
    }
}

