using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Data.SqlClient;
using System.Drawing.Drawing2D;
using System.Numerics;
using DirectShowLib;
using System.Threading.Tasks;

namespace Recognition_Cam {
	public partial class Form1 : Form {
		int Min_neighbors = 10;                 //минимальное количество совпадений в базе для признания объекта лицом
		public Emgu.CV.Image<Bgr,byte> Image_in_work;					//загруженное пользователем изображение для распознавания
		List<Bitmap> faces = new List<Bitmap>();//массив найденных на изображении лиц
		int face_number=0;                      //номер текущего обнаруженного лица
		//для запуска и выключения видеотрансляции
		System.Windows.Forms.Timer videoTimer = new System.Windows.Forms.Timer();
		//для автоматического режима добавления лиц без блокировки окна
		System.Windows.Forms.Timer addManyPhotoTimer = new System.Windows.Forms.Timer();
		public static Capture capture = new Capture();        //захват с камеры
		static int imgSize = 80;                //размер изображения лица для хранения в базе
		string info1;						    //строка для введения информации о пользователе в окне
		string info2;							//строка для изменения информации о пользователе в окне
		Point[] faces_points;					//прямоугольники с координатами лиц
		int photo_count = 10;                   //сколько лиц надо добавить во время автоматич.режима добав-я
		int addFotoCount = 0;                   //счетчик добавленных автоматически людей
		string addFotoName;						//имя добавляемого человека
		int imgVecSize = imgSize*imgSize;
		double threshold = 30000000;            //значение порога расстояния между значениями весов
		string[] MassOfInfo;					//информация о распознанных людях
		public static CascadeClassifier faceCascade = new CascadeClassifier("haarcascade_frontalface_alt.xml");
		//--Соединение с Базой Данных
		public static SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=FaceDB;Integrated Security=SSPI;");
		//--Создание адаптера для обмена данными с MS Access Table
		SqlDataAdapter dataAdapter; //для таб.Image
		SqlDataAdapter dataAdapter1; //для таб.Person
		SqlDataAdapter SettingAdapter; //для таб.Setting
		SqlDataAdapter EigenAdapter; //для таб.EigenFaces
		//--Создание таблиц (для получения данных с базы и записи в них)
		DataTable TSTable = new DataTable();
		DataTable TSTablePers = new DataTable();
		DataTable SettingTable = new DataTable();
		DataTable EigenTable = new DataTable();
		int TotalRows = 0; //номер последней строки 
		int rowNumber=0; //номер строки, на которой сейчас (работает как указатель)
		//--Подключение к БД
		private void ConnectToDB() {
			try {
				connection.Open();
				dataAdapter = new SqlDataAdapter("SELECT * FROM Image", connection);
				dataAdapter1 = new SqlDataAdapter("SELECT * FROM Person", connection);
				SettingAdapter = new SqlDataAdapter("SELECT * FROM Setting", connection);
				EigenAdapter = new SqlDataAdapter("SELECT * FROM EigenFaces", connection);
				SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
				SqlCommandBuilder commandBuilder1 = new SqlCommandBuilder(dataAdapter1);
				SqlCommandBuilder commandBuilder2 = new SqlCommandBuilder(SettingAdapter);
				SqlCommandBuilder commandBuilder3 = new SqlCommandBuilder(EigenAdapter);
				dataAdapter.Fill(TSTable);
				dataAdapter1.Fill(TSTablePers);
				SettingAdapter.Fill(SettingTable);
				EigenAdapter.Fill(EigenTable);
				//если таблица не пуста, то сохранить в переменную количество строк
				if (TSTable.Rows.Count != 0) TotalRows = TSTable.Rows.Count;
			}
			catch (Exception ex) {
				MessageBox.Show("Возникли проблемы подключения к базе данных\n");
			}
			}
		//--Обновление подключения
		private void RefreshDBConnection() {
			if (connection.State.Equals(ConnectionState.Open)) {
				connection.Close();
				TSTable.Clear();
				TSTablePers.Clear();
				SettingTable.Clear();
				EigenTable.Clear();
				ConnectToDB();
			}
			else ConnectToDB();
		}

