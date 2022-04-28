using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MathWorks.MATLAB.NET.Arrays;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace LIBSVM_GUI_Template_test
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : IControlWriter
    {

        #region Variables

        private int optimiseValue = 3;
        public int ColumnIndex = 2;
        public string C1Export = "0";
        public string C2Export = "0";
        public string C3Export = "0";
        public string C4Export = "0";
        public string C5Export = "0";
        public string C6Export = "0";
        public string DateExport = "0";
        public int AttributeIndex = 2;
        public string A1Export = "0";
        public string A2Export = "0";
        public string A3Export = "0";
        public string A4Export = "0";
        public string A5Export = "0";
        public string A6Export = "0";
        public int tAttributeIndex = 2;
        public string tA1Export = "0";
        public string tA2Export = "0";
        public string tA3Export = "0";
        public string tA4Export = "0";
        public string tA5Export = "0";
        public string tA6Export = "0";
        public int tAttributeIndexCache;
        public string tA1Cache;
        public string tA2Cache;
        public string tA3Cache;
        public string tA4Cache;
        public string tA5Cache;
        public string tA6Cache;
        public string lengthTextExport;
        public string tLengthTextExport;
        public int initialized;
        public int ConvertRunning;
        public int SplitRunning;
        public int GridSearchRunning;
        public int TrainRunning;
        public int TestRunning;
        private int matlabPid;
        public string assemblyLocation = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');


        #endregion

        #region Setup

        public Features Featuresobj { get; set; }

        public FeaturesLinked FeaturesLinkedobj { get; set; }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { DeleteObject(handle); }
        }

        #endregion

        public MainWindow()
        {
            InitializeComponent();

            #region Initialise DataContext

            Featuresobj = new Features
            {
                Train1Value = "1",
                Train2Value = "1",
                Train3Value = "1",
                Train4Value = "1",
                Train5Value = "1",
                Train6Value = "1",
                Test1Value = "1",
                Test2Value = "1",
                Test3Value = "1",
                Test4Value = "1",
                Test5Value = "1",
                Test6Value = "1",
            };
            FeaturesLinkedobj = new FeaturesLinked();

            Attribute1Text.DataContext = Featuresobj;
            Attribute2Text.DataContext = Featuresobj;
            Attribute3Text.DataContext = Featuresobj;
            Attribute4Text.DataContext = Featuresobj;
            Attribute5Text.DataContext = Featuresobj;
            Attribute6Text.DataContext = Featuresobj;
            TAttribute1Text.DataContext = Featuresobj;
            TAttribute2Text.DataContext = Featuresobj;
            TAttribute3Text.DataContext = Featuresobj;
            TAttribute4Text.DataContext = Featuresobj;
            TAttribute5Text.DataContext = Featuresobj;
            TAttribute6Text.DataContext = Featuresobj;

            Attribute1.Visibility = Visibility.Visible;
            Attribute2.Visibility = Visibility.Visible;
            Attribute3.Visibility = Visibility.Collapsed;
            Attribute4.Visibility = Visibility.Collapsed;
            Attribute5.Visibility = Visibility.Collapsed;
            Attribute6.Visibility = Visibility.Collapsed;
            Attribute1Text.Visibility = Visibility.Visible;
            Attribute2Text.Visibility = Visibility.Visible;
            Attribute3Text.Visibility = Visibility.Collapsed;
            Attribute4Text.Visibility = Visibility.Collapsed;
            Attribute5Text.Visibility = Visibility.Collapsed;
            Attribute6Text.Visibility = Visibility.Collapsed;
            AttributeMinus.Visibility = Visibility.Visible;
            AttributePlus.Visibility = Visibility.Visible;
            TAttribute1.Visibility = Visibility.Visible;
            TAttribute2.Visibility = Visibility.Visible;
            TAttribute3.Visibility = Visibility.Collapsed;
            TAttribute4.Visibility = Visibility.Collapsed;
            TAttribute5.Visibility = Visibility.Collapsed;
            TAttribute6.Visibility = Visibility.Collapsed;
            TAttribute1Text.Visibility = Visibility.Visible;
            TAttribute2Text.Visibility = Visibility.Visible;
            TAttribute3Text.Visibility = Visibility.Collapsed;
            TAttribute4Text.Visibility = Visibility.Collapsed;
            TAttribute5Text.Visibility = Visibility.Collapsed;
            TAttribute6Text.Visibility = Visibility.Collapsed;
            TAttributeMinus.Visibility = Visibility.Visible;
            TAttributePlus.Visibility = Visibility.Visible;
            Ep.Visibility = Visibility.Visible;
            Ep_Text.Visibility = Visibility.Visible;
            Nu.Visibility = Visibility.Hidden;
            Nu_Text.Visibility = Visibility.Hidden;
            Gamma.Visibility = Visibility.Visible;
            Gamma_Text.Visibility = Visibility.Visible;
            Degree.Visibility = Visibility.Hidden;
            Degree_Text.Visibility = Visibility.Hidden;
            R.Visibility = Visibility.Hidden;
            R_Text.Visibility = Visibility.Hidden;

            Column2.Visibility = Visibility.Visible;
            Column3.Visibility = Visibility.Collapsed;
            Column4.Visibility = Visibility.Collapsed;
            Column5.Visibility = Visibility.Collapsed;
            Column6.Visibility = Visibility.Collapsed;
            Column1Text.Visibility = Visibility.Visible;
            Column2Text.Visibility = Visibility.Visible;
            Column3Text.Visibility = Visibility.Collapsed;
            Column4Text.Visibility = Visibility.Collapsed;
            Column5Text.Visibility = Visibility.Collapsed;
            Column6Text.Visibility = Visibility.Collapsed;
            ColumnMinus.IsEnabled = true;
            ColumnPlus.IsEnabled = true;

            Has_Labels.IsChecked = true;
            Scale_Selected.IsChecked = true;
            TopToBottom.IsChecked = true;
            initialized = 1;

            Grid_Search_Contour_Image.Source = ImageSourceFromBitmap(Properties.Resources.Contour_Template);
            Grid_Search_Prediction_Image.Source = ImageSourceFromBitmap(Properties.Resources.Plot_Template);
            Plot_Image.Source = ImageSourceFromBitmap(Properties.Resources.Plot_Template);

            Saves_Directory.Text = assemblyLocation + "\\Saves";

            Folder_File_Create();

            #endregion

            //Grant Access to Directory Folder
            var dInfo = new DirectoryInfo(assemblyLocation);
            var dSecurity = dInfo.GetAccessControl();
            dSecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                FileSystemRights.FullControl, InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, 
                PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
            dInfo.SetAccessControl(dSecurity);

            Console.WriteLine(assemblyLocation);
        }

        #region ComboBoxes

        public void SVM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized != 1) return;
            if (SVM_ComboBox.SelectedIndex == 0)
            {
                Ep.Visibility = Visibility.Visible;
                Ep_Text.Visibility = Visibility.Visible;
                Nu.Visibility = Visibility.Hidden;
                Nu_Text.Visibility = Visibility.Hidden;
            }
            else
            {
                Ep.Visibility = Visibility.Hidden;
                Ep_Text.Visibility = Visibility.Hidden;
                Nu.Visibility = Visibility.Visible;
                Nu_Text.Visibility = Visibility.Visible;
            }
        }

        private void Kernel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (initialized != 1) return;
            switch (Kernel_ComboBox.SelectedIndex)
            {
            case 0:
                Gamma.Visibility = Visibility.Hidden;
                Gamma_Text.Visibility = Visibility.Hidden;
                Degree.Visibility = Visibility.Hidden;
                Degree_Text.Visibility = Visibility.Hidden;
                R.Visibility = Visibility.Hidden;
                R_Text.Visibility = Visibility.Hidden;
                break;
            case 1:
                Gamma.Visibility = Visibility.Visible;
                Gamma_Text.Visibility = Visibility.Visible;
                Degree.Visibility = Visibility.Visible;
                Degree_Text.Visibility = Visibility.Visible;
                R.Visibility = Visibility.Visible;
                R_Text.Visibility = Visibility.Visible;
                break;
            case 2:
                Gamma.Visibility = Visibility.Visible;
                Gamma_Text.Visibility = Visibility.Visible;
                Degree.Visibility = Visibility.Hidden;
                Degree_Text.Visibility = Visibility.Hidden;
                R.Visibility = Visibility.Hidden;
                R_Text.Visibility = Visibility.Hidden;
                break;
            default:
                Gamma.Visibility = Visibility.Visible;
                Gamma_Text.Visibility = Visibility.Visible;
                Degree.Visibility = Visibility.Hidden;
                Degree_Text.Visibility = Visibility.Hidden;
                R.Visibility = Visibility.Visible;
                R_Text.Visibility = Visibility.Visible;
                break;
            }
        }

        #endregion

        #region Grid Search CheckBoxes 

        public void Parameter_Checked(object sender, RoutedEventArgs e)
        {
            optimiseValue = 1;
            Feature_Selected.IsChecked = false;
        }

        private void Feature_Checked(object sender, RoutedEventArgs e)
        {
            optimiseValue = 2;
            Parameter_Selected.IsChecked = false;
        }

        #endregion  

        public void Saves_Directory_Button_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new FolderPicker();
            if (dlg.ShowFolderDialog(IntPtr.Zero) == true)
            { Saves_Directory.Text = dlg.ResultPath; }
            Saves_Directory.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void Data_File_Source_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.Csv(Data_File_Source);
            Data_File_Source.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void ColumnMinus_Click(object sender, RoutedEventArgs e)
        {
            ColumnIndex -= 1;
            if (ColumnIndex == 1)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Collapsed;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Collapsed;
                Column3Text.Visibility = Visibility.Collapsed;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = false;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 2)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Collapsed;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Collapsed;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 3)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Collapsed;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Collapsed;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 4)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Collapsed;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Collapsed;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 5)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Collapsed;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Collapsed;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = true;
            }
            if (ColumnIndex == 6)
            {
                //Column1.Visibility = Visibility.Visible;
                Column2.Visibility = Visibility.Visible;
                Column3.Visibility = Visibility.Visible;
                Column4.Visibility = Visibility.Visible;
                Column5.Visibility = Visibility.Visible;
                Column6.Visibility = Visibility.Visible;
                Column1Text.Visibility = Visibility.Visible;
                Column2Text.Visibility = Visibility.Visible;
                Column3Text.Visibility = Visibility.Visible;
                Column4Text.Visibility = Visibility.Visible;
                Column5Text.Visibility = Visibility.Visible;
                Column6Text.Visibility = Visibility.Visible;
                ColumnMinus.IsEnabled = true;
                ColumnPlus.IsEnabled = false;
            }
            UpdateLayout();
        }

        public void ColumnPlus_Click(object sender, RoutedEventArgs e)
        {
            ColumnIndex += 1;
            IfColumnIndex();
            UpdateLayout();
        }

        public async void Convert_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Convert is already running
            if (ConvertRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Conversion in progress, please wait");
            }

            // Check if the input data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Data_File_Source.Text, ".csv")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and data to convert");
            }
            else if (File.Exists(Helper.AddExtension(Data_File_Source.Text, ".csv")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid data to convert");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }
            else
            {

                #endregion

                // Set Up Convert + Button
                #region Set Up Convert

                ConvertRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Convert_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Convert_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Convert_Button_text.Text = "Converting";

                #endregion

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Exports Directories
                #region Export Convert Directories

                switch (ColumnIndex)
                {
                    case 1:
                        C1Export = Column1Text.Text;
                        C2Export = "0";
                        C3Export = "0";
                        C4Export = "0";
                        C5Export = "0";
                        C6Export = "0";
                        break;
                    case 2:
                        C1Export = Column1Text.Text;
                        C2Export = Column2Text.Text;
                        C3Export = "0";
                        C4Export = "0";
                        C5Export = "0";
                        C6Export = "0";
                        break;
                    case 3:
                        C1Export = Column1Text.Text;
                        C2Export = Column2Text.Text;
                        C3Export = Column3Text.Text;
                        C4Export = "0";
                        C5Export = "0";
                        C6Export = "0";
                        break;
                    case 4:
                        C1Export = Column1Text.Text;
                        C2Export = Column2Text.Text;
                        C3Export = Column3Text.Text;
                        C4Export = Column4Text.Text;
                        C5Export = "0";
                        C6Export = "0";
                        break;
                    case 5:
                        C1Export = Column1Text.Text;
                        C2Export = Column2Text.Text;
                        C3Export = Column3Text.Text;
                        C4Export = Column4Text.Text;
                        C5Export = Column5Text.Text;
                        C6Export = "0";
                        break;
                    case 6:
                        C1Export = Column1Text.Text;
                        C2Export = Column2Text.Text;
                        C3Export = Column3Text.Text;
                        C4Export = Column4Text.Text;
                        C5Export = Column5Text.Text;
                        C6Export = Column6Text.Text;
                        break;
                }

                if (Date_Column.Text == "" || Date_Column.Text == " ")
                { DateExport = "0"; }
                else
                { DateExport = Date_Column.Text; }

                var directoriesPath = assemblyLocation + "\\Required_Files\\Settings\\Directories_Convert.txt";
                string[] directoriesLines =
                {
                Saves_Directory.Text,   //1
                File_Name.Text,         //2
                Data_File_Source.Text,  //3
                (Convert.ToInt32(TopToBottom.IsChecked)).ToString(), //1
                ColumnIndex.ToString(), //2
                DateExport,     //3
                C1Export,       //4
                C2Export,       //5
                C3Export,       //6
                C4Export,       //7
                C5Export,       //8
                C6Export,       //9
                (Convert.ToInt32(Has_Labels.IsChecked)).ToString(),   //10
                (Convert.ToInt32(Scale_Selected.IsChecked)).ToString(), //11

            };
                File.WriteAllLines(directoriesPath, directoriesLines);

                #endregion

                // Run the DataManagement Program
                await Helper.RunDatamanagement(assemblyLocation + "\\Required_Files\\");

                //Update Prepared File Location
                Prepared_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_Converted";
                Prepared_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                ConvertRunning = 0;
                //Convert_gif1.Visibility = Visibility.Hidden;
                Convert_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Convert_Button_text.Text = "Convert to LIBSVM-Ready File";
            }
        }

        public void Converted_To_Split_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Data_Split_Location.Text = Prepared_File_Location.Text;
                Data_Split_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Converted_To_Train_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Training_Data_Location.Text = Prepared_File_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Converted_To_Test_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Prepared_File_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Testing_Data_Location.Text = Prepared_File_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Data_To_Split_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.Matlab(Data_Split_Location);
            Data_Split_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Split_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Split is already running
            if (SplitRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Split in progress, please wait");
            }

            // Check if the split data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Data_Split_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and data to split");
            }
            else if (File.Exists(Helper.AddExtension(Data_Split_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid data to split");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }

            #endregion

            else
            {
                // Set Up Split + Button
                #region Set Up Split

                SplitRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Split_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Split_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Split_Button_text.Text = "Splitting";

                #endregion

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Exports Directories
                #region Export Split Settings

                var directoriesPath = assemblyLocation + "\\Required_Files\\Settings\\Settings_Split.txt";
                string[] directoriesLines =
                {
                    Saves_Directory.Text,       // 1
                    File_Name.Text,             // 2
                    Data_Split_Location.Text,   // 3
                    Percentage_Split.Text,      // 1
                };
                File.WriteAllLines(directoriesPath, directoriesLines);

                #endregion

                // Run Split Program
                await Helper.RunSplit(assemblyLocation + "\\Required_Files\\");

                //Update Portion File Locations
                First_Portion_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_First_Portion";
                Second_Portion_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Data_Second_Portion";
                First_Portion_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
                Second_Portion_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                SplitRunning = 0;
                Split_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Split_Button_text.Text = "Split";
            }
        }

        public void First_To_Training_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(First_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Training_Data_Location.Text = First_Portion_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void First_To_Testing_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(First_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Testing_Data_Location.Text = First_Portion_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Second_To_Training_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Second_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Training_Data_Location.Text = Second_Portion_Location.Text;
                Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void Second_To_Testing_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Second_Portion_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Testing_Data_Location.Text = Second_Portion_Location.Text;
                Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void AttributeMinus_Click(object sender, RoutedEventArgs e)
        {
            AttributeIndex -= 1;

            IfAttributeIndexAll();

            if (Link_CheckBox.IsChecked == true)
            {
                TAttributeMinus_Click(sender, e);
            }

            UpdateLayout();
        }

        public void AttributePlus_Click(object sender, RoutedEventArgs e)
        {
            AttributeIndex += 1;

            IfAttributeIndexAll();

            if (Link_CheckBox.IsChecked == true)
            {
                TAttributePlus_Click(sender, e);
            }

            UpdateLayout();
        }

        public void Parameter_Selection_Button_Click(object sender, RoutedEventArgs e)
        {
            Gamma_Text.Text = G_Result_Text.Text;
            Cost_Text.Text = C_Result_Text.Text;
        }

        public void Feature_Selection_Button_Click(object sender, RoutedEventArgs e)
        {
            Attribute1Text.Text = Att1_Result_Text.Text;
            Attribute2Text.Text = Att2_Result_Text.Text;
        }

        public async void Run_Search_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesit Checks

            // Sets nfoldint to 1 if n-fold value is an integer
            var nfoldint = 0;
            if (int.TryParse(n_fold_Text.Text, out _))
            {
                nfoldint = 1;
            }

            // Checks if Grid Search is already running
            if (GridSearchRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Grid Search in progress, please wait");
            }

            // Check if the training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and training data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid training data");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }

            // Check if None is selected, if so say "Select a Grid Search method"
            else if (Feature_Selected.IsChecked == false & Parameter_Selected.IsChecked == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Select a Grid Search method");
            }

            // Check if n-fold value is less than two or not an integer
            else if (nfoldint == 0)
            {
                System.Windows.Forms.MessageBox.Show(@"n-fold value must be an integer greater than 1");
            }
            else if (float.Parse(n_fold_Text.Text) < 2)
            {
                System.Windows.Forms.MessageBox.Show(@"n-fold value must be an integer greater than 1");
            }

            // Checks if the upper values aren't greater than the lower values by at least one step
            else if (Parameter_Selected.IsChecked == true & (float.Parse(GUpper_Text.Text) - float.Parse(GLower_Text.Text) < float.Parse(GStep_Text.Text)) 
                & (float.Parse(CUpper_Text.Text) - float.Parse(CLower_Text.Text) < float.Parse(CStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"C and G upper values must be greater than their lower values by at least one step respectively");
            }
            else if (Parameter_Selected.IsChecked == true & (float.Parse(CUpper_Text.Text) - float.Parse(CLower_Text.Text) < float.Parse(CStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"C upper value must be greater than the lower value by at least one step");
            }
            else if (Parameter_Selected.IsChecked == true & (float.Parse(GUpper_Text.Text) - float.Parse(GLower_Text.Text) < float.Parse(GStep_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"G upper value must be greater than the lower value by at least one step");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att1Upper_Text.Text) - float.Parse(Att1Lower_Text.Text) < float.Parse(Att1Step_Text.Text)) & 
                (float.Parse(Att2Upper_Text.Text) - float.Parse(Att2Lower_Text.Text) < float.Parse(Att2Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"Attribute upper values must be greater than their lower values by at least one step respectively");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att1Upper_Text.Text) - float.Parse(Att1Lower_Text.Text) < float.Parse(Att1Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"Attribute 1 upper value must be greater than the lower value by at least one step");
            }
            else if (Feature_Selected.IsChecked == true & (float.Parse(Att2Upper_Text.Text) - float.Parse(Att2Lower_Text.Text) < float.Parse(Att2Step_Text.Text)))
            {
                System.Windows.Forms.MessageBox.Show(@"Attribute 2 upper value must be greater than the lower value by at least one step");
            }

            #endregion

            else
            {
                // Set Up Grid Search + Button
                #region Set Up Grid Search

                GridSearchRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Run_Search_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Run_Search_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Run_Search_text.Text = "Searching";

                #endregion

                // Create Plots folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Export Settings
                #region Export Settings

                switch (AttributeIndex)
                {
                    case 1:
                        A1Export = Attribute1Text.Text;
                        A2Export = "0";
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 2:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 3:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 4:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 5:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = "0";
                        break;
                    case 6:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = Attribute6Text.Text;
                        break;
                }

                if (false | Length_Text.Text == "" | Length_Text.Text == " ")
                { lengthTextExport = "0"; }
                else { lengthTextExport = Length_Text.Text; }

                File.Delete(assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt");
                var settingsPath = assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] settingsLines =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + lengthTextExport + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + optimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "Cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",

                };
                File.WriteAllLines(settingsPath, settingsLines);

            #endregion

                // Export Grid Search Directories
                #region Export Search And Traing Directories

                var directoriesPath = assemblyLocation + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] directoriesLines =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Training_Data_Location.Text,
                };
                File.WriteAllLines(directoriesPath, directoriesLines);

                #endregion

                // Run the Program
                Console.WriteLine(@"ID OF MAINWINDOW: " + Process.GetCurrentProcess().Id.ToString());
                await Helper.RunGridSearch(assemblyLocation + "\\Required_Files\\", this);

                // Update Image for Grid Search and Best Prediction
                #region Display Contour and Prediction Plots

                Grid_Search_Contour_Image.Source = null;
                Grid_Search_Contour_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg"))
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg"); 
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour_cache.jpg"))
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg");
                    var plotContourLocation = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Contour.jpg";
                    var bitmapContour = new BitmapImage();
                    bitmapContour.BeginInit();
                    bitmapContour.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapContour.UriSource = new Uri(plotContourLocation);
                    bitmapContour.EndInit();
                    Grid_Search_Contour_Image.Source = bitmapContour;
                    bitmapContour.UriSource = null;
                    GC.Collect();
                }

                Grid_Search_Prediction_Image.Source = null;
                Grid_Search_Prediction_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg"))
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg");
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction_cache.jpg"))
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg");
                    var plotPredictionLocation = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Grid_Search_Prediction.jpg";
                    var bitmapPrediction = new BitmapImage();
                    bitmapPrediction.BeginInit();
                    bitmapPrediction.CacheOption = BitmapCacheOption.OnLoad;
                    bitmapPrediction.UriSource = new Uri(plotPredictionLocation);
                    bitmapPrediction.EndInit();
                    Grid_Search_Prediction_Image.Source = bitmapPrediction;
                    bitmapPrediction.UriSource = null;
                    GC.Collect();

                #endregion

                // Update Grid Search Results
                #region Update Grid Search Results

                if (Parameter_Selected.IsChecked == true)
                {
                    using (TextReader reader = File.OpenText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Best_CandG"))
                    {
                        C_Result_Text.Text = decimal.Parse(await reader.ReadLineAsync() ?? string.Empty).Normalize().ToString();
                        G_Result_Text.Text = decimal.Parse(await reader.ReadLineAsync() ?? string.Empty).Normalize().ToString();
                    }
                    //string Best_CandG = System.IO.File.ReadAllText(Saves_Directory.Text + "\\MatlabOutputText\\" + "Best_CandG");
                    //C_Result_Text.Text = Helper.Normalize( ToDecimal(Helper.ReadLine(Best_CandG, 1));
                    //G_Result_Text.Text = Helper.ReadLine(Best_CandG, 2);
                }
                else if (Feature_Selected.IsChecked == true)
                {
                    using (TextReader reader = File.OpenText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Best_Features"))
                    {
                        Att1_Result_Text.Text = decimal.Parse(await reader.ReadLineAsync() ?? string.Empty).Normalize().ToString();
                        Att2_Result_Text.Text = decimal.Parse(await reader.ReadLineAsync() ?? string.Empty).Normalize().ToString();
                    }
                }

                #endregion

                // Update Best Prediction Accuracy
                #region Update Best Prediction Accuracy

                var accuracyLines = File.ReadAllText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Grid_Search_Accuracy");
                GridSearch_MSE_text.Text = Helper.ReadLine(accuracyLines, 2);
                GridSearch_SCC_text.Text = Helper.ReadLine(accuracyLines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"Error");
                }

                #endregion

                GridSearchRunning = 0;
                Run_Search_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Run_Search_text.Text = "Run Search";
            }
        }

        public void Training_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.Matlab(Training_Data_Location);
            Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Train_Model_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks
            // Check if training is already running
            if (TrainRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Training in progress, please wait");
            }

            // Check if the training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and training data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid training data");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }
            
            #endregion

            else
            {
                // Set Up Training + Button
                #region Set Up Training

                TrainRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Train_Model_text.Text = "Training";

                #endregion

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Export Settings
                #region Export Settings

                switch (AttributeIndex)
                {
                    case 1:
                        A1Export = Attribute1Text.Text;
                        A2Export = "0";
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 2:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 3:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 4:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 5:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = "0";
                        break;
                    case 6:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = Attribute6Text.Text;
                        break;
                }

                if (false | Length_Text.Text == "" | Length_Text.Text == " ")
                { lengthTextExport = "0"; }
                else { lengthTextExport = Length_Text.Text; }

                File.Delete(assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt");
                var settingsPath = assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] settingsLines =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + lengthTextExport + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + optimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",
                 };
                File.WriteAllLines(settingsPath, settingsLines);
                #endregion

                // Export Training Directories
                #region Export Search And Training Directories

                var directoriesPath = assemblyLocation + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] directoriesLines =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Training_Data_Location.Text,
                };
                File.WriteAllLines(directoriesPath, directoriesLines);

                #endregion

                // Run the Program
                await Helper.RunTraining(assemblyLocation + "\\Required_Files\\");

                // Update the Model Stats
                #region Import Model Stats

                var modelStatsLines = File.ReadAllText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Model_Stats");
                iter_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 2).Where(c => !char.IsWhiteSpace(c))).Split('=').Last();
                nu_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 3).Where(c => !char.IsWhiteSpace(c))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                obj_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 4).Where(c => !char.IsWhiteSpace(c))).UntilComma().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                rho_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 4).Where(c => !char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                nSV_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 5).Where(c => !char.IsWhiteSpace(c))).UntilComma().Split('=').Last();
                nBSV_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 5).Where(c => !char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last();

                #endregion

                //Update Model Output location
                Model_Output_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_Output_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                TrainRunning = 0;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_text.Text = "Train Model";
            }
        }

        public void Send_To_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Model_Output_Location.Text))
            {
                System.Windows.Forms.MessageBox.Show(@"No file to send");
            }
            else
            {
                Model_File_Location.Text = Model_Output_Location.Text;
                Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            }
        }

        public void TAttributeMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Link_CheckBox.IsChecked == true)
            {
                tAttributeIndex = AttributeIndex;
            }
            else
            {
                tAttributeIndex -= 1;
                IfTAttributeIndexPlusMinusIsEnabled();
            }
            IfTAttributeIndexVisibility();

            UpdateLayout();
        }

        public void TAttributePlus_Click(object sender, RoutedEventArgs e)
        {
            if (Link_CheckBox.IsChecked == true)
            {
                tAttributeIndex = AttributeIndex;
            }
            else
            {
                tAttributeIndex += 1;
                IfTAttributeIndexPlusMinusIsEnabled();
            }

            IfTAttributeIndexVisibility();

            UpdateLayout();
        }

        public void Testing_Data_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.Matlab(Testing_Data_Location);
            Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public void Model_Location_Button_Click(object sender, RoutedEventArgs e)
        {
            FilePicker.Matlab(Model_File_Location);
            Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
        }

        public async void Test_Model_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks

            // Checks if Split is already running
            if (TestRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Testing in progress, please wait");
            }

            // Check if the testing and training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory, testing data, and model file");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid testing data");
            }
            else if (File.Exists(Helper.AddExtension(Model_File_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid model file");
            }

            #endregion

            else
            {
                // Set Up Testing + Button
                #region Set Up Testing

                TestRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Test_Model_text.Text = "Testing";

                #endregion

                // Create Saves and Plots folders if they don't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Update directories file with: Saves location, File Name, Testing Data Location, Model File Location
                #region Directories Export Testing

                var directoriesPath = assemblyLocation + "\\Required_Files\\Settings\\Directories_Export_Testing.txt";
                string[] directoriesLines =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Testing_Data_Location.Text,
                    Model_File_Location.Text,
                };

                File.WriteAllLines(directoriesPath, directoriesLines);
                
                #endregion

                // Export Testing Settings
                #region Export Testing Settings

                switch (tAttributeIndex)
                {
                    case 1:
                        tA1Export = TAttribute1.Text;
                        tA2Export = "0";
                        tA3Export = "0";
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 2:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = "0";
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 3:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 4:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 5:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = TAttribute5Text.Text;
                        tA6Export = "0";
                        break;
                    case 6:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = TAttribute5Text.Text;
                        tA6Export = TAttribute6Text.Text;
                        break;
                }

                if (false | TLength_Text.Text == "" | TLength_Text.Text == " ")
                { tLengthTextExport = "0"; }
                else { tLengthTextExport = TLength_Text.Text; }

                File.Delete(assemblyLocation + "\\Required_Files\\Settings\\Settings_Testing.txt");
                var settingsPath = assemblyLocation + "\\Required_Files\\Settings\\Settings_Testing.txt";
                string[] settingsLines =
                {
                "Start- 01, " + TStart_Text.Text + " ,",
                "Length 02, " + tLengthTextExport + " ,",
                "Projec 03, " + ProjectionTest.Text + " ,",
                "Att1Ft 04, " + tA1Export + " ,",
                "Att2Ft 05, " + tA2Export + " ,",
                "Att3Ft 06, " + tA3Export + " ,",
                "Att4Ft 07, " + tA4Export + " ,",
                "Att5Ft 08, " + tA5Export + " ,",
                "Att6Ft 09, " + tA6Export + " ,",
                "AttInx 10, " + tAttributeIndex + " ,",
                "Step-- 11, " + TStep_Text.Text + " ,",
                "Output 12, " + File_Type.SelectedIndex + " ,",
                };
                File.WriteAllLines(settingsPath, settingsLines);

                #endregion

                // Run the Program
                await Helper.RunTesting(assemblyLocation + "\\Required_Files\\");

                // Update the prediction file location
                Prediction_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Prediction";
                Prediction_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                // Update Plot and Accuracy Text
                #region Display Plot + Text

                Plot_Image.Source = null;
                Plot_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg"))
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg"))
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                    var plotLocation = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg";
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(plotLocation);
                    bitmap.EndInit();
                    Plot_Image.Source = bitmap;
                    bitmap.UriSource = null;
                    GC.Collect();


                var accuracyLines = File.ReadAllText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\" + "Accuracy");
                MSE_text.Text = Helper.ReadLine(accuracyLines, 2);
                SCC_text.Text = Helper.ReadLine(accuracyLines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"Error");
                }

                #endregion

                TestRunning = 0;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_text.Text = "Test Model";
            }
        }

        public async void Train_and_Test_Button_Click(object sender, RoutedEventArgs e)
        {
            #region Prerequesite Checks
            // Check if training or testing are already running
            if (TrainRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Training in progress, please wait");
            }
            else if (TestRunning == 1)
            {
                System.Windows.Forms.MessageBox.Show(@"Testing in progress, please wait");
            }

            // Check if the testing and training data and save directory folder exist
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false
                & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory, training data, and testing data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid training and testing data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and training data");
            }
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false & Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory and testing data");
            }
            else if (File.Exists(Helper.AddExtension(Training_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid training data");
            }
            else if (File.Exists(Helper.AddExtension(Testing_Data_Location.Text, ".mat")) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select valid testing data");
            }
            else if (Directory.Exists(Saves_Directory.Text) == false)
            {
                System.Windows.Forms.MessageBox.Show(@"Please select a valid saves directory");
            }

            #endregion

            else
            {
                #region Training 

                // Set Up Training + Button
                #region Set Up Training 

                TrainRunning = 1;
                var animation = new ColorAnimation
                {
                    From = Colors.Silver,
                    To = Colors.Thistle,
                    Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                    RepeatBehavior = RepeatBehavior.Forever,
                    AutoReverse = true
                };
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Train_Model_text.Text = "Training";

                #endregion

                // Create Saves folder if it doesn't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");

                // Export Settings
                #region Export Settings

                switch (AttributeIndex)
                {
                    case 1:
                        A1Export = Attribute1Text.Text;
                        A2Export = "0";
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 2:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = "0";
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 3:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = "0";
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 4:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = "0";
                        A6Export = "0";
                        break;
                    case 5:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = "0";
                        break;
                    case 6:
                        A1Export = Attribute1Text.Text;
                        A2Export = Attribute2Text.Text;
                        A3Export = Attribute3Text.Text;
                        A4Export = Attribute4Text.Text;
                        A5Export = Attribute5Text.Text;
                        A6Export = Attribute6Text.Text;
                        break;
                }

                if (false | Length_Text.Text == "" | Length_Text.Text == " ")
                { lengthTextExport = "0"; }
                else { lengthTextExport = Length_Text.Text; }

                File.Delete(assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt");
                var settingsPathTraining = assemblyLocation + "\\Required_Files\\Settings\\Settings_Text.txt";
                string[] settingsLinesTraining =
                {
                    "Start- 01, " + Start_Text.Text + " ,",
                    "Length 02, " + lengthTextExport + " ,",
                    "AccTyp 03, " + Accuracy_ComboBox.SelectedIndex + " ,",
                    "Att1Ft 04, " + A1Export + " ,",
                    "Att2Ft 05, " + A2Export + " ,",
                    "Att3Ft 06, " + A3Export + " ,",
                    "Att4Ft 07, " + A4Export + " ,",
                    "Att5Ft 08, " + A5Export + " ,",
                    "Att6Ft 09, " + A6Export + " ,",
                    "AttInx 10, " + AttributeIndex + " ,",
                    "",
                    "SVMTyp 11, " + SVM_ComboBox.SelectedIndex + " ,",
                    "Kernel 12, " + Kernel_ComboBox.SelectedIndex + " ,",
                    "Gamma- 13, " + Gamma_Text.Text + " ,",
                    "Degree 14, " + Degree_Text.Text + " ,",
                    "R----- 15, " + R_Text.Text + " ,",
                    "Cost-- 16, " + Cost_Text.Text + " ,",
                    "Nu---- 17, " + Nu_Text.Text + " ,",
                    "Ep---- 18, " + Ep_Text.Text + " ,",
                    "Ee---- 19, " + Ee_Text.Text + " ,",
                    "",
                    "Select 20, " + optimiseValue + " ,", // OptimiseValue for Search, 3 for Train
                    "CLower 21, " + CLower_Text.Text + " ,",
                    "CUpper 22, " + CUpper_Text.Text + " ,",
                    "C_Step 23, " + CStep_Text.Text + " ,",
                    "GLower 24, " + GLower_Text.Text + " ,",
                    "GUpper 25, " + GUpper_Text.Text + " ,",
                    "G_Step 26, " + GStep_Text.Text + " ,",
                    "At1Low 27, " + Att1Lower_Text.Text + " ,",
                    "At1Upp 28, " + Att1Upper_Text.Text + " ,",
                    "At1Stp 29, " + Att1Step_Text.Text + " ,",
                    "At2Low 30, " + Att2Lower_Text.Text + " ,",
                    "At2Upp 31, " + Att2Upper_Text.Text + " ,",
                    "At2Stp 32, " + Att2Step_Text.Text + " ,",
                    "nfold- 33, " + n_fold_Text.Text + " ,",
                    "Scale- 34, " + Convert.ToInt32(Scale_Selected.IsChecked) + " ,",
                    "Projec 35, " + ProjectionTrain.Text + " ,",
                    "Step-- 36, " + Step_Text.Text + " ,",
                    "cache- 37, " + Cache_text.Text + " ,",
                    "Shrink 38, " + Convert.ToInt32(Shrinking_Selected.IsChecked) + " ,",
                    "Split- 39, " + Split_Preference.Text + " ,",
                 };
                File.WriteAllLines(settingsPathTraining, settingsLinesTraining);
                #endregion

                // Export Training Directories
                #region Export Search And Training Directories

                var directoriesPathTraining = assemblyLocation + "\\Required_Files\\Settings\\Directories_SearchAndTraining.txt";
                string[] directoriesLinesTraining =
                {
                        Saves_Directory.Text,
                        File_Name.Text,
                        Training_Data_Location.Text,
                    };
                File.WriteAllLines(directoriesPathTraining, directoriesLinesTraining);

                #endregion

                // Run the Program
                await Helper.RunTraining(assemblyLocation + "\\Required_Files\\");

                // Update the Model Stats
                #region Import Model Stats

                var modelStatsLines = File.ReadAllText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Model_Stats");
                iter_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 2).Where(c => !char.IsWhiteSpace(c))).Split('=').Last();
                nu_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 3).Where(c => !char.IsWhiteSpace(c))).Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                obj_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 4).Where(c => !char.IsWhiteSpace(c))).UntilComma().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                rho_text.Text = decimal.Round(decimal.Parse(string.Concat(Helper.ReadLine(modelStatsLines, 4).Where(c => !char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last()), 3, MidpointRounding.AwayFromZero).ToString();
                nSV_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 5).Where(c => !char.IsWhiteSpace(c))).UntilComma().Split('=').Last();
                nBSV_text.Text = string.Concat(Helper.ReadLine(modelStatsLines, 5).Where(c => !char.IsWhiteSpace(c))).Split(',').Last().Split('=').Last();

                #endregion

                //Update Model Output location
                Model_Output_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_Output_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                TrainRunning = 0;
                Train_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Train_Model_text.Text = "Train Model";

                // Update Model File Location
                Model_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Model";
                Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                #endregion

                #region Testing

                // Set Up Testing + Button
                #region Set Up Testing

                TestRunning = 1;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_Button.Background.BeginAnimation(SolidColorBrush.ColorProperty, animation);
                Test_Model_text.Text = "Testing";

                #endregion

                // Create Saves and Plots folders if they don't exist
                Directory.CreateDirectory(Saves_Directory.Text + "\\Saves");
                Directory.CreateDirectory(Saves_Directory.Text + "\\Plots");

                // Update directories file with: Saves location, File Name, Testing Data Location, Model File Location
                #region Directories Export Testing

                var directoriesPathTesting = assemblyLocation + "\\Required_Files\\Settings\\Directories_Export_Testing.txt";
                string[] directoriesLinesTesting =
                {
                    Saves_Directory.Text,
                    File_Name.Text,
                    Testing_Data_Location.Text,
                    Model_File_Location.Text,
                };
                File.WriteAllLines(directoriesPathTesting, directoriesLinesTesting);

                #endregion

                // Export Testing Settings
                #region Export Testing Settings

                switch (tAttributeIndex)
                {
                    case 1:
                        tA1Export = TAttribute1.Text;
                        tA2Export = "0";
                        tA3Export = "0";
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 2:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = "0";
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 3:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = "0";
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 4:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = "0";
                        tA6Export = "0";
                        break;
                    case 5:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = TAttribute5Text.Text;
                        tA6Export = "0";
                        break;
                    case 6:
                        tA1Export = TAttribute1Text.Text;
                        tA2Export = TAttribute2Text.Text;
                        tA3Export = TAttribute3Text.Text;
                        tA4Export = TAttribute4Text.Text;
                        tA5Export = TAttribute5Text.Text;
                        tA6Export = TAttribute6Text.Text;
                        break;
                }

                if (false | TLength_Text.Text == "" | TLength_Text.Text == " ")
                { tLengthTextExport = "0"; }
                else { tLengthTextExport = TLength_Text.Text; }

                File.Delete(assemblyLocation + "\\Required_Files\\Settings\\Settings_Testing.txt");
                var settingsPathTesting = assemblyLocation + "\\Required_Files\\Settings\\Settings_Testing.txt";
                string[] settingsLinesTesting =
                {
                    "Start- 01, " + TStart_Text.Text + " ,",
                    "Length 02, " + tLengthTextExport + " ,",
                    "Projec 03, " + ProjectionTest.Text + " ,",
                    "Att1Ft 04, " + tA1Export + " ,",
                    "Att2Ft 05, " + tA2Export + " ,",
                    "Att3Ft 06, " + tA3Export + " ,",
                    "Att4Ft 07, " + tA4Export + " ,",
                    "Att5Ft 08, " + tA5Export + " ,",
                    "Att6Ft 09, " + tA6Export + " ,",
                    "AttInx 10, " + tAttributeIndex + " ,",
                    "Step-- 11, " + TStep_Text.Text + " ,",
                    "Output 12, " + File_Type.SelectedIndex + " ,",
                };
                File.WriteAllLines(settingsPathTesting, settingsLinesTesting);

                #endregion

                // Run the Program
                await Helper.RunTesting(assemblyLocation + "\\Required_Files\\");

                // Update the prediction file location
                Prediction_File_Location.Text = Saves_Directory.Text + "\\Saves\\" + File_Name.Text + "_Prediction";
                Prediction_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);

                // Update Plot and Accuracy Text
                #region Display Plot + Text

                Plot_Image.Source = null;
                Plot_Image.UpdateLayout();
                GC.Collect();

                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg"))
                {
                    File.Delete(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                }
                if (File.Exists(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg"))
                {
                    File.Move(Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot_cache.jpg", Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg");
                    var plotLocation = Saves_Directory.Text + "\\Plots\\" + File_Name.Text + "_Plot.jpg";
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(plotLocation);
                    bitmap.EndInit();
                    Plot_Image.Source = bitmap;
                    bitmap.UriSource = null;
                    GC.Collect();

                    // Accuracy Text
                    var accuracyLines = File.ReadAllText(assemblyLocation + "\\Required_Files\\MatlabOutputText\\" + "Accuracy");
                    MSE_text.Text = Helper.ReadLine(accuracyLines, 2);
                    SCC_text.Text = Helper.ReadLine(accuracyLines, 3);

                }
                else
                {
                    System.Windows.Forms.MessageBox.Show(@"Error");
                }

                #endregion

                TestRunning = 0;
                Test_Model_Button.Background = new SolidColorBrush(Colors.DarkTurquoise);
                Test_Model_text.Text = "Test Model";

                #endregion
            }
        }

        public void Link_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            tA1Cache = TAttribute1Text.Text;
            tA2Cache = TAttribute2Text.Text;
            tA3Cache = TAttribute3Text.Text;
            tA4Cache = TAttribute4Text.Text;
            tA5Cache = TAttribute5Text.Text;
            tA6Cache = TAttribute6Text.Text;

            FeaturesLinkedobj = new FeaturesLinked
            {
                Train1Value = Attribute1Text.Text,
                Train2Value = Attribute2Text.Text,
                Train3Value = Attribute3Text.Text,
                Train4Value = Attribute4Text.Text,
                Train5Value = Attribute5Text.Text,
                Train6Value = Attribute6Text.Text,
                Test1Value = Attribute1Text.Text,
                Test2Value = Attribute2Text.Text,
                Test3Value = Attribute3Text.Text,
                Test4Value = Attribute4Text.Text,
                Test5Value = Attribute5Text.Text,
                Test6Value = Attribute6Text.Text,
            };

            Attribute1Text.DataContext = FeaturesLinkedobj;
            Attribute2Text.DataContext = FeaturesLinkedobj;
            Attribute3Text.DataContext = FeaturesLinkedobj;
            Attribute4Text.DataContext = FeaturesLinkedobj;
            Attribute5Text.DataContext = FeaturesLinkedobj;
            Attribute6Text.DataContext = FeaturesLinkedobj;
            TAttribute1Text.DataContext = FeaturesLinkedobj;
            TAttribute2Text.DataContext = FeaturesLinkedobj;
            TAttribute3Text.DataContext = FeaturesLinkedobj;
            TAttribute4Text.DataContext = FeaturesLinkedobj;
            TAttribute5Text.DataContext = FeaturesLinkedobj;
            TAttribute6Text.DataContext = FeaturesLinkedobj;

            tAttributeIndexCache = tAttributeIndex;
            tAttributeIndex = AttributeIndex;

            IfTAttributeIndexVisibility();

            TAttribute1Text.IsReadOnly = true;
            TAttribute2Text.IsReadOnly = true;
            TAttribute3Text.IsReadOnly = true;
            TAttribute4Text.IsReadOnly = true;
            TAttribute5Text.IsReadOnly = true;
            TAttribute6Text.IsReadOnly = true;
            TAttribute1Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute2Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute3Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute4Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute5Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttribute6Text.Background = System.Windows.Media.Brushes.WhiteSmoke;
            TAttributeMinus.IsEnabled = false;
            TAttributePlus.IsEnabled = false;
            UpdateLayout();
        }

        private void Link_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Featuresobj = new Features()
            {
                Train1Value = Attribute1Text.Text,
                Train2Value = Attribute2Text.Text,
                Train3Value = Attribute3Text.Text,
                Train4Value = Attribute4Text.Text,
                Train5Value = Attribute5Text.Text,
                Train6Value = Attribute6Text.Text,
                Test1Value = tA1Cache,
                Test2Value = tA2Cache,
                Test3Value = tA3Cache,
                Test4Value = tA4Cache,
                Test5Value = tA5Cache,
                Test6Value = tA6Cache,
            };

            Attribute1Text.DataContext = Featuresobj;
            Attribute2Text.DataContext = Featuresobj;
            Attribute3Text.DataContext = Featuresobj;
            Attribute4Text.DataContext = Featuresobj;
            Attribute5Text.DataContext = Featuresobj;
            Attribute6Text.DataContext = Featuresobj;
            TAttribute1Text.DataContext = Featuresobj;
            TAttribute2Text.DataContext = Featuresobj;
            TAttribute3Text.DataContext = Featuresobj;
            TAttribute4Text.DataContext = Featuresobj;
            TAttribute5Text.DataContext = Featuresobj;
            TAttribute6Text.DataContext = Featuresobj;

            tAttributeIndex = tAttributeIndexCache;

            IfTAttributeIndexVisibility();
            IfTAttributeIndexPlusMinusIsEnabled();

            TAttribute1Text.IsReadOnly = false;
            TAttribute2Text.IsReadOnly = false;
            TAttribute3Text.IsReadOnly = false;
            TAttribute4Text.IsReadOnly = false;
            TAttribute5Text.IsReadOnly = false;
            TAttribute6Text.IsReadOnly = false;
            TAttribute1Text.Background = System.Windows.Media.Brushes.White;
            TAttribute2Text.Background = System.Windows.Media.Brushes.White;
            TAttribute3Text.Background = System.Windows.Media.Brushes.White;
            TAttribute4Text.Background = System.Windows.Media.Brushes.White;
            TAttribute5Text.Background = System.Windows.Media.Brushes.White;
            TAttribute6Text.Background = System.Windows.Media.Brushes.White;

            UpdateLayout();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            Process.GetProcessById(matlabPid).Kill();
            //SignalHandler(int SIGINT);
            //myProcess.StandardInput.Close();
            //Process p;
            //if (AttachConsole((uint)p.Id))
            //{
            //    SetConsoleCtrlHandler(null, true);
            //    try
            //    {
            //        if (!GenerateConsoleCtrlEvent(CTRL_C_EVENT, p.SessionId))
            //            return false;
            //        p.WaitForExit();
            //    }
            //    finally
            //    {
            //        SetConsoleCtrlHandler(null, false);
            //        FreeConsole();
            //    }
            //    return true;
            //}

        }

        private void Save_Preset_Click(object sender, RoutedEventArgs e)
        {
            var presetPath = assemblyLocation + "\\Required_Files\\Presets\\" + Preset_Name.Text;
            string[] presetLines =
            {
                Saves_Directory.Text,
                File_Name.Text,
                Data_File_Source.Text,
                Column1Text.Text,
                Column2Text.Text,
                Column3Text.Text,
                Column4Text.Text,
                Column5Text.Text,
                Column6Text.Text,
                Date_Column.Text,
                TopToBottom.IsChecked.ToString(),
                Has_Labels.IsChecked.ToString(),
                Scale_Selected.IsChecked.ToString(),
                Prepared_File_Location.Text,
                Data_Split_Location.Text,
                Percentage_Split.Text,
                First_Portion_Location.Text,
                Second_Portion_Location.Text,
                Start_Text.Text,
                Length_Text.Text,
                ProjectionTrain.Text,
                Step_Text.Text,
                Attribute1Text.Text,
                Attribute2Text.Text,
                Attribute3Text.Text,
                Attribute4Text.Text,
                Attribute5Text.Text,
                Attribute6Text.Text,
                Accuracy_ComboBox.SelectedIndex.ToString(),
                SVM_ComboBox.SelectedIndex.ToString(),
                Cost_Text.Text,
                Ee_Text.Text,
                Ep_Text.Text,
                Nu_Text.Text,
                Kernel_ComboBox.SelectedIndex.ToString(),
                Gamma_Text.Text,
                R_Text.Text,
                Degree_Text.Text,
                Parameter_Selected.IsChecked.ToString(),
                CLower_Text.Text,
                CUpper_Text.Text,
                CStep_Text.Text,
                GLower_Text.Text,
                GUpper_Text.Text,
                GStep_Text.Text,
                Feature_Selected.IsChecked.ToString(),
                Att1Lower_Text.Text,
                Att1Upper_Text.Text,
                Att1Step_Text.Text,
                Att2Lower_Text.Text,
                Att2Upper_Text.Text,
                Att2Step_Text.Text,
                C_Result_Text.Text,
                G_Result_Text.Text,
                Att1_Result_Text.Text,
                Att2_Result_Text.Text,
                Training_Data_Location.Text,
                Model_Output_Location.Text,
                Testing_Data_Location.Text,
                Model_File_Location.Text,
                TAttribute1Text.Text,
                TAttribute2Text.Text,
                TAttribute3Text.Text,
                TAttribute4Text.Text,
                TAttribute5Text.Text,
                TAttribute6Text.Text,
                TStart_Text.Text,
                TLength_Text.Text,
                ProjectionTest.Text,
                TStep_Text.Text,
                Prediction_File_Location.Text,
                Link_CheckBox.IsChecked.ToString(),
                ColumnIndex.ToString(),
                AttributeIndex.ToString(),
                tAttributeIndex.ToString(),
                Shrinking_Selected.IsChecked.ToString(),
                Cache_text.Text,
                Split_Preference.Text,
                File_Type.SelectedIndex.ToString(),
            };
            File.WriteAllLines(presetPath, presetLines);
        }

        private void Load_Preset_Click(object sender, RoutedEventArgs e)
        {
            var main = new FilePicker();
            var results = main.Preset();
            if (results.Item1 == "1")
            {
                var presetLines = File.ReadAllText(results.Item2);
                Saves_Directory.Text = Helper.ReadLine(presetLines, 1);
                File_Name.Text = Helper.ReadLine(presetLines, 2);
                Data_File_Source.Text = Helper.ReadLine(presetLines, 3);
                Column1Text.Text = Helper.ReadLine(presetLines, 4);
                Column2Text.Text = Helper.ReadLine(presetLines, 5);
                Column3Text.Text = Helper.ReadLine(presetLines, 6);
                Column4Text.Text = Helper.ReadLine(presetLines, 7);
                Column5Text.Text = Helper.ReadLine(presetLines, 8);
                Column6Text.Text = Helper.ReadLine(presetLines, 9);
                Date_Column.Text = Helper.ReadLine(presetLines, 10);
                TopToBottom.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 11));
                Has_Labels.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 12));
                Scale_Selected.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 13));
                Prepared_File_Location.Text = Helper.ReadLine(presetLines, 14);
                Data_Split_Location.Text = Helper.ReadLine(presetLines, 15);
                Percentage_Split.Text = Helper.ReadLine(presetLines, 16);
                First_Portion_Location.Text = Helper.ReadLine(presetLines, 17);
                Second_Portion_Location.Text = Helper.ReadLine(presetLines, 18);
                Start_Text.Text = Helper.ReadLine(presetLines, 19);
                Length_Text.Text = Helper.ReadLine(presetLines, 20);
                ProjectionTrain.Text = Helper.ReadLine(presetLines, 21);
                Step_Text.Text = Helper.ReadLine(presetLines, 22);
                Attribute1Text.Text = Helper.ReadLine(presetLines, 23);
                Attribute2Text.Text = Helper.ReadLine(presetLines, 24);
                Attribute3Text.Text = Helper.ReadLine(presetLines, 25);
                Attribute4Text.Text = Helper.ReadLine(presetLines, 26);
                Attribute5Text.Text = Helper.ReadLine(presetLines, 27);
                Attribute6Text.Text = Helper.ReadLine(presetLines, 28);
                Accuracy_ComboBox.SelectedIndex = int.Parse(Helper.ReadLine(presetLines, 29));
                SVM_ComboBox.SelectedIndex = int.Parse(Helper.ReadLine(presetLines, 30));
                Cost_Text.Text = Helper.ReadLine(presetLines, 31);
                Ee_Text.Text = Helper.ReadLine(presetLines, 32);
                Ep_Text.Text = Helper.ReadLine(presetLines, 33);
                Nu_Text.Text = Helper.ReadLine(presetLines, 34);
                Kernel_ComboBox.SelectedIndex = int.Parse(Helper.ReadLine(presetLines, 35));
                Gamma_Text.Text = Helper.ReadLine(presetLines, 36);
                R_Text.Text = Helper.ReadLine(presetLines, 37);
                Degree_Text.Text = Helper.ReadLine(presetLines, 38);
                Parameter_Selected.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 39));
                CLower_Text.Text = Helper.ReadLine(presetLines, 40);
                CUpper_Text.Text = Helper.ReadLine(presetLines, 41);
                CStep_Text.Text = Helper.ReadLine(presetLines, 42);
                GLower_Text.Text = Helper.ReadLine(presetLines, 43);
                GUpper_Text.Text = Helper.ReadLine(presetLines, 44);
                GStep_Text.Text = Helper.ReadLine(presetLines, 45);
                Feature_Selected.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 46));
                Att1Lower_Text.Text = Helper.ReadLine(presetLines, 47);
                Att1Upper_Text.Text = Helper.ReadLine(presetLines, 48);
                Att1Step_Text.Text = Helper.ReadLine(presetLines, 49);
                Att2Lower_Text.Text = Helper.ReadLine(presetLines, 50);
                Att2Upper_Text.Text = Helper.ReadLine(presetLines, 51);
                Att2Step_Text.Text = Helper.ReadLine(presetLines, 52);
                C_Result_Text.Text = Helper.ReadLine(presetLines, 53);
                G_Result_Text.Text = Helper.ReadLine(presetLines, 54);
                Att1_Result_Text.Text = Helper.ReadLine(presetLines, 55);
                Att2_Result_Text.Text = Helper.ReadLine(presetLines, 56);
                Training_Data_Location.Text = Helper.ReadLine(presetLines, 57);
                Model_Output_Location.Text = Helper.ReadLine(presetLines, 58);
                Testing_Data_Location.Text = Helper.ReadLine(presetLines, 59);
                Model_File_Location.Text = Helper.ReadLine(presetLines, 60);
                TAttribute1Text.Text = Helper.ReadLine(presetLines, 61);
                TAttribute2Text.Text = Helper.ReadLine(presetLines, 62);
                TAttribute3Text.Text = Helper.ReadLine(presetLines, 63);
                TAttribute4Text.Text = Helper.ReadLine(presetLines, 64);
                TAttribute5Text.Text = Helper.ReadLine(presetLines, 65);
                TAttribute6Text.Text = Helper.ReadLine(presetLines, 66);
                TStart_Text.Text = Helper.ReadLine(presetLines, 67);
                TLength_Text.Text = Helper.ReadLine(presetLines, 68);
                ProjectionTest.Text = Helper.ReadLine(presetLines, 69);
                TStep_Text.Text = Helper.ReadLine(presetLines, 70);
                Prediction_File_Location.Text = Helper.ReadLine(presetLines, 71);
                Link_CheckBox.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 72));
                ColumnIndex = int.Parse(Helper.ReadLine(presetLines, 73));
                AttributeIndex = int.Parse(Helper.ReadLine(presetLines, 74));
                tAttributeIndex = int.Parse(Helper.ReadLine(presetLines, 75));
                Shrinking_Selected.IsChecked = Convert.ToBoolean(Helper.ReadLine(presetLines, 76));
                Cache_text.Text = Helper.ReadLine(presetLines, 77);
                Split_Preference.Text = Helper.ReadLine(presetLines, 78);
                File_Type.SelectedIndex = int.Parse(Helper.ReadLine(presetLines, 79));

                var presetPath = Path.GetFileName(results.Item2);
                System.Windows.Forms.MessageBox.Show(@"Loaded " + presetPath);

                IfColumnIndex();
                IfAttributeIndexAll();
                IfTAttributeIndexVisibility();
                IfTAttributeIndexPlusMinusIsEnabled();

                UpdateLayout();
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(@"Preset not found");
            }
        }

        #region On Loaded

        public void Training_Data_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Training_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Training_Data_Location.UpdateLayout();
        }

        public void Testing_Data_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Testing_Data_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Testing_Data_Location.UpdateLayout();
        }

        private void Model_File_Location_Loaded(object sender, RoutedEventArgs e)
        {
            Model_File_Location.ScrollToHorizontalOffset(double.PositiveInfinity);
            Model_File_Location.UpdateLayout();
        }

        #endregion

        #region Inclass Helpers

        public void Folder_File_Create()
        {
            Directory.CreateDirectory(assemblyLocation + "\\Required_Files");
            Directory.CreateDirectory(assemblyLocation + "\\Required_Files\\Settings");
            Directory.CreateDirectory(assemblyLocation + "\\Required_Files\\MatlabOutputText");
            Directory.CreateDirectory(assemblyLocation + "\\Required_Files\\Presets");
            Directory.CreateDirectory(assemblyLocation + "\\Saves");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Accuracy.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Best_CandG.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Best_Features.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Feature_Cross.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Grid_Search_Accuracy.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Model_Stats.txt");
            Helper.CreateEmptyFileIfNotThere(assemblyLocation + "\\Required_Files\\MatlabOutputText\\Parameter_Cross.txt");
        }

        public void IfTAttributeIndexVisibility()
        {
            switch (tAttributeIndex)
            {
                case 1:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Collapsed;
                    TAttribute3.Visibility = Visibility.Collapsed;
                    TAttribute4.Visibility = Visibility.Collapsed;
                    TAttribute5.Visibility = Visibility.Collapsed;
                    TAttribute6.Visibility = Visibility.Collapsed;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Collapsed;
                    TAttribute3Text.Visibility = Visibility.Collapsed;
                    TAttribute4Text.Visibility = Visibility.Collapsed;
                    TAttribute5Text.Visibility = Visibility.Collapsed;
                    TAttribute6Text.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Visible;
                    TAttribute3.Visibility = Visibility.Collapsed;
                    TAttribute4.Visibility = Visibility.Collapsed;
                    TAttribute5.Visibility = Visibility.Collapsed;
                    TAttribute6.Visibility = Visibility.Collapsed;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Visible;
                    TAttribute3Text.Visibility = Visibility.Collapsed;
                    TAttribute4Text.Visibility = Visibility.Collapsed;
                    TAttribute5Text.Visibility = Visibility.Collapsed;
                    TAttribute6Text.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Visible;
                    TAttribute3.Visibility = Visibility.Visible;
                    TAttribute4.Visibility = Visibility.Collapsed;
                    TAttribute5.Visibility = Visibility.Collapsed;
                    TAttribute6.Visibility = Visibility.Collapsed;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Visible;
                    TAttribute3Text.Visibility = Visibility.Visible;
                    TAttribute4Text.Visibility = Visibility.Collapsed;
                    TAttribute5Text.Visibility = Visibility.Collapsed;
                    TAttribute6Text.Visibility = Visibility.Collapsed;
                    break;
                case 4:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Visible;
                    TAttribute3.Visibility = Visibility.Visible;
                    TAttribute4.Visibility = Visibility.Visible;
                    TAttribute5.Visibility = Visibility.Collapsed;
                    TAttribute6.Visibility = Visibility.Collapsed;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Visible;
                    TAttribute3Text.Visibility = Visibility.Visible;
                    TAttribute4Text.Visibility = Visibility.Visible;
                    TAttribute5Text.Visibility = Visibility.Collapsed;
                    TAttribute6Text.Visibility = Visibility.Collapsed;
                    break;
                case 5:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Visible;
                    TAttribute3.Visibility = Visibility.Visible;
                    TAttribute4.Visibility = Visibility.Visible;
                    TAttribute5.Visibility = Visibility.Visible;
                    TAttribute6.Visibility = Visibility.Collapsed;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Visible;
                    TAttribute3Text.Visibility = Visibility.Visible;
                    TAttribute4Text.Visibility = Visibility.Visible;
                    TAttribute5Text.Visibility = Visibility.Visible;
                    TAttribute6Text.Visibility = Visibility.Collapsed;
                    break;
                case 6:
                    TAttribute1.Visibility = Visibility.Visible;
                    TAttribute2.Visibility = Visibility.Visible;
                    TAttribute3.Visibility = Visibility.Visible;
                    TAttribute4.Visibility = Visibility.Visible;
                    TAttribute5.Visibility = Visibility.Visible;
                    TAttribute6.Visibility = Visibility.Visible;
                    TAttribute1Text.Visibility = Visibility.Visible;
                    TAttribute2Text.Visibility = Visibility.Visible;
                    TAttribute3Text.Visibility = Visibility.Visible;
                    TAttribute4Text.Visibility = Visibility.Visible;
                    TAttribute5Text.Visibility = Visibility.Visible;
                    TAttribute6Text.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void IfTAttributeIndexPlusMinusIsEnabled()
        {
            switch (tAttributeIndex)
            {
                case 1:
                    TAttributeMinus.IsEnabled = false; TAttributePlus.IsEnabled = true;
                    break;
                case 2:
                    TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true;
                    break;
                case 3:
                    TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true;
                    break;
                case 4:
                    TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true;
                    break;
                case 5:
                    TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = true;
                    break;
                case 6:
                    TAttributeMinus.IsEnabled = true; TAttributePlus.IsEnabled = false;
                    break;
            }

        }

        public void IfAttributeIndexAll()
        {
            switch (AttributeIndex)
            {
                case 1:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Collapsed;
                    Attribute3.Visibility = Visibility.Collapsed;
                    Attribute4.Visibility = Visibility.Collapsed;
                    Attribute5.Visibility = Visibility.Collapsed;
                    Attribute6.Visibility = Visibility.Collapsed;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Collapsed;
                    Attribute3Text.Visibility = Visibility.Collapsed;
                    Attribute4Text.Visibility = Visibility.Collapsed;
                    Attribute5Text.Visibility = Visibility.Collapsed;
                    Attribute6Text.Visibility = Visibility.Collapsed;
                    AttributeMinus.IsEnabled = false;
                    AttributePlus.IsEnabled = true;
                    break;
                case 2:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Visible;
                    Attribute3.Visibility = Visibility.Collapsed;
                    Attribute4.Visibility = Visibility.Collapsed;
                    Attribute5.Visibility = Visibility.Collapsed;
                    Attribute6.Visibility = Visibility.Collapsed;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Visible;
                    Attribute3Text.Visibility = Visibility.Collapsed;
                    Attribute4Text.Visibility = Visibility.Collapsed;
                    Attribute5Text.Visibility = Visibility.Collapsed;
                    Attribute6Text.Visibility = Visibility.Collapsed;
                    AttributeMinus.IsEnabled = true;
                    AttributePlus.IsEnabled = true;
                    break;
                case 3:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Visible;
                    Attribute3.Visibility = Visibility.Visible;
                    Attribute4.Visibility = Visibility.Collapsed;
                    Attribute5.Visibility = Visibility.Collapsed;
                    Attribute6.Visibility = Visibility.Collapsed;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Visible;
                    Attribute3Text.Visibility = Visibility.Visible;
                    Attribute4Text.Visibility = Visibility.Collapsed;
                    Attribute5Text.Visibility = Visibility.Collapsed;
                    Attribute6Text.Visibility = Visibility.Collapsed;
                    AttributeMinus.IsEnabled = true;
                    AttributePlus.IsEnabled = true;
                    break;
                case 4:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Visible;
                    Attribute3.Visibility = Visibility.Visible;
                    Attribute4.Visibility = Visibility.Visible;
                    Attribute5.Visibility = Visibility.Collapsed;
                    Attribute6.Visibility = Visibility.Collapsed;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Visible;
                    Attribute3Text.Visibility = Visibility.Visible;
                    Attribute4Text.Visibility = Visibility.Visible;
                    Attribute5Text.Visibility = Visibility.Collapsed;
                    Attribute6Text.Visibility = Visibility.Collapsed;
                    AttributeMinus.IsEnabled = true;
                    AttributePlus.IsEnabled = true;
                    break;
                case 5:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Visible;
                    Attribute3.Visibility = Visibility.Visible;
                    Attribute4.Visibility = Visibility.Visible;
                    Attribute5.Visibility = Visibility.Visible;
                    Attribute6.Visibility = Visibility.Collapsed;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Visible;
                    Attribute3Text.Visibility = Visibility.Visible;
                    Attribute4Text.Visibility = Visibility.Visible;
                    Attribute5Text.Visibility = Visibility.Visible;
                    Attribute6Text.Visibility = Visibility.Collapsed;
                    AttributeMinus.IsEnabled = true;
                    AttributePlus.IsEnabled = true;
                    break;
                case 6:
                    Attribute1.Visibility = Visibility.Visible;
                    Attribute2.Visibility = Visibility.Visible;
                    Attribute3.Visibility = Visibility.Visible;
                    Attribute4.Visibility = Visibility.Visible;
                    Attribute5.Visibility = Visibility.Visible;
                    Attribute6.Visibility = Visibility.Visible;
                    Attribute1Text.Visibility = Visibility.Visible;
                    Attribute2Text.Visibility = Visibility.Visible;
                    Attribute3Text.Visibility = Visibility.Visible;
                    Attribute4Text.Visibility = Visibility.Visible;
                    Attribute5Text.Visibility = Visibility.Visible;
                    Attribute6Text.Visibility = Visibility.Visible;
                    AttributeMinus.IsEnabled = true;
                    AttributePlus.IsEnabled = false;
                    break;
            }
        }

        public void IfColumnIndex()
        {
            switch (ColumnIndex)
            {
                case 1:
                    Column2.Visibility = Visibility.Collapsed;
                    Column3.Visibility = Visibility.Collapsed;
                    Column4.Visibility = Visibility.Collapsed;
                    Column5.Visibility = Visibility.Collapsed;
                    Column6.Visibility = Visibility.Collapsed;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Collapsed;
                    Column3Text.Visibility = Visibility.Collapsed;
                    Column4Text.Visibility = Visibility.Collapsed;
                    Column5Text.Visibility = Visibility.Collapsed;
                    Column6Text.Visibility = Visibility.Collapsed;
                    ColumnMinus.IsEnabled = false;
                    ColumnPlus.IsEnabled = true;
                    break;
                case 2:
                    Column2.Visibility = Visibility.Visible;
                    Column3.Visibility = Visibility.Collapsed;
                    Column4.Visibility = Visibility.Collapsed;
                    Column5.Visibility = Visibility.Collapsed;
                    Column6.Visibility = Visibility.Collapsed;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Visible;
                    Column3Text.Visibility = Visibility.Collapsed;
                    Column4Text.Visibility = Visibility.Collapsed;
                    Column5Text.Visibility = Visibility.Collapsed;
                    Column6Text.Visibility = Visibility.Collapsed;
                    ColumnMinus.IsEnabled = true;
                    ColumnPlus.IsEnabled = true;
                    break;
                case 3:
                    Column2.Visibility = Visibility.Visible;
                    Column3.Visibility = Visibility.Visible;
                    Column4.Visibility = Visibility.Collapsed;
                    Column5.Visibility = Visibility.Collapsed;
                    Column6.Visibility = Visibility.Collapsed;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Visible;
                    Column3Text.Visibility = Visibility.Visible;
                    Column4Text.Visibility = Visibility.Collapsed;
                    Column5Text.Visibility = Visibility.Collapsed;
                    Column6Text.Visibility = Visibility.Collapsed;
                    ColumnMinus.IsEnabled = true;
                    ColumnPlus.IsEnabled = true;
                    break;
                case 4:
                    Column2.Visibility = Visibility.Visible;
                    Column3.Visibility = Visibility.Visible;
                    Column4.Visibility = Visibility.Visible;
                    Column5.Visibility = Visibility.Collapsed;
                    Column6.Visibility = Visibility.Collapsed;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Visible;
                    Column3Text.Visibility = Visibility.Visible;
                    Column4Text.Visibility = Visibility.Visible;
                    Column5Text.Visibility = Visibility.Collapsed;
                    Column6Text.Visibility = Visibility.Collapsed;
                    ColumnMinus.IsEnabled = true;
                    ColumnPlus.IsEnabled = true;
                    break;
                case 5:
                    Column2.Visibility = Visibility.Visible;
                    Column3.Visibility = Visibility.Visible;
                    Column4.Visibility = Visibility.Visible;
                    Column5.Visibility = Visibility.Visible;
                    Column6.Visibility = Visibility.Collapsed;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Visible;
                    Column3Text.Visibility = Visibility.Visible;
                    Column4Text.Visibility = Visibility.Visible;
                    Column5Text.Visibility = Visibility.Visible;
                    Column6Text.Visibility = Visibility.Collapsed;
                    ColumnMinus.IsEnabled = true;
                    ColumnPlus.IsEnabled = true;
                    break;
                case 6:
                    Column2.Visibility = Visibility.Visible;
                    Column3.Visibility = Visibility.Visible;
                    Column4.Visibility = Visibility.Visible;
                    Column5.Visibility = Visibility.Visible;
                    Column6.Visibility = Visibility.Visible;
                    Column1Text.Visibility = Visibility.Visible;
                    Column2Text.Visibility = Visibility.Visible;
                    Column3Text.Visibility = Visibility.Visible;
                    Column4Text.Visibility = Visibility.Visible;
                    Column5Text.Visibility = Visibility.Visible;
                    Column6Text.Visibility = Visibility.Visible;
                    ColumnMinus.IsEnabled = true;
                    ColumnPlus.IsEnabled = false;
                    break;
            }
        }

        public void SetMatlabPID(int pid)
        {
            matlabPid = pid;
        }

        #endregion

        #region Toolbar

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            if (ConvertRunning == 1 | SplitRunning == 1 | GridSearchRunning == 1 | TrainRunning == 1 | TestRunning == 1) 
            {

                const string message = "Are you sure that you would like to close while program is running?";
                const string caption = "Close Program";
                var result = System.Windows.Forms.MessageBox.Show(message, caption,
                                 MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question);

                if (result == System.Windows.Forms.DialogResult.Yes)
                    Close();
            }
            else
            {
                Close();
            }
        }

        private void Minimise_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        #endregion
    }
}



