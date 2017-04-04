namespace LOAMS
{
    partial class MainForm
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            DevExpress.XtraEditors.TileItemElement tileItemElement1 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement2 = new DevExpress.XtraEditors.TileItemElement();
            DevExpress.XtraEditors.TileItemElement tileItemElement3 = new DevExpress.XtraEditors.TileItemElement();
            this.radialMenu1 = new DevExpress.XtraBars.Ribbon.RadialMenu(this.components);
            this.tileControl1 = new DevExpress.XtraEditors.TileControl();
            this.tileGroup2 = new DevExpress.XtraEditors.TileGroup();
            this.tileItem1 = new DevExpress.XtraEditors.TileItem();
            this.tileItem2 = new DevExpress.XtraEditors.TileItem();
            this.tileItem3 = new DevExpress.XtraEditors.TileItem();
            ((System.ComponentModel.ISupportInitialize)(this.radialMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // radialMenu1
            // 
            this.radialMenu1.Glyph = ((System.Drawing.Image)(resources.GetObject("radialMenu1.Glyph")));
            this.radialMenu1.Name = "radialMenu1";
            // 
            // tileControl1
            // 
            this.tileControl1.AllowGlyphSkinning = true;
            this.tileControl1.DragSize = new System.Drawing.Size(0, 0);
            this.tileControl1.Groups.Add(this.tileGroup2);
            this.tileControl1.ItemTextShowMode = DevExpress.XtraEditors.TileItemContentShowMode.Always;
            this.tileControl1.Location = new System.Drawing.Point(3, 1);
            this.tileControl1.LookAndFeel.SkinName = "Visual Studio 2013 Blue";
            this.tileControl1.MaxId = 4;
            this.tileControl1.Name = "tileControl1";
            this.tileControl1.SelectionColor = System.Drawing.Color.Blue;
            this.tileControl1.Size = new System.Drawing.Size(403, 180);
            this.tileControl1.TabIndex = 0;
            this.tileControl1.Text = "tileControl";
            // 
            // tileGroup2
            // 
            this.tileGroup2.Items.Add(this.tileItem1);
            this.tileGroup2.Items.Add(this.tileItem2);
            this.tileGroup2.Items.Add(this.tileItem3);
            this.tileGroup2.Name = "tileGroup2";
            // 
            // tileItem1
            // 
            this.tileItem1.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.tileItem1.ContentAnimation = DevExpress.XtraEditors.TileItemContentAnimationType.Fade;
            tileItemElement1.Text = "Level2";
            tileItemElement1.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileItem1.Elements.Add(tileItemElement1);
            this.tileItem1.Id = 1;
            this.tileItem1.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem1.Name = "tileItem1";
            this.tileItem1.TextShowMode = DevExpress.XtraEditors.TileItemContentShowMode.Always;
            // 
            // tileItem2
            // 
            this.tileItem2.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.tileItem2.ContentAnimation = DevExpress.XtraEditors.TileItemContentAnimationType.Fade;
            tileItemElement2.Text = "Spreads";
            tileItemElement2.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileItem2.Elements.Add(tileItemElement2);
            this.tileItem2.Id = 2;
            this.tileItem2.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem2.Name = "tileItem2";
            // 
            // tileItem3
            // 
            this.tileItem3.AllowGlyphSkinning = DevExpress.Utils.DefaultBoolean.True;
            this.tileItem3.AppearanceItem.Normal.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tileItem3.AppearanceItem.Normal.Options.UseFont = true;
            this.tileItem3.ContentAnimation = DevExpress.XtraEditors.TileItemContentAnimationType.Fade;
            tileItemElement3.Text = "Orders";
            tileItemElement3.TextAlignment = DevExpress.XtraEditors.TileItemContentAlignment.MiddleCenter;
            this.tileItem3.Elements.Add(tileItemElement3);
            this.tileItem3.Id = 3;
            this.tileItem3.ItemSize = DevExpress.XtraEditors.TileItemSize.Medium;
            this.tileItem3.Name = "tileItem3";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(407, 193);
            this.Controls.Add(this.tileControl1);
            this.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.Name = "MainForm";
            this.Text = "ShaveTrader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radialMenu1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraBars.Ribbon.RadialMenu radialMenu1;
        private DevExpress.XtraEditors.TileControl tileControl1;
        private DevExpress.XtraEditors.TileGroup tileGroup2;
        private DevExpress.XtraEditors.TileItem tileItem1;
        private DevExpress.XtraEditors.TileItem tileItem2;
        private DevExpress.XtraEditors.TileItem tileItem3;
    }
}

