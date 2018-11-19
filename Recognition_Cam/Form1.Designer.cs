namespace Recognition_Cam
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.Load_from_camera_Button = new System.Windows.Forms.Button();
			this.Camera_comBox = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonTrain = new System.Windows.Forms.Button();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.ChangeInfoButton = new System.Windows.Forms.Button();
			this.DeleteFace = new System.Windows.Forms.Button();
			this.UpdateFaceInfo = new System.Windows.Forms.Button();
			this.textBoxNewName = new System.Windows.Forms.TextBox();
			this.buttonLastImg = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.buttonNextImg = new System.Windows.Forms.Button();
			this.buttonPrevImg = new System.Windows.Forms.Button();
			this.buttonFirstImg = new System.Windows.Forms.Button();
			this.pictureBoxImgFromDB = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.PhotoCount = new System.Windows.Forms.NumericUpDown();
			this.Stop_button = new System.Windows.Forms.Button();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.AddInfoButton = new System.Windows.Forms.Button();
			this.AddNewFaceButton = new System.Windows.Forms.Button();
			this.textBoxFaceName = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.ButtonNext = new System.Windows.Forms.Button();
			this.ButtonPrev = new System.Windows.Forms.Button();
			this.pictureBoxFaceAdd = new System.Windows.Forms.PictureBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.comboBox2 = new System.Windows.Forms.ComboBox();
			this.label5 = new System.Windows.Forms.Label();
			this.LoadImageButton = new System.Windows.Forms.Button();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.comboBox3 = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.labelName = new System.Windows.Forms.Label();
			this.button14 = new System.Windows.Forms.Button();
			this.CamChengeBox = new System.Windows.Forms.ComboBox();
			this.LoadImgToRecognize = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxImgFromDB)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PhotoCount)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFaceAdd)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			this.tabPage2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 13);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(913, 558);
			this.tabControl1.TabIndex = 0;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.TabSelectedChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.White;
			this.tabPage1.Controls.Add(this.Load_from_camera_Button);
			this.tabPage1.Controls.Add(this.Camera_comBox);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.buttonTrain);
			this.tabPage1.Controls.Add(this.groupBox4);
			this.tabPage1.Controls.Add(this.groupBox2);
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.LoadImageButton);
			this.tabPage1.Controls.Add(this.pictureBox2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(905, 532);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Обучение";
			// 
			// Load_from_camera_Button
			// 
			this.Load_from_camera_Button.BackColor = System.Drawing.Color.Lavender;
			this.Load_from_camera_Button.Location = new System.Drawing.Point(198, 477);
			this.Load_from_camera_Button.Name = "Load_from_camera_Button";
			this.Load_from_camera_Button.Size = new System.Drawing.Size(138, 28);
			this.Load_from_camera_Button.TabIndex = 13;
			this.Load_from_camera_Button.Text = "Трансляция с камеры";
			this.Load_from_camera_Button.UseVisualStyleBackColor = false;
			this.Load_from_camera_Button.Click += new System.EventHandler(this.Load_from_camera_Button_Click);
			// 
			// Camera_comBox
			// 
			this.Camera_comBox.FormattingEnabled = true;
			this.Camera_comBox.Location = new System.Drawing.Point(342, 477);
			this.Camera_comBox.Name = "Camera_comBox";
			this.Camera_comBox.Size = new System.Drawing.Size(87, 21);
			this.Camera_comBox.TabIndex = 11;
			this.Camera_comBox.Text = "NONE";
			this.Camera_comBox.SelectedIndexChanged += new System.EventHandler(this.Camera_comBox_SelectedIndexChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.BackColor = System.Drawing.Color.White;
			this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.ForeColor = System.Drawing.Color.Black;
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.MaximumSize = new System.Drawing.Size(450, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(444, 75);
			this.label4.TabIndex = 8;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// buttonTrain
			// 
			this.buttonTrain.BackColor = System.Drawing.Color.LightSteelBlue;
			this.buttonTrain.Font = new System.Drawing.Font("Berlin Sans FB", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.buttonTrain.ForeColor = System.Drawing.Color.Black;
			this.buttonTrain.Location = new System.Drawing.Point(701, 496);
			this.buttonTrain.Name = "buttonTrain";
			this.buttonTrain.Size = new System.Drawing.Size(178, 28);
			this.buttonTrain.TabIndex = 7;
			this.buttonTrain.Text = "Обучить на новой базе лиц";
			this.buttonTrain.UseVisualStyleBackColor = false;
			this.buttonTrain.Click += new System.EventHandler(this.buttonTrain_Click);
			// 
			// groupBox4
			// 
			this.groupBox4.BackColor = System.Drawing.Color.White;
			this.groupBox4.Controls.Add(this.ChangeInfoButton);
			this.groupBox4.Controls.Add(this.DeleteFace);
			this.groupBox4.Controls.Add(this.UpdateFaceInfo);
			this.groupBox4.Controls.Add(this.textBoxNewName);
			this.groupBox4.Controls.Add(this.buttonLastImg);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.buttonNextImg);
			this.groupBox4.Controls.Add(this.buttonPrevImg);
			this.groupBox4.Controls.Add(this.buttonFirstImg);
			this.groupBox4.Controls.Add(this.pictureBoxImgFromDB);
			this.groupBox4.Location = new System.Drawing.Point(661, 181);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(235, 302);
			this.groupBox4.TabIndex = 6;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "Редактирование базы лиц";
			this.groupBox4.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
			// 
			// ChangeInfoButton
			// 
			this.ChangeInfoButton.BackColor = System.Drawing.Color.Lavender;
			this.ChangeInfoButton.Location = new System.Drawing.Point(6, 233);
			this.ChangeInfoButton.Name = "ChangeInfoButton";
			this.ChangeInfoButton.Size = new System.Drawing.Size(152, 28);
			this.ChangeInfoButton.TabIndex = 13;
			this.ChangeInfoButton.Text = "Изменить информацию";
			this.ChangeInfoButton.UseVisualStyleBackColor = false;
			this.ChangeInfoButton.Click += new System.EventHandler(this.ChangeInfoButton_Click);
			// 
			// DeleteFace
			// 
			this.DeleteFace.BackColor = System.Drawing.Color.Lavender;
			this.DeleteFace.Location = new System.Drawing.Point(167, 200);
			this.DeleteFace.Name = "DeleteFace";
			this.DeleteFace.Size = new System.Drawing.Size(62, 52);
			this.DeleteFace.TabIndex = 16;
			this.DeleteFace.Text = "Удалить фото из базы";
			this.DeleteFace.UseVisualStyleBackColor = false;
			this.DeleteFace.Click += new System.EventHandler(this.DeleteFace_Click);
			// 
			// UpdateFaceInfo
			// 
			this.UpdateFaceInfo.BackColor = System.Drawing.Color.Lavender;
			this.UpdateFaceInfo.Location = new System.Drawing.Point(6, 268);
			this.UpdateFaceInfo.Name = "UpdateFaceInfo";
			this.UpdateFaceInfo.Size = new System.Drawing.Size(152, 28);
			this.UpdateFaceInfo.TabIndex = 12;
			this.UpdateFaceInfo.Text = "Обновить";
			this.UpdateFaceInfo.UseVisualStyleBackColor = false;
			this.UpdateFaceInfo.Click += new System.EventHandler(this.UpdateFaceInfo_Click);
			// 
			// textBoxNewName
			// 
			this.textBoxNewName.Location = new System.Drawing.Point(6, 200);
			this.textBoxNewName.Name = "textBoxNewName";
			this.textBoxNewName.Size = new System.Drawing.Size(152, 20);
			this.textBoxNewName.TabIndex = 13;
			// 
			// buttonLastImg
			// 
			this.buttonLastImg.BackColor = System.Drawing.Color.WhiteSmoke;
			this.buttonLastImg.Location = new System.Drawing.Point(156, 150);
			this.buttonLastImg.Name = "buttonLastImg";
			this.buttonLastImg.Size = new System.Drawing.Size(41, 23);
			this.buttonLastImg.TabIndex = 15;
			this.buttonLastImg.Text = " >l";
			this.buttonLastImg.UseVisualStyleBackColor = false;
			this.buttonLastImg.Click += new System.EventHandler(this.buttonLastImg_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 184);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(32, 13);
			this.label3.TabIndex = 12;
			this.label3.Text = "Имя:";
			// 
			// buttonNextImg
			// 
			this.buttonNextImg.BackColor = System.Drawing.Color.WhiteSmoke;
			this.buttonNextImg.Location = new System.Drawing.Point(109, 150);
			this.buttonNextImg.Name = "buttonNextImg";
			this.buttonNextImg.Size = new System.Drawing.Size(41, 23);
			this.buttonNextImg.TabIndex = 14;
			this.buttonNextImg.Text = ">";
			this.buttonNextImg.UseVisualStyleBackColor = false;
			this.buttonNextImg.Click += new System.EventHandler(this.buttonNextImg_Click);
			// 
			// buttonPrevImg
			// 
			this.buttonPrevImg.BackColor = System.Drawing.Color.WhiteSmoke;
			this.buttonPrevImg.Location = new System.Drawing.Point(62, 150);
			this.buttonPrevImg.Name = "buttonPrevImg";
			this.buttonPrevImg.Size = new System.Drawing.Size(41, 23);
			this.buttonPrevImg.TabIndex = 13;
			this.buttonPrevImg.Text = " <";
			this.buttonPrevImg.UseVisualStyleBackColor = false;
			this.buttonPrevImg.Click += new System.EventHandler(this.buttonPrevImg_Click);
			// 
			// buttonFirstImg
			// 
			this.buttonFirstImg.BackColor = System.Drawing.Color.WhiteSmoke;
			this.buttonFirstImg.Location = new System.Drawing.Point(15, 150);
			this.buttonFirstImg.Name = "buttonFirstImg";
			this.buttonFirstImg.Size = new System.Drawing.Size(41, 23);
			this.buttonFirstImg.TabIndex = 12;
			this.buttonFirstImg.Text = " l<";
			this.buttonFirstImg.UseVisualStyleBackColor = false;
			this.buttonFirstImg.Click += new System.EventHandler(this.buttonFirstImg_Click);
			// 
			// pictureBoxImgFromDB
			// 
			this.pictureBoxImgFromDB.Location = new System.Drawing.Point(40, 19);
			this.pictureBoxImgFromDB.Name = "pictureBoxImgFromDB";
			this.pictureBoxImgFromDB.Size = new System.Drawing.Size(132, 125);
			this.pictureBoxImgFromDB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxImgFromDB.TabIndex = 12;
			this.pictureBoxImgFromDB.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.BackColor = System.Drawing.Color.White;
			this.groupBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.groupBox2.Controls.Add(this.PhotoCount);
			this.groupBox2.Controls.Add(this.Stop_button);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Controls.Add(this.AddInfoButton);
			this.groupBox2.Controls.Add(this.AddNewFaceButton);
			this.groupBox2.Controls.Add(this.textBoxFaceName);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Controls.Add(this.ButtonNext);
			this.groupBox2.Controls.Add(this.ButtonPrev);
			this.groupBox2.Controls.Add(this.pictureBoxFaceAdd);
			this.groupBox2.Controls.Add(this.groupBox3);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.groupBox2.Location = new System.Drawing.Point(458, 181);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(184, 322);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Добавление лица в базу";
			this.groupBox2.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
			// 
			// PhotoCount
			// 
			this.PhotoCount.Location = new System.Drawing.Point(134, 301);
			this.PhotoCount.Name = "PhotoCount";
			this.PhotoCount.Size = new System.Drawing.Size(44, 20);
			this.PhotoCount.TabIndex = 10;
			this.PhotoCount.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// Stop_button
			// 
			this.Stop_button.BackColor = System.Drawing.Color.WhiteSmoke;
			this.Stop_button.Location = new System.Drawing.Point(134, 298);
			this.Stop_button.Name = "Stop_button";
			this.Stop_button.Size = new System.Drawing.Size(47, 23);
			this.Stop_button.TabIndex = 13;
			this.Stop_button.Text = "Стоп";
			this.Stop_button.UseVisualStyleBackColor = false;
			this.Stop_button.Visible = false;
			this.Stop_button.Click += new System.EventHandler(this.Stop_button_Click);
			// 
			// checkBox1
			// 
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new System.Drawing.Point(9, 302);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(119, 17);
			this.checkBox1.TabIndex = 10;
			this.checkBox1.Text = "несколько кадров";
			this.checkBox1.UseVisualStyleBackColor = true;
			// 
			// AddInfoButton
			// 
			this.AddInfoButton.BackColor = System.Drawing.Color.Lavender;
			this.AddInfoButton.Location = new System.Drawing.Point(12, 233);
			this.AddInfoButton.Name = "AddInfoButton";
			this.AddInfoButton.Size = new System.Drawing.Size(152, 28);
			this.AddInfoButton.TabIndex = 12;
			this.AddInfoButton.Text = "Добавить информацию";
			this.AddInfoButton.UseVisualStyleBackColor = false;
			this.AddInfoButton.Click += new System.EventHandler(this.AddInfoButton_Click);
			// 
			// AddNewFaceButton
			// 
			this.AddNewFaceButton.BackColor = System.Drawing.Color.Lavender;
			this.AddNewFaceButton.Location = new System.Drawing.Point(12, 268);
			this.AddNewFaceButton.Name = "AddNewFaceButton";
			this.AddNewFaceButton.Size = new System.Drawing.Size(152, 28);
			this.AddNewFaceButton.TabIndex = 7;
			this.AddNewFaceButton.Text = "Добавить лицо в базу";
			this.AddNewFaceButton.UseVisualStyleBackColor = false;
			this.AddNewFaceButton.Click += new System.EventHandler(this.AddNewFaceButton_Click);
			// 
			// textBoxFaceName
			// 
			this.textBoxFaceName.Location = new System.Drawing.Point(10, 200);
			this.textBoxFaceName.Name = "textBoxFaceName";
			this.textBoxFaceName.Size = new System.Drawing.Size(152, 20);
			this.textBoxFaceName.TabIndex = 11;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 184);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 13);
			this.label2.TabIndex = 10;
			this.label2.Text = "Введите имя:";
			// 
			// ButtonNext
			// 
			this.ButtonNext.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ButtonNext.Location = new System.Drawing.Point(87, 150);
			this.ButtonNext.Name = "ButtonNext";
			this.ButtonNext.Size = new System.Drawing.Size(64, 23);
			this.ButtonNext.TabIndex = 9;
			this.ButtonNext.Text = "След.";
			this.ButtonNext.UseVisualStyleBackColor = false;
			this.ButtonNext.Click += new System.EventHandler(this.ButtonNext_Click);
			// 
			// ButtonPrev
			// 
			this.ButtonPrev.BackColor = System.Drawing.Color.WhiteSmoke;
			this.ButtonPrev.Location = new System.Drawing.Point(19, 150);
			this.ButtonPrev.Name = "ButtonPrev";
			this.ButtonPrev.Size = new System.Drawing.Size(62, 23);
			this.ButtonPrev.TabIndex = 8;
			this.ButtonPrev.Text = "Предыд.";
			this.ButtonPrev.UseVisualStyleBackColor = false;
			this.ButtonPrev.Click += new System.EventHandler(this.ButtonPrev_Click);
			// 
			// pictureBoxFaceAdd
			// 
			this.pictureBoxFaceAdd.Location = new System.Drawing.Point(19, 19);
			this.pictureBoxFaceAdd.Name = "pictureBoxFaceAdd";
			this.pictureBoxFaceAdd.Size = new System.Drawing.Size(132, 125);
			this.pictureBoxFaceAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxFaceAdd.TabIndex = 7;
			this.pictureBoxFaceAdd.TabStop = false;
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(188, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(199, 223);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Добавить";
			// 
			// groupBox1
			// 
			this.groupBox1.BackColor = System.Drawing.Color.White;
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.comboBox2);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(458, 16);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(387, 159);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Настройка обнаружения лица";
			this.groupBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.groupBox1_Paint);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(9, 62);
			this.label7.MaximumSize = new System.Drawing.Size(200, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(106, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Добавлено кадров:";
			this.label7.Visible = false;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.ForeColor = System.Drawing.Color.DarkRed;
			this.label6.Location = new System.Drawing.Point(131, 62);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(0, 13);
			this.label6.TabIndex = 17;
			// 
			// comboBox2
			// 
			this.comboBox2.FormattingEnabled = true;
			this.comboBox2.Items.AddRange(new object[] {
            "3",
            "5",
            "7",
            "10"});
			this.comboBox2.Location = new System.Drawing.Point(218, 26);
			this.comboBox2.Name = "comboBox2";
			this.comboBox2.Size = new System.Drawing.Size(39, 21);
			this.comboBox2.TabIndex = 1;
			this.comboBox2.Text = "10";
			this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 20);
			this.label5.MaximumSize = new System.Drawing.Size(200, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(193, 26);
			this.label5.TabIndex = 0;
			this.label5.Text = "Минимальное количество совпадений для признания объекта:";
			// 
			// LoadImageButton
			// 
			this.LoadImageButton.BackColor = System.Drawing.Color.Lavender;
			this.LoadImageButton.Location = new System.Drawing.Point(9, 477);
			this.LoadImageButton.Name = "LoadImageButton";
			this.LoadImageButton.Size = new System.Drawing.Size(156, 28);
			this.LoadImageButton.TabIndex = 2;
			this.LoadImageButton.Text = "Загрузить изображение";
			this.LoadImageButton.UseVisualStyleBackColor = false;
			this.LoadImageButton.Click += new System.EventHandler(this.LoadImageButton_Click);
			// 
			// pictureBox2
			// 
			this.pictureBox2.Location = new System.Drawing.Point(3, 116);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(426, 297);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox2.TabIndex = 0;
			this.pictureBox2.TabStop = false;
			// 
			// tabPage2
			// 
			this.tabPage2.BackColor = System.Drawing.Color.White;
			this.tabPage2.Controls.Add(this.listBox1);
			this.tabPage2.Controls.Add(this.comboBox3);
			this.tabPage2.Controls.Add(this.label12);
			this.tabPage2.Controls.Add(this.labelName);
			this.tabPage2.Controls.Add(this.button14);
			this.tabPage2.Controls.Add(this.CamChengeBox);
			this.tabPage2.Controls.Add(this.LoadImgToRecognize);
			this.tabPage2.Controls.Add(this.pictureBox1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(905, 532);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Распознавание";
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(612, 91);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(263, 277);
			this.listBox1.Sorted = true;
			this.listBox1.TabIndex = 16;
			// 
			// comboBox3
			// 
			this.comboBox3.FormattingEnabled = true;
			this.comboBox3.Items.AddRange(new object[] {
            "3",
            "5",
            "7",
            "10"});
			this.comboBox3.Location = new System.Drawing.Point(823, 27);
			this.comboBox3.Name = "comboBox3";
			this.comboBox3.Size = new System.Drawing.Size(39, 21);
			this.comboBox3.TabIndex = 9;
			this.comboBox3.Text = "10";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(609, 22);
			this.label12.MaximumSize = new System.Drawing.Size(200, 0);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(193, 26);
			this.label12.TabIndex = 8;
			this.label12.Text = "Минимальное количество совпадений для признания объекта:";
			// 
			// labelName
			// 
			this.labelName.AutoSize = true;
			this.labelName.ForeColor = System.Drawing.Color.DarkRed;
			this.labelName.Location = new System.Drawing.Point(624, 62);
			this.labelName.MaximumSize = new System.Drawing.Size(300, 0);
			this.labelName.Name = "labelName";
			this.labelName.Size = new System.Drawing.Size(0, 13);
			this.labelName.TabIndex = 6;
			// 
			// button14
			// 
			this.button14.Location = new System.Drawing.Point(612, 433);
			this.button14.Name = "button14";
			this.button14.Size = new System.Drawing.Size(156, 28);
			this.button14.TabIndex = 5;
			this.button14.Text = "Сохранить изображение";
			this.button14.UseVisualStyleBackColor = true;
			this.button14.Click += new System.EventHandler(this.button14_Click);
			// 
			// CamChengeBox
			// 
			this.CamChengeBox.FormattingEnabled = true;
			this.CamChengeBox.Location = new System.Drawing.Point(467, 440);
			this.CamChengeBox.Name = "CamChengeBox";
			this.CamChengeBox.Size = new System.Drawing.Size(123, 21);
			this.CamChengeBox.TabIndex = 3;
			this.CamChengeBox.Text = "NONE";
			// 
			// LoadImgToRecognize
			// 
			this.LoadImgToRecognize.Location = new System.Drawing.Point(61, 435);
			this.LoadImgToRecognize.Name = "LoadImgToRecognize";
			this.LoadImgToRecognize.Size = new System.Drawing.Size(156, 28);
			this.LoadImgToRecognize.TabIndex = 1;
			this.LoadImgToRecognize.Text = "Загрузить изображение";
			this.LoadImgToRecognize.UseVisualStyleBackColor = true;
			this.LoadImgToRecognize.Click += new System.EventHandler(this.LoadImageButton2_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Location = new System.Drawing.Point(6, 22);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(584, 407);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(924, 571);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "Form1";
			this.Text = "Camera_Capture";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxImgFromDB)).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PhotoCount)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxFaceAdd)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button LoadImgToRecognize;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox CamChengeBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonTrain;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button DeleteFace;
        private System.Windows.Forms.Button UpdateFaceInfo;
        private System.Windows.Forms.Button buttonLastImg;
        private System.Windows.Forms.Button buttonNextImg;
        private System.Windows.Forms.Button buttonPrevImg;
        private System.Windows.Forms.Button buttonFirstImg;
        private System.Windows.Forms.PictureBox pictureBoxImgFromDB;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button AddNewFaceButton;
        private System.Windows.Forms.TextBox textBoxFaceName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonNext;
        private System.Windows.Forms.Button ButtonPrev;
        private System.Windows.Forms.PictureBox pictureBoxFaceAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button LoadImageButton;
        private System.Windows.Forms.Button button14;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboBox2;
		private System.Windows.Forms.ComboBox comboBox3;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Button ChangeInfoButton;
		private System.Windows.Forms.Button AddInfoButton;
		private System.Windows.Forms.TextBox textBoxNewName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button Stop_button;
		private System.Windows.Forms.NumericUpDown PhotoCount;
		private System.Windows.Forms.ComboBox Camera_comBox;
		private System.Windows.Forms.Button Load_from_camera_Button;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label labelName;
	}
}