#region Helpers

internal static class Helper
{
    public static string ReadLine(string text, int lineNumber)
    {
        var reader = new StringReader(text);

        string line;
        var currentLineNumber = 0;

        do
        {
            currentLineNumber += 1;
            line = reader.ReadLine();
        }
        while (line != null && currentLineNumber < lineNumber);

        return (currentLineNumber == lineNumber) ? line : string.Empty;
    }

    public static string UntilEquals(this string text, string stopAt = "=")
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

        return charLocation > 0 ? text.Substring(0, charLocation) : string.Empty;
    }

    public static string UntilComma(this string text, string stopAt = ",")
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        var charLocation = text.IndexOf(stopAt, StringComparison.Ordinal);

        return charLocation > 0 ? text.Substring(0, charLocation) : string.Empty;
    }

    public static decimal Normalize(this decimal value)
    {
        return value / 1.000000000000000000000000000000000m;
    }

    public static void CreateEmptyFileIfNotThere(string filename)
    {
        if (File.Exists(filename) == false)
        {
            File.Create(filename).Dispose();
        }
    }

    public static string AddExtension(string filename, string extension)
    {
        var ext = Path.GetExtension(filename);
        if (extension == ext)
        {
            return filename;
        }
        else
        {
            return filename + extension;
        }
    }

    public static async Task RunDatamanagement(string matlabInput)
    {
        var matlabTask = Task.Run(() => {
            try
            {
                var class1Instance = new DataManagement.Class1();
                var rootIn = new MWCharArray(matlabInput);
                class1Instance.DataManagement(rootIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
        await matlabTask;
    }

    public static async Task RunGridSearch(string matlabInput, object netCallback)
    {

        var matlabTask = Task.Run(() => {
            try
            {
                var class1Instance = new ForGUI_GridSearch.Class1();
                var rootIn = new MWCharArray(matlabInput);
                class1Instance.ForGUI_GridSearch(rootIn, new MWObjectArray(netCallback));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
        await matlabTask;
    }

    public static async Task RunTraining(string matlabInput)
    {
        var matlabTask = Task.Run(() => {
            try
            {
                var class1Instance = new Training.Class1();
                var rootIn = new MWCharArray(matlabInput);
                class1Instance.Training(rootIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
        await matlabTask;
    }
    
    public static async Task RunTesting(string matlabInput)
    {
        var matlabTask = Task.Run(() => {

            try
            {
                var class1Instance = new Testing.Class1();
                var rootIn = new MWCharArray(matlabInput);
                class1Instance.Testing(rootIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        });
        await matlabTask;
    }

    public static async Task RunSplit(string matlabInput)
    {
        var matlabTask = Task.Run(() => {
            try
            {
                var class1Instance = new Split.Class1();
                var rootIn = new MWCharArray(matlabInput);
                class1Instance.Split(rootIn);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        });
        await matlabTask;
    }

}

internal class FolderPicker
{
    public virtual string ResultPath { get; protected set; }
    public virtual string ResultName { get; protected set; }
    public virtual string InputPath { get; set; }
    public virtual bool ForceFileSystem { get; set; }
    public virtual string Title { get; set; }
    public virtual string OkButtonLabel { get; set; }
    public virtual string FileNameLabel { get; set; }

    protected virtual int SetOptions(int options)
    {
        if (ForceFileSystem)
        {
            options |= (int)Fos.FOS_FORCEFILESYSTEM;
        }
        return options;
    }

    public virtual bool? ShowFolderDialog(IntPtr owner, bool throwOnError = false)
    {
        var dialog = (IFileOpenDialog)new FileOpenDialog();
        if (!string.IsNullOrEmpty(InputPath))
        {
            if (CheckHr(SHCreateItemFromParsingName(InputPath, null, typeof(IShellItem).GUID, out var item), throwOnError) != 0)
                return null;

            dialog.SetFolder(item);
        }

        var options = Fos.FOS_PICKFOLDERS;
        options = (Fos)SetOptions((int)options);
        dialog.SetOptions(options);

        if (Title != null)
        {
            dialog.SetTitle(Title);
        }

        if (OkButtonLabel != null)
        {
            dialog.SetOkButtonLabel(OkButtonLabel);
        }

        if (FileNameLabel != null)
        {
            dialog.SetFileName(FileNameLabel);
        }

        if (owner == IntPtr.Zero)
        {
            owner = Process.GetCurrentProcess().MainWindowHandle;
            if (owner == IntPtr.Zero)
            {
                owner = GetDesktopWindow();
            }
        }

        var hr = dialog.Show(owner);
        if (hr == ErrorCancelled)
            return null;

        if (CheckHr(hr, throwOnError) != 0)
            return null;

        if (CheckHr(dialog.GetResult(out var result), throwOnError) != 0)
            return null;

        if (CheckHr(result.GetDisplayName(Sigdn.SIGDN_DESKTOPABSOLUTEPARSING, out var path), throwOnError) != 0)
            return null;

        ResultPath = path;

        if (CheckHr(result.GetDisplayName(Sigdn.SIGDN_DESKTOPABSOLUTEEDITING, out path), false) == 0)
        {
            ResultName = path;
        }
        return true;
    }

    private static int CheckHr(int hr, bool throwOnError)
    {
        if (hr == 0) return hr;
        if (throwOnError)
            Marshal.ThrowExceptionForHR(hr);
        return hr;
    }

    [DllImport("shell32")]
    private static extern int SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] string pszPath, System.Runtime.InteropServices.ComTypes.IBindCtx pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IShellItem ppv);
    //IBindCtx
    [DllImport("user32")]
    private static extern IntPtr GetDesktopWindow();

    private const int ErrorCancelled = unchecked((int)0x800704C7);

    [ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")] // CLSID_FileOpenDialog
    private class FileOpenDialog
    {
    }

    [ComImport, Guid("42f85136-db7e-439c-85f1-e4075d135fc8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IFileOpenDialog
    {
        [PreserveSig] int Show(IntPtr parent); // IModalWindow
        [PreserveSig] int SetFileTypes();  // not fully defined
        [PreserveSig] int SetFileTypeIndex(int iFileType);
        [PreserveSig] int GetFileTypeIndex(out int piFileType);
        [PreserveSig] int Advise(); // not fully defined
        [PreserveSig] int Unadvise();
        [PreserveSig] int SetOptions(Fos fos);
        [PreserveSig] int GetOptions(out Fos pfos);
        [PreserveSig] int SetDefaultFolder(IShellItem psi);
        [PreserveSig] int SetFolder(IShellItem psi);
        [PreserveSig] int GetFolder(out IShellItem ppsi);
        [PreserveSig] int GetCurrentSelection(out IShellItem ppsi);
        [PreserveSig] int SetFileName([MarshalAs(UnmanagedType.LPWStr)] string pszName);
        [PreserveSig] int GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);
        [PreserveSig] int SetTitle([MarshalAs(UnmanagedType.LPWStr)] string pszTitle);
        [PreserveSig] int SetOkButtonLabel([MarshalAs(UnmanagedType.LPWStr)] string pszText);
        [PreserveSig] int SetFileNameLabel([MarshalAs(UnmanagedType.LPWStr)] string pszLabel);
        [PreserveSig] int GetResult(out IShellItem ppsi);
        [PreserveSig] int AddPlace(IShellItem psi, int alignment);
        [PreserveSig] int SetDefaultExtension([MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);
        [PreserveSig] int Close(int hr);
        [PreserveSig] int SetClientGuid();  // not fully defined
        [PreserveSig] int ClearClientData();
        [PreserveSig] int SetFilter([MarshalAs(UnmanagedType.IUnknown)] object pFilter);
        [PreserveSig] int GetResults([MarshalAs(UnmanagedType.IUnknown)] out object ppenum);
        [PreserveSig] int GetSelectedItems([MarshalAs(UnmanagedType.IUnknown)] out object ppsai);
    }

    [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    private interface IShellItem
    {
        [PreserveSig] int BindToHandler(); // not fully defined
        [PreserveSig] int GetParent(); // not fully defined
        [PreserveSig] int GetDisplayName(Sigdn sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);
        [PreserveSig] int GetAttributes();  // not fully defined
        [PreserveSig] int Compare();  // not fully defined
    }

#pragma warning disable CA1712 // Do not prefix enum values with type name
    private enum Sigdn : uint
    {
        SIGDN_DESKTOPABSOLUTEEDITING = 0x8004c000,
        SIGDN_DESKTOPABSOLUTEPARSING = 0x80028000,
    }

    [Flags]
    private enum Fos
    {
        FOS_PICKFOLDERS = 0x20,
        FOS_FORCEFILESYSTEM = 0x40,
    }
}

internal class FilePicker
{
    public static void Matlab(System.Windows.Controls.TextBox textBox)
    {
        var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Browse MATLAB Data Files",
            DefaultExt = "mat",
            Filter = "mat files (*.mat)|*.mat",
            RestoreDirectory = true,
        };
        if (openFileDialog1.ShowDialog() == Convert.ToBoolean(DialogResult.OK))
        {
            textBox.Text = openFileDialog1.FileName;
        }
    }
    public static void Csv(System.Windows.Controls.TextBox textBox)
    {
        var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Browse CSV Data Files",
            DefaultExt = "csv",
            Filter = "csv files (*.csv)|*.csv",
            RestoreDirectory = true,
        };
        if (openFileDialog1.ShowDialog() == Convert.ToBoolean(DialogResult.OK))
        {
            textBox.Text = openFileDialog1.FileName;
        }
    }
    public (string, string) Preset()
    {
        var main = new LIBSVM_GUI_Template_test.MainWindow();
        var openFileDialog1 = new Microsoft.Win32.OpenFileDialog
        {
            Title = "Browse Presets",
            RestoreDirectory = true,
            InitialDirectory = main.assemblyLocation + "\\Required_Files\\Presets\\",
        };
        return openFileDialog1.ShowDialog() == Convert.ToBoolean(DialogResult.OK) ? ("1", openFileDialog1.FileName) : ("0", openFileDialog1.FileName);
    }
}

#endregion