		public Form1() {
			InitializeComponent();
			//Список доступных камер
			DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
			int _DeviceIndex = 0;
			foreach (DirectShowLib.DsDevice _Camera in _SystemCamereas) {
				Camera_comBox.Items.Add(_DeviceIndex.ToString() + " "+ _Camera.Name);
				CamChengeBox.Items.Add(_DeviceIndex.ToString() + " " + _Camera.Name);
				_DeviceIndex++;
			}
			//Настройки таймеров обновления трансляции, автоматического добавления
			videoTimer.Interval = 10;
			videoTimer.Tick += TranslationUpdate;   //для обновления видеопотока
			addManyPhotoTimer.Interval = 100;
			addManyPhotoTimer.Tick += add_many_photo;
		}
		//Переход между вкладками окна
		private void TabSelectedChanged(object sender, EventArgs e) {
			//Переход между вкладками окна (Обучение/Распознавание)
			if (tabControl1.SelectedIndex == 1) {
					//распознавание.Запуск трансляции с камеры
					videoTimer.Start();
			}
			else {
				//обучение. Остановка трансляции с камеры
				videoTimer.Stop();
			}
		}
		//Обновление трансляции с камеры(по таймеру)
		private void TranslationUpdate(object sender, EventArgs e) {
			//захват кадра с камеры
			Mat frame = capture.QueryFrame();
			//преобразование формата
			Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
			frame.Dispose();
			//Bitmap BmpInput = image.ToBitmap();
			//если вкладка - распознавание 
			if (tabControl1.SelectedIndex == 1) {
				Detect(image, pictureBox1);
				RefreshDBConnection();
				byte NeedTrain = (byte) SettingTable.Rows[0]["NeedTraining"];
				if ((faces.Count > 0) && (NeedTrain == 0)) {
					//обрабатывает faces и возвращает массив имен
					//System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
					//myStopwatch.Start();
					string[] NameMas = Recognize();
					//myStopwatch.Stop(); //запуск
					//TimeSpan ts = myStopwatch.Elapsed;
					// Format and display the TimeSpan value.
					//string elapsedTime = String.Format("{0:00}", ts.Milliseconds / 10);
					//MessageBox.Show("Время распознавания: " + elapsedTime + " (мc)");
					//myStopwatch.Stop();
					if (NameMas != null) {
						Image img = pictureBox1.Image;
						//Вывод информации о распознанных людях
						listBox1.Items.Clear();
						Font drawFont = new Font("Arial", 20);
						using (Graphics g = Graphics.FromImage(img)) {
							for (int i = 0; i < faces.Count; i++) {
								g.DrawString(NameMas[i], drawFont, Brushes.Green, faces_points[i]);
								listBox1.Items.Add((i + 1).ToString() + " " + "Имя: " + NameMas[i]);
								listBox1.Items.Add((i + 1).ToString() + " " + "Информация:" + MassOfInfo[i]);
							}
						}
						drawFont.Dispose();
						//pictureBox1.Image.Save("Image.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
					}
				} else {
					if (NeedTrain != 0) labelName.Text = "База была изменена. Необходимо провести обучение..";
				}
			}
			//если вкладка обучение
			else Detect(image, pictureBox2);
			image.Dispose();
		}
		//Пользователь загружает изображение с ПК
		private void LoadImageButton_Click(object sender, EventArgs e) {
			//отключение трансляции с камеры
			videoTimer.Stop();
			Load_from_camera_Button.Enabled = true;
			OpenFileDialog openFileDialog1 = new OpenFileDialog();
			//устанавливаем формат файлов для загрузки - jpg или png
			openFileDialog1.Filter = "Image Files (*.jpg; *.bmp; *.png) | *.jpg; *.bmp; *.png | All files (*.*) | *.*";
			//Если пользователь выбрал файл и нажал ОК
			if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				try
				{
					//пытаемся загрузить файл пользователя из проводника
					if (Image_in_work != null) {Image_in_work.Dispose(); }
					Image_in_work = new Emgu.CV.Image<Bgr, byte>(new Bitmap(Image.FromFile(openFileDialog1.FileName)));
					//обнаружение лиц и вывод результата в  pictureBox
					if (tabControl1.SelectedIndex == 1)
						Detect(Image_in_work, pictureBox1);
					else
						Detect(Image_in_work, pictureBox2);
				}
				catch (Exception ex) // если попытка загрузки не удалась 
				{
					// выводим сообщение об ошибке
					MessageBox.Show("Не удалось загрузить файл: " + ex.Message);
				}
			}
		}
		//Добавление новых лиц в базу прямо с трансляции
		private void Load_from_camera_Button_Click(object sender, EventArgs e) {
			videoTimer.Start();
			Load_from_camera_Button.Enabled = false;
		}
		//Обработка смены параметров обнаружения лица
		private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) {
			switch (comboBox2.SelectedIndex) {
				case 0:
					Min_neighbors = 3;
					break;
				case 1:
					Min_neighbors = 5;
					break;
				case 2:
					Min_neighbors = 7;
					break;
				case 3:
					Min_neighbors = 10;
					break;
			}
			//Bitmap bmpImage = new Bitmap(Image_in_work);

			Detect(Image_in_work, pictureBox1);
		}
		//Обнаружение лица
		private void Detect(Emgu.CV.Image<Bgr, Byte> image, PictureBox picterbox)  {
			//перевод в формат Emnu.Image для работы с методами из Emnu
			//Image<Bgr, byte> image = new Emgu.CV.Image<Bgr, Byte>(bmpImage);
			Bitmap bmpImage = image.ToBitmap();
			var grayframe = image.Convert<Gray, byte>();
			//третий параметр-необходимое количество совпадений с базой для признания объекта лицом
			//rect_faces-массив контуров (прямоугольников) найденных лиц
			Rectangle[] rect_faces = faceCascade.DetectMultiScale(grayframe, 1.1, Min_neighbors);
			grayframe.Dispose();
			foreach (Bitmap b in faces) b.Dispose();
			faces.Clear();
			faces_points = new Point[rect_faces.Length];
			int k = 0;
			foreach (var rect_face in rect_faces) {
				//копирование каждого лица в массив изображений лиц
				faces.Add(bmpImage.Clone(rect_face, bmpImage.PixelFormat));
				//наложить результат распознавания на исходное изображение
				image.Draw(rect_face, new Bgr(0, 255, 0),2);
				//Сохранение координат для наложения надписи
				faces_points[k].X = rect_face.X;
				faces_points[k].Y = rect_face.Y-28;
				k++;
			}
			//вывод изображения с выделенными лицами в pictureBox
			picterbox.Image = image.ToBitmap();
			//вывод первого лица в PictureBox с лицами
			face_number = 0;
			if (faces.Count > 0) {
				labelName.Text = "";
				pictureBoxFaceAdd.Image = faces[face_number];
				if (faces.Count > face_number + 1) ButtonNext.Enabled = true;
				else ButtonNext.Enabled = false;
				bmpImage.Dispose();
			}
			//если не нашлось ни одного лица на изображении
			else {
				ButtonNext.Enabled = false;
				//если это трансляция с камеры
				if (Load_from_camera_Button.Enabled == false) {
					//Mat frame = capture.QueryFrame();
					//Image<Bgr, byte> imag = frame.ToImage<Bgr, byte>();
					//picterbox.Image = imag.ToBitmap();
					picterbox.Image = bmpImage;
				}
				else {//если это загруженное изображение, то вывод его и сообщения
					bmpImage.Dispose();
					if (Image_in_work != null) picterbox.Image = Image_in_work.ToBitmap();
					if (pictureBoxFaceAdd.Image != null) pictureBoxFaceAdd.Image.Dispose();
					pictureBoxFaceAdd.Image = null;
					labelName.Text = "На загруженном изображении отсутствуют лица..Попробуйте снизить параметр минимального количества совпадений";
				}
			}
			ButtonPrev.Enabled = false;
		}
		//Пользователь решил добавить обнаруженное лицо в базу
		private void AddNewFaceButton_Click(object sender, EventArgs e) {
			//Добавить несколько фото
			if (checkBox1.Checked == true) {
				addFotoName = textBoxFaceName.Text;
				photo_count = (int) PhotoCount.Value;
				PhotoCount.Visible = false;
				Stop_button.Visible = true;
				PhotoCount.Refresh();
				addManyPhotoTimer.Start();
			}
			//если добавление одного кадра
			else {
				//если не обнаружены лица		
				if (faces.Count == 0) MessageBox.Show("На загруженном изображении отсутствуют лица..Попробуйте снизить параметр минимального количества совпадений", "Лица отсутствуют", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				//если обнаружены
				else {
					addFotoName = textBoxFaceName.Text;
					//если не введено имя
					if (addFotoName.Length == 0)
						MessageBox.Show("Для добавления человека в базу необходимо ввести имя..", "Не введено имя..", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					//если введено
					else {
						RefreshDBConnection();
						Image InputFace = pictureBoxFaceAdd.Image;
						//изменение размера до нужного (imgSize-задается в глобальных переменных)
						Bitmap BmpImage = ScaleImage(InputFace, imgSize, imgSize);
						byte[] FaceAsBytes = BitmapToBytes(BmpImage);
						//новый Image_Id для вставки
						int img_id = 0;
						if (TotalRows > 0) { img_id = (int)TSTable.Rows[TotalRows - 1]["Img_Id"] + 1; }
						try {
							//Проверка - есть ли уже такое имя в базе
							SqlCommand select = new SqlCommand("SELECT Person_Id FROM Person WHERE Person_Name = N'" + addFotoName + "'", connection);
							SqlDataReader reader = select.ExecuteReader();
							int id = -1;//т.к. id в базе точно не может быть отрицательным
							while (reader.Read()) {
								//id человека с таким именем, (либо остается -1)
								id = reader.GetInt32(0);
							}
							//если пользователь ввел существующее в базе имя
							if (id > 0) {
								DialogResult result = MessageBox.Show("Человек с таким именем уже существует в базе. Добавить эту фотографию к остальным фотографиям этого человека?", "Такое имя уже есть в базе..", MessageBoxButtons.YesNo);
								if (result == DialogResult.Yes) {
									//Добавление нового изображения к существующему человеку в базе
									reader.Close();
									SqlCommand insert2 = new SqlCommand("INSERT INTO Image (Img_Id,Image,Person_Id) VALUES(" + img_id + ",@FaceImage," + id + ")", connection);
									SqlParameter imageParameter5 = insert2.Parameters.AddWithValue("@FaceImage", SqlDbType.Image);
									imageParameter5.Value = BitmapToBytes(BmpImage);
									imageParameter5.Size = BitmapToBytes(BmpImage).Length;
									int rowsAffected2 = insert2.ExecuteNonQuery();
									Success_add(sender, e);
								}
							}
							//если человека с таким именем в базе нет, то надо создать записи в таб. изображений и в таб. людей
							else {
								//Добавление в таблицу людей
								reader.Close();
								SqlCommand insert0 = new SqlCommand("INSERT INTO Person (Person_Name,Person_Info) VALUES(N'" + addFotoName + "',N'" + info1 + "')", connection);
								int rowsAffected0 = insert0.ExecuteNonQuery();
								if (info1 != null) { if (info1.Length > 0) info1 = info1.Remove(0, info1.Length); }
								//Добавление в таблицу изображений
								SqlCommand insert1 = new SqlCommand("INSERT INTO Image (Img_Id,Image,Person_Id) VALUES(" + img_id + ",@FaceImage,(SELECT Person_Id FROM Person WHERE Person_Name = N'" + addFotoName + "'))", connection);
								SqlParameter imageParameter1 = insert1.Parameters.AddWithValue("@FaceImage", SqlDbType.Image);
								imageParameter1.Value = FaceAsBytes;
								imageParameter1.Size = FaceAsBytes.Length;
								int rowsAffected = insert1.ExecuteNonQuery();
								Success_add(sender, e);
							}
						}
						catch (Exception ex) {
							MessageBox.Show(ex.Message.ToString());
							MessageBox.Show(ex.StackTrace.ToString());
						}
						finally {
							RefreshDBConnection();
						}
					}
				}
			}
		}
		//Сжатие изображения 
		public static Bitmap ScaleImage(Image image, int maxWidth, int maxHeight) {
			var ratioX = (double)maxWidth / image.Width;
			var ratioY = (double)maxHeight / image.Height;
			var ratio = Math.Min(ratioX, ratioY);

			var newWidth = (int)(image.Width * ratio);
			var newHeight = (int)(image.Height * ratio);

			var newImage = new Bitmap(newWidth, newHeight);

			using (var graphics = Graphics.FromImage(newImage))
				graphics.DrawImage(image, 0, 0, newWidth, newHeight);
			return newImage;
		}
		//Перевод из Bitmap в Bytes
		private static byte[] BitmapToBytes(Bitmap BmpImage) {
			MemoryStream stream = new MemoryStream();
			BmpImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
			return stream.ToArray();
		}
		//Перевод массива double в массив bytes
		private static byte[] DoubleArrayToBytes(double[] d) {
			byte[] bytes = new byte[d.Length * sizeof(double)];
			Buffer.BlockCopy(d, 0, bytes, 0, d.Length * sizeof(double));
			return bytes;
		}
		//Перевод массива bytes в массив double
		private static double[] BytesToDoubeleArray(byte[] b) {
			double[] d = new double[b.Length / sizeof(double)];
			Buffer.BlockCopy(b, 0, d, 0, d.Length * sizeof(double));
			return d;
		}
		//Пролистывание лиц среди обнаруженных на фото
		private void ButtonPrev_Click(object sender, EventArgs e) {
			//если было выведено не первое найденное лицо то вывести предыдущее
			if (face_number > 0 && faces.Count > 0) {
				face_number--;
				pictureBoxFaceAdd.Image = faces[face_number];
				if (face_number == 0) ButtonPrev.Enabled = false;
				ButtonNext.Enabled = true;
			}
		}
		//Пролистывание лиц среди обнаруженных на фото
		private void ButtonNext_Click(object sender, EventArgs e) {
			//если было выведено не последнее найденное лицо то вывести следующее
			if (faces.Count > face_number + 1) {
				face_number++;
				pictureBoxFaceAdd.Image = faces[face_number];
				if (face_number == faces.Count - 1) ButtonNext.Enabled = false;
				ButtonPrev.Enabled = true;
			}
		}
		//Получить лицо из базы (по указателю rowNumber)
		private Image GetFaceFromBD() {
			Image FetchedImg;
			if ((rowNumber >= 0)&&(TotalRows>0)) {
				byte[] FetchedImgBytes = (byte[])TSTable.Rows[rowNumber]["Image"];
				MemoryStream stream = new MemoryStream(FetchedImgBytes);
				FetchedImg = Image.FromStream(stream);
				return (FetchedImg);
			}
			else {
				MessageBox.Show("В базе нет изображений");
				return null;
			}
		}
		//Добавить информацию перед добавлением фото в БД
		private void AddInfoButton_Click(object sender, EventArgs e) {
			if (InputBox("Добавление информации о человеке", "Введите информацию:", ref info1) == DialogResult.OK) {
				MessageBox.Show("Ok. Для загрузки информации в базу, нажмите на кнопку 'Добавить лицо в базу'", "Информация сохранена в буфер.." , MessageBoxButtons.OK,MessageBoxIcon.Information);
			}
		}
		//Изменить информацию о человеке, которому соответствует это фото
		private void ChangeInfoButton_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if (TotalRows > 0) {
				DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
				foreach (DataRow row in foundRows) { info2 = row["Person_Info"].ToString(); }
				if (InputBox("Добавление информации о человеке", "Введите информацию:", ref info2) == DialogResult.OK) {
					MessageBox.Show("Ok. Для загрузки информации в базу, нажмите на кнопку 'Добавить лицо в базу'", "Информация сохранена в буфер..", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
		}
		//Удалить фото из базы
		private void DeleteFace_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if ((TotalRows> 0)&&(rowNumber<= TotalRows)) {
				TSTable.Rows[rowNumber].Delete();
				dataAdapter.Update(TSTable);
				if (rowNumber > 0) rowNumber--;
				TotalRows--;
				pictureBoxImgFromDB.Image = GetFaceFromBD();
				if (TotalRows > 0) {
					DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
					foreach (DataRow row in foundRows) { textBoxNewName.Text = row["Person_Name"].ToString(); }
				}
				MessageBox.Show("Лицо удалено из базы");
				//Базу надо обучить
				SettingTable.Rows[0]["NeedTraining"] = 1;
				SettingAdapter.Update(SettingTable);
			}
		}
		//Пролистать до первого изображения из базы
		private void buttonFirstImg_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			rowNumber = 0;
			pictureBoxImgFromDB.Image = GetFaceFromBD();
			if (TotalRows > 0) {
				DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
				foreach (DataRow row in foundRows) { textBoxNewName.Text = row["Person_Name"].ToString(); }
			}
		}
		//Показать предыдущее изображение из базы
		private void buttonPrevImg_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if ((rowNumber > 0)&&(TotalRows>0)) {
			rowNumber--;
			pictureBoxImgFromDB.Image = GetFaceFromBD();
			DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
			foreach (DataRow row in foundRows) { textBoxNewName.Text = row["Person_Name"].ToString(); }
			}
			
		}
		//Показать следующее изображение из базы
		private void buttonNextImg_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if (rowNumber < TotalRows-1) {
				rowNumber++;
				pictureBoxImgFromDB.Image = GetFaceFromBD();
				DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
				foreach (DataRow row in foundRows) { textBoxNewName.Text = row["Person_Name"].ToString(); }
			}
		}
		//Пролистать до последнего изображения из базы
		private void buttonLastImg_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if (TotalRows > 0) {
				rowNumber = TotalRows - 1;
				pictureBoxImgFromDB.Image = GetFaceFromBD();
				DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[rowNumber]["Person_Id"].ToString());
				foreach (DataRow row in foundRows) { textBoxNewName.Text = row["Person_Name"].ToString(); }
			}
		}
		//Обновить информацию (и имя) о человеке из базы
		private void UpdateFaceInfo_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			if (TotalRows > 0) {
				//получение Person_Id из таб.Image
				int pers_id = (int)TSTable.Rows[rowNumber]["Person_Id"];
				//индекс строки для редактирования - результат поиска в таб Person значения pers_id
				DataRow[] customerRow = TSTablePers.Select("Person_Id =" + pers_id);
				//вставить в найденную строку поля имя и информация
				customerRow[0]["Person_Name"] = textBoxNewName.Text;
				if (info2 != null) {
					if (info2.Length > 0) {
						customerRow[0]["Person_Info"] = info2;
						try {
							dataAdapter1.Update(TSTablePers);
							info2 = info2.Remove(0, info2.Length);
							MessageBox.Show("Информация обновлена!");
						}
						catch {
							MessageBox.Show("Информация обновлена!");
						}
					}
				}
			}
		}
		//Загрузка изображения на распознавание (во вкладке распознавания)
		private void LoadImageButton2_Click(object sender, EventArgs e) {
			videoTimer.Stop();
			LoadImageButton_Click( sender, e);
			byte NeedTrain = (byte)SettingTable.Rows[0]["NeedTraining"];
			if ((faces.Count > 0) && (NeedTrain == 0)) {
				string[] NameMas = Recognize();
				if (NameMas != null) {
					Image img = pictureBox1.Image;
					//Вывод информации о распознанных людях
					listBox1.Items.Clear();
					using (Graphics g = Graphics.FromImage(img)) {
						Font drawFont = new Font("Arial", 20);
						SolidBrush drawBrush = new SolidBrush(Color.Green);
						for (int i = 0; i < faces.Count; i++) {
							g.DrawString(NameMas[i], drawFont, drawBrush, faces_points[i]);
							listBox1.Items.Add("Имя: " + NameMas[i]);
							listBox1.Items.Add("Информация:" + MassOfInfo[i]);
						}
					}
					pictureBox1.Image.Save("Image.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
				}
			}
		}
		//--Обучение
		private void buttonTrain_Click(object sender, EventArgs e) {
			RefreshDBConnection();
			//Обучить базу на новых изображениях
			if (TotalRows > 0) {//если они есть
				System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
				myStopwatch.Start(); //запуск
				PCA();
				myStopwatch.Stop(); //запуск
				TimeSpan ts = myStopwatch.Elapsed;
				// Format and display the TimeSpan value.
				string elapsedTime = String.Format("{0:00}.{1:00}.{2:00}", ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
				MessageBox.Show("Обучение завершено! Время обучения: " + elapsedTime + " (c)");
			}
			else { MessageBox.Show("Добавьте изображения в базу"); }
		}
		//--Рапознавание
		private string[] Recognize() {
			//массив найденных на изображении лиц
			int count = faces.Count;
			byte[][] dataset_vectors = new byte[count][];
			int i=0;
				foreach (var face in faces) {
				dataset_vectors[i] = new byte[imgVecSize];
				Bitmap Bmp1 = new Bitmap(face);
				Bitmap BmpImage = ScaleImage(Bmp1, imgSize, imgSize);
				Image<Bgr, byte> image = new Emgu.CV.Image<Bgr, Byte>(BmpImage);
				Image<Gray, byte> gray = image.Convert<Gray, byte>();
				//Проход по пикселям двумерного изображения
				for (int j = 0; j < gray.Size.Width; j++) {
					for (int k = 0; k < gray.Size.Height; k++) {
						Byte color = gray.Data[k, j, 0];
						//Запись очередного байта(оттенка серого) в вектор
						dataset_vectors[i][j * imgSize + k] = color;
					}
				} i++;
			}
			//Среднее изображение - считать с БД	
			RefreshDBConnection();
			Image FetchedImg;
			if (SettingTable.Rows.Count > 0) {
				byte[] FetchedImgBytes = (byte[])SettingTable.Rows[0]["MediumImage"];
				MemoryStream stream = new MemoryStream(FetchedImgBytes);
				FetchedImg = Image.FromStream(stream);
			}
			else {
				//MessageBox.Show("База была изменена. Для осуществления распознавания, обучите базу на новых данных..", "База была изменена..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return null;
			}
			//Среднее изображение - перевод в одномерный вектор
			Bitmap Bmp2 = new Bitmap(FetchedImg);
			Bitmap BmpImage2 = ScaleImage(Bmp2, imgSize, imgSize);
			Image<Bgr, byte> imag = new Emgu.CV.Image<Bgr, Byte>(BmpImage2);
			Image<Gray, byte> gray1 = imag.Convert<Gray, byte>();
			double[] medium_img = new double[imgVecSize];
			for (int j = 0; j < gray1.Size.Width; j++) {
				for (int k = 0; k < gray1.Size.Height; k++) {
					Byte color = gray1.Data[k, j, 0];
					medium_img[j * imgSize + k] = color;
				}
			}
			//Создание массива нормализованных векторов изображений
			double[][] norm_vectors = new double[count][];
			//нормализация (вычитание среднего изображения из каждого)
			for (i = 0; i < count; i++) {
				norm_vectors[i] = new double[imgVecSize];
				for (int j = 0; j < imgVecSize; j++) {
					norm_vectors[i][j] = dataset_vectors[i][j] - medium_img[j];
				}
			}
			//Считывание из базы K
			int K = (int)SettingTable.Rows[0]["K"];
			if (K > TotalRows) {
				//MessageBox.Show("База была изменена. Для осуществления распознавания, обучите базу на новых данных..", "База была изменена..", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return null;
			}
			else {
				//Считывание из бд EigenFaces
				RefreshDBConnection();
				double[][] EigenFaces = new double[imgVecSize][];
				for (int j = 0; j < imgVecSize; j++) {
					EigenFaces[j] = BytesToDoubeleArray((byte[])EigenTable.Rows[j]["EigenFaces"]);			
				}
				//посчитать веса новых лиц (P=X*EigenFaces)
				double[][] NewP = new double[count][];
					for (i = 0; i < count; i++) {
					NewP[i] = new double[K];
						for (int j = 0; j < K; j++) {
							for (int k = 0; k < imgVecSize; k++) {
								NewP[i][j] += norm_vectors[i][k] * EigenFaces[k][j];
							}
						}
					}
				//считать с БД записанные веса из таб.Image
				double[][] P = new double[TotalRows] [];
				byte[][] MasByte = new byte[TotalRows][];
				RefreshDBConnection();
				for (int j = 0; j < TotalRows; j++) {
					MasByte[j] = (byte[])TSTable.Rows[j]["P"];
					P[j] = BytesToDoubeleArray(MasByte[j]);
				}
				//вычисление расстояний между старыми и новыми весами
				double[] min_dist = new double[count];
				//номер изображения из БД, которое подходит больше всего
				int[] img_num = new int[count];
				//чтобы не распознавался по умолчанию человек на 0м изобр.
				img_num[0] = int.MaxValue;
				double Summ = 0;
				double dist;
				//dist=sqrt(sqr(A1-B1) + ... + sqr(An-Bn))
				//проход по всем лицам, которые найдены камерой сейчас
				for (i = 0; i < count; i++) {
					min_dist[i] = double.MaxValue;
					//проход по всем лицам из бд
					for (int j = 0; j < TotalRows; j++) {
						for (int k = 0; k < K; k++) {
							Summ += Math.Pow((NewP[i][k] - P[j][k]), 2);
						}
						//расстояние между каждым
						dist = Math.Sqrt(Summ);
						Summ = 0;
						//если расстояние меньше остальных и меньше порога
						if ((dist < min_dist[i])&&(dist<threshold)) {
							min_dist[i] = dist;
							//запоминаем номер лица из базы, при котором расстояние минимальное
							img_num[i] = j;
						}
					}
				}
				//массив для хранения имен распознанных людей
				string[] MassOfName = new string[count];
				MassOfInfo = new string[count];
				for (i = 0; i < count; i++) {
					if (img_num[i] != int.MaxValue) {
						DataRow[] foundRows = TSTablePers.Select("Person_Id=" + TSTable.Rows[img_num[i]]["Person_Id"].ToString());
						foreach (DataRow row in foundRows) {
							MassOfName[i] = row["Person_Name"].ToString();
							MassOfInfo[i] = row["Person_Info"].ToString();
						}
					}
				}
				return MassOfName;
			}
		}
		//-------------МЕТОД ГЛАВНЫХ КОМПОНЕНТ-------------//
		private void PCA() {
			//Изображения вытягиваются в вектор.Размер одного вектора=квадрату размера изображения
			imgVecSize = imgSize * imgSize;
			RefreshDBConnection();   
			//Массив изображений для хранения полученных из БД изображений
			Image[] dataset = new Image[TotalRows];				
			//Массив изображений преобразуется в массив векторов:
			byte[][] dataset_vectors = new byte[TotalRows][];
			for (int i = 0; i < TotalRows; i++) {
				dataset_vectors[i] = new byte[imgVecSize];
				rowNumber = i;   //GetFaceFromBD() обращается к строке БД под номером rowNumber
				//Добавление очередного изображения из БД в массив изображений для обучения
				dataset[i] = GetFaceFromBD();        
				//--Сделать изображение серым: 
				Bitmap Bmp1 = new Bitmap(dataset[i]);
				Image<Bgr, byte> image = new Emgu.CV.Image<Bgr, Byte>(Bmp1);
				Image<Gray, byte> gray = image.Convert<Gray, byte>();
				//Проход по пикселям двумерного изображения
				for (int j = 0; j < gray.Size.Width; j++) {
					for (int k = 0; k < gray.Size.Height; k++) {
						Byte color = gray.Data[k, j, 0];
						//Запись очередного байта(оттенка серого) в вектор
						dataset_vectors[i][j * imgSize + k] = color;
					}
				}
			}
			//Создание массива нормализованных векторов изображений
			double[][] norm_vectors = new double[TotalRows][];
			//Среднее изображение
			double[] medium_img = new double[imgVecSize];
			//Нормализация (X=X-Xcp)
			Image<Gray, Double> MediumImage = Normalization(dataset_vectors,ref norm_vectors,ref medium_img, TotalRows);
			//------------------ВЫЧИСЛЕНИЕ КОВАРИАЦИОННОЙ МАТРИЦЫ -------------------//
			//транспанированная матрица изображений (X')
			double[][] transp_vectors = new double[imgVecSize][];
			//вычисление транспанированной матрицы изображений
			for (int i = 0; i < imgVecSize; i++) {
				transp_vectors[i] = new double[TotalRows];
				for (int j = 0; j < TotalRows; j++) {
					transp_vectors[i][j] = norm_vectors[j][i];
				}
			}
			//----------------КОВАРИАЦИОННАЯ МАТРИЦА-----------------// (Q=X*X')
			double[,] covariance_matrix = new double[TotalRows, TotalRows];
			System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
			Parallel.For(0, TotalRows, (i) => {
				Parallel.For(0, TotalRows, (j) => {
					double sum = 0.0;
					for (int inner = 0; inner < imgVecSize; inner++) {
						sum += norm_vectors[i][inner] * norm_vectors[j][inner];
					}
					covariance_matrix[i, j] = sum;
				});
			});
			//Cобственные значения ковариационной матрицы (λ)
			double[] EigenValues = new double[TotalRows];
			//Собственные векторы ковариационной матрицы (U)
			double[,] EigenVectors = new double[TotalRows, TotalRows];
			//Их вычисление
			bool calceigen = alglib.smatrixevd(covariance_matrix, TotalRows, 1,true,out EigenValues, out EigenVectors);
			//Вычисление EigenFaces (перевод собственных векторов в размер изображения) (=Х*U)
			int K = 0;//новое количество векторов (K<TotalRows)
			double[][] EigenFaces = CalcEigenFaces(norm_vectors, EigenVectors, EigenValues, ref K);
			//------------------ВЫЧИСЛЕНИЕ ВЕСОВ -------------------//
			//веса (P=X*EigenFaces)
			double[][] P = new double[TotalRows][];
			for (int i = 0; i < TotalRows; i++) {
				P[i] = new double [K];
				for (int j = 0; j < K; j++) {
					for (int k = 0; k < imgVecSize; k++) {
						P[i][j] += norm_vectors[i][k] * EigenFaces[k][j];
					}
				}
			}
			//-----------------СОХРАНЕНИЕ В БАЗУ--------------------//
			RefreshDBConnection();
			//Сохранение сред.изоб,числа гл.компонент в таб. Setting
			Bitmap BmpImage = ScaleImage(MediumImage.ToBitmap(), imgSize, imgSize);
			byte[] FaceAsBytes = BitmapToBytes(BmpImage);
			//Очистка таблицы
			string sqlTrunc = "TRUNCATE TABLE Setting";
			SqlCommand cmd = new SqlCommand(sqlTrunc, connection);
			cmd.ExecuteNonQuery();
			SettingAdapter.Update(SettingTable);
			try {
				SqlCommand insertImg = new SqlCommand("INSERT INTO Setting (id,K,MediumImage,NeedTraining) VALUES(0,"+K+",@FaceImage,0)", connection);
				SqlParameter imageParameter5 = insertImg.Parameters.AddWithValue("@FaceImage", SqlDbType.Image);
				imageParameter5.Value = BitmapToBytes(BmpImage);
				imageParameter5.Size = BitmapToBytes(BmpImage).Length;
				int rowsAffected = insertImg.ExecuteNonQuery();
			}
			catch (Exception ex) {
				MessageBox.Show("Не удалось обучить БД\n" + ex.Message);
			}
			//Сохранение значений весов в таб. Image
			RefreshDBConnection();
			for (int i = 0; i < TotalRows; i++) {
				TSTable.Rows[i]["P"] = DoubleArrayToBytes(P[i]);
			}
			dataAdapter.Update(TSTable);
			//Сохранение EigenFaces в таб. Setting
			//Очистка таблицы
			string sqlTruncEig = "TRUNCATE TABLE EigenFaces";
			SqlCommand trEig = new SqlCommand(sqlTruncEig, connection);
			trEig.ExecuteNonQuery();
			EigenAdapter.Update(SettingTable);
			//заполнение таб.EigenFaces
			try {
				for (int i = 0; i < imgVecSize; i++) {
					SqlCommand com = new SqlCommand("INSERT INTO EigenFaces (EigenFaces) VALUES(@EigImage)", connection);
					SqlParameter imageParameter = com.Parameters.AddWithValue("@EigImage", SqlDbType.Image);
					imageParameter.Value = DoubleArrayToBytes(EigenFaces[i]);
					//imageParameter.Size = K;
					int rowsAffected = com.ExecuteNonQuery();
				}
			}
			catch (Exception ex) {MessageBox.Show("Не удалось обучить БД\n" + ex.Message);}
			//-Конец сохр.в БД
			//-Конец обучения PCA
		}

		//--Центрирование изображений лиц
		private Image<Gray, Double> Normalization(byte[][] dataset_vectors,ref double[][] norm_vectors, ref double[] medium_img, int N) {
			//сумма изображений
			byte[] sum_img = new byte[imgVecSize];
			for (int i = 0; i < N; i++) {
				for (int j = 0; j < imgVecSize; j++) {
					sum_img[j] += dataset_vectors[i][j];
				}
			}
			for (int j = 0; j < imgVecSize; j++) {
				medium_img[j] = sum_img[j] / N;
			}
			//среднее изображение из вектора в Image
			Image<Gray, Double> MediumImage = new Image<Gray, Double>(imgSize, imgSize, new Gray(30));
			for (int j = 0; j < imgVecSize; j++) {
				MediumImage.Data[j % imgSize, j / imgSize, 0] = medium_img[j];
			}
			//нормализация (вычитание среднего изображения из каждого)
			for (int i = 0; i < N; i++) {
				norm_vectors[i] = new double[imgVecSize];
				for (int j = 0; j < imgVecSize; j++) {
					norm_vectors[i][j] = dataset_vectors[i][j] - medium_img[j];
				}
			}
			return MediumImage;
		}
		//--Вычисление EigenFaces
		private double[][] CalcEigenFaces(double[][] norm_vectors, double[,] EigenVectors, double[] EigenValues, ref int K) {
			//Если EigenValues<1, то EigenVectors отбрасывается.
			//Таким образом, число ненулевых собственных векторов может быть меньше, чем количество изображений.
			double[,] tmp = new double[TotalRows, TotalRows];
			K = 0; //количество векторов в NewEigenVectors
			for (int j = 0; j < TotalRows; j++) {
				if (EigenValues[j] > 1) {
					for (int i = 0; i < TotalRows; i++) {
						tmp[i,K] = EigenVectors[i,j];
					}
					K++;
				}
			}
			//Копирование первых K элементов (отсечение пустых векторов)
			double[,] NewEigenVectors = new double[TotalRows, K];
			for (int j = 0; j < K; j++) {
				for (int i = 0; i < TotalRows; i++) {
					NewEigenVectors[i, j] = tmp[i,j];
				}
			}
			double[][] EigenFaces = new double[imgVecSize][];
			//вычисление транспанированной матрицы 
			double[,] Transp_normVectors = new double[imgVecSize, TotalRows];
				for (int i = 0; i < imgVecSize; i++) {
					for (int j = 0; j < TotalRows; j++) {
						Transp_normVectors[i,j] = norm_vectors[j][i];
					}
				}
			//EigenFaces=Х'*U
			for (int i = 0; i < imgVecSize; i++) {
				EigenFaces[i] = new double[K];
				for (int j = 0; j < K; j++) {
					for (int k = 0; k < TotalRows; k++) {
						EigenFaces[i][j] += Transp_normVectors[i,k]* NewEigenVectors[k,j];
					}
				}
			}
			return EigenFaces;
		}
		//--Автоматическое добавление нескольких изображений
		private void add_many_photo(object sender, EventArgs e) {
			label7.Visible = true;
			PhotoCount.Visible = false;
			Stop_button.Visible = true;
			String Name = textBoxFaceName.Text;
			//захват кадра с камеры
			Mat frame = capture.QueryFrame();
			//преобразование формата
			Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
			//Bitmap BmpInput = image.ToBitmap();
			//Найти лицо
			Detect(image, pictureBox1);
			image.Dispose();
			//если обнаружены лица
			if (faces.Count > 0) {
				RefreshDBConnection();
				Image InputFace = pictureBoxFaceAdd.Image;
				//изменение размера до нужного (imgSize-задается в глобальных переменных)
				Bitmap BmpImage = ScaleImage(InputFace, imgSize, imgSize);
				byte[] FaceAsBytes = BitmapToBytes(BmpImage);
				//новый Image_Id для вставки
				int img_id = 0;
				if (TotalRows > 0) { img_id = (int)TSTable.Rows[TotalRows - 1]["Img_Id"] + 1; }
				try {
					//Проверка - есть ли уже такое имя в базе
					SqlCommand select = new SqlCommand("SELECT Person_Id FROM Person WHERE Person_Name = N'" + addFotoName + "'", connection);
					SqlDataReader reader = select.ExecuteReader();
					int id = -1;//т.к. id в базе точно не может быть отрицательным
					while (reader.Read()) {
						//id человека с таким именем, (либо остается -1)
						id = reader.GetInt32(0);
					}
					//если пользователь ввел существующее в базе имя
					if (id > 0) {
						//Добавление нового изображения к существующему человеку в базе
						reader.Close();
						SqlCommand insert2 = new SqlCommand("INSERT INTO Image (Img_Id,Image,Person_Id) VALUES(" + img_id + ",@FaceImage," + id + ")", connection);
						SqlParameter imageParameter5 = insert2.Parameters.AddWithValue("@FaceImage", SqlDbType.Image);
						imageParameter5.Value = BitmapToBytes(BmpImage);
						imageParameter5.Size = BitmapToBytes(BmpImage).Length;
						int rowsAffected2 = insert2.ExecuteNonQuery();
						textBoxFaceName.Text = "";
					}
					//если человека с таким именем в базе нет, то надо создать записи в таб. изображений и в таб. людей
					else {
						//Добавление в таблицу людей
						reader.Close();
						SqlCommand insert0 = new SqlCommand("INSERT INTO Person (Person_Name,Person_Info) VALUES(N'" + addFotoName + "',N'" + info1 + "')", connection);
						int rowsAffected0 = insert0.ExecuteNonQuery();
						if (info1 != null) { if (info1.Length > 0) info1 = info1.Remove(0, info1.Length); }
						//Добавление в таблицу изображений
						SqlCommand insert1 = new SqlCommand("INSERT INTO Image (Img_Id,Image,Person_Id) VALUES(" + img_id + ",@FaceImage,(SELECT Person_Id FROM Person WHERE Person_Name = N'" + Name + "'))", connection);
						SqlParameter imageParameter1 = insert1.Parameters.AddWithValue("@FaceImage", SqlDbType.Image);
						imageParameter1.Value = FaceAsBytes;
						imageParameter1.Size = FaceAsBytes.Length;
						int rowsAffected = insert1.ExecuteNonQuery();
						textBoxFaceName.Text = "";
					}
				}
				catch (Exception ex) {
					MessageBox.Show(ex.Message.ToString());
					MessageBox.Show(ex.StackTrace.ToString());
				}
				//увеличение счетчика добавленных лиц
				addFotoCount++;
				label6.Text = (addFotoCount + 1).ToString();
				if (addFotoCount + 1 >= photo_count) {
					addManyPhotoTimer.Stop();
					MessageBox.Show("Добавлено " + (addFotoCount + 1).ToString() + " лиц!");
					label7.Visible = false;
					label6.Text = "";
					checkBox1.Checked = false;
					PhotoCount.Visible = true;
					Stop_button.Visible = false;
					addFotoCount = 0;
					//Базу надо обучить
					SettingTable.Rows[0]["NeedTraining"] = 1;
					SettingAdapter.Update(SettingTable);
				}
			}	
		}
		//--Остановить автоматическое добавление лиц
		private void Stop_button_Click(object sender, EventArgs e) {
			addManyPhotoTimer.Stop();
			if (addFotoCount>0)
			MessageBox.Show("Добавлено " + (addFotoCount+1).ToString() + " лиц!");
			label7.Visible = false;
			label6.Text = "";
			checkBox1.Checked = false;
			Stop_button.Visible = false;
			PhotoCount.Visible = true;
			addFotoCount = 0;
		}
		//--Выбор камеры из списка при обучении
		private void Camera_comBox_SelectedIndexChanged(object sender, EventArgs e) {
			capture.Dispose();
			capture = new Capture(Camera_comBox.SelectedIndex);
		}
		//--Выбор камеры из списка при распознавании
		private void CamChangeBox_SelectedIndexChanged(object sender, EventArgs e) {
			capture.Dispose();
			capture = new Capture(CamChengeBox.SelectedIndex);
		}

		//--Успешное добавление изобр в базу
		private void Success_add(object sender, EventArgs e) {
			MessageBox.Show("Добавлено!");
			buttonLastImg_Click(sender, e);
			pictureBoxFaceAdd.Image = null;
			pictureBox2.Image = null;
			textBoxFaceName.Text = "";
			//Базу надо обучить
			SettingAdapter.Fill(SettingTable);
			SettingTable.Rows[0]["NeedTraining"] = 1;
			SettingAdapter.Update(SettingTable);
		}
		//-----------МЕТОДЫ ДЛЯ ИЗМЕНЕНИЯ ВНЕШНЕГО ВИДА ОКНА------------//
		public static DialogResult InputBox(string title, string promptText, ref string value) {
			Form form = new Form();
			Label label = new Label();
			TextBox textBox = new TextBox();
			Button buttonOk = new Button();
			Button buttonCancel = new Button();

			form.Text = title;
			label.Text = promptText;
			textBox.Text = value;

			buttonOk.Text = "OK";
			buttonCancel.Text = "Cancel";
			buttonOk.DialogResult = DialogResult.OK;
			buttonCancel.DialogResult = DialogResult.Cancel;

			label.SetBounds(9, 20, 372, 13);
			textBox.SetBounds(12, 36, 372, 20);
			buttonOk.SetBounds(228, 72, 75, 23);
			buttonCancel.SetBounds(309, 72, 75, 23);

			label.AutoSize = true;
			textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
			buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

			form.ClientSize = new Size(396, 107);
			form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
			form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
			form.FormBorderStyle = FormBorderStyle.FixedDialog;
			form.StartPosition = FormStartPosition.CenterScreen;
			form.MinimizeBox = false;
			form.MaximizeBox = false;
			form.AcceptButton = buttonOk;
			form.CancelButton = buttonCancel;

			DialogResult dialogResult = form.ShowDialog();
			value = textBox.Text;
			return dialogResult;
		}
		private void groupBox1_Paint(object sender, PaintEventArgs e) {
			GroupBox box = sender as GroupBox;
			DrawGroupBox(box, e.Graphics, Color.Black, Color.LightSteelBlue);
		}
		private void DrawGroupBox(GroupBox box, Graphics g, Color textColor, Color borderColor) {
			if (box != null) {
				Brush textBrush = new SolidBrush(textColor);
				Brush borderBrush = new SolidBrush(borderColor);
				//Brush borderBrush = new HatchBrush (HatchStyle.Sphere, borderColor, Color.White);
				Pen borderPen = new Pen(borderBrush,0.1f);
				SizeF strSize = g.MeasureString(box.Text, box.Font);
				Rectangle rect = new Rectangle(box.ClientRectangle.X,
											   box.ClientRectangle.Y + (int)(strSize.Height / 2),
											   box.ClientRectangle.Width - 1,
											   box.ClientRectangle.Height - (int)(strSize.Height / 2) - 1);
				
				// Clear text and border
				g.Clear(this.BackColor);
				// Draw text
				g.DrawString(box.Text, box.Font, textBrush, box.Padding.Left, 0);
				// Drawing Border
				//Left
				g.DrawLine(borderPen, rect.Location, new Point(rect.X, rect.Y + rect.Height));
				//Right
				g.DrawLine(borderPen, new Point(rect.X + rect.Width, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height));
				//Bottom
				g.DrawLine(borderPen, new Point(rect.X, rect.Y + rect.Height), new Point(rect.X + rect.Width, rect.Y + rect.Height));
				//Top1
				g.DrawLine(borderPen, new Point(rect.X, rect.Y), new Point(rect.X + box.Padding.Left, rect.Y));
				//Top2
				g.DrawLine(borderPen, new Point(rect.X + box.Padding.Left + (int)(strSize.Width), rect.Y), new Point(rect.X + rect.Width, rect.Y));
			}
		}

		private void button14_Click(object sender, EventArgs e) {
			SaveFileDialog saveFileDialog1 = new SaveFileDialog();
			saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
			if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				try {
					System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
					switch (saveFileDialog1.FilterIndex) {
						case 1:
							pictureBox1.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Jpeg);
							break;

						case 2:
							pictureBox1.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Bmp);
							break;

						case 3:
							pictureBox1.Image.Save(fs,System.Drawing.Imaging.ImageFormat.Gif);
							break;
					}

					fs.Close();
				}

				catch (Exception ex) // если попытка загрузки не удалась 
				{
					// выводим сообщение об ошибке
					MessageBox.Show("Не удалось сохранить файл: " + ex.Message);
				}
			}
		}

	}
}

