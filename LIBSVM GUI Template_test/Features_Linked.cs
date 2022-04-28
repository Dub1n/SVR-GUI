namespace LIBSVM_GUI_Template_test
{
    public class FeaturesLinked : ObservableObject
    {
        private string train1  ;      // = "0"
        private string train2  ;      // = "0"
        private string train3  ;      // = "0"
        private string train4  ;      // = "0"
        private string train5  ;      // = "0"
        private string train6  ;      // = "0"
        private string test1   ;      // = "0"
        private string test2   ;      // = "0"
        private string test3   ;      // = "0"
        private string test4   ;      // = "0"
        private string test5   ;      // = "0"
        private string test6   ;      // = "0"


        public string Train1Value
        { 
            get => train1;
            set
            {
                if (train1 == value) return;
                train1 = value;

                OnPropertyChanged("Train1Value");
                OnPropertyChanged("Test1Value");
            } 
        }

        public string Train2Value
        {
            get => train2;
            set
            {
                if (train2 == value) return;
                train2 = value;

                OnPropertyChanged("Train2Value");
                OnPropertyChanged("Test2Value");
            }
        }
        public string Train3Value
        {
            get => train3;
            set
            {
                if (train3 == value) return;
                train3 = value;

                OnPropertyChanged("Train3Value");
                OnPropertyChanged("Test3Value");
            }
        }
        public string Train4Value
        {
            get => train4;
            set
            {
                if (train4 == value) return;
                train4 = value;

                OnPropertyChanged("Train4Value");
                OnPropertyChanged("Test4Value");
            }
        }
        public string Train5Value
        {
            get => train5;
            set
            {
                if (train5 == value) return;
                train5 = value;
                
                OnPropertyChanged("Train5Value");
                OnPropertyChanged("Test5Value");
            }
        }
        public string Train6Value
        {
            get => train6;
            set
            {
                if (train6 == value) return;
                train6 = value;

                OnPropertyChanged("Train6Value");
                OnPropertyChanged("Test6Value");
            }
        }
        public string Test1Value
        {
            get { return test1 = train1; }
            set
            {
                if (test1 == value) return;
                test1 = value;

                OnPropertyChanged("Train1Value");
                OnPropertyChanged("Test1Value");
            }
        }
        public string Test2Value
        {
            get { return test2 = train2; }
            set
            {
                if (test2 == value) return;
                test2= value;
                OnPropertyChanged("Train2Value");
                OnPropertyChanged("Test2Value");
            }
        }
        public string Test3Value
        {
            get { return test3 = train3; }
            set
            {
                if (test3 == value) return;
                test3 = value;
                OnPropertyChanged("Train3Value");
                OnPropertyChanged("Test3Value");
            }
        }
        public string Test4Value
        {
            get { return test4 = train4; }
            set
            {
                if (test4 == value) return;
                test4 = value;
                OnPropertyChanged("Train4Value");
                OnPropertyChanged("Test4Value");
            }
        }
        public string Test5Value
        {
            get { return test5 = train5; }
            set
            {
                if (test5 == value) return;
                test5 = value;
                OnPropertyChanged("Train5Value");
                OnPropertyChanged("Test5Value");
            }
        }
        public string Test6Value
        {
            get { return test6 = train6; }
            set
            {
                if (test6 == value) return;
                test6 = value;
                OnPropertyChanged("Train6Value");
                OnPropertyChanged("Test6Value");
            }
        }
    }
}
